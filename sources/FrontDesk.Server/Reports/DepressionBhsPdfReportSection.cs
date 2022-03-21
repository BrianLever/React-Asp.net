using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrontDesk.Common.Screening;
using iTextSharp.text.pdf;

namespace FrontDesk.Server.Reports
{
    public class DepressionBhsPdfReportSection : BhsPdfReportSectionBase
    {
        protected DepressionScreeningSectionResult _depressionScreeningSectionResult;

        public DepressionBhsPdfReportSection(PdfReport report) : base(report)
        {
            _depressionScreeningSectionResult = report.ScreeningResult.FindSectionByID(ScreeningSectionDescriptor.Depression).AsDepressionSection();
        }

        protected override bool RenderScoreSection { get { return _depressionScreeningSectionResult?.IsPhq2Mode ?? false; } }

        public override string ScreeningSectionID { get { return ScreeningSectionDescriptor.Depression; } }

        public override float[] TableColumnWidths
        {
            get { return new float[] { 0.04f, 0.36f, 0.1f, 0.13f, 0.19f, 0.18f }; }
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

        protected override List<ScreeningSectionQuestion> GetQuestionsDataSource(ScreeningSection section)
        {
            if (_depressionScreeningSectionResult.IsPhq2Mode)
            {
                return new List<ScreeningSectionQuestion>();
            }

            return section.NotMainSectionQuestions;
        }

        protected override string GetSectionHeaderName()
        {
            return _depressionScreeningSectionResult.ScreeningSectionName;
        }

        protected override string GetSectionShortName()
        {
            return _depressionScreeningSectionResult.ScreeningSectionShortName;
        }
    }
}
