using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FrontDesk.Server.Web;
using System.Web.UI.HtmlControls;
using System.Data;
using FrontDesk.Server;
using System.Web.Security;
using FrontDesk.Server.Membership;

public partial class management_ScreenProfile : BasePage
{
    protected void Page_Init(object sender, EventArgs e)
    {

        odsItems.TypeName = typeof(FrontDesk.Server.Screening.Services.ScreeningProfileService).FullName;

        RegisterGridViewForCustomPaging(gvItems);
        gvItems.RowDataBound += new GridViewRowEventHandler(gvItems_RowDataBound);
    }

    private void gvItems_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            var dataItem = e.Row.DataItem as BranchLocation;
            if (dataItem != null)
            {
                if (dataItem.Disabled) e.Row.CssClass = (e.Row.RowIndex % 2 > 0 ? "alt" : "") + " disabled";
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Master.PageHeaderText = "Screen Profile List";
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        gvItems.DataBind();
    }

    protected void lnbNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/systemtools/ScreenProfile.aspx");
    }

    protected void ucFilter_Searching(object sender, FilterSearchingEventArgs e)
    {
        if (!String.IsNullOrEmpty(e.FilterBy))
        {
            odsItems.SelectParameters["filterByName"].DefaultValue = e.Value.ToString();
        }
        else
        {
            odsItems.SelectParameters["filterByName"].DefaultValue = String.Empty;
        }

        gvItems.PageIndex = 0;
        gvItems.DataBind();
    }

    protected override void SetPagePermissions()
    {
        base.SetPagePermissions();

        if (Roles.IsUserInRole(UserRoles.SuperAdministrator) || Roles.IsUserInRole(UserRoles.BranchAdministrator))
        {
            ReadPermission = true;
            WritePermission = true;
            PrintPermission = true;
            DeletePermission = true;
        }
        else
        {
            ReadPermission = false;
            WritePermission = false;
            PrintPermission = false;
            DeletePermission = false;

            Response.Redirect("~/Default.aspx");
        }

    }

}
