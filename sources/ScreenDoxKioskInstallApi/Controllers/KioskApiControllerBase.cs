using ScreenDox.Server.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ScreenDoxKioskInstallApi.Controllers
{
    public abstract class KioskApiControllerBase : ApiController
    {
        public string KioskKey { get; set; }

        public short KioskID
        {
            get
            {
                return Kiosk.GetKioskIDFromString(KioskKey);
            }
        }

        public HttpResponseMessage Options()
        {
            return new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
        }
    }
}