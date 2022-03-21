using Common.Logging;

using FrontDesk.Common;
using FrontDesk.Common.Bhservice.Import;
using FrontDesk.Common.Screening;
using FrontDesk.Server.Controllers;
using FrontDesk.Server.Data.BhsVisits;
using FrontDesk.Server.Data.KioskDataValues;
using FrontDesk.Server.Data.ScreeningProfile;
using FrontDesk.Server.Data.ScreenngProfile;
using FrontDesk.Server.Licensing.Services;
using FrontDesk.Server.Logging;
using FrontDesk.Server.Screening.Services;
using FrontDesk.Server.Services.Security;

using Newtonsoft.Json;

using RPMS.Common.Models;

using ScreenDox.Server.Common.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;

namespace FrontDesk.Server.Services
{
    [Obsolete]
    public class KioskEndpoint : IKioskEndpoint
    {
        // Injected by DI
        private readonly IValidatePatientRecordService _validatePatientRecordService;

        private readonly IKioskMinimalAgeRepository _minimalAgeRepository = new ScreeningProfileMinimalAgeDatabase();
        private readonly IBranchLocationService _branchLocationService = new BranchLocationService();
        private readonly IKioskService _kioskService = new KioskService();

        private ILog _log = LogManager.GetLogger<KioskEndpoint>();


        public KioskEndpoint(IValidatePatientRecordService validatePatientRecordService)
        {
            _validatePatientRecordService = validatePatientRecordService ?? throw new ArgumentNullException(nameof(validatePatientRecordService));
        }

        public bool? SaveScreeningResult_v2(ScreeningResult result, ScreeningTimeLogRecord[] timeLog)
        {
            var creator = new ScreeningResultCreator();

            {
                //process Primary and alternative sections (PHQ9A/PHQ-9 and other similar)
                var pairs = ScreeningSectionDescriptor.AlternativeOptionalMandatorySections;

                foreach (var pair in pairs)
                {
                    // replace alternative to primary sections in question values
                    var allQuestionSection = result.FindSectionByID(pair.AllQuestions);
                    if (allQuestionSection != null)
                    {
                        _log.InfoFormat("{0} ({1}) section has been received.", pair.Primary, pair.AllQuestions);

                        // check that Primary section does not exists - otherwise it will be an exception
                        if (result.FindSectionByID(pair.Primary) != null)
                        {
                            _log.ErrorFormat("Both {0} and {1} have been sent from the kiosk! Alternative {0} has been removed. Kiosk ID: {2}",
                                pair.Primary,
                                pair.AllQuestions,
                                result.KioskID);

                            result.SectionAnswers.Remove(allQuestionSection);
                        }
                        else
                        {
                            allQuestionSection.ScreeningSectionID = pair.Primary;
                            allQuestionSection.Answers.ForEach(x => x.ScreeningSectionID = pair.Primary);
                        }
                    }

                    //when used alternative flow - detect and switch to All Questions in time reporting tracking
                    var fullSection = result.FindSectionByID(pair.Primary);
                    if (fullSection != null && fullSection.Answers?.Count > 2)
                    {
                        _log.InfoFormat("{0} section has been received when all questions has been answered.", pair.Primary);
                        
                        timeLog.Where(x => x.ScreeningSectionID == pair.Primary)
                            .ToList()
                            .ForEach(x => x.ScreeningSectionID = pair.AllQuestions);
                    }
                }
            }
            return creator.SaveScreeningResult(result, timeLog);
        }
        
        /// <summary>
        /// Get the list of section minimal age settings that have been changed from the lastModifiedDateUTC for assigned Screening Profile
        /// </summary>
        /// <param name="lastModifiedDateUTC">The last date of modification in the kiosk database</param>
        /// <returns></returns>
        public List<ScreeningSectionAgeView> GetModifiedAgeSettings_v2(short kioskId, DateTime lastModifiedDateUTC)
        {
            try
            {
                IList<ScreeningSectionAge> changes = _minimalAgeRepository.GetModifiedSectionMinimalAgeSettingsForKiosk(kioskId, lastModifiedDateUTC);
                List<ScreeningSectionAgeView> result = new List<ScreeningSectionAgeView>(changes.Count);
                foreach (var item in changes)
                {
                    result.Add(new ScreeningSectionAgeView(item));
                }

                return result;
            }
            catch (Exception ex)
            {
                ErrorLog.Add(ex.Message, ex.StackTrace, null);

                _log.ErrorFormat("Failed to proceed KioskEndpoint.GetModifiedAgeSettings. Kiosk id: {0}. Timestamp: {1}", ex, kioskId, lastModifiedDateUTC);
                return new List<ScreeningSectionAgeView>();
            }

        }

