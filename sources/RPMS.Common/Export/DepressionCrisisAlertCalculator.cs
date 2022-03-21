using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPMS.Common.Models;
using FrontDesk;
using RPMS.Common.Configuration;
using ScreenDox.EHR.Common.Properties;

namespace RPMS.Common.Export
{
    public class DepressionCrisisAlertCalculator : AbstractCalculator<CrisisAlert>, ICrisisAlertCalculator
    {


        public override IEnumerable<FrontDesk.ScreeningSectionResult> FilterSupportedSections(IEnumerable<FrontDesk.ScreeningSectionResult> sectionResults)
        {
            return sectionResults.Where(x => x.ScreeningSectionID == ScreeningSectionDescriptor.Depression);
        }



        protected override IList<CrisisAlert> CalculateFilteredResults(IEnumerable<ScreeningSectionResult> filteredSections)
        {
            List<CrisisAlert> alertList = new List<CrisisAlert>();

            CrisisAlert alert = null;

            var result = filteredSections.First();
            if (result.AnswerValue > 0) //Yes
            {

                var cfg = RpmsExportConfiguration.GetConfiguration().CrisisAlerts;
                var hurtYouselfAnswer = result.Answers.FirstOrDefault(x => x.QuestionID == DepressionQuestionsDescriptor.HurtYouselfQuestion);

                if (hurtYouselfAnswer != null && hurtYouselfAnswer.AnswerValue > DepressionQuestionsDescriptor.NotAtAllAnswer)
                {

                    string answerText = DepressionQuestionsDescriptor.AnswerTexts[hurtYouselfAnswer.AnswerValue];

                    alert = new CrisisAlert
                    {
                        Title = cfg[CrisisAlertKeys.ClinicalWarningDocument].Name,
                        DocumentTypeID = cfg[CrisisAlertKeys.ClinicalWarningDocument].Id,
                        DocumentTypeAbbr = cfg[CrisisAlertKeys.ClinicalWarningDocument].Abbr,
                        Details = Resources.DepressionCrisisAlertDetailsTemplate.FormatWith(answerText)

                    };

                    alertList.Add(alert);
                }

            }
            return alertList;
        }
    }
}
