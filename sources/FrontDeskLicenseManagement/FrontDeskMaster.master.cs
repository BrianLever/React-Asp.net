using System;
using System.Web.UI;
using FrontDesk.Server.LicenseManagerWeb;
using System.Reflection;
using FrontDesk;

public partial class FrontDeskMaster : System.Web.UI.MasterPage
{

    #region Header and Title

    [Obfuscation(Feature = "renaming", Exclude = true)]
    protected virtual string PageTitlePostfix
    {
        get
        {
            return FrontDesk.Common.Configuration.AppSettingsProxy.GetStringValue("PageTitlePostfix", 
                "ScreenDox - License Management");
        }
    }

    protected string _PageTitleText;
    /// <summary>
    /// Page Title (displays in the browser title. Don't use this poperty directly - use PageHeaderText instead
    /// </summary>
    public string PageTitleText
    {
        get
        {
            return this._PageTitleText;
        }
        set
        {
            this._PageTitleText = string.Format("{0} :: {1}", value, PageTitlePostfix);
            this.Page.Header.Title = this._PageTitleText;
        }
    }


    protected string _PageHeaderText = string.Empty;
    /// <summary>
    /// Page Header Text, Synchronized with a Page Title
    /// </summary>
    public string PageHeaderText
    {
        get { return Server.HtmlEncode(this._PageHeaderText); }
        set
        {
            this._PageHeaderText = value;
            //sync title
            this.PageTitleText = value;
        }
    }

    #endregion

    #region Logged User Properties
    /// <summary>
    /// get Currently Logged user name
    /// </summary>
    public string LoggedUserName
    {
        get
        {
            var name = String.Empty;
            
            //var user = FDUser.GetCurrentUser();
            //if (user != null)
            //{
            //    name = user.FullName;
            //}
            //else
            //{
            //    name = Page.User.Identity.Name;
            //}
            name = Page.User.Identity.Name;
            return name;
        }
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        Menu1.Visible = Page.User.Identity.IsAuthenticated;
        //LoginStatus1.Visible = Page.User.Identity.IsAuthenticated;

        Page.ClientScript.RegisterClientScriptInclude("misc.js", ResolveClientUrl("~/scripts/misc.js"));
        Page.ClientScript.RegisterClientScriptInclude("common.js", ResolveClientUrl("~/scripts/common.js"));

        Page.ClientScript.RegisterStartupScript(this.GetType(), "_onresize", "onResize();", true);
        //disable cache

        scriptManager.AsyncPostBackError += new EventHandler<AsyncPostBackErrorEventArgs>(scriptManager_AsyncPostBackError);
        ShowIe6UpgradeMessage();
    }

    /// <summary>
    /// check if user uses IE6 and show message to upgrade browser
    /// </summary>
    private void ShowIe6UpgradeMessage()
    {
        System.Web.HttpBrowserCapabilities browser = Request.Browser;
        if (browser.Type.ToUpper() == "IE6")
        {
            string script = string.Format(@"
var h = $get('header'); if(h != null){{
var d= document.createElement('div');
d.className='updateBrowser';
d.innerHTML=""{0}"";
h.appendChild(d);
}}
", Resources.TextMessages.Warning_UpgradeBrowser);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "upgradeie6", script, true);
        }

    }

    //protected void hlLogin_Load(object sender, EventArgs e)
    //{
    //    (sender as HyperLink).Visible = Request.Path.IndexOf("Login.aspx") < 0;
    //}

    void scriptManager_AsyncPostBackError(object sender, AsyncPostBackErrorEventArgs e)
    {
        FrontDesk.Common.Debugging.DebugLogger.TraceException(e.Exception);
        if (String.IsNullOrEmpty(scriptManager.AsyncPostBackErrorMessage))
        {
            scriptManager.AsyncPostBackErrorMessage = FrontDesk.Common.Messages.CustomError.GetInternalErrorMessage();
        }
    }

    public string AppVersion
    {
        get
        {
            return ((LMBasePage)Page).CurrentAppVersion;
            
            //string version = "0.0.0.0";

            //var assemblyName = System.Reflection.Assembly.GetCallingAssembly().GetName();
            //version = assemblyName.Version.ToString();

            //return version;
        }
    }

   

    /// <summary>
    /// Get Company name from web.config
    /// </summary>
    public string ApplicationName
    {
        get
        {
            return FrontDesk.Server.Configuration.AppSettings.ApplicationName;
        }
    }
    protected string GetCurrentYear()
    {
        return "{0:yyyy}".FormatWith(DateTime.Today);
    }
}
