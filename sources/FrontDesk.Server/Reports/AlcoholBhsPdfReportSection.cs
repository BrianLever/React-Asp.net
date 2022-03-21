using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text.pdf;

namespace FrontDesk.Server.Reports
{
    public class AlcoholBhsPdfReportSection : BhsPdfReportSectionBase
    {
        public AlcoholBhsPdfReportSection(PdfReport report) : base(report)
        {
        }

        public override string ScreeningSectionID { get { return ScreeningSectionDescriptor.Alcohol; ; } }
        public override float[] TableColumnWidths { get { return new float[] { 0.04f, 0.8f, 0.08f, 0.08f }; } }

    }
}