        public bool Ping_v3(ScreenDox.Server.Common.Models.KioskPingMessage message)
        {
            bool succeed = true;

            try
            {
                var cert = LicenseService.Current.GetActivatedLicense();
                // Ping if kiosks count has not been exceeded 


                if (cert != null && _kioskService.GetNotDisabledCount() <= cert.License.MaxKiosks
                    && _branchLocationService.GetNotDisabledCount() <= cert.License.MaxBranchLocations)
                {
                    //Update kiosk last activity date
                    succeed = _kioskService.ChangeLastActivityDate(message, DateTimeOffset.Now);
                }
                else
                {
                    succeed = false;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Add(ex.Message, ex.StackTrace, message?.KioskID);
                succeed = false;
            }

            return succeed;
        }

        public bool TestKioskInstallation(string kioskKey)
        {
            try
            {
                return _kioskService.TestKioskInstallation(kioskKey);

            }
            catch (Exception ex)
            {
                ErrorLog.AddServerException(string.Format("Kiosk Key validation failed for key {0}", kioskKey), ex);
                return false;
            }
        }

        private FrontDesk.Configuration.PatientScreeningFrequencyService CreatePatientScreeningFrequencyService()
        {
            //TODO: Instantiate repositories and service through IoC container
            return new FrontDesk.Configuration.PatientScreeningFrequencyService(
                    () => new FrontDesk.Server.Data.PatientScreeningFrequencyDatabase(),
                    new ScreeningProfileFrequencyDb()
                    );
        }

        public Dictionary<string, int> GetPatientScreeningFrequencyStatistics_v2(ScreeningPatientIdentity patient, short kioskID)
        {
            int screeningProfileId = _branchLocationService.GetScreeningProfileByKioskID(kioskID);

            return CreatePatientScreeningFrequencyService().GetGPRAScreeningCount(patient, screeningProfileId);
        }

        public bool SaveDemographicsResults(PatientDemographicsKioskResult result, ScreeningTimeLogRecord[] timeLog)
        {
            var creator = new ScreeningResultCreator();

            return creator.SaveDemographicsResult(result, timeLog);
        }


        /// <summary>
        /// Get the list of changes into Lookup tables that are cached on the kiosk app
        /// </summary>
        /// <param name="lastModifiedDateUTC">The lastest timestamp of data on the kiosk</param>
        public Dictionary<string, List<LookupValue>> GetModifiedLookupValues(DateTime lastModifiedDateUTC, short kioskID)
        {
            var lookupService = new LookupListsDataSource();
            var typeaheadService = new TypeaheadDataSource();
            var deleteQueue = new LookupValuesDeleteLogDb();


            var kiosk = _kioskService.GetByID(kioskID);
            if (kiosk == null)
            {
                _log.WarnFormat("KioskEndpoint.GetModifiedLookupValues. Invalid kiosk ID provided. Kiosk timestamp: {0}. Kiosk id: {1}", lastModifiedDateUTC, kioskID);

                return new Dictionary<string, List<LookupValue>>();
            }

            var result = new Dictionary<string, List<LookupValue>>();

            Action<string, List<LookupValue>> addFunc = (key, values) =>
           {
               if (values.Any())
               {
                   result.Add(key, values);
               }
           };


            try
            {
                //get tribes
                addFunc(
                    TypeaheadListDescriptor.Tribe,
                    typeaheadService.Tribes().GetModifiedValues(lastModifiedDateUTC).Select(x => new LookupValue
                    {
                        Id = 0,
                        Name = x
                    }).ToList());



                //get counties
                addFunc(
                   TypeaheadListDescriptor.County,
                    typeaheadService.Counties().GetModifiedValues(lastModifiedDateUTC).Select(x => new LookupValue
                    {
                        Id = 0,
                        Name = x
                    }).ToList()
                    );

                //get Race
                addFunc(
                   LookupListDescriptor.Race,
                    lookupService.Get(LookupListDescriptor.Race).GetModifiedValues(lastModifiedDateUTC)
                    );
                //get EducationLevel
                addFunc(
                   LookupListDescriptor.EducationLevel,
                    lookupService.Get(LookupListDescriptor.EducationLevel).GetModifiedValues(lastModifiedDateUTC)
                    );

                //get MaritalStatus
                addFunc(
                   LookupListDescriptor.MaritalStatus,
                    lookupService.Get(LookupListDescriptor.MaritalStatus).GetModifiedValues(lastModifiedDateUTC)
                    );

                //get SexualOrientation
                addFunc(
                   LookupListDescriptor.SexualOrientation,
                    lookupService.Get(LookupListDescriptor.SexualOrientation).GetModifiedValues(lastModifiedDateUTC)
                    );

                //get MilitaryExperience
                addFunc(
                   LookupListDescriptor.MilitaryExperience,
                   lookupService.Get(LookupListDescriptor.MilitaryExperience).GetModifiedValues(lastModifiedDateUTC)
                   );

                //get Gender
                addFunc(
                   LookupListDescriptor.Gender,
                    lookupService.Get(LookupListDescriptor.Gender).GetModifiedValues(lastModifiedDateUTC)
                    );

                //get LivingOnReservation
                addFunc(
                   LookupListDescriptor.LivingOnReservation,
                    lookupService.Get(LookupListDescriptor.LivingOnReservation).GetModifiedValues(lastModifiedDateUTC)
                    );

                if (_log.IsInfoEnabled)
                {
                    _log.InfoFormat("KioskEndpoint.GetModifiedLookupValues succeed. Kiosk timestamp: {0}. Kiosk: {1}. Result: {2}",
                        lastModifiedDateUTC,
                        kiosk.KioskKey,
                        JsonConvert.SerializeObject(result));
                }

                return result;
            }
            catch (Exception ex)
            {
                ErrorLog.Add(ex.Message, ex.StackTrace, null);

                _log.ErrorFormat("Failed to proceed KioskEndpoint.GetModifiedAgeSettings. Kiosk timestamp: {0}. Kiosk: {1}", ex, lastModifiedDateUTC, kiosk.KioskKey);

                return new Dictionary<string, List<LookupValue>>();
            }

        }


        /// <summary>
        /// Get the list of deleted items that need to be propagated to Kiosk App
        /// </summary>
        /// <param name="lastModifiedDateUTC">The lastest timestamp of data on the kiosk</param>
        public Dictionary<string, List<LookupValue>> GetLookupValuesDeleteLog(DateTime lastModifiedDateUTC, short kioskID)
        {
            ILookupValuesDeleteLogDb deleteQueue = new LookupValuesDeleteLogDb();

            var kiosk = _kioskService.GetByID(kioskID);

            if (kiosk == null)
            {
                _log.WarnFormat("KioskEndpoint.GetLookupValuesDeleteLog. Invalid kiosk ID provided. Kiosk timestamp: {0}. Kiosk id: {1}", lastModifiedDateUTC, kioskID);

                return new Dictionary<string, List<LookupValue>>();
            }

            try
            {
                Dictionary<string, List<LookupValue>> result = deleteQueue.Get(lastModifiedDateUTC).Aggregate(
                        new Dictionary<string, List<LookupValue>>(),
                        (dict, x) =>
                        {
                            List<LookupValue> value = new List<LookupValue>();

                            if (!dict.TryGetValue(x.TableName, out value))
                            {
                                value = new List<LookupValue>();
                                dict.Add(x.TableName, value);
                            }

                            value.Add(new LookupValue
                            {
                                Id = x.Id,
                                Name = x.Name
                            });

                            return dict;
                        }
                    );

                if (_log.IsInfoEnabled)
                {
                    _log.InfoFormat("KioskEndpoint.GetLookupValuesDeleteLog succeeded. Kiosk timestamp: {0}. Kiosk: {1}. Result: {2}",
                        lastModifiedDateUTC,
                        kiosk.KioskKey,
                        JsonConvert.SerializeObject(result));
                }

                return result;
            }
            catch (Exception ex)
            {
                ErrorLog.Add(ex.Message, ex.StackTrace, null);

                _log.ErrorFormat("Failed to proceed KioskEndpoint.GetLookupValuesDeleteLog. Kiosk timestamp: {0}. Kiosk: {1}", ex, lastModifiedDateUTC, kiosk.KioskKey);

                return new Dictionary<string, List<LookupValue>>();
            }

        }

        // Patient name validation 

        public PatientSearch ValidatePatient(PatientSearch patient)
        {
            if (!AuthorizeKiosk()) { return null; }

            var result =  _validatePatientRecordService.ValidatePatientRecord(patient);

            return result;
        }


        private bool AuthorizeKiosk()
        {
            var kioskKey = GetHeader<string>(KioskEndpointHeaderDescriptor.KioskIDHeader, "");
            var kioskSecret = GetHeader<string>(KioskEndpointHeaderDescriptor.KioskSecretHeader, "");

            if (string.IsNullOrEmpty(kioskKey))
            {
                throw new KioskAuthorizeException("Kiosk ID has not been provided.");
            }

            if (string.IsNullOrEmpty(kioskSecret))
            {
                throw new KioskAuthorizeException("Kiosk Secret has not been provided.");
            }
            try
            {
                bool result = _kioskService.ValidateKiosk(kioskKey, kioskSecret);

                if (!result)
                {
                    _log.WarnFormat("Rejected unauthorized request from the kiok. Key:{0}. Secret: {1}", kioskKey, kioskSecret);
                }

                return result;
            }
            catch (Exception ex)
            {
                var message = $"Failed to authorize kiosk. Server error. Key:{kioskKey}. Secret: {kioskSecret}";
                _log.Error(message, ex);

                // all error to the error database
                ErrorLog.Add(message, ex.ToString(), null);

                return false;
            }
        }

        private static T GetHeader<T>(string name, string ns)
        {
            return OperationContext.Current.IncomingMessageHeaders.FindHeader(name, ns) > -1
                ? OperationContext.Current.IncomingMessageHeaders.GetHeader<T>(name, ns)
                : default(T);
        }


    }


}
