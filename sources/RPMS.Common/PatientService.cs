using System.Collections.Generic;
using RPMS.Common.Models;
using System;
using System.Linq;

namespace RPMS.Common
{
    public class PatientService : IPatientService
    {

        private readonly IPatientRepository _repository;

        public PatientService(IPatientRepository repository)
        {
            _repository = repository;
        }

        public virtual List<Patient> GetMatchedPatients(Patient searchPattern, int startRow, int maxRows)
        {
            if (searchPattern == null)
            {
                throw new ArgumentNullException("searchPattern");
            }

            var patients = _repository.GetMatchedPatients(searchPattern); ;


            foreach (var item in patients)
            {
                item.SetMatchRank(searchPattern);
            }
            patients.Sort();

            //paging
            IEnumerable<Patient> expr = patients;
            expr = expr.Skip(startRow);
            expr = expr.Take(maxRows);

            return expr.ToList();
        }




        public virtual int GetMatchedPatientsCount(Patient searchPattern)
        {
            return _repository.GetMatchedPatientsCount(searchPattern);
        }

        public virtual Patient GetPatientRecord(PatientSearch patientSearch)
        {
            return _repository.GetPatientRecord(patientSearch);
        }


        public virtual string GetPatientName(PatientSearch patientSearch)
        {
            return _repository.GetPatientName(patientSearch);
        }

        public Patient GetPatientRecord(int patientID)
        {
            return GetPatientRecord(new PatientSearch
            {
                ID = patientID
            });
        }

        public string GetPatientName(int patientID)
        {
            return GetPatientName(new PatientSearch
            {
                ID = patientID
            });
        }
    }
}
