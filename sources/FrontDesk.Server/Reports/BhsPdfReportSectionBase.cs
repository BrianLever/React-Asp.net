using System;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using FrontDesk.Server.Utils;

namespace FrontDesk.Server.Reports
{
    public abstract class BhsPdfReportSectionBase
    {

        protected PdfReport Report;

        public abstract string ScreeningSectionID { get; }

        public abstract float[] TableColumnWidths { get; }

        public Image checkImage = Image.GetInstance(FilePathResolver.ResolveFilePath(System.Web.Configuration.WebConfigurationManager.AppSettings["ChekedImagePath"]));
        public Image uncheckImage = Image.GetInstance(FilePathResolver.ResolveFilePath(System.Web.Configuration.WebConfigurationManager.AppSettings["UnchekedImagePath"]));

        protected virtual bool RenderScoreSection
        {
            get { return true; }
        }

        protected BhsPdfReportSectionBase(PdfReport report)
        {
            if (report == null) throw new ArgumentNullException("report");

            Report = report;
        }

        protected virtual ScreeningSection ScreeningSectionInfo { get; set; }

        protected virtual string GetSectionHeaderName()
        {
            return GetScreeningSectionInfo().ScreeningSectionName;
        }

        protected virtual string GetSectionShortName()
        {
            return GetScreeningSectionInfo().ScreeningSectionShortName;
        }

        public virtual Document RenderSectionContent(Document document)
        {
            PdfPTable contentTable = GetQuestionsContentTable(TableColumnWidths, RenderScoreSection);
            if (contentTable != null)
            {
                ScreeningSectionInfo = Report.ScreeningInfo.FindSectionByID(ScreeningSectionID);
                document.Add(PrintSection(GetSectionHeaderName(), GetPreambleTextForSection(), contentTable));
            }

            return document;
        }

        protected virtual string GetPreambleTextForSection()
        {
            return string.Empty;
        }

        protected virtual PdfPTable PrintSection(string header, string preambleText, PdfPTable contentTable)
        {
            PdfPTable content = CreateSectionContent(header);
            content.SpacingAfter = 10;
            content.KeepTogether = true;

            if (!String.IsNullOrEmpty(preambleText))
            {
                PdfPCell preambleCell = new PdfPCell(new Phrase(preambleText, PdfReport.preambleFont));
                preambleCell.Border = Rectangle.NO_BORDER;
                preambleCell.HorizontalAlignment = Element.ALIGN_LEFT;
                preambleCell.PaddingBottom = 0;
                content.AddCell(preambleCell);
            }
            
            content.AddCell(WrapContentTable(contentTable));

            return content;
        }

        protected PdfPCell WrapContentTable(PdfPTable table)
        {
            //create column with content
            return new PdfPCell(table)
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER,
                PaddingTop = 5
            };
        }


        protected ScreeningSectionResult GetScreeningSectionResult()
        {
            return Report.ScreeningResult.FindSectionByID(ScreeningSectionID);
        }

        protected ScreeningSection GetScreeningSectionInfo()
        {
            return Report.ScreeningInfo.FindSectionByID(ScreeningSectionID);
        }


        protected virtual PdfPTable GetQuestionsContentTable(float[] columnSize, bool viewComment = true)
        {
            ScreeningSectionResult sectionResult = GetScreeningSectionResult();
            ScreeningSection section = GetScreeningSectionInfo();

            if (section == null || sectionResult == null) return null;
            if (sectionResult.Answers.Count <= 0) return null;

            PdfPTable commentTable = GetSectionComment(GetCopyRight(section.ScreeningSectionID), section.ScreeningSectionID, sectionResult);

            var table = new PdfPTable(columnSize) { WidthPercentage = 100 };

            int lineIndex;

            RenderMainQuestionsWithAnswers(table, sectionResult, out lineIndex);

            List<ScreeningSectionQuestion> questionList = GetQuestionsDataSource(section);

            for (int i = 0; i < questionList.Count; i++)
            {
                if (TestRenderCommentsConditionMet(i, questionList, section) && commentTable != null)
                    RenderCommentSection(table, commentTable, columnSize.Length, true);

                var questionInfo = questionList[i];
                var questionResult = sectionResult.FindQuestionByID(questionInfo.QuestionID);

                RenderQuestion(lineIndex + i, table, questionInfo, questionResult);
            }
            if (commentTable != null && viewComment)
                RenderCommentSection(table, commentTable, columnSize.Length);

            return table;
        }

        protected virtual List<ScreeningSectionQuestion> GetQuestionsDataSource(ScreeningSection section)
        {
            return section.NotMainSectionQuestions;
        }

