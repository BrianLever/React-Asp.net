using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Common.Logging;
using FrontDesk.Common.Configuration;
using Newtonsoft.Json;
using RPMS.Data.CareBridge.Infrastructure;

namespace RPMS.Data.CareBridge
{
    public class RestApiRepository
    {
        private readonly IApiCredentialsService _credentialsService;
        private readonly IHttpService _httpService;
        private readonly ILog _logger = LogManager.GetLogger<RestApiRepository>();

        protected string BaseApiUrl { get; set; }
        protected string ApiResourceName { get; set; }
        protected string _credentials;


        protected virtual string BaseApiUrlSettingPropertyName => "CareBridgeApiUrl";

        public RestApiRepository(IApiCredentialsService credentialsService,
            IHttpService httpService,
            string baseApiUrl,
            string apiResourceName)
            : this(credentialsService, httpService, apiResourceName)
        {
            BaseApiUrl = baseApiUrl;
        }

        public RestApiRepository(IApiCredentialsService credentialsService, IHttpService httpService, string apiResourceName)
        {
            if (string.IsNullOrEmpty(apiResourceName))
            {
                throw new ArgumentException("message", nameof(apiResourceName));
            }

            BaseApiUrl = AppSettingsProxy.GetStringValue(BaseApiUrlSettingPropertyName, "");
            _credentialsService = credentialsService ?? throw new ArgumentNullException(nameof(credentialsService));
            _httpService = httpService ?? throw new ArgumentNullException(nameof(httpService));
            ApiResourceName = apiResourceName;


            _credentials = _credentialsService.GetCredentials().ToBase64();
        }

        public RestApiRepository(IApiCredentialsService credentialsService, string apiResourceName)
            : this(credentialsService, new Infrastructure.HttpService(), apiResourceName)
        {

        }

        protected AuthenticationHeaderValue GetAuthorizationHeader()
        {
            return new AuthenticationHeaderValue("Basic", _credentials);
        }

        private HttpService GetClient()
        {
            var client = _httpService.Create();

            client.DefaultRequestHeaders.Authorization = GetAuthorizationHeader();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            client.BaseAddress = new Uri(BaseApiUrl);


            return client;
        }

        protected TResponse GetPost<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : class, new()
        {
            return GetPost<TRequest, TResponse>(request, ApiResourceName);
        }



        protected TResponse GetPost<TRequest, TResponse>(TRequest request, string apiResourceName)
            where TRequest : class
            where TResponse : class, new()
        {
            TResponse result = null;

            var requestPayload = JsonConvert.SerializeObject(request);

            var content = new StringContent(requestPayload, Encoding.UTF8, "application/json");

            try
            {

                using (var httpClient = GetClient())
                {
                    TraceInfo("API Call. POST {0}{1}. Request payload: {2}", httpClient.BaseAddress, apiResourceName, requestPayload);

                    var response = httpClient.Post(apiResourceName, content);
                    if (response.IsSuccessStatusCode)
                    {

                        TraceInfo("API call succeeded. Status Code: {2} POST {0}{1}.", httpClient.BaseAddress, apiResourceName, response.StatusCode);

                        var responseMessage = response.Content.ReadAsStringAsync().Result;

                        TraceInfo("API Call. POST {0}{1}. Response payload:{2}", httpClient.BaseAddress, apiResourceName, responseMessage);
                        

                        result = JsonConvert.DeserializeObject<TResponse>(responseMessage)?? new TResponse();

                    }
                    else if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        //empty response
                        _logger.InfoFormat("API Call. POST. Not found. POST {0}{1}.", httpClient.BaseAddress, apiResourceName);
                    }
                    else
                    {
                        _logger.WarnFormat("POST. Failed request to API POST {0}{1}. Status code: {2}.", httpClient.BaseAddress, apiResourceName, response.StatusCode);
                        //TODO: send exception
                        throw new RestApiStatusCodeException(response);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to send REST request to {ApiResourceName}.", ex);
                throw;
            }
            finally
            {

            }

            return result;
        }


        private void TraceInfo(string format, params object[] args)
        {
            if (_logger.IsTraceEnabled)
            {
                _logger.TraceFormat(format, args);
            }
        }
    }
}
