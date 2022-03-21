using FrontDesk.Server.Descriptors;
using FrontDesk.Server.Screening.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Server.Screening.Models
{
    public class BhsVisitListItemPrintoutModel: IPatientId
    {
        public string PatientName { get; set; }
        public DateTime Birthday { get; set; }

        public List<BhsVisitViewModel> ReportItems { get; set; }


        //readonly dynamic properties
        public DateTimeOffset LastScreeningDate
        {
            get
            {
                return ReportItems.Max(x => x.ScreeningDate);
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

        public BhsVisitListItemPrintoutModel()
        {
            ReportItems = new List<BhsVisitViewModel>();
        }

        public override int GetHashCode()
        {
            return this.GetUniquePatientKey().GetHashCode();
        }

    }
}