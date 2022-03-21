using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Permissions;
using System.ComponentModel;


namespace FrontDesk.Server.Web.Controls
{
    /// <summary>
    /// Text Form Label on the web form
    /// </summary>
    [ToolboxItem(true)]
    [Themeable(true)]
    [AspNetHostingPermissionAttribute(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [ToolboxData("<{0}:FormLabel Text=\"Label\" runat=\"server\" />")]
    public class FormLabel : Label
    {


        public FormLabel()
        {
            this.CssClass = "fieldLabel";
           
        }

        private bool _Mandatory = false;

        /// <summary>
        /// If true the Asterix added after the label text
        /// </summary>
        [Themeable(true)]
        public bool Mandatory
        {
            get { return _Mandatory; }
            set { _Mandatory = value; }
        }

        private bool _DisplayColon = true;
        /// <summary>
        /// If true the colon added after the label text and mandatory mark
        /// </summary>
        [Themeable(true)]
        public bool DisplayColon
        {
            get { return _DisplayColon; }
            set { _DisplayColon = value; }
        }

        private ColonPosition _colonPosition = ColonPosition.AfterAsterix;

        /// <summary>
        /// Position of the colon
        /// </summary>
        [Themeable(true)]
        public ColonPosition ColonPosition
        {
            get { return _colonPosition; }
            set { _colonPosition = value; }
        }


        private string _MandatorySymbol = "*";

        /// <summary>
        /// Mandatory symbol. By default it's asterix
        /// </summary>
        [Themeable(true)]
        public string MandatorySymbol
        {
            get { return _MandatorySymbol; }
            set { _MandatorySymbol = value; }
        }

        private string _MandatorySymbolCssClass = "ast";

        /// <summary>
        /// Css class for mandatory symbol
        /// </summary>
        [Themeable(true)]
        public string MandatorySymbolCssClass
        {
            get { return _MandatorySymbolCssClass; }
            set { _MandatorySymbolCssClass = value; }
        }


        //private string _CssClass = String.Empty;

        ///// <summary>
        ///// Css class for mandatory symbol
        ///// </summary>
        //[Themeable(true)]
        //public new string CssClass
        //{
        //    get { return _CssClass; }
        //    set { _CssClass = value; }
        //}
        /// <summary>
        /// Set default styles
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (String.IsNullOrEmpty(MandatorySymbolCssClass))
            {
                if (!Page.ClientScript.IsClientScriptBlockRegistered("formlabel_ast_css"))
                {

                    //create default style
                    Style astStyle = new Style();
                    astStyle.ForeColor = System.Drawing.Color.Red;
                    astStyle.Font.Size = FontUnit.Point(4);
                    astStyle.Font.Name = "Verdana";
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "formlabel_ast_css", string.Empty);

                    this.Page.Header.StyleSheet.CreateStyleRule(astStyle, null, ".ast");
                }
            }
           

        }

        /// <summary>
        /// Render addtional text (colon and asterix)
        /// </summary>
        /// <param name="writer"></param>
        public override void RenderEndTag(HtmlTextWriter writer)
        {

            if (_DisplayColon && _colonPosition == ColonPosition.BeforeAsterix)
            {
                writer.Write(":");
            }
            RenderMandatorySymbol(writer);
            if (_DisplayColon && _colonPosition == ColonPosition.AfterAsterix)
            {
                writer.Write(":");
            }



            base.RenderEndTag(writer);
        }
        ///// <summary>
        ///// Render control
        ///// </summary>
        ///// <param name="writer"></param>
        //public override void RenderControl(HtmlTextWriter writer)
        //{
        //    //if (this.Visible)
        //    //{
        //    //if (!String.IsNullOrEmpty(CssClass))
        //    //{
        //    //    writer.AddAttribute(HtmlTextWriterAttribute.Class, CssClass);
        //    //    writer.RenderBeginTag(HtmlTextWriterTag.Span);
        //    //}

        //    base.RenderControl(writer);
        //    //if (!String.IsNullOrEmpty(CssClass))
        //    //{
        //    //    writer.RenderEndTag();
        //    //}


        //    //}

        //}

        //protected override void RenderChildren(HtmlTextWriter writer)
        //{
        //    base.RenderChildren(writer);


        //}

        /// <summary>
        /// Render Mandatory symbol if needed
        /// </summary>
        /// <param name="writer"></param>
        private void RenderMandatorySymbol(HtmlTextWriter writer)
        {
            if (_Mandatory)
            {
                if (!String.IsNullOrEmpty(MandatorySymbolCssClass))
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, MandatorySymbolCssClass);
                }
                else
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, MandatorySymbolCssClass);
                    writer.AddStyleAttribute(HtmlTextWriterStyle.VerticalAlign, "super");
                }
                writer.RenderBeginTag(HtmlTextWriterTag.Span);
                writer.Write(this._MandatorySymbol);
                writer.RenderEndTag();
            }
        }

    }
    /// <summary>
    /// Colon position
    /// </summary>
    public enum ColonPosition
    {
        /// <summary>
        /// Right from mandatory symbol
        /// </summary>
        AfterAsterix = 0,
        /// <summary>
        /// Left from mandatory symbol
        /// </summary>
        BeforeAsterix = 1
    }
}
