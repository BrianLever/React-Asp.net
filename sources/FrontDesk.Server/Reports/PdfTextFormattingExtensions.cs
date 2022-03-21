using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace FrontDesk.Server.Reports
{
	public static class PdfTextFormattingExtensions
	{
		public static void PrintAsMainQuestionWithPreamble(this string questionWithPreamble, PdfPCell container, char delimiter = '|')
		{

			questionWithPreamble = (questionWithPreamble ?? string.Empty).Replace(delimiter, ' ');

			container.AddElement(new Paragraph(questionWithPreamble, PdfReport.labelFont));

		}
	}
}
