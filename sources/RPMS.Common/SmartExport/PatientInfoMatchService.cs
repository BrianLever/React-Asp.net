using Common.Logging;

using FrontDesk;

using RPMS.Common.Models;

using ScreenDox.EHR.Common.SmartExport.Repository;

using System;
using System.Collections.Generic;
using System.Linq;

namespace ScreenDox.EHR.Common.SmartExport
{
    /// <summary>
    /// Validate patient data against ScreenDox exported data
    /// </summary>
    public class PatientInfoMatchService : IPatientInfoMatchService
    {
        private readonly IPatientMatchRepository _repository;
        private readonly ILog _logger = LogManager.GetLogger<PatientInfoMatchService>();

        private Lazy<Dictionary<string, string>> _patientNameCorrectionMap = null;

        public PatientInfoMatchService(IPatientMatchRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));

            _patientNameCorrectionMap =
                new Lazy<Dictionary<string, string>>(() => _repository.GetNameCorrectionMap(), false);
        }

        public Patient GetBestMatch(PatientSearch patientSearch)
        {
            var dbResults = _repository.ValidatePatientInfo(patientSearch);

            var result = dbResults.SortByBestMatch();

            if (result.BestResult == null)
            {
                _logger.InfoFormat("[PatientInfoMatchService] No matches found in exported records. Original name: {0}, DOB: {1}",
                    patientSearch.FullName().AsMaskedFullName(), patientSearch.DateOfBirth);

                return null;
            }
            else if (dbResults.Count > 0)
            {
                _logger.InfoFormat("[PatientInfoMatchService] Several matches found. Original name: {0}, DOB: {1}. Best Match: {2}:{3}, All names: [{4}]",
                    patientSearch.FullName().AsMaskedFullName(),
                    patientSearch.DateOfBirth,
                    result.BestResult.ID,
                    result.BestResult.FullName().AsMaskedFullName(),
                    String.Join("||", result.AllResuls.Select(x => "ID: {0} Name: {1}".FormatWith(x.ID, x.FullName().AsMaskedFullName())))
                    );
            }
            return result.BestResult;
        }


        private void InitializeNameCorrectionMap()
        {

        }
        /// <summary>
        /// Try to replace patient names using internal map
        /// </summary>
        /// <param name="source">Patient name</param>
        /// <returns>Return corrected cloned object</returns>
        public PatientSearch CorrectNamesWithMap(PatientSearch source)
        {
            var destination = source.Clone();

            var map = _patientNameCorrectionMap.Value;

            destination.LastName = map.Map(source.LastName);
            destination.FirstName = map.Map(source.FirstName);
            destination.MiddleName = map.Map(source.MiddleName);

            if (string.CompareOrdinal(source.FullName(), destination.FullName()) != 0)
            {
                _logger.InfoFormat("[PatientInfoMatchService] Patient name was corrected with map. Source: {0}. Destination: {1}",
                    source.FullName().AsMaskedFullName(),
                    destination.FullName().AsMaskedFullName()
                    );
            }
            return destination;
        }

    }


    public static class PatientNameMapExtensions
    {
        public static string Map(this Dictionary<string, string> patientNameMap, string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return string.Empty;

            var result = patientNameMap.TryGetValue(name, out string mappedName);

            return result ? mappedName : name;
        }
    }

}
