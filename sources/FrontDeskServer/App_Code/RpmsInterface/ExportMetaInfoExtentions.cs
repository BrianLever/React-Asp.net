using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RPMS.Common.Models;

namespace EhrInterface {

    public static class ExportMetaInfoExtentions
    {
        public static string GetExternalEhrSystemLabel(this ExportMetaInfo value)
        {
            if (value.IsRpmsMode)
            {
                return "EHR";
            }
            else if (value.IsNextGenMode)
            {
                return "NextGen";
            }
            return string.Empty;
        }

    }
}