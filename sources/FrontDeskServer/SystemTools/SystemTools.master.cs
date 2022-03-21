using RPMS.Common.GlobalConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SystemToolsMaster : System.Web.UI.MasterPage
{
    private readonly IGlobalSettingsService _globalSettingsService = new GlobalSettingsService();

    private bool IsRpmsMode;

    public string PageHeaderText { get { return Master.PageHeaderText; } set { Master.PageHeaderText = value; } }
    public string PageTitleText { get { return Master.PageTitleText; } set { Master.PageTitleText = value; } }

    protected void Page_Init(object sender, EventArgs e)
    {
        IsRpmsMode = _globalSettingsService.IsRpmsMode;

        menuLeft.DataBound += MenuLeft_DataBound;
    }

    private void MenuLeft_DataBound(object sender, EventArgs e)
    {
        if (IsRpmsMode) return;

        MenuItem ehrPasswordItem = null;
        foreach (MenuItem item in menuLeft.Items)
        {
            if (item.NavigateUrl.EndsWith("RpmsCredentials.aspx"))
            {
                ehrPasswordItem = item;
                break;
            }
        }

        if(ehrPasswordItem != null)
        {
            menuLeft.Items.Remove(ehrPasswordItem);
        }

    }

    protected void Page_Load(object sender, EventArgs e)
    {


    }

}
