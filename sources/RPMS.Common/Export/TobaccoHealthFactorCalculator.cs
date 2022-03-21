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
    public sealed class TobaccoHealthFactorCalculator : AbstractHealthFactorCalculator, IHealthFactorCalculator
    {

        public override IEnumerable<ScreeningSectionResult> FilterSupportedSections(IEnumerable<ScreeningSectionResult> sectionResults)
        {
            return sectionResults.Where(s => s.ScreeningSectionID == ScreeningSectionDescriptor.Tobacco ||
                s.ScreeningSectionID == ScreeningSectionDescriptor.SmokerInHome);
        }

        protected override IList<HealthFactor> CalculateFilteredResults(IEnumerable<ScreeningSectionResult> filteredSections)
        {
            List<HealthFactor> hfList = new List<HealthFactor>();
            HealthFactor hf = null;

            var result = filteredSections.FirstOrDefault(x => x.ScreeningSectionID == ScreeningSectionDescriptor.SmokerInHome);


            if (result != null)
            {
                if (result.AnswerValue == 1)
                {
                    hf = CreateHealthFactor(HealthFactorKeys.TobaccoSmokerInHome);
                    hfList.Add(hf);
                }
                else
                {
                    hf = CreateHealthFactor(HealthFactorKeys.TobaccoSmokerFreeHome);

                }

            }
            //tobacco
            result = filteredSections.FirstOrDefault(x => x.ScreeningSectionID == ScreeningSectionDescriptor.Tobacco);
            if (result != null)
            {
                if (result.AnswerValue == 1) //If No, leave just smoker in home if present
                {
                    bool smoke;
                    smoke = false;

                    if (result.FindQuestionByID(TobaccoQuestionsDescriptor.UseForCeremonyOnlyQuestionID).AnswerValue == 1 /*&& !smokeless && !smoke*/)
                    {
                        hf = CreateHealthFactor(HealthFactorKeys.TobaccoCeremonialUseOnly);
                        hfList.Add(hf);
                    }

                    if (result.FindQuestionByID(TobaccoQuestionsDescriptor.DoYouSmokeQuestionID).AnswerValue == 1)
                    {
                        hf = CreateHealthFactor(HealthFactorKeys.TobaccoCurrentSmoker);
                        hfList.Add(hf);

                        smoke = true;
                    }

                    if (result.FindQuestionByID(TobaccoQuestionsDescriptor.DoYouSmokeSmokelessQuestionID).AnswerValue == 1)
                    {
                        hf = CreateHealthFactor(HealthFactorKeys.TobaccoCurrentSmokeless);
                        hfList.Add(hf);

                        //Do you smoke - yes
                        if (smoke)
                        {

                            hf = CreateHealthFactor(HealthFactorKeys.TobaccoCurrentSmokerAndSmokeless);
                        }

                    }

                }

                if (hf == null) //both section questions answered with No
                {
                    hf = CreateHealthFactor(HealthFactorKeys.TobaccoCurrentNonSmoker);
                    hfList.Add(hf);
                }

            }

            if (hf != null && !hfList.Contains(hf))
            {
                hfList.Add(hf);
            }
            return hfList;
        }



        private HealthFactor CreateHealthFactor(string key)
        {
            var fEl = RpmsExportConfiguration.GetConfiguration().HealthFactors[key];

            HealthFactor hf = new HealthFactor
                {
                    Comment = Resources.HealthFactorCommentTemplate
                };

            hf.Factor = fEl.Factor;
            hf.Code = fEl.Code;
            hf.FactorID = fEl.Id;

            return hf;
        }

    }
}
