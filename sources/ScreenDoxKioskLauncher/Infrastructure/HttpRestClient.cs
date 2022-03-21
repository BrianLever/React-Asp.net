using Common.Logging;

using RestSharp;

using ScreenDoxKioskInstallApi.Models;

using System;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text;

namespace ScreenDoxKioskLauncher.Infrastructure
{
    /// <summary>
    /// Implementation of Rest API client
    /// </summary>
    public class HttpRestClient
    {
        private readonly IEnvironmentProvider _environmentProvider;
        private readonly ILog _logger = LogManager.GetLogger<HttpRestClient>();

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="environmentProvider"></param>
        public HttpRestClient(IEnvironmentProvider environmentProvider)
        {
            _environmentProvider = environmentProvider ?? throw new ArgumentNullException(nameof(environmentProvider));
        }

        protected TResult GetResult<TResult>(string resource)
           where TResult : class
        {
            var baseUrl = _environmentProvider.ScreenDoxInstallationServiceBaseUrl;

            _logger.DebugFormat("Calling REST api using GET method. Base url: {0}. Resource: {1}.",
                baseUrl,
                resource);

            var client = CreateRestClient();

            var request = new RestRequest(resource, Method.GET, DataFormat.Json);

            AddAuthenticationHeaders(request);

            var restResponse = client.Execute<TResult>(request);

            if (restResponse.IsSuccessful)
            {
                return restResponse.Data;
            }

            //handle error
            _logger.ErrorFormat("Failed to call REST api. Base url: {0}. Resource: {1}. Http Status: {2}. Message: {3}",
                restResponse.ErrorException,
                baseUrl,
                resource,
                restResponse.ResponseStatus,
                restResponse.ErrorMessage);

            return null;
        }

        protected FileContent GetFile(string resource)
        {
            FileContent result = new FileContent();

            var baseUrl = _environmentProvider.ScreenDoxInstallationServiceBaseUrl;

            _logger.DebugFormat("Downloading file. Base url: {0}. Resource: {1}.",
                baseUrl,
                resource);

            var client = CreateRestClient();

            var request = new RestRequest(resource, Method.GET);

            AddAuthenticationHeaders(request);

            IRestResponse restResponse = client.Execute(request);

            if (!restResponse.IsSuccessful)
            {
                //handle error
                _logger.ErrorFormat("Failed to call REST api. Base url: {0}. Resource: {1}. Http Status: {2}. Message: {3}",
                    restResponse.ErrorException,
                    baseUrl,
                    resource,
                    restResponse.ResponseStatus,
                    restResponse.ErrorMessage);

                return null;
            }

            var contentDispositionValue = restResponse.Headers.ToList()
                  .FirstOrDefault(x => string.Compare(x.Name, "content-disposition", true) == 0)?.Value?.ToString();

            if (!string.IsNullOrEmpty(contentDispositionValue))
            {
                result.FileName = new ContentDisposition(contentDispositionValue).FileName;
            }

            result.Content = restResponse.RawBytes;

            return result;

        }

        private IRestClient CreateRestClient()
        {
            var baseUrl =_environmentProvider.ScreenDoxInstallationServiceBaseUrl;
            var client  = new RestClient(baseUrl);
            client.AddHandler(NewtonsoftRestSharpJsonSerializer.SupportedContentType, () => NewtonsoftRestSharpJsonSerializer.Default);

            //client.Proxy = new WebProxy("127.0.0.1:8888", false, null);

            client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

            return client;
        }

        private void AddAuthenticationHeaders(IRestRequest request)
        {
            //add headers
            request.AddHeader("ScreenDox-KioskKey", _environmentProvider.KioskKey);
            request.AddHeader("Authorization", "Bearer " + Convert.ToBase64String(Encoding.UTF8.GetBytes(_environmentProvider.KioskSecret)));
        }
    }
}
