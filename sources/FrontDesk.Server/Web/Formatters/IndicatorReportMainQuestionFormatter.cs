using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Server.Web.Formatters
{
    public static class IndicatorReportMainQuestionFormatter
    {
        public static string FormatAsQuestionWithPreamble(this string questionWithPreambleText)
        {
            if (string.IsNullOrEmpty(questionWithPreambleText)) return questionWithPreambleText;

            if (!questionWithPreambleText.Contains("|"))
            {
                return questionWithPreambleText;
            }
            var slices = questionWithPreambleText.Split('|');
            if (slices.Length == 1)
            {
                return slices[0];
            }

            return @"<span>{0} {1}</span>".FormatWith(slices[0], slices[1]);
        }
    }
}
