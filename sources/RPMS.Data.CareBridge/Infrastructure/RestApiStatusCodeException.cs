using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace RPMS.Data.CareBridge.Infrastructure
{
    public class RestApiStatusCodeException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public string ReasonPhrase { get; set; }
        public string Content { get; set; }

        public RestApiStatusCodeException()
        {

        }

        public RestApiStatusCodeException(HttpResponseMessage responseMessage)
        {
            if (responseMessage == null)
            {
                throw new ArgumentNullException(nameof(responseMessage));
            }

            StatusCode = responseMessage.StatusCode;
            ReasonPhrase = responseMessage.ReasonPhrase;
            Content = responseMessage.Content?.ReadAsStringAsync().Result;
        }
    }
}
