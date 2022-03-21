using FluentValidation;

using ScreenDox.Server.Api.Infrastructure;
using ScreenDox.Server.Api.Infrastructure.RestResults;

using System.Web.Http;

namespace ScreenDox.Server.Api.Controllers
{
    public abstract class ApiControllerWithValidationBase<TRequestModel>: ApiController
        where TRequestModel : class
    {

        protected abstract AbstractValidator<TRequestModel> GetValidator();

        /// <summary>
        /// Validate request model
        /// </summary>
        /// <param name="inputModel"></param>
        protected virtual void Validate(TRequestModel inputModel)
        {
            var result = GetValidator().Validate(inputModel);

            if (result.IsValid) return;

            ResponseDataFactory.ThrowInvalidModelMessage(result.Errors);
        }

        /// <summary>
        /// Generates Accepted (HTTP 202) response with the content. Used for updating collections and application settings
        /// </summary>
        /// <typeparam name="T">Content Type</typeparam>
        /// <param name="content">Updated content</param>
        /// <returns></returns>
        protected IHttpActionResult AcceptedContentResult<T>(T content)
        {
            return new AcceptedContentResult<T>(content, this);
        }
    }
}