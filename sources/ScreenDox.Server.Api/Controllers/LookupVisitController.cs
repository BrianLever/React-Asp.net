using CacheCow.Server.WebApi;

using FrontDesk.Common;
using FrontDesk.Server.Data.BhsVisits;

using System;
using System.Collections.Generic;
using System.Web.Http;

namespace ScreenDox.Server.Api.Controllers
{
    /// <summary>
    /// Lookup data sets for UI bindings
    /// </summary>
    public class LookupVisitController : ApiController
    {
        private readonly ILookupListsDataSource _dataSource;
        
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="dataSource"></param>
        public LookupVisitController(ILookupListsDataSource dataSource)
        {
            _dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
        }

        /// <summary>
        /// Get lookup values for list binding.
        /// </summary>
        /// <returns>Lookup list.</returns>
        [HttpCache(DefaultExpirySeconds = 0)] // no cache
        [Route("api/lookupvisit")]
        [HttpGet, HttpPost]
        public string[] GetSupportedList()
        {
           return LookupListsDataSource.VisitListNameDescriptor.GetSupportedLists();
        }

        /// <summary>
        /// Get lookup values for list binding.
        /// </summary>
        /// <returns>Lookup list.</returns>
        [HttpCache(DefaultExpirySeconds = 300)] // cache 5 mins
        [Route("api/lookupvisit/{listName}")]
        [HttpGet, HttpPost]
        public List<LookupValue> GetList(string listName)
        {
            return _dataSource.Get(listName).GetList();
        }
    }
}
