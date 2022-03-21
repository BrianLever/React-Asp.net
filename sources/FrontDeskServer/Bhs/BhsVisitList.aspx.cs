using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using FrontDesk.Server.Membership;
using FrontDesk.Server.Web;

public partial class BhsVisitListForm :BasePage 
{
    protected override void SetPagePermissions()
    {
        base.SetPagePermissions();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Master.PageHeaderText = "Visit List";
    }

    public override void SetTabIndex()
    {
        base.SetTabIndex();

        short tabIndex = 1;
        ucList.ApplyTabIndexToControl(ref tabIndex);

        this.Form.DefaultButton = ucList.DefaultSearchButton.UniqueID;

        if (!IsPostBack)
        {
            ucList.Focus();
        }
    }
}
