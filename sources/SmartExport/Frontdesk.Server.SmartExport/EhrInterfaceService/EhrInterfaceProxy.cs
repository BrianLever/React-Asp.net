using Common.Logging;

using FrontDesk;
using FrontDesk.Server.Logging;
using FrontDesk.Server.Services;

using RPMS.Common.Models;
using RPMS.Common.Models.Factory;
using RPMS.Common.Models.PatientValidation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;

namespace Frontdesk.Server.SmartExport.EhrInterfaceService
{

    public class EhrInterfaceProxy : IEhrInterfaceProxy, IValidatePatientRecordService
    {
        private readonly ILog _logger = LogManager.GetLogger<EhrInterfaceProxy>();
        private readonly TimeSpan DefaultCacheExpirationTimeout;

        private static EhrInterfaceProxy _instance = null;
        private static object _instanceLock = new object();
        public static EhrInterfaceProxy Instance
        {
            get
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                    {
                        lock (_instanceLock)
                        {
                            _instance = new EhrInterfaceProxy();
                        }

                    }
                    return _instance;
                }
            }
        }

        protected EhrInterfaceProxy()
        {
            DefaultCacheExpirationTimeout = TimeSpan.FromSeconds(FrontDesk.Common.Configuration.AppSettingsProxy.GetIntValue("RpmsCachingTimeoutInSec", 300)); //by default - 5 mins
        }



        #region Patient methods

        /// <summary>
        /// Get matched records
        /// </summary>
        public List<Patient> GetMatchedPatients(ScreeningResult screeningResult, int startRow, int maxRows)
        {
            if (screeningResult == null)
                return new List<Patient>();

            string key = "rpmspatientlist:{0}:{1}:{2:yyMMdd}:{3}:{4}".FormatWith(
                screeningResult.ID,
                screeningResult.FullName,
                screeningResult.Birthday,
                startRow,
                maxRows);

            return GetFromCache<List<Patient>>(() =>
            {
                Patient patient = screeningResult.ToPatient();
                return GetMatchedPatients(patient, startRow, maxRows);
            },
                key
            ) ?? new List<Patient>();
        }

        public void ResetCache4GetMatchedPatients(ScreeningResult screeningResult)
        {
            if (screeningResult != null)
            {
                string key = "rpmspatientlist:{0}:{1}:{2:yyMMdd}".FormatWith(
                    screeningResult.ID,
                    screeningResult.FullName,
                    screeningResult.Birthday
                    );

                ResetCache(key);
            }

        }

        /// <summary>
        /// Get matched records
        /// </summary>
        /// <param name="patient"></param>
        /// <param name="startRow"></param>
        /// <param name="maxRows"></param>
        /// <returns></returns>
        public List<Patient> GetMatchedPatients(Patient patient, int startRow, int maxRows)
        {

            List<Patient> patients = null;


            EhrInterfaceClient client = new EhrInterfaceClient();
            try
            {

                patients = client.GetMatchedPatients(patient, startRow, maxRows);
                client.Close();
            }
            catch (Exception exc)
            {
                ErrorLog.Add("Failed to get patient matches from the the EHR service.", exc.ToString(), null);

                _logger.ErrorFormat("[FrontDeskRpmsInterfaceProxy] Failed to get patient matches from the the EHR service. Patient Name: {0}, {1} {2}", exc, patient.LastName, patient.FirstName, patient.MiddleName);

                client.Abort();
                throw;
            }
            finally
            {

            }

            return patients ?? new List<Patient>();
        }

        public Patient GetPatientRecord(PatientSearch patientSearch)
        {
            string key = "rpmspatientrecord:{0}".FormatWith(patientSearch.ID);

            return GetFromCache<Patient>(() =>
            {
                Patient patient = null;
                EhrInterfaceClient client = new EhrInterfaceClient();
                try
                {
                    patient = client.GetPatientRecord(patientSearch);
                    client.Close();
                }
                catch (Exception exc)
                {
                    ErrorLog.Add("Failed to get patient record from the external EHR service.", exc.ToString(), null);
                    _logger.Error("Failed to get patient record from the EHR service.", exc);
                    client.Abort();
                    throw;
                }
                return patient;
            },
                key
            );

        }

        public int GetPatientCount(ScreeningResult screeningResult)
        {
            return GetPatientCount(screeningResult.ToPatient());
        }

        /// <summary>
        /// Get number of matched records
        /// </summary>
        /// <param name="screeningResult"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        public int GetPatientCount(Patient patient)
        {
            string key = string.Format("rpmspatientcount:{0}|{1}", patient.FullName().ToUpperInvariant(), patient.DateOfBirth);

            return GetFromCache(() =>
            {
                int count = 0;
                EhrInterfaceClient client = new EhrInterfaceClient();
                try
                {
                    count = client.GetPatientCount(patient);
                    client.Close();
                }
                catch (Exception exc)
                {
                    ErrorLog.Add("Failed to get the number of patient's matches from the external EHR service.", exc.ToString(), null);
                    _logger.Error(exc);
                    client.Abort();
                    throw;
                }
                return count;
            },
                key
            );
        }

        #endregion


        #region Scheduled Visits methods


        /// <summary>
        /// Get scheduled visits
        /// </summary>
        public List<Visit> GetScheduledVisitsByPatient(PatientSearch rpmsPatient, int startRow, int maxRows)
        {
            if (rpmsPatient == null)
            {
                throw new ArgumentNullException("rpmsPatient");
            }

            if (rpmsPatient.ID == 0)
            {
                _logger.WarnFormat("Invalid EHR Patient ID passed as parameter for visit export. EHR ID: {0}, Name: {1}.",
                    rpmsPatient.ID,
                    rpmsPatient.FullName().AsMaskedFullName()
                    );
            }

            List<Visit> visits = null;

            var ehrId = rpmsPatient.ID;
            var patientName = rpmsPatient.FullName();

            EhrInterfaceClient client = new EhrInterfaceClient();
            try
            {
                _logger.InfoFormat("Getting visits for patient. EHR ID: {0}, Name: {1}",
                   ehrId,
                   patientName.AsMaskedFullName()
                    );

                visits = client.GetScheduledVisitsByPatient(rpmsPatient, startRow, maxRows);
                client.Close();
            }
            catch (Exception exc)
            {
                ErrorLog.Add("Failed to get scheduled visits from the external EHR service.", exc.ToString(), null);
                _logger.Error("Failed to get scheduled visits from the EHR service.", exc);

                client.Abort();
                throw;
            }
            finally
            {

            }

            _logger.TraceFormat("Found {0} visits for patient. EHR ID: {1}. Name: {2}.",
                visits.Count,
                ehrId,
                patientName.AsMaskedFullName()
                    );

            return visits;

        }


        public Visit GetScheduledVisit(int visitID, PatientSearch patient)
        {
            if (patient == null)
            {
                throw new ArgumentNullException("patient");
            }


            string key = "rpmssinglevisit:{0}|{1}".FormatWith(visitID, patient.ID);

            return GetFromCache<Visit>(() =>
            {

                Visit result = null;
                EhrInterfaceClient client = new EhrInterfaceClient();
                try
                {

                    result = client.GetVisitRecord(visitID, patient);
                    client.Close();
                }
                catch (Exception exc)
                {
                    ErrorLog.Add("Failed to get scheduled visit from the external EHR service.", exc.ToString(), null);
                    _logger.ErrorFormat("Failed to get scheduled visit from the EHR service", exc);

                    client.Abort();
                    throw;
                }
                finally
                {

                }
                return result;
            },
            key);
        }


        /// <summary>
        /// Get number of scheduled visits for patient
        /// </summary>
        public int GetScheduledVisitsByPatientCount(PatientSearch patient)
        {
            if (patient == null)
            {
                throw new ArgumentNullException("patient");
            }

            string key = "rpmsvisitcount:{0}".FormatWith(patient.ID);

            return GetFromCache<int>(() =>
            {
                int count = 0;

                EhrInterfaceClient client = new EhrInterfaceClient();
                try
                {
                    count = client.GetScheduledVisitsByPatientCount(patient);
                    client.Close();
                }
                catch (Exception exc)
                {
                    ErrorLog.Add("Failed to get the number of scheduled visits from the external EHR system.", exc.ToString(), null);
                    
                    _logger.Error(exc);

                    client.Abort();
                    throw;
                }

                return count;
            },
            key
            );
        }


        #endregion


        #region Export Preview

        /// <summary>
        /// Get number of scheduled visits for patient
        /// </summary>
        /// <remarks>Method uses caching at 60 seconds</remarks>
        public ExportTask PreviewExportResult(ScreeningResult screeningResult, PatientSearch selectedPatient, int selectedVisitRowId)
        {

            if (screeningResult == null) throw new ArgumentNullException("screeningResult");


            string key = "rpmspreviewexport:{0}:{1}:{2}:{3}:{4}".FormatWith(
                screeningResult.ID,
                screeningResult.FullName,
                screeningResult.Birthday,
                selectedPatient.ID,
                selectedVisitRowId);

            return GetFromCache<ExportTask>(() =>
            {
                ExportTask previewResult = null;

                EhrInterfaceClient client = new EhrInterfaceClient();
                try
                {
                    previewResult = client.PreviewExportResult(screeningResult, selectedPatient, selectedVisitRowId);
                    client.Close();
                }
                catch (Exception exc)
                {
                    ErrorLog.Add("Failed to get preview resuls for exporting report from the external EHR service.", exc.ToString(), null);
                    _logger.Error("Failed to get preview resuls for exporting report from the EHR service.", exc);
                    client.Abort();
                    throw;
                }

                return previewResult;
            },
            key,
            TimeSpan.FromSeconds(60) //custom cache period for page reload and multiple binding
            );
        }

        public List<ExportResult> CommitExportTask(int patientID, int visitID, ExportTask exportTask)
        {
            List<ExportResult> result;
            EhrInterfaceClient client = new EhrInterfaceClient();

            try
            {
                result = client.CommitExportTask(patientID, visitID, exportTask);
                client.Close();

            }
            catch (Exception exc)
            {
                ErrorLog.Add("Failed to export report via external EHR service.", exc.ToString(), null);
                _logger.Trace("Failed to export report via EHR service.", exc);
                client.Abort();
                throw;
            }
            return result ?? new List<ExportResult>();
        }


        public List<ExportResult> CommitExportResult(int patientID, int visitID, ScreeningResult screeningResult, Screening screeningInfo)
        {
            List<ExportResult> result;
            EhrInterfaceClient client = new EhrInterfaceClient();


            ScreeningResultRecord record = ScreeningResultRecordFactory.Create(patientID, visitID, screeningResult, screeningInfo);

            try
            {
                result = client.ExportScreeningData(record);
                client.Close();

            }
            catch (Exception exc)
            {
                ErrorLog.Add("Failed to export report via external EHR service.", exc.ToString(), null);
                _logger.Trace("Failed to export report via external EHR service.", exc);
                client.Abort();
                throw;
            }
            return result ?? new List<ExportResult>();
        }


        #endregion


        #region Patient Name Validation

        /// <summary>
        /// Get matched records
        /// </summary>
        public Patient FindEhrPatientRecord(ScreeningResult screeningResult)
        {
            if (screeningResult == null)
                return null;

            string key = "ehrpatientvalidationinfo:{0}:{1}:{2:yyMMdd}".FormatWith(
                screeningResult.ID,
                screeningResult.FullName,
                screeningResult.Birthday
                );

            return GetFromCache<Patient>(() =>
            {
                return ValidatePatient(screeningResult.ToPatient());
            },
                key
            );
        }

        /// <summary>
        /// Get matched records
        /// </summary>
        public PatientSearch ValidatePatientRecord(PatientSearch patient)
        {
            if (patient == null)
                return new PatientSearch();

            string key = "ehrpatientvalidationinfo:{0}:{1:yyMMdd}".FormatWith(
                patient.FullName(),
                patient.DateOfBirth
                );

            return GetFromCache<PatientSearch>(() =>
            {
                return ValidatePatient(patient);
            },
                key
            );
        }

        private Patient ValidatePatient(PatientSearch patient)
        {
            PatientValidationResult result = null;


            EhrInterfaceClient client = new EhrInterfaceClient();
            try
            {

                result = client.ValidatePatientRecord(patient);
                client.Close();
            }
            catch (Exception exc)
            {
                ErrorLog.Add("Failed to validate patient name through external EHR system.", exc.ToString(), null);
                _logger.ErrorFormat("Failed to validate patient name through external EHR system.", exc);

                client.Abort();
                throw;
            }
            finally
            {

            }

            var patientResult = result.PatientRecord;

            if (result.IsMatchFound() && result.CorrectionsLog != null && result.CorrectionsLog.Count > 0)
            {
                _logger.InfoFormat("Patient name has been corrected through EHR. Result: {0}, {1}. DOB: {2}. Strategy applied: [{3}].",
                    patientResult.LastName,
                    patientResult.FirstName,
                    patientResult.DateOfBirth,
                    string.Join("||", result.CorrectionsLog)
                );
            }

            return patientResult;
        }


        #endregion



        #region Caching

        private readonly List<string> ListOfUsedKeysInCache = new List<string>();

        private T GetFromCache<T>(Func<T> getter, string key)
        {
            return GetFromCache<T>(getter, key, this.DefaultCacheExpirationTimeout);
        }

        private T GetFromCache<T>(Func<T> getter, string key, TimeSpan cacheExpirationTimeout)
        {
            T result = default(T);
            var cache = MemoryCache.Default; ;

            object value = cache.Get(key);
            if (value == null)
            {
                result = getter();

                if (result != null)
                {
                    CacheItemPolicy policy = new CacheItemPolicy
                    {
                        AbsoluteExpiration = DateTimeOffset.Now.Add(cacheExpirationTimeout),
                        Priority = CacheItemPriority.Default,
                    };

                    cache.Add(key,
                        result,
                        policy
                      );
                    ListOfUsedKeysInCache.Add(key);
                }
            }
            else
            {
                result = (T)value;
            }
            return result;
        }

        private void ResetCache(string key)
        {
            var cache = HttpContext.Current.Cache;

            var keys = ListOfUsedKeysInCache.Where(x => x.Contains(key)).ToList();

            foreach (var k in keys)
            {
                cache.Remove(k);
                ListOfUsedKeysInCache.Remove(k);
            }

        }


        #endregion

        /// <summary>
        /// Get number of scheduled visits for patient
        /// </summary>
        public ExportMetaInfo GetMeta()
        {

            string key = "ExportMetaInfo";

            return GetFromCache<ExportMetaInfo>(() =>
            {
                ExportMetaInfo result = null;

                var client = new EhrInterfaceClient();
                try
                {
                    result = client.GetMeta();
                    client.Close();
                }
                catch (Exception exc)
                {
                    ErrorLog.Add("Failed to get meta information from ScreenDox EHR system connector.", exc.ToString(), null);
                    _logger.Trace("Failed to get meta information from ScreenDox EHR system connector.", exc);


                    client.Abort();
                    throw;
                }

                return result;
            },
            key
            );
        }
    }
}
