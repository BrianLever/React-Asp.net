using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RPMS.Common.GlobalConfiguration
{
    public static class GlobalSettingsDescriptor
    {
        public static string ExternalEhrSystemKey = "EHRSystem";

        public static string CareBridgeCredentials = "CareBridgeAPICredentials";
    }


    public static class ExternalEhrSystemDescriptor
    {
        public static string EhrRpms = "RPMS";
        public static string NextGen = "NEXTGEN";
        public static string None = "NONE";
    }
}