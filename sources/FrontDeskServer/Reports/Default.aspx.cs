using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FrontDesk.Server.Web;

public partial class ReportsDefaultForm : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Redirect("BhsIndexReport.aspx");
    }
}
