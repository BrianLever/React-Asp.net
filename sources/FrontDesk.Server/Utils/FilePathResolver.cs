using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FrontDesk.Server.Utils
{
    public static class FilePathResolver
    {
        public static string ResolveFilePath(string relativeFilePath)
        {
            if(string.IsNullOrEmpty(relativeFilePath))
            {
                throw new ArgumentNullException(relativeFilePath);
            }

            if (System.Web.Hosting.HostingEnvironment.IsHosted)
            {
                return System.Web.Hosting.HostingEnvironment.MapPath(relativeFilePath);
            }

            relativeFilePath = relativeFilePath.Replace("~", ".");

            return Path.GetFullPath(relativeFilePath);
        }
    }
}
