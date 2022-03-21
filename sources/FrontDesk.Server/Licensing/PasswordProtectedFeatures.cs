using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrontDesk.Common.Configuration;
using System.Globalization;

namespace FrontDesk.Server.Licensing
{
    public static class PasswordProtectedFeaturesProvider
    {
        private const string Dast10FeaturePassword = "DAST-10 tool is permitted";

        public static bool IsDast10Enabled
        {
            get
            {
                var password = AppSettingsProxy.GetStringValue("DAST10Password", "");
                return String.Compare(Dast10FeaturePassword, password, false, CultureInfo.InvariantCulture) == 0;

            }
        }
    }
}