        protected virtual void RenderMainQuestionsWithAnswers(PdfPTable table, ScreeningSectionResult sectionResult, out int numberedListLineIndex)
        {
            numberedListLineIndex = 0;
            var questionList = GetMainSectionQuestionsDataSource();

            for (int i = 0; i < questionList.Count; i++)
            {
                var questionInfo = questionList[i];
                var questionResult = sectionResult.FindQuestionByID(questionInfo.QuestionID);

                RenderQuestion(null, table, questionInfo, questionResult);
            }

        }

        protected virtual bool TestRenderCommentsConditionMet(int lineIndex, IList<ScreeningSectionQuestion> questions, ScreeningSection section)
        {
            return lineIndex + 1 == questions.Count &&
                (section.ScreeningSectionID == ScreeningSectionDescriptor.Depression 
                || section.ScreeningSectionID == ScreeningSectionDescriptor.Anxiety);
        }

        protected virtual void RenderQuestion(int? lineIndex, PdfPTable table, ScreeningSectionQuestion questionInfo, ScreeningSectionQuestionResult questionResult, bool addLeftPadding = true)
        {

            int? selectedValue = questionResult != null ? questionResult.AnswerValue : (int?)null;

            bool normalRow = lineIndex.GetValueOrDefault() % 2 == 0;
            if (lineIndex.HasValue)
            {
                PrintBodyCell(table, lineIndex + 1 + ".", normalRow, addLeftPadding);
            }

            PrintBodyCell(table, questionInfo.QuestionText, normalRow, lineIndex.HasValue ? 1 : 2);

            for (int j = 0; j < questionInfo.AnswerOptions.Count; j++)
            {
                CreateCheckingCell(table, questionInfo.AnswerOptions[j].Text,
                    selectedValue == questionInfo.AnswerOptions[j].Value, normalRow ? Report.NormalRowBackground : Report.AltRowBackground);
            }
        }

        /// <summary>
        /// Print comment (copyright and indicates)
        /// </summary>
        private void RenderCommentSection(PdfPTable container, PdfPTable commentTable, int columnCount, bool paddingBottom = false)
        {
            var cell = new PdfPCell(commentTable);
            cell.Colspan = columnCount;
            cell.Border = Rectangle.NO_BORDER;
            cell.PaddingTop = 10;
            if (paddingBottom)
                cell.PaddingBottom = 10;
            container.AddCell(cell);
        }

        protected PdfPTable GetSectionComment(string commentText, string sectionID, ScreeningSectionResult sectionResult)
        {
            PdfPTable commentTable = new PdfPTable(new float[] { 0.40f, 0.15f, 0.45f });
            commentTable.WidthPercentage = 100;

            var cell = new PdfPCell();
            cell.Border = Rectangle.NO_BORDER;
            commentTable.AddCell(cell);

            cell = new PdfPCell(new Phrase(String.Format("{0} Score", GetSectionShortName()), PdfReport.boldFont));
            cell.BackgroundColor = Report.GreenBackground;
            cell.Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            cell.BorderColor = Report.GrayBorder;
            cell.Padding = 3;
            commentTable.AddCell(cell);

            cell = new PdfPCell(new Phrase(Resources.TextMessages.BHS_REPORT_INDICATES, PdfReport.boldFont));
            cell.Padding = 3;
            cell.BackgroundColor = Report.GreenBackground;
            cell.Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            cell.BorderColor = Report.GrayBorder;
            commentTable.AddCell(cell);

            cell = new PdfPCell(new Phrase(commentText, PdfReport.commentFont));
            cell.Border = Rectangle.NO_BORDER;
            cell.Padding = 3;
            cell.VerticalAlignment = Element.ALIGN_BOTTOM;
            commentTable.AddCell(cell);

            cell = new PdfPCell(new Phrase(String.Format("{0}", sectionResult.Score), PdfReport.valueFont));
            cell.PaddingLeft = 10;
            cell.PaddingBottom = 1;
            cell.PaddingTop = 1;
            cell.BackgroundColor = Report.GreenBackground;
            cell.Border = Element.RECTANGLE;
            cell.BorderColor = Report.GrayBorder;
            commentTable.AddCell(cell);

            if (!String.IsNullOrEmpty(sectionResult.Indicates))
            {
                cell = new PdfPCell(new Phrase(String.Format("{0}: {1}", sectionResult.ScoreLevelLabel, sectionResult.Indicates), PdfReport.valueFont));
            }
            else
            {
                cell = new PdfPCell(new Phrase(String.Format("{0}", sectionResult.ScoreLevelLabel), PdfReport.valueFont));
            }
            cell.PaddingLeft = 10;
            cell.PaddingBottom = 1;
            cell.PaddingTop = 1;
            cell.BackgroundColor = Report.GreenBackground;
            cell.Border = Element.RECTANGLE;
            cell.BorderColor = Report.GrayBorder;
            commentTable.AddCell(cell);

            return commentTable;
        }

