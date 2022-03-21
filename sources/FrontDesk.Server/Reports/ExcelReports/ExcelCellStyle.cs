using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontDesk.Server.Reports.ExcelReports
{

    public class ExportCellStyle
    {
        public string FontName { get; set; }
        public double? FontSize { get; set; }
        /// <summary>
        /// RGB color: #RRGGBB
        /// </summary>
        public string FontColor { get; set; }

        /// RGB color: #RRGGBB
        /// </summary>
        public bool? FontIsBold { get; set; }

        /// <summary>
        /// RGB color: #RRGGBB
        /// </summary>
        public string BackgroundColor { get; set; }

        public Border Borders { get; set; }

        public string RegisteredStyleID { get; set; }

        public HorizontalCellAllignment HorizontalAlign { get; set; }
        public VerticalCellAllignment VerticalAlign { get; set; }

        public static ExportCellStyle DefaultStyle
        {
            get
            {
                ExportCellStyle defStyle = new ExportCellStyle();
                defStyle.FontColor = "#000000";
                defStyle.Borders = Border.None;
                defStyle.RegisteredStyleID = "Default";
                return defStyle;
            }
        }

        /// <summary>
        /// Create copy of the object. RegisteredStyleID value is not propagated to the clone object
        /// </summary>
        /// <returns></returns>
        public ExportCellStyle Clone()
        {
            ExportCellStyle clone = new ExportCellStyle();
            clone.FontName = this.FontName;
            clone.FontSize = this.FontSize;
            clone.FontColor = this.FontColor;
            clone.FontIsBold = this.FontIsBold;
            clone.BackgroundColor = this.BackgroundColor;
            clone.Borders = this.Borders;
            clone.HorizontalAlign = this.HorizontalAlign;
            clone.VerticalAlign = this.VerticalAlign;

            return clone;
        }

    }


    public enum HorizontalCellAllignment
    {
        NotSet = 0,
        Left,
        Center,
        Right
    }
    public enum VerticalCellAllignment
    {
        NotSet = 0,
        Top,
        Center,
        Bottom
    }

    [Flags]
    public enum Border
    {
        None = 0,
        Left = 1,
        Top = 2,
        Right = 4,
        Bottom = 8,
        All = Left | Top | Right | Bottom
    }
}
