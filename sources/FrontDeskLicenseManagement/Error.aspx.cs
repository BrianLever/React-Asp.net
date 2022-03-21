using System;
using System.Web;
//using FrontDesk.Server.Web;
using FrontDesk.Server.LicenseManagerWeb;

/// <summary>
/// Summary description for start.
/// </summary>
public partial class ErrorForm : LMBasePage
{
    protected string ErrorStr;

    private void Page_Load(object sender, System.EventArgs e)
    {
        if (Request.QueryString["msg"] != null)
        {
            ErrorStr = Server.UrlDecode(Request.QueryString["msg"]);
        }
        else
        {
            Exception ex = Server.GetLastError();
            if (ex != null)
            {
                ErrorStr = (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message)) ? ex.InnerException.Message : ex.Message;
                Server.ClearError();
            }
        }

        if (!string.IsNullOrEmpty(ErrorStr))
        {
            ErrorStr = string.Format("<br><u>{0}</u>", HttpUtility.HtmlEncode(ErrorStr));
        }

        lblErrorMessage.Text = string.Format("Error has occurred during page request processing.{0}", ErrorStr);
    }

    protected void Page_Error(object sender, EventArgs e)
    {
        Exception ex = Server.GetLastError();

        Response.Write(string.Format("Error has occurred during page request processing.<br><br><u>{0}</u>",
             //HttpUtility.HtmlEncode(ex.InnerException == null ? ex.Message : ex.InnerException.Message)));
             HttpUtility.HtmlEncode((ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message)) ? ex.InnerException.Message : ex.Message)));
        Server.ClearError();
    }
}


