using FrontDesk.Common.Resources;

using System;
using System.Collections.Generic;
using System.Linq;

namespace FrontDesk.Server.Screening.Models
{
    public class IndicatorReportViewModel
    {
        public List<IndicatorReportItem> BriefQuestionsSectionItems { get; set; }
        public List<IndicatorReportItem> SectionsScoreLevelItems { get; set; }

        public FrontDesk.Screening ScreeningInfo { get; set; }

        /// <summary>
        /// The total number of screenings for certain filter criteria
        /// </summary>
        public long TotalPatientScreenings { get; set; }

        public Dictionary<string, int> UniquePatentCountByScreeningSection { get; set; }

        public IndicatorReportViewModel() {
            BriefQuestionsSectionItems = new List<IndicatorReportItem>();
            SectionsScoreLevelItems = new List<IndicatorReportItem>();
        }
        #region Section Headers and data

        private BHIReportSectionViewModel GetReportSectionModel(string header, string copyrights, string sectionID, Func<IndicatorReportItem, bool> mainQuestionsSelector = null)
        {
            var model = new BHIReportSectionViewModel
            {
                Header = header,
                Items = this.SectionsScoreLevelItems.FindAll(s => s.ScreeningSectionID == sectionID),
                
                Copyrights = copyrights
            };

            if (mainQuestionsSelector == null)
            {
                mainQuestionsSelector = (x) => x.ScreeningSectionID == sectionID;
            }
            model.MainQuestions = this.BriefQuestionsSectionItems.Where(mainQuestionsSelector).ToList(); ;

            return model;

        }


        public BHIReportSectionViewModel TobaccoSection
        {
            get
            {
                return GetReportSectionModel("Tobacco Use", "", ScreeningSectionDescriptor.Tobacco, 
                    (x) => x.ScreeningSectionID == ScreeningSectionDescriptor.SmokerInHome ||
                        x.ScreeningSectionID == ScreeningSectionDescriptor.Tobacco);
            }
        }

        public BHIReportSectionViewModel AlcoholSection
        {
            get
            {
                return GetReportSectionModel("Alcohol Use (CAGE)", Resources.Copyrights.CAGE_HTML, ScreeningSectionDescriptor.Alcohol);
            }
        }

        public BHIReportSectionViewModel DepressionSectionPhq9
        {
            get
            {
                var model = GetReportSectionModel(FrontDesk.Common.Resources.ScreeningLabels.Screening_Report_Section_PHQ9, Resources.Copyrights.PHQ9_HTML, 
                    ScreeningSectionDescriptor.Depression);

                model.QuestionOnFocus = new QuestionOnFocus<IndicatorReportItem>
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

        public BHIReportSectionViewModel DepressionSectionPhq2
        {
            get
            {
                return GetReportSectionModel(ScreeningLabels.Screening_Report_Section_PHQ2, Resources.Copyrights.PHQ9_HTML, 
                    ScreeningSectionDescriptor.DepressionPhq2ID);
            }
        }

        public BHIReportSectionViewModel PartnerViolenceSection
        {
            get
            {
                return GetReportSectionModel("Intimate Partner/Domestic Violence (HITS)", Resources.Copyrights.HITS_HTML, ScreeningSectionDescriptor.PartnerViolence);
            }
        }

        public BHIReportSectionViewModel SubstanceAbuseSection
        {
            get
            {
                return GetReportSectionModel("Non-Medical Drug Use (DAST-10)", Resources.Copyrights.DAST10_HTML,  ScreeningSectionDescriptor.SubstanceAbuse);
            }
        }


        public BHIReportSectionViewModel AnxietySectionPhq7
        {
            get
            {
                var model = GetReportSectionModel(ScreeningLabels.Screening_Report_Section_GAD7, Resources.Copyrights.GAD7_HTML, 
                    ScreeningSectionDescriptor.Anxiety);
               
                return model;
            }
        }

        public BHIReportSectionViewModel AnxietySectionGad2
        {
            get
            {
                return GetReportSectionModel(ScreeningLabels.Screening_Report_Section_GAD2, Resources.Copyrights.GAD7_HTML, 
                    ScreeningSectionDescriptor.AnxietyGad2ID);
            }
        }

        public BHIReportSectionViewModel ProblemGamblingSection
        {
            get
            {
                return GetReportSectionModel(
                    "Problem Gambling (BBGS)", 
                    Resources.Copyrights.BBGS_HTML, 
                    ScreeningSectionDescriptor.ProblemGambling);
            }
        }

        #endregion

    }

    public class BHIReportSectionViewModel
    {
        public string Header { get; set; }
        public List<IndicatorReportItem> Items { get; set; }
        public string Copyrights {get; set;}
        public ICollection<IndicatorReportItem> MainQuestions { get; set; }
        public QuestionOnFocus<IndicatorReportItem> QuestionOnFocus { get; set; }
    }

    public class QuestionOnFocus<T>
    {
        public ScreeningSectionQuestion Question { get; set; }
        public List<T> Items { get; set; }
    }
}
