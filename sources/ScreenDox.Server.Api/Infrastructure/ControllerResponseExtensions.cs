using System.Collections.Generic;

namespace ScreenDox.Server.Api.Infrastructure
{
    /// <summary>
    /// Extentions to API conroller results
    /// </summary>
    public static class ControllerResponseExtensions
    {
        /// <summary>
        /// Returns Not Found if module is null
        /// </summary>
        /// <param name="model">Model expected not be null.</param>
        public static void ShouldNotBeNull(this object model)
        {
            if (model == null)
            {
                ResponseDataFactory.ThrowNotFound();
            }
        }

        /// <summary>
        /// Returns Not Found if module is null
        /// </summary>
        /// <param name="model">Model expected not be null.</param>
        /// <param name="errorMessage"></param>
        public static void ShouldNotBeNull(this object model, string errorMessage)
        {
            if (model == null)
            {
                ResponseDataFactory.ThrowNotFound();
            }
        }
        /// <summary>
        /// Returns Not Found if module is null and put id value into the phrase message
        /// </summary>

        public static void ShouldNotBeNull(this object model, int id)
        {
            if (model == null)
            {
                ResponseDataFactory.ThrowNotFound(id);
            }
        }
        /// <summary>
        /// Returns Not Found if module is null and put id value into the phrase message
        /// </summary>
        /// <param name="model"></param>
        /// <param name="id"></param>

        public static void ShouldNotBeNull(this object model, long id)
        {
            if (model == null)
            {
                ResponseDataFactory.ThrowNotFound(id);
            }
        }

        /// <summary>
        /// Returns Bad Request if number is default.
        /// </summary>
        /// <param name="value"></param>
        public static void ShouldNotBeDefault<T>(this T value)
        {
            if (EqualityComparer<T>.Default.Equals(value, default(T)))
            {
                ResponseDataFactory.ThrowBadRequestMessage("Parameter should not be a default value.");
            }
        }


        /// <summary>
        /// Returns Bad Request if value is false.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="message"></param>
        public static void ShouldBeTrue(this bool value, string message = "Invalid format")
        {
            if (value != true)
            {
                ResponseDataFactory.ThrowBadRequestMessage(message);
            }
        }

        /// <summary>
        /// Returns text string as bad request HTTP response
        /// </summary>
        /// <param name="message">Response content</param>
        public static void ThrowBadRequestMessage(this string message)
        {
            ResponseDataFactory.ThrowBadRequestMessage(message);
        }




    }
}