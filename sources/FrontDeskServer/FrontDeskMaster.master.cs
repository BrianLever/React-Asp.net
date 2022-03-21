using System;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using FrontDesk.Server;
using FrontDesk.Server.Licensing.Services;
using FrontDesk.Server.Membership;
using FrontDesk.Server.Web;
using System.Text;
using System.Web.UI.HtmlControls;
using FrontDesk;

public partial class FrontDeskMaster : System.Web.UI.MasterPage
{

    #region Header and Title

    [Obfuscation(Feature = "renaming", Exclude = true)]
    protected virtual string PageTitlePostfix
    {
        get
        {
            return FrontDesk.Common.Configuration.AppSettingsProxy.GetStringValue("PageTitlePostfix", "ScreenDox - Health Behavioral Screener");
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

            var user = FDUser.GetCurrentUser();
            if (user != null)
            {
                name = user.FullName;
            }
            else
            {
                name = Page.User.Identity.Name;
            }
            return name;
        }
    }

    #endregion

    protected void Page_Init(object sender, EventArgs e)
    {
        Page.ClientScript.RegisterClientScriptInclude("misc.js", ResolveClientUrl("~/scripts/misc.js"));
        Page.ClientScript.RegisterClientScriptInclude("jquery-1.9.1.min.js", ResolveClientUrl("~/scripts/jquery-1.9.1.min.js"));
        Page.ClientScript.RegisterClientScriptInclude("jquery-migration.js", ResolveClientUrl("~/scripts/jquery-migration.js"));

        Page.ClientScript.RegisterClientScriptInclude("common.js", ResolveClientUrl("~/scripts/common.js"));

        //Page.ClientScript.RegisterClientScriptInclude("jquery-ui-1.10.3.min.js", ResolveClientUrl("~/scripts/jquery-ui-1.10.3.min.js"));
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Menu1.Visible = Page.User.Identity.IsAuthenticated;
        LoginStatus1.Visible = Page.User.Identity.IsAuthenticated;

        Page.ClientScript.RegisterStartupScript(this.GetType(), "_onresize", "onResize();", true);
        //disable cache

        scriptManager.AsyncPostBackError += new EventHandler<AsyncPostBackErrorEventArgs>(scriptManager_AsyncPostBackError);

        ShowIe6UpgradeMessage();

        SetLicenseExpirationNotification();
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

    protected void hlLogin_Load(object sender, EventArgs e)
    {
        (sender as HyperLink).Visible = Request.Path.IndexOf("Login.aspx") < 0;
    }

    void scriptManager_AsyncPostBackError(object sender, AsyncPostBackErrorEventArgs e)
    {
        FrontDesk.Common.Debugging.DebugLogger.TraceException(e.Exception);
        if (String.IsNullOrEmpty(scriptManager.AsyncPostBackErrorMessage))
        {
            scriptManager.AsyncPostBackErrorMessage = FrontDesk.Common.Messages.CustomError.GetInternalErrorMessage();
        }
    }

    private const string LICENSE_EXPIRATION_COOKIE_KEY = "LicenseExpiration";


    private void SetLicenseExpirationNotification()
    {


        if (Request.Path.ToLower().EndsWith("login.aspx")) return; //ignore login page


        LicenseCertificate certificate = LicenseService.Current.GetLicenseCertificate();
        DateTime now = DateTime.Now.Date;

        if (certificate != null &&
            (certificate.ExpirationDate.HasValue && certificate.ExpirationDate.Value.Date > now))
        {
            phLicenseExpiration.Visible = true;
            int[] LicenseExpirationNotificationDays = { 90, 60, 55, 50, 45, 40, 35, 30 };
            int daysBeforeExpiration = Convert.ToInt32(certificate.ExpirationDate.Value.Date.Subtract(now).TotalDays);


            phLicenseExpiration.Visible = Page.User.Identity.IsAuthenticated
                && (LicenseExpirationNotificationDays.Contains(daysBeforeExpiration) || daysBeforeExpiration <= 30)
                && Request.Cookies[LICENSE_EXPIRATION_COOKIE_KEY] == null;

            if (phLicenseExpiration.Visible)
            {
                lblExpirationText.Text = String.Format(Resources.TextMessages.LicenseExpirationText, daysBeforeExpiration);
                hlRenewLicense.Visible = Roles.IsUserInRole(UserRoles.SuperAdministrator);
            }
        }
        else
        {
            phLicenseExpiration.Visible = false;
        }
    }


    protected void OnExpirationNotificationClose(object sender, EventArgs e)
    {
        Response.AppendCookie(new HttpCookie(LICENSE_EXPIRATION_COOKIE_KEY));
        phLicenseExpiration.Visible = false;
    }

    public string AppVersion
    {
        get
        {
            return ((BasePage)Page).CurrentAppVersion;
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

    protected override void OnPreRender(EventArgs e)
    {

        var themePath = String.Format("~/App_Themes/{0}/", Page.StyleSheetTheme);
        //set version to app theme files
        foreach (Control oLink in Page.Header.Controls)
        {
            if (oLink is HtmlLink)
            {
                HtmlLink cssLink = oLink as HtmlLink;
                //Check if CSS link 
                if (String.Compare(cssLink.Attributes["type"], "text/css", StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    if (cssLink.Attributes["href"] != null && cssLink.Attributes["href"].Contains(themePath))
                    {
                        //add a version of your app here.                                                                
                        cssLink.Attributes["href"] += "?v." + AppVersion;
                    }
                }
            }
            base.OnPreRender(e);
        }
    }
    protected string GetCurrentYear()
    {
        return "{0:yyyy}".FormatWith(DateTime.Today);
    }
}
