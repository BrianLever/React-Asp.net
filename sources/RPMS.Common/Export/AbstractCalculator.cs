using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrontDesk;

namespace RPMS.Common.Export
{
    public abstract class AbstractCalculator<T>
        where T: class
    {
        /// <summary>
        /// Extract sections from result required to calculate health factor 
        /// </summary>
        /// <param name="sectionResults">All screening results</param>
        /// <returns>Filtered sections</returns>
        public abstract IEnumerable<ScreeningSectionResult> FilterSupportedSections(IEnumerable<ScreeningSectionResult> sectionResults);


        public IList<T> Calculate(IEnumerable<ScreeningSectionResult> sectionResults)
        {
            if (sectionResults == null) return null;

            var sections = FilterSupportedSections(sectionResults);
            if (sections.Any())
            {
                return CalculateFilteredResults(sections);
            }
            else
            {
                return new T[0];
            }
        }

        protected abstract IList<T> CalculateFilteredResults(IEnumerable<ScreeningSectionResult> filteredSections);
    }
}
