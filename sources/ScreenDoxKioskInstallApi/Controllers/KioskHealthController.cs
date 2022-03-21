using FrontDesk.Common.InfrastructureServices;
using NSwag.Annotations;
using ScreenDox.Server.Common.Services;
using ScreenDoxKioskInstallApi.Filters;
using ScreenDoxKioskInstallApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ScreenDoxKioskInstallApi.Controllers
{
    [KioskAuthenticationFilter]
    public class KioskHealthController : KioskApiControllerBase
    {
        private readonly IKioskService _service;
        private readonly ITimeService _timeService;


        public KioskHealthController(IKioskService service, ITimeService timeService)
        {
            _service = service ?? throw new System.ArgumentNullException(nameof(service));
            _timeService = timeService ?? throw new ArgumentNullException(nameof(timeService));
        }

        [OpenApiOperation("GetTimeFromKioskLastActivity", "Get time interval since last kiosk activity")]
        [HttpGet]
        [Route("api/lastactivity")]
        public KioskLastActivity GetTimeInSecondsSinceLastActivity()
        {
            KioskLastActivity result = new KioskLastActivity();

            var lastActivity = _service.GetLastActivityDate(KioskID);
            var now = _timeService.GetDateTimeOffsetNow();

            lastActivity = lastActivity ?? now; // when first time installation

            result.LastActivityUtc = lastActivity.Value.UtcDateTime;

            if (!lastActivity.HasValue)
            {
                result.TimeSinceLastActivity = TimeSpan.MaxValue;
            }
            else
            {
                result.TimeSinceLastActivity = (now - lastActivity.Value);
            }

            return result;
        }
    }
}
