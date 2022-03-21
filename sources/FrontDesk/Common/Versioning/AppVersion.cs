using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Common.Versioning
{
    public class AppVersion
    {
        public static string CurrentAppVersion
        {
            get
            {
                string version = "0.0.0.0";

                var assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName();
                version = assemblyName.Version.ToString();

                return version;
            }
        }
    }
}
