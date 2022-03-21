using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrontDesk.Common;
using FrontDesk.Server.Data.BhsVisits;
using iTextSharp.text.pdf;

namespace FrontDesk.Server.Reports
{
    public class DrugOfChoicePdfReportSection : BhsPdfReportSectionBase
    {
        private ILookupListsDataSource _lookupListsDataSource;

        private IList<LookupValue> _answerValues;

        public DrugOfChoicePdfReportSection(PdfReport report) : this(report, new LookupListsDataSource())
        {
            _answerValues = _lookupListsDataSource.GetDrugOfChoice();
        }

        public DrugOfChoicePdfReportSection(PdfReport report, ILookupListsDataSource lookupListsDataSource) : base(report)
        {
            _lookupListsDataSource = lookupListsDataSource ?? throw new ArgumentNullException(nameof(lookupListsDataSource));
        }

        public override string ScreeningSectionID { get { return ScreeningSectionDescriptor.DrugOfChoice; ; } }

        public override float[] TableColumnWidths
        {
            get { return new float[] { 0.04f, 0.46f, 0.5f}; }
        }

        protected override string GetPreambleTextForSection()
        {
            return string.Empty;
        }

        protected override List<ScreeningSectionQuestion> GetQuestionsDataSource(ScreeningSection section)
        {
           return new List<ScreeningSectionQuestion>
            {
                new ScreeningSectionQuestion{ QuestionID = 1, QuestionText = "Primary"},
                new ScreeningSectionQuestion{ QuestionID = 2, QuestionText = "Secondary"},
                new ScreeningSectionQuestion{ QuestionID = 3, QuestionText = "Tertiary"}
            };
        }

        protected override PdfPTable GetQuestionsContentTable(float[] columnSize, bool viewComment = true)
        {
            ScreeningSectionResult sectionResult = GetScreeningSectionResult();
   
            if (sectionResult == null) return null;
            if (sectionResult.Answers.Count <= 0) return null;

            var table = new PdfPTable(columnSize) { WidthPercentage = 100 };

            int lineIndex = 0;


            List<ScreeningSectionQuestion> questionList = GetQuestionsDataSource(null);
            

            for (int i = 0; i < questionList.Count; i++)
            {

                var questionInfo = questionList[i];
                var questionResult = sectionResult.FindQuestionByID(questionInfo.QuestionID);

                RenderQuestion(lineIndex + i, table, questionInfo, questionResult);
            }

            return table;
        }

        protected override void RenderQuestion(int? lineIndex, PdfPTable table, ScreeningSectionQuestion questionInfo, ScreeningSectionQuestionResult questionResult, bool addLeftPadding = true)
        {

            int selectedValue = questionResult != null ? questionResult.AnswerValue : 0;
            var answerText = _answerValues.FirstOrDefault(x => x.Id == selectedValue)?.Name ?? _answerValues.First().Name ;

            bool normalRow = lineIndex.GetValueOrDefault() % 2 == 0;
            if (lineIndex.HasValue)
            {
                PrintBodyCell(table, lineIndex + 1 + ".", normalRow, addLeftPadding);
            }

            PrintBodyCell(table, questionInfo.QuestionText, normalRow, lineIndex.HasValue ? 1 : 2);

            PrintBodyCell(table, answerText, normalRow, lineIndex.HasValue ? 1 : 2);

        }
    }
}
