using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace FrontDesk.Server.Reports
{
	public class ViolenceBhsPdfReportSection : BhsPdfReportSectionBase
	{
		public ViolenceBhsPdfReportSection(PdfReport report) : base(report)
		{
		}

		public override string ScreeningSectionID { get { return ScreeningSectionDescriptor.PartnerViolence; ; } }
		public override float[] TableColumnWidths { get { return new float[] { 0.04f, 0.42f, 0.1f, 0.095f, 0.115f, 0.12f, 0.11f }; } }

		protected override void RenderMainQuestionsWithAnswers(PdfPTable table, ScreeningSectionResult sectionResult, out int numberedListLineIndex)
		{
			numberedListLineIndex = 0;
			//do nothing
		}

		protected override PdfPTable PrintSection(string header, string preambleText, PdfPTable contentTable)
		{

			PdfPTable content = CreateSectionContent(header);
			content.SpacingAfter = 10;
			content.KeepTogether = true;

			RenderMainQuestionContentTable(content);

			if (!String.IsNullOrEmpty(preambleText))
			{
				PdfPCell preambleCell = new PdfPCell(new Phrase(preambleText, PdfReport.preambleFont))
				{
					Border = Rectangle.NO_BORDER,
					HorizontalAlignment = Element.ALIGN_LEFT,
					PaddingBottom = 0,
					PaddingLeft = 4
				};
				content.AddCell(preambleCell);
			}

			content.AddCell(WrapContentTable(contentTable));

			return content;
		}


		protected void RenderMainQuestionContentTable(PdfPTable containerTable)
		{
			var questionList = GetMainSectionQuestionsDataSource();
			var sectionResult = GetScreeningSectionResult();


			PdfPTable content = new PdfPTable(new float[] { 0.04f, 0.8f, 0.08f, 0.08f });

			foreach (var questionInfo in questionList)
			{
				var questionResult = sectionResult.FindQuestionByID(questionInfo.QuestionID);

				questionInfo.QuestionText = questionInfo.PreambleText + " " + questionInfo.QuestionText;

				RenderQuestion(null, content, questionInfo, questionResult);
			}

			containerTable.AddCell(WrapContentTable(content));
		}

		protected override string GetPreambleTextForSection()
		{
			return GetScreeningSectionInfo().NotMainSectionQuestions.First().PreambleText;
		}
	}
}
