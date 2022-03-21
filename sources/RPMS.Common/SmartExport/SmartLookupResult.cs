using System.Collections.Generic;

namespace ScreenDox.EHR.Common.SmartExport
{
    public class SmartLookupResult<T> where T : class
    {
        public T BestResult { get; set; }

        public List<T> AllResuls { get; set; }
        /// <summary>
        /// Confidence coefficient. 1 - 100% confident on match, 0 - confident that they are not matched.
        /// </summary>
        public double Confidence { get; set; }
    }
}
