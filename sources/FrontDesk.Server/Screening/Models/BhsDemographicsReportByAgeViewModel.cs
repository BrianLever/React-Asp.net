using FrontDesk.Common.Screening.Bhs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Server.Screening.Models
{
    public class BhsDemographicsReportByAgeViewModel
    {
        public List<BhsIndicatorReportByAgeItemViewModel> Items { get; set; }

        public BhsDemographicsReportByAgeViewModel()
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


        public BhsBHIReportSectionByAgeViewModel RaceSection
        {
            get
            {
                return GetReportSectionModel(BhsDemographicsIndicatorDescriptor.Race, nameof(BhsDemographicsIndicatorDescriptor.Race));
            }
        }

        public BhsBHIReportSectionByAgeViewModel GenderSection
        {
            get
            {
                return GetReportSectionModel(BhsDemographicsIndicatorDescriptor.Gender, nameof(BhsDemographicsIndicatorDescriptor.Gender));
            }
        }

        public BhsBHIReportSectionByAgeViewModel SexualOrientationSection
        {
            get
            {
                return GetReportSectionModel(BhsDemographicsIndicatorDescriptor.SexualOrientation, nameof(BhsDemographicsIndicatorDescriptor.SexualOrientation));
            }
        }

        public BhsBHIReportSectionByAgeViewModel MaritalStatusSection
        {
            get
            {
                return GetReportSectionModel(BhsDemographicsIndicatorDescriptor.MaritalStatus, nameof(BhsDemographicsIndicatorDescriptor.MaritalStatus));
            }
        }
        public BhsBHIReportSectionByAgeViewModel EducationLevelSection
        {
            get
            {
                return GetReportSectionModel(BhsDemographicsIndicatorDescriptor.EducationLevel, nameof(BhsDemographicsIndicatorDescriptor.EducationLevel));
            }
        }
        public BhsBHIReportSectionByAgeViewModel LeavingOnOrOffReservationSection
        {
            get
            {
                return GetReportSectionModel(BhsDemographicsIndicatorDescriptor.LivingOnReservation, nameof(BhsDemographicsIndicatorDescriptor.LivingOnReservation));
            }
        }
        public BhsBHIReportSectionByAgeViewModel MilitaryExperienceSection
        {
            get
            {
                return GetReportSectionModel(BhsDemographicsIndicatorDescriptor.MilitaryExperience, nameof(BhsDemographicsIndicatorDescriptor.MilitaryExperience));
            }
        }


        #endregion

    }
}
