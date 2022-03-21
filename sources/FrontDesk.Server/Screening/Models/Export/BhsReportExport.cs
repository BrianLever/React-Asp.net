using FrontDesk.Common.Bhservice;
using FrontDesk.Common.Bhservice.Export;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontDesk.Server.Screening.Models.Export
{
    public class BhsReportExport
    {
        public List<BhsVisitExtendedWithIdentity> Visits { get; set; }
        public List<BhsFollowUpExtendedWithIdentity> FollowUps { get; set; }
        public List<BhsDemographics> Demographics { get; set; }
        public DataTable Screenings { get; set; }
        public DataTable DrugsOfChoice { get; set; }

        public DataTable Combined { get; set; }

        public BhsReportExport()
        {
            Visits = new List<BhsVisitExtendedWithIdentity>();
            FollowUps = new List<BhsFollowUpExtendedWithIdentity>();
            Demographics = new List<BhsDemographics>();
            Screenings = new DataTable();
            DrugsOfChoice = new DataTable();
            Combined = new DataTable();
        }
    }
}
