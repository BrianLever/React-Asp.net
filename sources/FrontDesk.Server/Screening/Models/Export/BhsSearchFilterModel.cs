using FrontDesk.Server.Screening.Services;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace FrontDesk.Server.Printouts.Bhs
{
    public struct BhsExportFilterModel
    {
        public int? LocationId;
        public DateTime? StartDate;
        public DateTime? EndDate;

        public bool IncludeScreenings;
        public bool IncludeVisits;
        public bool IncludeDemographics;
        public bool IncludeFollowUps;
        public bool IncludeDrugsOfChoice;
        public bool IncludeCombined;

        public bool UniquePatientMode;

        public string IncludedReports
        {
            get
            {
                var items = new List<string>(3);
                if (IncludeFollowUps)
                {
                    items.Add("Demographics");
                }
                if (IncludeVisits)
                {
                    items.Add("Visit");
                }
                if (IncludeDemographics)
                {
                    items.Add("Follow-Up");
                }
                if (IncludeScreenings)
                {
                    items.Add("Screenings");
                }
                if(IncludeDrugsOfChoice)
                {
                    items.Add("Drug Use Results");
                }
                if (IncludeCombined)
                {
                    items.Add("Combined");
                }
                return string.Join<string>(", ", items);
            }
        }

        
    }
}
