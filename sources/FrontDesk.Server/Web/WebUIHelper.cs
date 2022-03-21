using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;


namespace FrontDesk.Web.Helpers
{/// <summary>
    /// Contains Help methods for Web Controls
    /// </summary>
    public class WebUIHelper
    {
        public WebUIHelper()
        {

        }

        #region Merge CSS classes
        /// <summary>
        /// Merge Styles and CSS classes
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="style"></param>
        public static void AppendStyle(TableCell cell, Style style)
        {
            cell.MergeStyle(style);

            if (!String.IsNullOrEmpty(style.CssClass))
            {
                AppendClass(cell, style.CssClass);
            }

        }

        public static void AppendClass(TableCell cell, string className)
        {
            Regex regex = new Regex(string.Format("^.* ?{0}( .*)?$", className));

            if (!regex.IsMatch(cell.CssClass))
            {
                cell.CssClass = AppendClass(cell.CssClass, className);
            }
        }


        public static void AppendClass(TableRow row, string className)
        {
            Regex regex = new Regex(string.Format("^.* ?{0}( .*)?$", className));

            row.CssClass = AppendClass(row.CssClass, className);


        }

        /// <summary>
        /// Appends new class name to the end of existing class name string. Method doesn't appended class is he alredy existed 
        /// </summary>
        /// <param name="originalClassName"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        public static string AppendClass(string originalClassName, string className)
        {
            Regex regex = new Regex(string.Format("^.* ?{0}( .*)?$", className));

            if (!regex.IsMatch(originalClassName)) //!cell.CssClass.Contains(className))
            {
                if (!string.IsNullOrEmpty(originalClassName))
                {
                    originalClassName += " ";
                }
                originalClassName += className;
            }
            return originalClassName;
        }



        public static void AppendHorizontalAligmentClass(TableCell cell, HorizontalAlign align)
        {

            string className = "";
            if (align == HorizontalAlign.Right) className = "r";
            else if (align == HorizontalAlign.Left) className = "l";
            else if (align == HorizontalAlign.Center) className = "c";
            else if (align == HorizontalAlign.Justify) className = "j";
            AppendClass(cell, className);

        }


        #endregion

        #region Register control's css 
        /// <summary>
        /// Register reference to css file only once
        /// </summary>
        /// <param name="control"></param>
        /// <param name="key"></param>
        /// <param name="href"></param>
        public static void RegisterCssStyle( Control control,string key, string href)
        {

             string dummyScriptKey = control.GetType() + "_" + key;
            //check if CSS is already registered
             if (!control.Page.ClientScript.IsClientScriptBlockRegistered(dummyScriptKey))
             {
                 control.Page.ClientScript.RegisterClientScriptBlock(control.GetType(), dummyScriptKey, string.Empty);

                 HtmlLink cssLink = new HtmlLink();
                 cssLink.Href = control.ResolveClientUrl(href);
                 cssLink.Attributes.Add("rel", "stylesheet");
                 cssLink.Attributes.Add("type", "text/css");

                 control.Page.Header.Controls.Add(cssLink);
             }
        }

        #endregion
    }
}