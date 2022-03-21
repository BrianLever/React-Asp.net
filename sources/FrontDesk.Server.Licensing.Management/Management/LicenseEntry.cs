using System;
using System.Data;
using System.Data.Common;
using FrontDesk.Common;
using System.Globalization;
using FrontDesk.Licensing;

namespace FrontDesk.Server.Licensing.Management
{
    public class LicenseEntry : License
    {
        public int? ClientID = null;

        public DateTimeOffset Issued;

        public string CompanyName;

        public bool IsAssignedToClient
        {
            get
            {
                return ClientID.HasValue;
            }
        }

        public bool IsActivated;

        public LicenseEntry()
        {
        }
      
        /// <summary>
        /// Recreate only License features from license key. LicenseID, ClientID will be unknown.
        /// </summary>
        /// <param name="bytes"></param>
        public LicenseEntry(string licenseKey) : base(licenseKey)
        {
        }


    }
}

