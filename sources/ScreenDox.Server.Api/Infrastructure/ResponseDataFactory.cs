using FluentValidation.Results;

using FrontDesk;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace ScreenDox.Server.Api.Infrastructure
{
    /// <summary>
    /// Contains helper methods working with HTTP reponses
    /// </summary>
    public static class ResponseDataFactory
    {
        /// <summary>
        /// Return Not Found status code
        /// </summary>
        /// <param name="message">Reason phrase.</param>
        public static void ThrowNotFound(string message)
        {
            throw new HttpResponseException(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = message
            });
        }

        /// <summary>
        /// Return Not Found status code
        /// </summary>
        public static void ThrowNotFound()
        {
            ThrowNotFound("Unable to find resource.");
        }

        /// <summary>
        /// Return Not Found status code
        /// </summary>
        public static void ThrowNotFound(long id)
        {
            ThrowNotFound("Unable to find resource. Resoure {0} may not exists.".FormatWith(id));
        }


        /// <summary>
        /// Returns text string as bad request HTTP response
        /// </summary>
        /// <param name="reasonPhrase">Response content</param>
        public static void ThrowBadRequestMessage(string reasonPhrase)
        {
            throw new HttpResponseException(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
                ReasonPhrase = reasonPhrase
            });
        }

        /// <summary>
        /// Unauthorized access
        /// </summary>
        /// <param name="message"></param>
        public static void Unauthorized(string message)
        {
            var result = new ValidationFailureResponse(message);

            throw new HttpResponseException(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Content = new StringContent(result.ToString()),
            });
        }

        /// <summary>
        /// Returns text string as bad request HTTP response
        /// </summary>
        /// <param name="reasonPhrase">Response content</param>
        /// <param name="validationMessages">Validatation messages that us returned as JSON object</param>
        public static void ThrowBadRequestMessage(string reasonPhrase, IEnumerable<string> validationMessages)
        {

            var result = new ValidationFailureResponse(validationMessages);

            var httpMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
                ReasonPhrase = reasonPhrase,
                Content = new StringContent(result.ToString()),
            };
            httpMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            throw new HttpResponseException(httpMessage);
        }

        /// <summary>
        /// Returns invalid model status
        /// </summary>
        /// <param name="validationMessages"></param>
        public static void ThrowBadRequestMessage(IEnumerable<string> validationMessages)
        {
            ThrowBadRequestMessage("Invalid model", validationMessages);
        }

        /// <summary>
        /// Returns text string as bad request HTTP response
        /// </summary>
        public static void ThrowInvalidModelMessage(IEnumerable<ValidationFailure> validationMessages)
        {
            if (validationMessages is null)
            {
                throw new ArgumentNullException(nameof(validationMessages));
            }

            ThrowBadRequestMessage("Invalid model", validationMessages.Select(x => x.ErrorMessage));
        }

        /// <summary>
        /// Returns exception text as validation error
        /// </summary>
        public static void ThrowBadRequestMessage(Exception ex)
        {
            ThrowBadRequestMessage("Invalid model", new[] { ex.Message });
        }

        /// <summary>
        /// Returns text string as invalid server error HTTP response
        /// </summary>
        /// <param name="errorMessage">Error message that us returned as JSON object</param>
        public static void ThrowInvalidOperationError(string errorMessage)
        {

            var result = new ValidationFailureResponse(new string[] { errorMessage });

            var httpMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError,
                ReasonPhrase = "Unknown server error",
                Content = new StringContent(result.ToString()),
            };
            httpMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            throw new HttpResponseException(httpMessage);
        }

        /// <summary>
        /// EHR Service not available error
        /// </summary>
        /// <param name="message"></param>
        public static void DependencyError(string message)
        {
            var result = new ValidationFailureResponse(message);

            throw new HttpResponseException(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.ServiceUnavailable,
                Content = new StringContent(result.ToString()),
            });
        }

    }
}