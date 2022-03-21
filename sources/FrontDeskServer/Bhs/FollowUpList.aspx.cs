using FrontDesk.Server.Web;

using System;

public partial class BhsFollowUpListForm : BasePage 
{
    protected override void SetPagePermissions()
    {
        base.SetPagePermissions();

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Master.PageHeaderText = "Follow-Up List";
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
