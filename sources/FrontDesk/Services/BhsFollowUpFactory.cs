using FrontDesk.Common.Bhservice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FrontDesk.Common.Extensions;
using FrontDesk.Common.Bhservice.Export;

namespace FrontDesk.Services
{
    public interface IBhsFollowUpFactory
    {
        BhsFollowUp Create(BhsVisit visit);
        BhsFollowUp Create(BhsFollowUp followUp);
        BhsFollowUp InitFromReader(IDataReader reader);
    }

    public class BhsFollowUpFactory : IBhsFollowUpFactory
    {
        public BhsFollowUp Create(BhsVisit visit)
        {
            if (visit == null || !visit.NewVisitDate.HasValue || !visit.FollowUpDate.HasValue) { return null; }

            var model = new BhsFollowUp
            {
                Visit = visit,
                Result = visit.Result,
                BhsVisitID = visit.ID,
                ScreeningResultID = visit.ScreeningResultID,
                ScheduledVisitDate = visit.NewVisitDate.Value,
                ScheduledFollowUpDate = visit.FollowUpDate.Value.Date,
                CreatedDate = DateTimeOffset.Now
            };

            return model;
        }


        /// <summary>
        /// Create new Follow Up from existing
        /// </summary>
        /// <param name="existingFollowUp"></param>
        /// <returns>Return new follow up visit or null, if NewVisitDate is null ir visit argument is null</returns>
        public BhsFollowUp Create(BhsFollowUp existingFollowUp)
        {
            if (existingFollowUp == null || !existingFollowUp.NewVisitDate.HasValue)
            {
                return null;
            }

            var model = new BhsFollowUp
            {
                Visit = existingFollowUp.Visit,
                Result = existingFollowUp.Result,
                BhsVisitID = existingFollowUp.BhsVisitID,
                ScreeningResultID = existingFollowUp.ScreeningResultID,
                ParentFollowUpID = existingFollowUp.ID,
                ScheduledVisitDate = existingFollowUp.NewVisitDate.Value, //
                CreatedDate = DateTimeOffset.Now,

            };

            return model;
        }

        public BhsFollowUp InitFromReader(IDataReader reader)
        {
            var model = new BhsFollowUp();
            return InitFromReader(reader, model);
        }


        protected BhsFollowUp InitFromReader(IDataReader reader, BhsFollowUp model)
        {

            model.ID = reader.Get<long>("ID");
            model.ScreeningResultID = reader.Get<long>("ScreeningResultID");
            model.ScheduledVisitDate = reader.Get<DateTimeOffset>("VisitDate");
            model.ScheduledFollowUpDate = reader.Get<DateTimeOffset>("FollowUpDate");
            model.BhsVisitID = reader.Get<long>("BhsVisitID");
            model.ParentFollowUpID = reader.GetNullable<long>("ParentFollowUpID");
            model.CreatedDate = reader.Get<DateTimeOffset>("CreatedDate");
            model.BhsStaffNameCompleted = reader.Get<string>("BhsStaffNameCompleted");
            model.CompleteDate = reader.GetNullable<DateTimeOffset>("CompleteDate");
            model.FollowUpContactDate = reader.GetNullable<DateTimeOffset>("FollowUpContactDate");
            model.Notes = reader.Get<string>("Notes");
            model.NewVisitDate = reader.GetNullable<DateTimeOffset>("NewVisitDate");
            model.FollowUpDate = reader.GetNullable<DateTimeOffset>("NewFollowUpDate");




            model.PatientAttendedVisit = new Common.LookupValue
            {
                Id = reader.Get("PatientAttendedVisitID", 0),
                Name = reader.Get<string>("PatientAttendedVisitName"),
            };

            model.FollowUpContactOutcome = new Common.LookupValue
            {
                Id = reader.Get("FollowUpContactOutcomeID", 0),
                Name = reader.Get<string>("FollowUpContactOutcomeName"),
            };

            model.NewVisitReferralRecommendation = new Common.LookupValueWithDescription
            {
                Id = reader.Get("NewVisitReferralRecommendationID", 0),
                Name = reader.Get<string>("NewVisitReferralRecommendationName"),
                Description = reader.Get<string>("NewVisitReferralRecommendationDescription")
            };

            model.NewVisitReferralRecommendationAccepted = new Common.LookupValue
            {
                Id = reader.Get("NewVisitReferralRecommendationAcceptedID", 0),
                Name = reader.Get<string>("NewVisitReferralRecommendationAcceptedName"),
            };

            model.ReasonNewVisitReferralRecommendationNotAccepted = new Common.LookupValue
            {
                Id = reader.Get("ReasonNewVisitReferralRecommendationNotAcceptedID", 0),
                Name = reader.Get<string>("ReasonNewVisitReferralRecommendationNotAcceptedName"),
            };

            model.Discharged = new Common.LookupValue
            {
                Id = reader.Get("DischargedID", 0),
                Name = reader.Get<string>("DischargedName"),
            };

            return model;
        }


        public BhsFollowUpExtendedWithIdentity InitExtendedModelFromReader(IDataReader reader)
        {
            var model = new BhsFollowUpExtendedWithIdentity();
            InitFromReader(reader, model);

            model.FirstName = reader.Get<string>("LastName");
            model.LastName = reader.Get<string>("LastName");
            model.MiddleName = reader.Get<string>("MiddleName");
            model.Birthday = reader.Get<DateTime>("Birthday");
            model.ExportedToHRN = reader.Get<string>("ExportedToHRN");
            model.DemographicsID = reader.Get<long>("DemographicsID");
            model.LocationName = reader.Get<string>("BranchLocationName");

            model.Visit = new BhsVisit
            {
                ID = reader.Get<long>("BHSVisitID"),
                ScreeningDate = reader.Get<DateTimeOffset>("ScreeningDate")
            };

            return model;

        }
    }
}
