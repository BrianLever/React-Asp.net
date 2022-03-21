using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using RPMS.Common;
using RPMS.Common.Models;
using RPMS.Data.CareBridge.Dto;
using RPMS.Data.CareBridge.Infrastructure;

namespace RPMS.Data.CareBridge
{
    public class PatientRepository : RestApiRepository, IPatientRepository
    {

        public PatientRepository(IApiCredentialsService credentialsProvider)
           : base(credentialsProvider, "FindPatient")
        {

        }

        public PatientRepository(IApiCredentialsService credentialsProvider, IHttpService httpService, string baseApiUrl)
            : base(credentialsProvider, httpService, baseApiUrl, "FindPatient")
        {

        }


        private List<PatientRecord> GetPatients(PatientSearch searchPattern)
        {
            var request = new RequestWithParam<FindPatientRequest>
            {
                Value = new FindPatientRequest
                {
                    LastName = searchPattern.LastName,
                    Birthday = searchPattern.DateOfBirth
                }
            };

            var response = GetPost<RequestWithParam<FindPatientRequest>, FindPatientResponse>(request);

            return response?.Items ?? new List<PatientRecord>();
        }

        public List<Patient> GetMatchedPatients(Patient patient)
        {
            List<Patient> results = new List<Patient>();

            var items = GetPatients(patient);

            foreach (var item in items)
            {
                if (item.Birthday == patient.DateOfBirth)
                {
                    results.Add(Mapper.Map<Patient>(item));
                }
            }

            return results;
        }



        public int GetMatchedPatientsCount(Patient patient)
        {
            var items = GetPatients(patient);

            return items.Where(item => item.Birthday == patient.DateOfBirth).Count();
        }

        public string GetPatientName(PatientSearch patientSearch)
        {
            throw new NotSupportedException();
        }

        public Patient GetPatientRecord(PatientSearch patientSearch)
        {

            var request = new RequestWithParam<GetPatientRequest>
            {
                Value = new GetPatientRequest
                {
                    PatientId = patientSearch.ID
                }
            };

            var response = GetPost<RequestWithParam<GetPatientRequest>, GetPatientResponse>(request, "GetPatientRecord");

            if (response == null) return null;
            
            return Mapper.Map<Patient>(response.Value);

        }

        public int UpdatePatientRecordFields(IEnumerable<PatientRecordModification> modifications, int patientId, int visitId)
        {
            throw new NotImplementedException();
        }
    }
}
