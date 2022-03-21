using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using FrontDesk.Server.Web;
using System.Security.Principal;
using FrontDesk.Server.LicenseManagerWeb;

public partial class _Default : LMBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //IPrincipal me = User;
        //lblUser.Text = User.Identity.Name;      
        Response.Redirect("Clients.aspx");
    }
}
