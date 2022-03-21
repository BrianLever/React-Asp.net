using FrontDesk.Common.Messages;
using FrontDesk.Server.Logging;
using FrontDesk.Server.Web;

using System;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;

public partial class LoginForm : BasePage
{
    private const string LICENSE_EXPIRATION_COOKIE_KEY = "LicenseExpiration";

    protected void Page_Load(object sender, EventArgs e)
    {
        Master.PageTitleText = "Login";

        if (User.Identity.IsAuthenticated)
        {
            Response.Redirect("~/Default.aspx");
        }
        FormsAuthentication.SignOut();
        Roles.DeleteCookie();

        var txtUsername = UserLogin.FindControl("Username") as TextBox;
        if (txtUsername != null) txtUsername.Focus();
        //UserLogin.Focus();
        var btnLogin = UserLogin.FindControl("Login") as Button;
        if (btnLogin != null)
            this.Form.DefaultButton = btnLogin.UniqueID;
    }

    protected void Login_Authenticate(object sender, AuthenticateEventArgs e)
    {
        //remove license expiration notification cookie
        if (Request.Cookies[LICENSE_EXPIRATION_COOKIE_KEY] != null)
        {
            HttpCookie c = new HttpCookie(LICENSE_EXPIRATION_COOKIE_KEY);
            c.Expires = DateTime.Now.AddDays(-1d);
            Response.Cookies.Add(c);
        }


        try
        {
            Session.Clear();
            Roles.DeleteCookie();

            var lblError = UserLogin.FindControl("lblError") as Label;


            if (Membership.ValidateUser(UserLogin.UserName, UserLogin.Password))
            {
                e.Authenticated = true;
                FormsAuthentication.RedirectFromLoginPage(UserLogin.UserName, false);
                //check for cycle reference
                System.Web.Security.MembershipUser user = System.Web.Security.Membership.GetUser(UserLogin.UserName);
                string url = FormsAuthentication.GetRedirectUrl(UserLogin.UserName, false);

                //add log record
                SecurityLog.Add((int)user.ProviderUserKey, DateTimeOffset.Now, SecurityEvents.LogIn, null);

                if (url.IndexOf("Login.aspx") >= 0 || url.IndexOf("Logout.aspx") >= 0)
                {
                    Response.Redirect("~/Default.aspx", true);
                }
                else
                {
                    e.Authenticated = false;
                }
            }
            else
            {
                //invalid login attempt
                e.Authenticated = false;
                lblError.Text = Resources.TextMessages.LoginFailedMessage;
            }
            /*
            if (!FDUser.IsLocked(UserLogin.UserName))
            {
                if (System.Web.Security.Membership.ValidateUser(UserLogin.UserName, UserLogin.Password))
                {
                    Roles.DeleteCookie();
                    e.Authenticated = true;
                    FormsAuthentication.RedirectFromLoginPage(UserLogin.UserName, false);
                    //check for cycle reference
                    System.Web.Security.MembershipUser user = System.Web.Security.Membership.GetUser(UserLogin.UserName);
                    string url = FormsAuthentication.GetRedirectUrl(UserLogin.UserName, false);
                    if (url.IndexOf("Login.aspx") >= 0 || url.IndexOf("Logout.aspx") >= 0)
                    {
                        Response.Redirect("~/Default.aspx", true);
                    }
                    else
                    {
                        e.Authenticated = false;
                    }
                }
                else
                {
                    e.Authenticated = false;
                    lblError.Text = "Failed to login. You have entered invalid username or password or your account has been blocked.";
                }
            }
            else
            {
                lblError.Text = "Unable to log you on because your account has been locked out, please contact your administrator.";
            }
            */
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            ErrorLog.AddServerException("Failed to login", ex);
            SetErrorAlert(CustomError.GetMessageForCustomOperation("perform", "authentication"), this.GetType());
        }
    }    
}
