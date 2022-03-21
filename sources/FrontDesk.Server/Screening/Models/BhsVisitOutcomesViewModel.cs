
using FrontDesk.Common.Screening.Bhs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Server.Screening.Models
{
    public class BhsVisitOutcomesViewModel
    {
        public List<BhsIndicatorReportByAgeItemViewModel> Items { get; set; }

        public BhsVisitOutcomesViewModel()
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


        public BhsBHIReportSectionByAgeViewModel TreatmentAction1
        {
            get
            {
                return GetReportSectionModel(BhsVisitReportDescriptor.TreatmentAction1, nameof(BhsVisitReportDescriptor.TreatmentAction1));
            }
        }
        public BhsBHIReportSectionByAgeViewModel TreatmentAction2
        {
            get
            {
                return GetReportSectionModel(BhsVisitReportDescriptor.TreatmentAction2, nameof(BhsVisitReportDescriptor.TreatmentAction2));
            }
        }
        public BhsBHIReportSectionByAgeViewModel TreatmentAction3
        {
            get
            {
                return GetReportSectionModel(BhsVisitReportDescriptor.TreatmentAction3, nameof(BhsVisitReportDescriptor.TreatmentAction3));
            }
        }

        public BhsBHIReportSectionByAgeViewModel TreatmentAction4
        {
            get
            {
                return GetReportSectionModel(BhsVisitReportDescriptor.TreatmentAction4, nameof(BhsVisitReportDescriptor.TreatmentAction4));
            }
        }

        public BhsBHIReportSectionByAgeViewModel TreatmentAction5
        {
            get
            {
                return GetReportSectionModel(BhsVisitReportDescriptor.TreatmentAction5, nameof(BhsVisitReportDescriptor.TreatmentAction5));
            }
        }

        public BhsBHIReportSectionByAgeViewModel NewVisitReferralRecommendation
        {
            get
            {
                return GetReportSectionModel(BhsVisitReportDescriptor.NewVisitReferralRecommendation, nameof(BhsVisitReportDescriptor.NewVisitReferralRecommendation));
            }
        }

        public BhsBHIReportSectionByAgeViewModel NewVisitReferralRecommendationAccepted
        {
            get
            {
                return GetReportSectionModel(BhsVisitReportDescriptor.NewVisitReferralRecommendationAccepted, nameof(BhsVisitReportDescriptor.NewVisitReferralRecommendationAccepted));
            }
        }
        public BhsBHIReportSectionByAgeViewModel ReasonNewVisitReferralRecommendationNotAccepted
        {
            get
            {
                return GetReportSectionModel(BhsVisitReportDescriptor.ReasonNewVisitReferralRecommendationNotAccepted, nameof(BhsVisitReportDescriptor.ReasonNewVisitReferralRecommendationNotAccepted));
            }
        }
        public BhsBHIReportSectionByAgeViewModel Discharged
        {
            get
            {
                return GetReportSectionModel(BhsVisitReportDescriptor.Discharged, nameof(BhsVisitReportDescriptor.Discharged));
            }
        }

        public BhsBHIReportSectionByAgeViewModel FollowUps
        {
            get
            {
                return GetReportSectionModel(BhsVisitReportDescriptor.FollowUp, nameof(BhsVisitReportDescriptor.FollowUp));
            }
        }
    }


    #endregion

}
