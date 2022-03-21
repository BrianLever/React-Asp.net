using FrontDesk.Server.Descriptors;
using FrontDesk.Server.Screening.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Server.Screening.Models
{
    public class BhsFollowUpListItemPrintoutModel : IPatientId
    {
        public string PatientName { get; set; }
        public DateTime Birthday { get; set; }

        public List<BhsFollowUpViewModel> ReportItems { get; set; }


        //readonly dynamic properties
        public DateTimeOffset LastFollowUpDate
        {
            get
            {
                return ReportItems.Max(x => x.FollowUpDate);
            }
        }

        public DateTimeOffset? LastCompletionDate
        {
            get
            {
                return ReportItems.Any(x => x.CompletedDate.HasValue) ?
                        ReportItems.Max(x => x.CompletedDate) : (DateTimeOffset?)null;
            }
        }
        public DateTimeOffset? LastVisitDate
        {
            get
            {
                return ReportItems.Max(x => x.VisitDate);
            }
        }

        public BhsFollowUpListItemPrintoutModel()
        {
            ReportItems = new List<BhsFollowUpViewModel>();
        }

        public override int GetHashCode()
        {
            return this.GetUniquePatientKey().GetHashCode();
        }

    }
}