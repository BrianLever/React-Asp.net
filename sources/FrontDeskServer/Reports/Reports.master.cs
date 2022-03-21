using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ReportsMaster : System.Web.UI.MasterPage
{
    public string PageHeaderText { get { return Master.PageHeaderText; } set { Master.PageHeaderText = value; } }
    public string PageTitleText { get { return Master.PageTitleText; } set { Master.PageTitleText = value; } }

    protected void Page_Load(object sender, EventArgs e)
    {


    }

}
