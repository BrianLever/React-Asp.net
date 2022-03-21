
using FrontDesk.Common.Screening.Bhs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Server.Screening.Models
{
    public class BhsFollowUpOutcomesViewModel
    {
        public List<BhsIndicatorReportByAgeItemViewModel> Items { get; set; }

        public BhsFollowUpOutcomesViewModel()
        {
            Items = new List<BhsIndicatorReportByAgeItemViewModel>();

        }

        #region Section Headers and data

        private BhsBHIReportSectionByAgeViewModel GetReportSectionModel(string header, string categoryId)
        {
            var model = new BhsBHIReportSectionByAgeViewModel
            {
                Header = header,
                Items = Items.FindAll(s => s.CategoryID == categoryId),
            };

            return model;
        }

        public BhsBHIReportSectionByAgeViewModel NewVisitReferralRecommendation
        {
            get
            {
                return GetReportSectionModel(BhsFollowUpReportDescriptor.NewVisitReferralRecommendation, nameof(BhsFollowUpReportDescriptor.NewVisitReferralRecommendation));
            }
        }

        public BhsBHIReportSectionByAgeViewModel NewVisitReferralRecommendationAccepted
        {
            get
            {
                return GetReportSectionModel(BhsFollowUpReportDescriptor.NewVisitReferralRecommendationAccepted, nameof(BhsFollowUpReportDescriptor.NewVisitReferralRecommendationAccepted));
            }
        }
        public BhsBHIReportSectionByAgeViewModel ReasonNewVisitReferralRecommendationNotAccepted
        {
            get
            {
                return GetReportSectionModel(BhsFollowUpReportDescriptor.ReasonNewVisitReferralRecommendationNotAccepted, nameof(BhsFollowUpReportDescriptor.ReasonNewVisitReferralRecommendationNotAccepted));
            }
        }
        public BhsBHIReportSectionByAgeViewModel Discharged
        {
            get
            {
                return GetReportSectionModel(BhsFollowUpReportDescriptor.Discharged, nameof(BhsFollowUpReportDescriptor.Discharged));
            }
        }

        public BhsBHIReportSectionByAgeViewModel FollowUps
        {
            get
            {
                return GetReportSectionModel(BhsFollowUpReportDescriptor.FollowUp, nameof(BhsFollowUpReportDescriptor.FollowUp));
            }
        }

        public BhsBHIReportSectionByAgeViewModel FollowUpContactOutcome
        {
            get
            {
                return GetReportSectionModel(BhsFollowUpReportDescriptor.FollowUpContactOutcome, nameof(BhsFollowUpReportDescriptor.FollowUpContactOutcome));
            }
        }

        public BhsBHIReportSectionByAgeViewModel PatientAttendedVisit
        {
            get
            {
                return GetReportSectionModel(BhsFollowUpReportDescriptor.PatientAttendedVisit, nameof(BhsFollowUpReportDescriptor.PatientAttendedVisit));
            }
        }
    }


    #endregion

}
