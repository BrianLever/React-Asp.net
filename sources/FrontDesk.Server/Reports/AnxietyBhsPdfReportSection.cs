using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrontDesk.Common.Screening;
using iTextSharp.text.pdf;

namespace FrontDesk.Server.Reports
{
    public class AnxietyBhsPdfReportSection : BhsPdfReportSectionBase
    {
        protected AnxietyScreeningSectionResult _anxietyScreeningSectionResult;

        public AnxietyBhsPdfReportSection(PdfReport report) : base(report)
        {
            _anxietyScreeningSectionResult = report.ScreeningResult.FindSectionByID(ScreeningSectionDescriptor.Anxiety).AsAnxietySection();
        }

        protected override bool RenderScoreSection { get { return _anxietyScreeningSectionResult?.IsGad2Mode ?? false; } }

        public override string ScreeningSectionID { get { return ScreeningSectionDescriptor.Anxiety; } }

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
            if (_anxietyScreeningSectionResult.IsGad2Mode)
            {
                return new List<ScreeningSectionQuestion>();
            }

            return section.NotMainSectionQuestions;
        }

        protected override string GetSectionHeaderName()
        {
            return _anxietyScreeningSectionResult.ScreeningSectionName;
        }

        protected override string GetSectionShortName()
        {
            return _anxietyScreeningSectionResult.ScreeningSectionShortName;
        }
    }
}
