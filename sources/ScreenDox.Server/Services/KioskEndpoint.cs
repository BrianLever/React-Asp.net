using Common.Logging;

using FrontDesk;
using FrontDesk.Common;
using FrontDesk.Common.Bhservice.Import;
using FrontDesk.Common.Screening;
using FrontDesk.Configuration;
using FrontDesk.Server;
using FrontDesk.Server.Data.BhsVisits;
using FrontDesk.Server.Data.KioskDataValues;
using FrontDesk.Server.Data.ScreeningProfile;
using FrontDesk.Server.Licensing.Services;
using FrontDesk.Server.Logging;
using FrontDesk.Server.Screening.Services;
using FrontDesk.Server.Services.Security;

using Newtonsoft.Json;

using RPMS.Common.Models;

using ScreenDox.Server.Common.Services;
using ScreenDox.Server.Controllers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace ScreenDox.Server.Services
{
    public class KioskEndpoint : IKioskEndpoint
    {
        // Injected by DI
        private readonly FrontDesk.Server.Services.IValidatePatientRecordService _validatePatientRecordService;

        private readonly IKioskMinimalAgeRepository _minimalAgeRepository;
        private readonly IBranchLocationService _branchLocationService;
        private readonly IKioskService _kioskService;
        private readonly ScreeningResultCreator _creator;
        private readonly IPatientScreeningFrequencyService _patientScreeningFrequencyService;
        private readonly ILookupValuesDeleteLogDb _deleteQueueRepository;
        private readonly ILookupListsDataSource _lookupService;
        private readonly ITypeaheadDataSourceFactory _typeaheadService;

        private ILog _log = LogManager.GetLogger<KioskEndpoint>();


        public KioskEndpoint(FrontDesk.Server.Services.IValidatePatientRecordService validatePatientRecordService,
                             ScreeningResultCreator creator,
                             IKioskService kioskService,
                             IBranchLocationService branchLocationService,
                             IKioskMinimalAgeRepository minimalAgeRepository,
                             IPatientScreeningFrequencyService patientScreeningFrequencyService,
                             ILookupValuesDeleteLogDb deleteQueueRepository,
                             ITypeaheadDataSourceFactory typeaheadService,
                             ILookupListsDataSource lookupService)
        {
            _validatePatientRecordService = validatePatientRecordService ?? throw new ArgumentNullException(nameof(validatePatientRecordService));
            _creator = creator ?? throw new ArgumentNullException(nameof(creator));
            _kioskService = kioskService ?? throw new ArgumentNullException(nameof(kioskService));
            _branchLocationService = branchLocationService ?? throw new ArgumentNullException(nameof(branchLocationService));
            _minimalAgeRepository = minimalAgeRepository ?? throw new ArgumentNullException(nameof(minimalAgeRepository));
            _patientScreeningFrequencyService = patientScreeningFrequencyService ?? throw new ArgumentNullException(nameof(patientScreeningFrequencyService));
            _deleteQueueRepository = deleteQueueRepository ?? throw new ArgumentNullException(nameof(deleteQueueRepository));
            _typeaheadService = typeaheadService ?? throw new ArgumentNullException(nameof(typeaheadService));
            _lookupService = lookupService ?? throw new ArgumentNullException(nameof(lookupService));
        }

        public bool? SaveScreeningResult_v2(ScreeningResult result, ScreeningTimeLogRecord[] timeLog)
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
            return _creator.SaveScreeningResult(result, timeLog);
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

        public Dictionary<string, int> GetPatientScreeningFrequencyStatistics_v2(ScreeningPatientIdentity patient, short kioskID)
        {
            int screeningProfileId = _branchLocationService.GetScreeningProfileByKioskID(kioskID);

            return _patientScreeningFrequencyService.GetGPRAScreeningCount(patient, screeningProfileId);
        }

        public bool SaveDemographicsResults(PatientDemographicsKioskResult result, ScreeningTimeLogRecord[] timeLog)
        {
            return _creator.SaveDemographicsResult(result, timeLog);
        }


        /// <summary>
        /// Get the list of changes into Lookup tables that are cached on the kiosk app
        /// </summary>
        /// <param name="lastModifiedDateUTC">The lastest timestamp of data on the kiosk</param>
        public Dictionary<string, List<LookupValue>> GetModifiedLookupValues(DateTime lastModifiedDateUTC, short kioskID)
        {

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
                    _typeaheadService.Tribes().GetModifiedValues(lastModifiedDateUTC).Select(x => new LookupValue
                    {
                        Id = 0,
                        Name = x
                    }).ToList());



                //get counties
                addFunc(
                   TypeaheadListDescriptor.County,
                   _typeaheadService.Counties().GetModifiedValues(lastModifiedDateUTC).Select(x => new LookupValue
                    {
                        Id = 0,
                        Name = x
                    }).ToList()
                    );

                //get Race
                addFunc(
                   LookupListDescriptor.Race,
                    _lookupService.Get(LookupListDescriptor.Race).GetModifiedValues(lastModifiedDateUTC)
                    );
                //get EducationLevel
                addFunc(
                   LookupListDescriptor.EducationLevel,
                    _lookupService.Get(LookupListDescriptor.EducationLevel).GetModifiedValues(lastModifiedDateUTC)
                    );

                //get MaritalStatus
                addFunc(
                   LookupListDescriptor.MaritalStatus,
                    _lookupService.Get(LookupListDescriptor.MaritalStatus).GetModifiedValues(lastModifiedDateUTC)
                    );

                //get SexualOrientation
                addFunc(
                   LookupListDescriptor.SexualOrientation,
                    _lookupService.Get(LookupListDescriptor.SexualOrientation).GetModifiedValues(lastModifiedDateUTC)
                    );

                //get MilitaryExperience
                addFunc(
                   LookupListDescriptor.MilitaryExperience,
                   _lookupService.Get(LookupListDescriptor.MilitaryExperience).GetModifiedValues(lastModifiedDateUTC)
                   );

                //get Gender
                addFunc(
                   LookupListDescriptor.Gender,
                    _lookupService.Get(LookupListDescriptor.Gender).GetModifiedValues(lastModifiedDateUTC)
                    );

                //get LivingOnReservation
                addFunc(
                   LookupListDescriptor.LivingOnReservation,
                    _lookupService.Get(LookupListDescriptor.LivingOnReservation).GetModifiedValues(lastModifiedDateUTC)
                    );

                if (_log.IsInfoEnabled && result.Any())
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

            var kiosk = _kioskService.GetByID(kioskID);

            if (kiosk == null)
            {
                _log.WarnFormat("KioskEndpoint.GetLookupValuesDeleteLog. Invalid kiosk ID provided. Kiosk timestamp: {0}. Kiosk id: {1}", lastModifiedDateUTC, kioskID);

                return new Dictionary<string, List<LookupValue>>();
            }

            try
            {
                Dictionary<string, List<LookupValue>> result = _deleteQueueRepository.Get(lastModifiedDateUTC).Aggregate(
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

                if (_log.IsInfoEnabled && result.Any())
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

            var result = _validatePatientRecordService.ValidatePatientRecord(patient);

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
