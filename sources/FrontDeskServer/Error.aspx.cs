using System;
using System.Web;
using FrontDesk.Server.Web;

/// <summary>
/// Summary description for start.
/// </summary>
public partial class ErrorForm : BasePage
{
    protected string ErrorStr;

    private void Page_Load(object sender, System.EventArgs e)
    {
        if (Request.QueryString["msg"] != null)
        {
            ErrorStr = Server.HtmlDecode(Server.UrlDecode(Request.QueryString["msg"]));
        }

        lblErrorMessage.Text = string.Format("Error has occurred during page request processing.<br><u>{0}</u>",
               HttpUtility.HtmlEncode(ErrorStr));
    }

    protected void Page_Error(object sender, EventArgs e)
    {
        Exception ex = Server.GetLastError();

        Response.Write(string.Format("Error has occurred during page request processing.<br><br><u>{0}</u>",
             HttpUtility.HtmlEncode(ex.InnerException == null ? ex.Message : ex.InnerException.Message)));
        Server.ClearError();
    }
}


