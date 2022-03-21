using Common.Logging;

using FrontDesk;
using FrontDesk.Server.Data;

using ScreenDox.Server.Data;
using ScreenDox.Server.Models;
using ScreenDox.Server.Models.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenDox.Server.Services
{
    public interface IPatientService
    {
        List<PatientSearchInfoMatch> FindPatient(PatientSearchFilter filter);
    }

    public class PatientService : IPatientService
    {
        private readonly ILog _logger = LogManager.GetLogger<PatientService>();
        private readonly IScreensRepository _repository;

        public PatientService(IScreensRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// Find patient in screendox
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<PatientSearchInfoMatch> FindPatient(PatientSearchFilter filter)
        {
            if (filter is null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            var patientInfo = new ScreeningResult
            {
                ID = 0,
                LastName = filter.LastName,
                FirstName = filter.FirstName,
                MiddleName = filter.MiddleName,
                Birthday = filter.Birthday
            };

            var results = _repository.SearchPatient(filter);

            foreach(var x in results)
            {
                x.SetNotMatchesFields(patientInfo);
            }

            return results;
        }



    }
}
