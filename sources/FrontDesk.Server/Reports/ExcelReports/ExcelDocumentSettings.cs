using System.Drawing;
namespace FrontDesk.Server.Reports.ExcelReports
{
    public static class ExcelDocumentSettings
    {
        #region Report`s font and table parameters

        //Fonts
        public static float FooterFontSize = 8;


        //fonts
        public static Font TitleFont = new Font("Calibri", 18, FontStyle.Bold);
        public static Font TableCaptionFont = new Font("Calibri", 13, FontStyle.Bold);
        public static Font TableHeaderFontBold = new Font("Calibri", 12, FontStyle.Bold);
        public static Font DocumentPropertiesFont = new Font("Calibri", 12, FontStyle.Regular);
        public static Font TableTotalFont = new Font("Calibri", 12, FontStyle.Bold);

        #endregion
    }
}
