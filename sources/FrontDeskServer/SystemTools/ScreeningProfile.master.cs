using FrontDesk;
using FrontDesk.Server.Web;

using System;
using System.Web.UI;

public partial class ScreeningProfileMaster : System.Web.UI.MasterPage
{
    public string PageHeaderText { get { return Master.PageHeaderText; } set { Master.PageHeaderText = value; } }
    public string PageTitleText { get { return Master.PageTitleText; } set { Master.PageTitleText = value; } }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Page_Init(object sender, EventArgs e)
    {
        Page.ClientScript.RegisterClientScriptInclude("screeningProfileMasterCtrl.js", ResolveClientUrl("~/scripts/controls/screeningProfileMasterCtrl.js"));
    }

    /// <summary>
    /// redirect to parent page with list
    /// </summary>
    public void RedirectToList()
    {
        Response.Redirect("ScreenProfileList.aspx", false);
    }

    protected int ScreeningProfileId
    {
        get {
            var formPage = Page as BaseManagementWebForm<ScreeningProfile, int>;

            if(formPage != null)
            {
                formPage.EnsureFormObjectCreated();

                return formPage.formObjectID;
            }
            return 0;
        }
    }
    

}
