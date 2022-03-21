using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Security.Permissions;
using FrontDesk.Server.Resources;
using System.Reflection;

[assembly: WebResource("FrontDesk.Server.Web.scripts.AsyncUpdateIndicator.js", "text/javascript")]
[assembly: WebResource("FrontDesk.Server.Web.images.loader.gif", "image/gif")]

namespace FrontDesk.Server.Web.Controls
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:AsyncUpdateIndicator runat=server></{0}:AsyncUpdateIndicator>")]
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [Obfuscation(Feature = "renaming", Exclude = true, ApplyToMembers = true)]
    public class AsyncUpdateIndicator : WebControl
    {

        UpdateProgress uppProgress = null;

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Text
        {
            get
            {
                String s = (String)ViewState["Text"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["Text"] = value;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            //register script
            //this.Page.ClientScript.RegisterClientScriptInclude(this.GetType(), "__AsyncUpdateIndicator.js",
            //              Page.ClientScript.GetWebResourceUrl(typeof(AsyncUpdateIndicator), "CityBoard.IMN.Web.scripts.AsyncUpdateIndicator.js"));

            Page.ClientScript.RegisterClientScriptResource(typeof(AsyncUpdateIndicator), "FrontDesk.Server.Web.scripts.AsyncUpdateIndicator.js");
            base.OnLoad(e);
            
        }
        

        protected override void RenderContents(HtmlTextWriter output)
        {
            output.Write(Text);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            uppProgress = new UpdateProgress();
            uppProgress.ID = "uppPickListProgress";
            uppProgress.DisplayAfter = 400;

            // Create the ProgressTemplate based on a class the implements ITemplate.
            EmptyUpdateProgressTemplate value = new EmptyUpdateProgressTemplate();
            uppProgress.ProgressTemplate = value;



            HtmlGenericControl progressBar = new HtmlGenericControl("DIV");
            progressBar.ID = "progressBar";
            progressBar.Attributes.Add("class", "progressBar");

            var progressBarText = new HtmlGenericControl("DIV");
            progressBarText.ID = "progressBarText";
            progressBarText.Attributes.Add("class", "progressBarText");
            

            var imgLoader = new Image();
            //imgLoader.ID = "imgLoader";
            imgLoader.ImageUrl = this.Page.ClientScript.GetWebResourceUrl(typeof(AsyncUpdateIndicator), "FrontDesk.Server.Web.images.loader.gif");
            imgLoader.AlternateText = string.Empty;
            imgLoader.EnableViewState = false;
            progressBarText.Controls.Add(imgLoader);

            //add text
            var ltrUpdating = new LiteralControl(TextMessages.Async_Updating_Text);
            progressBarText.Controls.Add(ltrUpdating);

            var progressBarTextSmall = new HtmlGenericControl("P");
            progressBarTextSmall.Controls.Add(new LiteralControl(TextMessages.Async_Updating_Wait_Text));
            progressBarTextSmall.ID = "progressBarTextSmall";
            progressBarTextSmall.Attributes.Add("class", "progressBarTextSmall");
            
            progressBarText.Controls.Add(progressBarTextSmall);

            progressBar.Controls.Add(progressBarText);
            uppProgress.Controls.Add(progressBar);

          
            Page.ClientScript.RegisterStartupScript(typeof(AsyncUpdateIndicator), "__initAsyncHandlers",
                "$addHandler(window, \"load\", initAsyncHandlers);", true);

            this.Controls.Add(uppProgress);

        }

        protected override void Render(HtmlTextWriter writer)
        {
            //base.Render(writer);
            uppProgress.RenderControl(writer);
        }
    }


    public class EmptyUpdateProgressTemplate : ITemplate
    {
        public void InstantiateIn(Control container)
        {
            // This is empty.
        }
    }
   
}
