using Common.Logging;

using System;
using System.Web;
using System.Web.Security;

/// <summary>
/// Global asax code behind
/// </summary>
public partial class Global : HttpApplication
{
    private ILog _logger = LogManager.GetLogger<Global>();

    void Application_Start(object sender, EventArgs e)
    {
        _logger.Info("Application_Start was called.");

        //var container = Startup.RegisterDependecies();
        //AutofacHostFactory.Container = container;

        //_logger.Info("Registered WCF IoC dependencies.");
    }

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown

    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs
        Exception ex = Server.GetLastError();

        if (ex is System.Security.Authentication.AuthenticationException)
        {
            Server.ClearError();
            Server.Transfer("~/Login.aspx");
        }
        else if (ex is System.Web.HttpException)
        {
            Exception exep = ex;
            while (exep.InnerException != null)
            {
                exep = exep.InnerException;
            }
            Server.ClearError();
            Response.Clear();

            _logger.Error("Internal server error", ex);

            FrontDesk.Server.Logging.ErrorLog.AddServerException("Internal server error", ex);
            Server.Transfer("~/error.aspx?msg=" + HttpUtility.UrlEncode(exep.Message));
        }
        else
        {
            Response.Clear();
            Server.ClearError();

            _logger.Error("Internal server error", ex);
            FrontDesk.Server.Logging.ErrorLog.AddServerException("Internal server error", ex);

            Server.Transfer("~/error.aspx");
        }
    }

    void Session_Start(object sender, EventArgs e)
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }

    void Application_BeginRequest(object sender, EventArgs e)
    {

    }

    void Application_EndRequest(object sender, EventArgs e)
    {
        #region Global account validation

        if (HttpContext.Current.User != null
            && HttpContext.Current.User.Identity.IsAuthenticated)
        {
            string changePasswordPage = "ChangePassword.aspx";
            string setupPasswordPage = "SetupPassword.aspx";
            string logoutPage = "Logout.aspx";

            MembershipUser user = Membership.GetUser();
            if (user != null)
            {
                if (Request.Path.IndexOf(logoutPage) < 0 &&
                    Request.Path.IndexOf(setupPasswordPage) < 0 &&
                    Request.Path.IndexOf(changePasswordPage) < 0)
                {
                    try
                    {
                        //Check for first time user
                        //Note, only first time users don't have security question and answer
                        if (String.IsNullOrEmpty(user.PasswordQuestion))
                        {
                            Response.Redirect("~/" + setupPasswordPage);
                        }

                        //Check for password expiration
                        if (FrontDesk.Server.FDUser.IsExpired(user))
                        {
                            Response.Redirect("~/" + changePasswordPage);
                        }
                    }
                    catch (System.Web.HttpException)
                    {
                        //do nothing
                    }
                }
            }
        }

        #endregion
    }
}