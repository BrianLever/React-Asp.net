using FrontDesk;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenDox.Server.Models.ViewModels
{
    public class UpdatePatientRequest : IScreeningPatientIdentityWithAddress
    {
        public string City { get; set; }
        public string FullName { get; }
        public string Phone { get; set; }
        public string StateID { get; set; }
        public string StateName { get; set; }
        public string StreetAddress { get; set; }
        public string ZipCode { get; set; }
        public DateTime Birthday { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
    }
}
