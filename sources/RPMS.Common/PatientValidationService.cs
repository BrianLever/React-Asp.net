using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using FrontDesk;
using RPMS.Common;
using RPMS.Common.Models;
using RPMS.Common.Models.PatientValidation;
using ScreenDox.EHR.Common.SmartExport;
using ScreenDox.EHR.Common.SmartExport.AutoCorrection;

namespace ScreenDox.EHR.Common
{
    /// <summary>
    /// Validate patient info against existing records in ScreenDox and EHR databases
    /// </summary>
    public class PatientValidationService : IPatientValidationService
    {
        private readonly IPatientService _patientService;
        private readonly ILog _logger = LogManager.GetLogger<PatientValidationService>();
        private readonly PatientAutoCorrectionStrategyFactory _correctionStrategyFactory = new PatientAutoCorrectionStrategyFactory();
        private readonly IPatientInfoMatchService _patientInfoMatchService;
        private readonly IPatientNameCorrectionLogService _patientNameCorrectionLogService;

        public PatientValidationService(
            IPatientService patientService, 
            IPatientInfoMatchService patientInfoMatchService,
            IPatientNameCorrectionLogService patientNameCorrectionLogService)
        {
            _patientService = patientService ?? throw new ArgumentNullException(nameof(patientService));
            _patientInfoMatchService = patientInfoMatchService ?? throw new ArgumentNullException(nameof(patientInfoMatchService));
            _patientNameCorrectionLogService = patientNameCorrectionLogService ?? throw new ArgumentNullException(nameof(patientNameCorrectionLogService));
        }

        /// <summary>
        /// Find best match of patient record from EHR using auto-correction algorithms
        /// </summary>
        /// <param name="submittedPatientInfo"></param>
        /// <returns></returns>
        public PatientValidationResult ValidatePatientRecord(PatientSearch submittedPatientInfo)
        {
            var result = new PatientValidationResult();
            Patient ehrPatient;

            // save original patient Info
            var originalPatientInfo = submittedPatientInfo;

            // validate original record first
            if (TryFindMatchedPatient(submittedPatientInfo, out ehrPatient))
            {
                result.PatientRecord = ehrPatient;
                return result;
            }

            // 0. try to correct names using internal map
            var mappedPatientSearch = _patientInfoMatchService.CorrectNamesWithMap(submittedPatientInfo);

            // validate mapped record
            if (TryFindMatchedPatient(mappedPatientSearch, out ehrPatient))
            {
                _patientNameCorrectionLogService.Add(originalPatientInfo, mappedPatientSearch, "Usage of internal mapping dictionary.");

                result.PatientRecord = ehrPatient;
                return result;
            }

            // set mapped value as default for further processing
            submittedPatientInfo = mappedPatientSearch;

            // 1. get list of strategies
            var strategies = _correctionStrategyFactory.Create();

            // 2. check all strategies
            foreach (var strategy in strategies)
            {
                var patientCandidates = strategy.Apply(submittedPatientInfo).ToList();

                foreach (var candidate in patientCandidates)
                {

                    if (TryFindMatchedPatient(candidate, out ehrPatient))
                    {
                        // return value from EHR
                        result.PatientRecord = ehrPatient;

                        _patientNameCorrectionLogService.Add(
                            originalPatientInfo,
                            candidate,
                            "Strategy: " + string.Join(" AND THEN ", strategy.GetModificationsLogDescription())
                            );

                        result.CorrectionsLog.AddRange(strategy.GetModificationsLogDescription());


                        return result; //when match found, stop search and return result.
                    }
                }
            }

            return result; // when match not found, return empty result
        }

        protected bool TryFindMatchedPatient(PatientSearch patientSearch, out Patient ehrPatient)
        {
            ehrPatient = null;

            // step 1: check against ScreenDox database
            var dbLookupResult = _patientInfoMatchService.GetBestMatch(patientSearch);
            if (dbLookupResult != null)
            {
                // match found
                _logger.InfoFormat("[PatientValidationService] Patient record found in ScreenDox database. Patient: ID: {0}, Name: {1}. DOB: {2}.",
                    dbLookupResult.ID,
                    dbLookupResult.FullName().AsMaskedFullName(),
                    dbLookupResult.DateOfBirth);

                ehrPatient = dbLookupResult;

                return true;
            }

            // step 2: Continue search in EHR database
            var patientLookupResult = _patientService.GetMatchedPatients(patientSearch.ToPatient(), 0, 100)
                .FindBestMatch();

            if (patientLookupResult.BestResult == null)
            {
                string reason = "[PatientValidationService] Patient record not found. Patient: {0}, DOB: {1}. All Options: [{2}]".FormatWith(
                    patientSearch.FullName().AsMaskedFullName(),
                    patientSearch.DateOfBirth,
                    string.Join("|| ", patientLookupResult.AllResuls?.Select(x => "{0}, {1}, {2}".FormatWith(x.ID, x.FullName, x.EHR)))
                    );

                _logger.Info(reason);

                return false;
            }

            ehrPatient = patientLookupResult.BestResult;
            _logger.InfoFormat("[PatientValidationService] Patient record found in EHR database. Patient: {0}. DOB: {1}.",
                   ehrPatient.FullName().AsMaskedFullName(),
                   ehrPatient.DateOfBirth);



            ehrPatient.CapitalizeName();

            return true;

        }
    }
}
