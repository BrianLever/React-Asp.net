using FrontDesk.Common.Resources;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Server.Screening.Models
{
    public class IndicatorReportByAgeViewModel
    {
        public List<IndicatorReportByAgeItemViewModel> BriefQuestionsSectionItems { get; set; }
        public List<IndicatorReportByAgeItemViewModel> SectionsScoreLevelItems { get; set; }
        public FrontDesk.Screening ScreeningInfo { get; set; }

        public IndicatorReportByAgeViewModel()
        {
            BriefQuestionsSectionItems = new List<IndicatorReportByAgeItemViewModel>();
            SectionsScoreLevelItems = new List<IndicatorReportByAgeItemViewModel>();
 
        }

        #region Section Headers and data

        private BHIReportSectionByAgeViewModel GetReportSectionModel(string header, string copyrights, string sectionID, Func<IndicatorReportByAgeItemViewModel, bool> mainQuestionsSelector = null)
        {
            var model = new BHIReportSectionByAgeViewModel
            {
                Header = header,
                Items = SectionsScoreLevelItems.FindAll(s => s.ScreeningSectionID == sectionID),
                Copyrights = copyrights
            };

            if (mainQuestionsSelector == null)
            {
                mainQuestionsSelector = (x) => x.ScreeningSectionID == sectionID;
            }
            model.MainQuestions = this.BriefQuestionsSectionItems.Where(mainQuestionsSelector).ToList(); ;

            return model;
        }


        public BHIReportSectionByAgeViewModel TobaccoSection
        {
            get
            {
                return GetReportSectionModel("Tobacco Use", "", ScreeningSectionDescriptor.Tobacco,
                    (x) => x.ScreeningSectionID == ScreeningSectionDescriptor.SmokerInHome ||
                        x.ScreeningSectionID == ScreeningSectionDescriptor.Tobacco);
            }
        }

        public BHIReportSectionByAgeViewModel AlcoholSection
        {
            get
            {
                return GetReportSectionModel("Alcohol Use (CAGE)", Resources.Copyrights.CAGE_HTML, ScreeningSectionDescriptor.Alcohol);
            }
        }


        public BHIReportSectionByAgeViewModel DepressionSectionPhq2
        {
            get
            {
                return GetReportSectionModel(ScreeningLabels.Screening_Report_Section_PHQ2, Resources.Copyrights.PHQ9_HTML, ScreeningSectionDescriptor.DepressionPhq2ID);
            }
        }

        public BHIReportSectionByAgeViewModel DepressionSectionPhq9
        {
            get
            {
                var model = GetReportSectionModel(ScreeningLabels.Screening_Report_Section_PHQ9, Resources.Copyrights.PHQ9_HTML, ScreeningSectionDescriptor.Depression);

                model.QuestionOnFocus = new QuestionOnFocus<IndicatorReportByAgeItemViewModel>
                {
                    Question = ScreeningInfo.FindSectionByID(ScreeningSectionDescriptor.Depression)
                        .FindQuestionByID(ScreeningSectionDescriptor.DepressionThinkOfDeathQuestionID),
                    Items =
                        SectionsScoreLevelItems.FindAll(
                            s => s.ScreeningSectionID == ScreeningSectionDescriptor.DepressionThinkOfDeath)
                };

                return model;
            }
        }

        public BHIReportSectionByAgeViewModel PartnerViolenceSection
        {
            get
            {
                return GetReportSectionModel("Intimate Partner/Domestic Violence (HITS)", Resources.Copyrights.HITS_HTML, ScreeningSectionDescriptor.PartnerViolence);
            }
        }
        /*
         * Remark: DAST-10 shall not be shown on Indicator Report regarding customer's feedback 
         * http://jobcard.3sicorp.com/Project/ProjectUnitComments.aspx?id=2635&brief=False&deleted=False
         */
        public BHIReportSectionByAgeViewModel SubstanceAbuseSection
        {
            get
            {
                return GetReportSectionModel("Non-Medical Drug Use (DAST-10)", Resources.Copyrights.DAST10_HTML, ScreeningSectionDescriptor.SubstanceAbuse);
            }
        }


        public BHIReportSectionByAgeViewModel DrugOfChoicePrimaryAnswers
        {
            get
            {
                return GetReportSectionModel("Primary Drug Use", string.Empty, ScreeningSectionDescriptor.DrugOfChoice + "_1");
            }
        }

        public BHIReportSectionByAgeViewModel DrugOfChoiceSecondaryAnswers
        {
            get
            {
                return GetReportSectionModel("Secondary Drug Use", string.Empty, ScreeningSectionDescriptor.DrugOfChoice + "_2");
            }
        }


        public BHIReportSectionByAgeViewModel DrugOfChoiceTertiaryAnswers
        {
            get
            {
                return GetReportSectionModel("Tertiary Drug Use", string.Empty, ScreeningSectionDescriptor.DrugOfChoice + "_3");
            }
        }


        public BHIReportSectionByAgeViewModel AnxietySectionGad2
        {
            get
            {
                return GetReportSectionModel(ScreeningLabels.Screening_Report_Section_GAD2, 
                    Resources.Copyrights.GAD7_HTML, 
                    ScreeningSectionDescriptor.AnxietyGad2ID
                    );
            }
        }

        public BHIReportSectionByAgeViewModel AnxietySectionGad7
        {
            get
            {
                var model = GetReportSectionModel(ScreeningLabels.Screening_Report_Section_GAD7, 
                    Resources.Copyrights.GAD7_HTML, 
                    ScreeningSectionDescriptor.Anxiety
                    );

                return model;
            }
        }

        public BHIReportSectionByAgeViewModel ProblemGamblingSection
        {
            get
            {
                return GetReportSectionModel(
                    "Problem Gambling (BBGS)", 
                    Resources.Copyrights.BBGS_HTML, 
                    ScreeningSectionDescriptor.ProblemGambling
                );
            }
        }

        #endregion

    }

    public class BHIReportSectionByAgeViewModel
    {
        public string Header { get; set; }
        public List<IndicatorReportByAgeItemViewModel> Items { get; set; }

        public ICollection<IndicatorReportByAgeItemViewModel> MainQuestions { get; set; }
        public QuestionOnFocus<IndicatorReportByAgeItemViewModel> QuestionOnFocus { get; set; }

        public string Copyrights {get; set;}
    }
}
