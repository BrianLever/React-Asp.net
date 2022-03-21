using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrontDesk.Configuration;
using FrontDesk.Server.Data;
using FrontDesk.Server.Screening.Mappers;
using FrontDesk.Server.Screening.Models;
using FrontDesk.Server.Data.BhsVisits;
using FrontDesk.Common.Screening.Bhs;
using System.Threading.Tasks;

namespace FrontDesk.Server.Screening.Services
{
    public class BhsDemographicsIndicatorReportService
    {
        private readonly IBhsDemographicsRepository _repository;

        public BhsDemographicsIndicatorReportService(IBhsDemographicsRepository repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }

            _repository = repository;
        }


        public BhsDemographicsIndicatorReportService() : this(new BhsDemographicsDb()) { }

        public BhsDemographicsReportByAgeViewModel GetDemographicsReportByAge(SimpleFilterModel filter, int[] ageGroups, bool uniquePatientsMode)
        {


            var db = _repository;
            var model = new BhsDemographicsReportByAgeViewModel();

            db.Connect();
            try
            {
                db.BeginTransaction();
                db.StartConnectionSharing();


                var entries = new[]
                {
                    nameof(BhsDemographicsIndicatorDescriptor.Race),
                    nameof(BhsDemographicsIndicatorDescriptor.Gender),
                    nameof(BhsDemographicsIndicatorDescriptor.SexualOrientation),
                    nameof(BhsDemographicsIndicatorDescriptor.MaritalStatus),
                    nameof(BhsDemographicsIndicatorDescriptor.EducationLevel),
                    nameof(BhsDemographicsIndicatorDescriptor.LivingOnReservation),
                    nameof(BhsDemographicsIndicatorDescriptor.MilitaryExperience)
                };

                foreach(var item in entries)
                {
                    model.Items.AddRange(
                    (uniquePatientsMode ?
                        db.GetDemographicsReportUniquePatientsByAge(item, filter.Location, filter.StartDate, filter.EndDate) :
                        db.GetDemographicsReportPatientsByAge(item, filter.Location, filter.StartDate, filter.EndDate))
                    .ToViewModel(ageGroups));

                }
                
                db.StopConnectionSharing();
                db.CommitTransaction();
            }
            catch (Exception ex)
            {
                db.StopConnectionSharing();
                db.RollbackTransaction();
                throw new Exception("Failed to create BHI report", ex);
            }
            finally
            {
                db.Disconnect();
            }


            return model;
        }

    }
}
