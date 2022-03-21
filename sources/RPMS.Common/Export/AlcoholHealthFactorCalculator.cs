using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrontDesk;
using RPMS.Common.Models;
using RPMS.Common.Configuration;
using ScreenDox.EHR.Common.Properties;

namespace RPMS.Common.Export
{
    public sealed class AlcoholHealthFactorCalculator : AbstractHealthFactorCalculator
    {

        public override IEnumerable<ScreeningSectionResult> FilterSupportedSections(IEnumerable<ScreeningSectionResult> sectionResults)
        {
            return sectionResults.Where(x => x.ScreeningSectionID == ScreeningSectionDescriptor.Alcohol);
        }


        protected override IList<HealthFactor> CalculateFilteredResults(IEnumerable<ScreeningSectionResult> filteredSections)
        {
            var section = filteredSections.First();

            if (section.Score.HasValue)
            {
                HealthFactorElement factorEl = RpmsExportConfiguration.GetConfiguration().HealthFactors["AlcoholCage" + section.Score];

                return new HealthFactor[]{ new HealthFactor
                {
                    Factor = factorEl.Factor,
                    FactorID = factorEl.Id,
                    Code = factorEl.Code,
                    Comment = Resources.HealthFactorCommentTemplate
                }};
            }
            return new HealthFactor[0];
        }
    }
}
