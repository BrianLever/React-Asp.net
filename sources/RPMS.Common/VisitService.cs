using System;
using System.Collections.Generic;
using RPMS.Common.GlobalConfiguration;
using RPMS.Common.Models;

namespace RPMS.Common
{
    public class VisitService : IVisitService
    {
        private readonly IVisitRepository _repository;
        private readonly IGlobalSettingsService _globalSettingsService;

        public VisitService(IVisitRepository repository, IGlobalSettingsService globalSettingsService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _globalSettingsService = globalSettingsService ?? throw new ArgumentNullException(nameof(globalSettingsService));
        }

        #region IVisitService Members

        public virtual List<Visit> GetVisitsByPatient(PatientSearch patientSearch, int currentPageIndex, int rowsPerPage)
        {
            if (patientSearch == null)
            {
                throw new ArgumentNullException(nameof(patientSearch));
            }

            if (_globalSettingsService.IsRpmsMode)
            {
                //this is hack for rpms
                if (string.IsNullOrEmpty(patientSearch.LastName) || patientSearch.LastName.Contains("'"))
                {
                    //this is much slower query but can return visits for patient who has ' in the name
                    return _repository.GetVisitsByPatient(patientSearch.ID, currentPageIndex, rowsPerPage);
                }
            }
            return _repository.GetVisitsByPatient(patientSearch, currentPageIndex, rowsPerPage);
        }

        public virtual Visit GetVisitRecord(int visitId, PatientSearch patientSearch)
        {
            return _repository.GetVisitRecord(visitId, patientSearch);
        }

        public virtual int GetVisitsByPatientCount(PatientSearch patientSearch)
        {
            if (patientSearch == null)
            {
                throw new ArgumentNullException(nameof(patientSearch));
            }


            if (string.IsNullOrEmpty(patientSearch.LastName))
            {
                return _repository.GetVisitsByPatientCount(patientSearch.ID);
            }

            if (_globalSettingsService.IsRpmsMode && patientSearch.LastName.Contains("'"))
            {
                return _repository.GetVisitsByPatientCount(patientSearch.ID);
            }

            return _repository.GetVisitsByPatientCount(patientSearch);

        }

        #endregion


    }
}
