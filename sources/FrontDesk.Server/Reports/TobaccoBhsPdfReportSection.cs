using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace FrontDesk.Server.Reports
{
    public class TobaccoBhsPdfReportSection : BhsPdfReportSectionBase
    {
        private string _screeningSectionID = ScreeningSectionDescriptor.Tobacco;

        protected override bool RenderScoreSection { get { return false; } }

        public TobaccoBhsPdfReportSection(PdfReport report) : base(report)
        {
        }

        public override string ScreeningSectionID { get { return _screeningSectionID; } }
        public override float[] TableColumnWidths { get { return new float[] { 0.04f, 0.8f, 0.08f, 0.08f }; } }


        public override Document RenderSectionContent(Document document)
        {
            //select Tobacco or Smoker in Home sections for filtering results
            ;
            if (Report.ScreeningResult.FindSectionByID(ScreeningSectionDescriptor.Tobacco) == null)
            {
                _screeningSectionID = ScreeningSectionDescriptor.SmokerInHome;
            }

            return base.RenderSectionContent(document);
        }

        protected override IList<ScreeningSectionQuestion> GetMainSectionQuestionsDataSource()
        {
            List<ScreeningSectionQuestion> result = new List<ScreeningSectionQuestion>();
            if (_screeningSectionID != ScreeningSectionDescriptor.SmokerInHome)
            {
                result.AddRange(Report.ScreeningInfo.FindSectionByID(ScreeningSectionDescriptor.SmokerInHome).MainSectionQuestions);

            }
            result.AddRange(base.GetMainSectionQuestionsDataSource());
            return result;
        }

        protected override void RenderMainQuestionsWithAnswers(PdfPTable table, ScreeningSectionResult sectionResult, out int numberedListLineIndex)
        {
            numberedListLineIndex = 0;
            var questionList = GetMainSectionQuestionsDataSource();

            for (int i = 0; i < questionList.Count; i++)
            {
                var questionInfo = questionList[i];

                sectionResult = Report.ScreeningResult.FindSectionByID(questionInfo.ScreeningSectionID);

                if (sectionResult == null) continue;

                var questionResult = sectionResult.FindQuestionByID(questionInfo.QuestionID);

                
                RenderQuestion(null, table, questionInfo, questionResult);
            }

        }
    }
}
