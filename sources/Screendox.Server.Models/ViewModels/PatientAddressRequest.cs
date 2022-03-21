using ScreenDox.Server.Models.ViewModels.Validators;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenDox.Server.Models.ViewModels
{
    public class PatientAddressRequest : IRequestModelWithValidation<PatientAddressRequestValidator>
    {
        public int PatientID { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string StateID { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }

    }
}
