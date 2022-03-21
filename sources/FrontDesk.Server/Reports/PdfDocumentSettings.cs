using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace FrontDesk.Server.Reports
{
    public static class PdfDocumentSettings
    {
        public static Font headerFont = FontFactory.GetFont(BaseFont.HELVETICA, 13, Font.BOLD);
        public static Font headerSubFont = FontFactory.GetFont(BaseFont.HELVETICA, 12, Font.NORMAL);

        public static Font labelFont = FontFactory.GetFont(BaseFont.HELVETICA, 8, Font.NORMAL);
        public static Font valueFont = FontFactory.GetFont(BaseFont.HELVETICA, 8, Font.NORMAL, new Color(0x3D, 0x56, 0xA7));
        public static Font headerBlueFont = FontFactory.GetFont(BaseFont.HELVETICA, 8, Font.BOLD, new Color(0x3D, 0x56, 0xA7));
        public static Font boldFont = FontFactory.GetFont(BaseFont.HELVETICA, 8, Font.BOLD);
        public static Font sectionTitleFont = FontFactory.GetFont(BaseFont.HELVETICA, 9, Font.BOLD, Color.WHITE);
        public static Font sectionTitleBigFont = FontFactory.GetFont(BaseFont.HELVETICA, 10, Font.BOLD, Color.WHITE);
        public static Font commentFont = FontFactory.GetFont(BaseFont.HELVETICA, 6, Font.ITALIC);
        public static Font preambleFont = FontFactory.GetFont(BaseFont.HELVETICA, 9, Font.BOLD | Font.ITALIC);
        public static Font depresionCommentFont = FontFactory.GetFont(BaseFont.HELVETICA, 6, Font.NORMAL);
        public static Font depresionCommentBFont = FontFactory.GetFont(BaseFont.HELVETICA, 6, Font.BOLD);
        public static Font symbolFont;

        public static Color blueBackground = new Color(0xAD, 0xD5, 0xF2);
        public static Color grayBackground = new Color(0xe0, 0xe0, 0xe0);
        public static Color whiteBackground = Color.WHITE;

        public static Color HeaderBackground = new Color(0xBC, 0xBC, 0xBC);
        public static Color NormalRowBackground = new Color(0xFF, 0xFF, 0xFF);
        public static Color AltRowBackground = new Color(0xEE, 0xEE, 0xEE);
        public static Color GreenBackground = new Color(0xbe, 0xd5, 0xaa);
        public static Color GrayBorder = new Color(0xee, 0xee, 0xee);

        public static string PathToFooterLogo = System.Web.Hosting.HostingEnvironment.MapPath(System.Web.Configuration.WebConfigurationManager.AppSettings["LogoPath"]);
        public static string PathToHeaderLogo = System.Web.Hosting.HostingEnvironment.MapPath(System.Web.Configuration.WebConfigurationManager.AppSettings["HeaderLogoPath"]);


        public static float FooterFontSize = 8;
    }
}
