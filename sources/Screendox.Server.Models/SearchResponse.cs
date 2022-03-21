using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenDox.Server.Models
{
    /// <summary>
    /// Search result that includes found items and total number of rows that match given search criteria
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SearchResponse<T>
    {
        /// <summary>
        /// Results
        /// </summary>
        public List<T> Items { get; set; } = new List<T>();

        /// <summary>
        /// Total number of rows
        /// </summary>
        public long TotalCount { get; set; }
    }
}
