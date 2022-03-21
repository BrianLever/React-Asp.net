using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Server.Screening.Models
{
    public class BhsFollowUpListItemDtoModel : BhsFollowUpViewModel, IPatientId
    {
        public string PatientName { get; set; }
        public DateTime Birthday { get; set; }
    }
}
