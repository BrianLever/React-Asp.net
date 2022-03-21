using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FrontDesk.Server.Web;

public partial class _Default :BasePage 
{
    protected override void SetPagePermissions()
    {
        base.SetPagePermissions();

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Master.PageHeaderText = "Patient Check-In Screen";
    }

    public override void SetTabIndex()
    {
        base.SetTabIndex();

        short tabIndex = 1;
        ucCheckInList.ApplyTabIndexToControl(ref tabIndex);

        this.Form.DefaultButton = ucCheckInList.DefaultSearchButton.UniqueID;

        if (!IsPostBack)
        {
            ucCheckInList.Focus();
        }
    }
}
