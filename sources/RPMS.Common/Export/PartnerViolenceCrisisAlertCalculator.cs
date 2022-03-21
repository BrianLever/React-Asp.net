using System.Collections.Generic;
using System.Linq;
using FrontDesk;
using RPMS.Common.Configuration;
using RPMS.Common.Models;
using ScreenDox.EHR.Common.Properties;

namespace RPMS.Common.Export
{
    public class PartnerViolenceCrisisAlertCalculator : AbstractCalculator<CrisisAlert>, ICrisisAlertCalculator
    {
        private class CrisisAlertMetaData
        {
            public string DocumentTypeName { get; set; }
            public int DocumentTypeID { get; set; }
            public string DocumentTypeAbbr { get; set; }
            public int HurtYouQuestionId { get; set; }

            public Dictionary<int, CrisisAlertElement> Answers = null;
        }

        private CrisisAlertMetaData metaData = null;


        public override IEnumerable<FrontDesk.ScreeningSectionResult> FilterSupportedSections(IEnumerable<FrontDesk.ScreeningSectionResult> sectionResults)
        {
            return sectionResults.Where(x => x.ScreeningSectionID == ScreeningSectionDescriptor.PartnerViolence);
        }

        protected override IList<CrisisAlert> CalculateFilteredResults(IEnumerable<FrontDesk.ScreeningSectionResult> filteredSections)
        {
            List<CrisisAlert> alertList = new List<CrisisAlert>();
            CrisisAlert alert = null;

            var result = filteredSections.First();
            if (result.AnswerValue > 0) //Yes
            {

                var cfg = RpmsExportConfiguration.GetConfiguration().CrisisAlerts;
                var hurtYouAnswer = result.Answers.FirstOrDefault(x => x.QuestionID == PartnerViolenceQuestionsDescriptor.PhysicallyHurtYouQuestion);

                if (hurtYouAnswer != null && hurtYouAnswer.AnswerValue > PartnerViolenceQuestionsDescriptor.NeverAnswer)
                {

                    string answerText = PartnerViolenceQuestionsDescriptor.AnswerTexts[hurtYouAnswer.AnswerValue];

                    alert = new CrisisAlert
                    {
                        Title = cfg[CrisisAlertKeys.ClinicalWarningDocument].Name,
                        DocumentTypeID = cfg[CrisisAlertKeys.ClinicalWarningDocument].Id,
                        DocumentTypeAbbr = cfg[CrisisAlertKeys.ClinicalWarningDocument].Abbr,
                        Details = Resources.PartnerViolenceCrisisAlertDetailsTemplate.FormatWith(answerText)

                    };

                    alertList.Add(alert);
                }

            }
            return alertList;
        }
    }
}
