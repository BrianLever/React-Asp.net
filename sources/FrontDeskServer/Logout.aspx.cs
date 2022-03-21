using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using FrontDesk.Server.Web;

public partial class Logout : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Roles.DeleteCookie();
        FormsAuthentication.SignOut();
        //Response.Redirect("~/Login.aspx");
        FormsAuthentication.RedirectToLoginPage();
    }
}
