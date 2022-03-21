using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using RPMS.Common;
using RPMS.Common.Models;
using RPMS.Data.CareBridge.Dto;

namespace RPMS.Data.CareBridge
{
    public class VisitRepository : RestApiRepository, IVisitRepository
    {

        public VisitRepository(IApiCredentialsService credentialsProvider)
            : base(credentialsProvider, "FindPatientVisit")
        {

        }

        public Visit GetVisitRecord(int visitId, PatientSearch patientSearch)
        {
            var request = new RequestWithParam<FindPatientVisitRequest>
            {
                Value = new FindPatientVisitRequest
                {
                    PatientId = patientSearch.ID,
                    PatientName = Mapper.Map<PatientName>(patientSearch)
                }
            };

            var response = GetPost<RequestWithParam<FindPatientVisitRequest>, FindPatientVisitResponse>(request);

            var visitRecord = response.Items.FirstOrDefault(x => x.VisitId == visitId);

            if (visitRecord == null) return null;

            return Mapper.Map<Visit>(visitRecord);
        }

        public List<Visit> GetVisitsByPatient(PatientSearch patientSearch, int startRow, int maxRows)
        {
            var request = new RequestWithParam<FindPatientVisitRequest>
            {
                Value = new FindPatientVisitRequest
                {   
                    PatientId =  patientSearch.ID,
                    PatientName = Mapper.Map<PatientName>(patientSearch)
                }
            };

            var response = GetPost<RequestWithParam<FindPatientVisitRequest>, FindPatientVisitResponse>(request);

            if(response == null ) // not found
            {
                return new List<Visit>();
            }

            return response.Items.Select(x => Mapper.Map<Visit>(x))
                .OrderByDescending(x => x.Date)
                .Skip(startRow)
                .Take(maxRows)
                .ToList();
        }

        public int GetVisitsByPatientCount(PatientSearch patientSearch)
        {
            var request = new RequestWithParam<FindPatientVisitRequest>
            {
                Value = new FindPatientVisitRequest
                {
                    PatientId = patientSearch.ID,
                    PatientName = Mapper.Map<PatientName>(patientSearch)
                }
            };

            var response = GetPost<RequestWithParam<FindPatientVisitRequest>, FindPatientVisitResponse>(request);

            return response?.Items?.Count ?? 0;
        }


        public int GetVisitsByPatientCount(int patientID)
        {
            throw new System.NotSupportedException();
        }

        public List<Visit> GetVisitsByPatient(int patientID, int startRow, int maxRows)
        {
            throw new System.NotSupportedException();
        }

       

        
    }
}