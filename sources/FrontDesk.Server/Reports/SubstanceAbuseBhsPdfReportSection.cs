using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text.pdf;

namespace FrontDesk.Server.Reports
{
    public class SubstanceAbuseBhsPdfReportSection : BhsPdfReportSectionBase
    {
        public SubstanceAbuseBhsPdfReportSection(PdfReport report) : base(report)
        {
        }

        public override string ScreeningSectionID { get { return ScreeningSectionDescriptor.SubstanceAbuse; ; } }

        public override float[] TableColumnWidths
        {
            get { return new float[] { 0.04f, 0.8f, 0.08f, 0.08f }; }
        }

        protected override string GetPreambleTextForSection()
        {
            return GetScreeningSectionInfo().Questions[0].PreambleText;
        }

        protected override void RenderMainQuestionsWithAnswers(PdfPTable table, ScreeningSectionResult sectionResult, out int numberedListLineIndex)
        {
            var questionList = GetMainSectionQuestionsDataSource();

            for (numberedListLineIndex = 0; numberedListLineIndex < questionList.Count; numberedListLineIndex++)
            {
                var questionInfo = questionList[numberedListLineIndex];
                var questionResult = sectionResult.FindQuestionByID(questionInfo.QuestionID);

                RenderQuestion(numberedListLineIndex, table, questionInfo, questionResult, true);
            }
        }
    }
}
