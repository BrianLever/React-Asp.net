using System;
using System.Web.Security;
using System.Web.UI.WebControls;
using FrontDesk.Server;
using FrontDesk.Server.Membership;
using FrontDesk.Server.Screening.Services;
using FrontDesk.Server.Web;

public partial class management_UserList : BasePage
{
    private readonly IBranchLocationService _branchLocationService = new BranchLocationService();

    protected void Page_Init(object sender, EventArgs e)
    {
        // Enable obfuscation for TypeName="...". This overloads class name from .aspx
        // Need to protect (select) method name from obfuscation: unable to determine obfuscated name of method.
        // Method names and parameters may stay in .aspx. If named parameters used, protect whole datasource class from obfuscation.
        odsUsers.TypeName = typeof(FrontDesk.Server.Membership.FDMembershipUser).FullName;
        odsBranchLocation.TypeName = typeof(FrontDesk.Server.Screening.Services.BranchLocationService).FullName;

        RegisterGridViewForCustomPaging(gvUsers);
        if (!Roles.IsUserInRole(UserRoles.SuperAdministrator))
        {
            try
            {
                FDUser me = FDUser.GetCurrentUser();
                if (!me.BranchLocationID.HasValue)
                {
                    // actually must not get here, any other admin must belong to the branch location
                    RedirectToErrorPage("You must be assigned to a branch location.");
                }

                Master.PageHeaderText = string.Format("Users in {0}", _branchLocationService.Get(me.BranchLocationID.Value).Name);

                if (!IsPostBack)
                {
                    odsUsers.SelectParameters["branchLocationID"].DefaultValue = me.BranchLocationID.Value.ToString();
                }
            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                FrontDesk.Common.Debugging.DebugLogger.TraceException(ex);
                RedirectToErrorPage(FrontDesk.Common.Messages.CustomError.GetInternalErrorMessage());
            }
        }
    }

    protected override void SetControlsEnabledState()
    {
        base.SetControlsEnabledState();

        if (!Roles.IsUserInRole(UserRoles.SuperAdministrator))
        {
            //ddlBranchLocation.Visible = false;
            pnlFilter.Visible = false;
        }

    }

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            // Show all locations only to su, and only own location to others

            //if (me.UserName.Equals("su"))
            if (Roles.IsUserInRole(UserRoles.SuperAdministrator))
            {
                Master.PageHeaderText = "Users";
            }
            else
            {

            }
        }
        catch (Exception ex)
        {
            FrontDesk.Common.Debugging.DebugLogger.TraceException(ex);
            RedirectToErrorPage(FrontDesk.Common.Messages.CustomError.GetInternalErrorMessage());
        }
    }

    protected void ddlBranchLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlBranchLocation.SelectedValue == "0")
        {
            odsUsers.SelectParameters["branchLocationID"].DefaultValue = String.Empty;
        }
        else
        {
            odsUsers.SelectParameters["branchLocationID"].DefaultValue = ddlBranchLocation.SelectedValue;
        }

        odsUsers.DataBind();
    }

    protected void gvUsers_Sorting(object sender, GridViewSortEventArgs e)
    {
        this.OnGridViewSorting(sender, e);
    }

    protected void lnbNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/UserDetails.aspx");
    }
}
