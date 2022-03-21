using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Server.Printouts
{
    public class TableColumnDefinition
    {
        public string Name { get; set; }
        public int HorizontalAlignment { get; set; } = Element.ALIGN_LEFT;

    }
}