        private string GetCopyRight(string sectionID)
        {
            switch (sectionID)
            {
                case ScreeningSectionDescriptor.Tobacco:
                    return String.Empty;
                case ScreeningSectionDescriptor.Alcohol:
                    return Resources.Copyrights.CAGE_HTML;
                case ScreeningSectionDescriptor.SubstanceAbuse:
                    return Resources.Copyrights.DAST10_HTML;
                case ScreeningSectionDescriptor.Depression:
                    return Resources.Copyrights.PHQ9_HTML;
                case ScreeningSectionDescriptor.Anxiety:
                    return Resources.Copyrights.GAD7_HTML;
                case ScreeningSectionDescriptor.PartnerViolence:
                    return Resources.Copyrights.HITS_HTML;
                default:
                    return String.Empty;
            }
        }

        protected PdfPTable CreateSectionContent(string title)
        {
            //print section title
            PdfPTable sectionTitleTable = new PdfPTable(new float[] { 1f });
            sectionTitleTable.WidthPercentage = 100;

            PdfPCell titleCell = new PdfPCell(new Phrase(title, PdfReport.headerBlueFont));
            titleCell.BackgroundColor = Report.HeaderBackground;
            titleCell.Border = Element.RECTANGLE | Rectangle.TOP_BORDER;
            titleCell.BorderColor = Report.GrayBorder;
            titleCell.Padding = 3;
            sectionTitleTable.AddCell(titleCell);

            return sectionTitleTable;
        }

        /// <summary>
        /// Creater for column with checkbox
        /// </summary>
        protected void CreateCheckingCell(PdfPTable table, string text, bool isChecked, Color bgColor)
        {
            float[] tableWidth = new float[] { 1f };
            PdfPTable checktable = new PdfPTable(tableWidth);
            checktable.WidthPercentage = 100;

            checkImage.ScaleToFit(0.125F * 72, 0.125F * 72);
            checkImage.Alignment = Element.ALIGN_TOP;
            uncheckImage.ScaleToFit(0.125F * 72, 0.125F * 72);
            uncheckImage.Alignment = Element.ALIGN_TOP;

            Paragraph p = new Paragraph();
            p.SetAlignment(Element.ALIGN_TOP.ToString());
            p.SetLeading(checkImage.Height, 0);
            Chunk imageChunk = new Chunk(isChecked ? checkImage : uncheckImage, 0, 0, true);
            Chunk textChunk = new Chunk("  " + text, PdfReport.labelFont);
            p.Add(imageChunk);
            p.Add(textChunk);

            PdfPCell Checkcell = new PdfPCell(p);
            Checkcell.Border = Rectangle.NO_BORDER;
            Checkcell.Padding = 0;
            Checkcell.PaddingTop = 1;
            Checkcell.PaddingLeft = 4;
            Checkcell.HorizontalAlignment = Element.ALIGN_LEFT;
            Checkcell.VerticalAlignment = Element.ALIGN_TOP;

            checktable.AddCell(Checkcell);

            var cell = new PdfPCell(checktable);
            cell.Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            cell.BorderColor = Report.GrayBorder;
            if (bgColor != null)
            {
                cell.BackgroundColor = bgColor;
            }
            table.AddCell(cell);
        }

        protected void PrintBodyCell(PdfPTable table, string value, bool normalRow, bool addLeftPadding = false)
        {
            PrintBodyCell(table, value, normalRow, 1, addLeftPadding);
        }

        protected void PrintBodyCell(PdfPTable table, string value, bool normalRow, int colspan = 1, bool addLeftPadding = false)
        {
            var cell = new PdfPCell(new Phrase(value, PdfReport.labelFont))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER,
                BorderColor = Report.GrayBorder,
                BackgroundColor = normalRow ? Report.NormalRowBackground : Report.AltRowBackground,
                PaddingLeft = 4,
                Colspan = colspan
            };
            if (addLeftPadding)
            {
                cell.PaddingLeft += 5;
            }

            table.AddCell(cell);
        }

        protected virtual IList<ScreeningSectionQuestion> GetMainSectionQuestionsDataSource()
        {
            return GetScreeningSectionInfo().MainSectionQuestions;
        }
    }
}
