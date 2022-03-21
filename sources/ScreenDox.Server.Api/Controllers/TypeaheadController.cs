using CacheCow.Server.WebApi;

using FrontDesk.Server.Data.BhsVisits;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ScreenDox.Server.Api.Controllers
{

    /// <summary>
    /// Typeahead lists resource
    /// </summary>
    public class TypeaheadController : ApiController
    {
        private readonly ITypeaheadDataSourceFactory _typeaheadDataSourceFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="typeaheadDataSourceFactory"></param>
        public TypeaheadController(ITypeaheadDataSourceFactory typeaheadDataSourceFactory)
        {
            _typeaheadDataSourceFactory = typeaheadDataSourceFactory ?? throw new ArgumentNullException(nameof(typeaheadDataSourceFactory));
        }


        /// <summary>
        /// Get items for the specific list that contains string in q parameter
        /// Supported lists:
        /// - tribe
        /// - county
        /// </summary>
        /// <param name="listName">List name</param>
        /// <param name="q">Text for type ahead.</param>
        /// <returns></returns>
        [HttpCache(DefaultExpirySeconds = 3600)] // 1 hours cache
        [HttpGet]
        [Route("api/typeahead/{listName}")]
        public List<string> GetList([FromUri]string listName, [FromUri] string q)
        {
            listName = (listName ?? string.Empty).Trim();
            var filter = (q ?? string.Empty).Trim();

            List<string> result = new List<string>();

            if (listName == "tribe")
            {
                result = _typeaheadDataSourceFactory.GetTribe(filter);
            }
            else if (listName == "county")
            {
                result = _typeaheadDataSourceFactory.GetCounty(filter);
            }

            return result;
        }
    }
}
