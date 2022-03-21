using ScreenDoxKioskInstallApi.Filters;
using ScreenDoxKioskInstallApi.Models;
using ScreenDoxKioskInstallApi.Services;

using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace ScreenDoxKioskInstallApi.Controllers
{
    [KioskAuthenticationFilter]
    public class InstallationPackageController : KioskApiControllerBase
    {
        private readonly InstallationPackageService _service;

        public InstallationPackageController(InstallationPackageService service)
        {
            _service = service ?? throw new System.ArgumentNullException(nameof(service));
        }

        [HttpGet]
        [Route("api/version")]
        public InstallationPackageInfo Get()
        {
            var currentPackage = _service.Get(this.KioskKey);

            return currentPackage;
        }

        [HttpGet]
        [Route("api/version/{version}")]
        public HttpResponseMessage GetFile(string version)
        {
            InstallationPackageFile file = _service.GetFile(version);

            if (file == null)
            {
                return GetNotFound();
            }

            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(file.Content)
            };

            result.Content.Headers.ContentDisposition =
                new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                {
                    FileName = file.FileName
                };
            result.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/octet-stream");

            return result;
        }

        protected HttpResponseMessage GetNotFound(string message = "File not found")
        {
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.NotFound);
            result.Content = new StringContent(message); // Put the message in the response body (text/plain content).

            return result;

        }





    }
}
