using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FrontDesk.Server.Web.Controls
{
    [DefaultProperty("Checked")]
    [ToolboxData("<{0}:YesNoLabel runat=server></{0}:YesNoLabel>")]
    public class YesNoLabel : WebControl
    {
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        public bool Checked
        {
            get
            {
                bool c = false;
                if (ViewState["Checked"] != null)
                {
                    c = (bool)ViewState["Checked"];
                }
                return c;
            }

            set
            {
                ViewState["Checked"] = value;
            }
        }
        [UrlProperty]
        [Category("Appearance")]
        [DefaultValue("~/images/checked.gif")]
        [Themeable(true)]
        public string YesImageUrl { get; set; }

        [UrlProperty]
        [Category("Appearance")]
        [DefaultValue("~/images/unchecked.gif")]
        [Themeable(true)]
        public string NoImageUrl { get; set; }

       
        /// <summary>
        /// Constructor with default values
        /// </summary>
        public YesNoLabel()
        {
            YesImageUrl = "~/images/checked.gif";
            NoImageUrl = "~/images/unchecked.gif";
        }

        protected override void Render(HtmlTextWriter writer)
        {
            //render image with yes or no image
            writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID);

            string imageUrl = this.Checked ? YesImageUrl : NoImageUrl;


            writer.AddAttribute(HtmlTextWriterAttribute.Src, Page.ResolveClientUrl(imageUrl));
            if (!string.IsNullOrEmpty(this.CssClass))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, this.CssClass);
            }
            writer.RenderBeginTag(HtmlTextWriterTag.Img);
            writer.RenderEndTag();
        }

       
    }
}
