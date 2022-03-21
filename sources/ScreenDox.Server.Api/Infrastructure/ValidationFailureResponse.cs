using Newtonsoft.Json;

using System.Collections.Generic;

namespace ScreenDox.Server.Api.Infrastructure
{
    /// <summary>
    /// Response model with validation error messages
    /// </summary>
    public class ValidationFailureResponse
    {
        /// <summary>
        /// Collection of validations errors
        /// </summary>
        public List<string> Errors { get; set; } = new List<string>();

        /// <summary>
        /// Default constructor 
        /// </summary>
        public ValidationFailureResponse()
        {

        }

        /// <summary>
        /// Contructor that init errors collection
        /// </summary>
        /// <param name="errors"></param>
        public ValidationFailureResponse(IEnumerable<string> errors)
        {
            this.Errors.AddRange(errors);
        }

        public ValidationFailureResponse(string errorMesssage)
        {
            this.Errors.Add(errorMesssage);
        }

        /// <summary>
        /// Return json
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}