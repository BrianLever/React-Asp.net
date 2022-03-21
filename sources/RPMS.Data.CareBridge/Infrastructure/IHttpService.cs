using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RPMS.Data.CareBridge.Infrastructure
{
    public interface IHttpService
    {
        HttpService Create();
    }

    /// <summary>
    /// Wrapper for HttpClient to mock the data in tests
    /// </summary>
    public class HttpService : HttpClient, IHttpService
    {
        public HttpService Create()
        {
            return new HttpService();
        }


        public virtual HttpResponseMessage  Post(string requestUri, HttpContent content)
        {
            return this.PostAsync(requestUri, content).Result;
        }
    }
}
