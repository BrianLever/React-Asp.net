using FrontDesk.Server.Extensions;

using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace ScreenDox.Server.Api.Infrastructure.RestResults
{
    /// <summary>Represents an action result that returns an empty <see cref="HttpStatusCode.OK"/> response.</summary>
    public class AcceptedContentResult<T> : IHttpActionResult
    {
        private readonly T _content;
        private readonly HttpRequestMessage _request;

        /// <summary>
        /// Creates instance of AcceptedContentResult that returns HTTP 202 status with content
        /// </summary>
        /// <param name="content"></param>
        /// <param name="controller"></param>
        public AcceptedContentResult(T content, ApiController controller)
        {

            _content = content;
            _request = controller.Request;
        }

        /// <summary>Gets the content value to negotiate and format in the entity body.</summary>
        public T Content
        {
            get { return _content; }
        }

        /// <summary>
        /// Create response messate
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = _request.CreateResponse(HttpStatusCode.Accepted);

            response.Content = new StringContent(_content.AsJson());
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return Task.FromResult(response);
        }
    }
}
