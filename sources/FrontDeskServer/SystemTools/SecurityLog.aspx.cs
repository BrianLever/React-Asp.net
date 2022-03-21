using System;
using System.Data;
using System.Text;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using FrontDesk.Server.Configuration;
using FrontDesk.Server.Logging;
using FrontDesk.Server.Membership;
using FrontDesk.Server.Web;

public partial class SystemTools_SecurityLog : BasePage
{
    
    int totalRowsCount = 0;

    protected void Page_Init(object sender, EventArgs e)
    {
        // Enable obfuscation for TypeName="...". This overloads class name from .aspx
        // Need to protect (select) method name from obfuscation: unable to determine obfuscated name of method.
        // Method names and parameters may stay in .aspx. If named parameters used, protect whole datasource class from obfuscation.
        odsLog.TypeName = typeof(FrontDesk.Server.Logging.SecurityLog).FullName;

        RegisterGridViewForCustomPaging(gvLog);

        odsLog.Selecting += new ObjectDataSourceSelectingEventHandler(odsLog_Selecting);
        odsLog.Selected += new ObjectDataSourceStatusEventHandler(odsLog_Selected);
        ddlCategory.SelectedIndexChanged += new EventHandler(ddlCategory_SelectedIndexChanged);
        gvLog.Sorting += new GridViewSortEventHandler(gvLog_Sorting);
        gvLog.PageIndexChanging += new GridViewPageEventHandler(gvLog_PageIndexChanging);
    }

    protected void odsLog_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["isSA"] = Roles.IsUserInRole(UserRoles.SuperAdministrator);

        string evID = Request.Form[ddlEvent.UniqueID];
        e.InputParameters["eventID"] = clearPerformed ? 0 :
            evID == null ? 0 : Convert.ToInt32(evID);
    }

    protected void gvLog_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
    }

    protected void gvLog_Sorting(object sender, GridViewSortEventArgs e)
    {
        this.OnGridViewSorting(sender, e);
    }

    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    private void LoadEvents()
    {
        if (ddlCategory.SelectedValue != "0")
        {
            ddlEvent.Enabled = true;
            ddlEvent.DataSource = SecurityLog.GetEvents(Convert.ToInt32(ddlCategory.SelectedValue));
            ddlEvent.DataBind();
            ddlEvent.Items.Insert(0, new ListItem(Resources.TextMessages.DropDown_NotSelectedFilter, "0"));
            ddlEvent.SelectedValue = Request.Form[ddlEvent.UniqueID];
        }
        else
        {
            ddlEvent.Enabled = false;
            ddlEvent.Items.Clear();
            ddlEvent.Items.Add(new ListItem(Resources.TextMessages.DropDown_NotSelectedFilter, "0"));
        }
    }

    protected void odsLog_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        if (!(e.ReturnValue is DataSet))
        {
            totalRowsCount = Convert.ToInt32(e.ReturnValue);
            SetupExcelExport();
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Master.PageHeaderText = "Security Log";

        ddlCategory.Attributes.Add("onchange", "PopulateEvents(this)");


        if (!Page.IsPostBack)
        {
            bool isSA = Roles.IsUserInRole(UserRoles.SuperAdministrator);

            if (isSA)
            {
                ddlCategory.Items.Add(new ListItem(Resources.TextMessages.DropDown_NotSelectedFilter, "0"));
            }
            ddlEvent.Items.Add(new ListItem(Resources.TextMessages.DropDown_NotSelectedFilter, "0"));

            ddlCategory.DataSource = SecurityLog.GetCategories(isSA);
            ddlCategory.DataBind();
        }

        LoadEvents();
        

        btnExport.Visible = true;
    }

    protected override void SetPagePermissions()
    {
        base.SetPagePermissions();

        if (Roles.IsUserInRole(UserRoles.SuperAdministrator) || Roles.IsUserInRole(UserRoles.BranchAdministrator))
        {
            ReadPermission = true;
            WritePermission = true;
            DeletePermission = true;
        }
        else
        {
            ReadPermission = false;
            WritePermission = false;
            DeletePermission = false;

            Response.Redirect("~/Default.aspx");
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        gvLog.PageIndex = 0;
        gvLog.DataBind();
    }

    bool clearPerformed = false;

    protected void btnClear_Click(object sender, EventArgs e)
    {
        clearPerformed = true;

        ddlCategory.ClearSelection();
        ddlEvent.ClearSelection();
        if (ddlCategory.SelectedValue == "0")
        {
            ddlEvent.Items.Clear();
            ddlEvent.Items.Add(new ListItem(Resources.TextMessages.DropDown_NotSelectedFilter, "0"));
            ddlEvent.Enabled = false;
        }

        dpStartDate.SelectedDate = null;
        dpEndDate.SelectedDate = null;

        upFilter.Update();

        gvLog.PageIndex = 0;
        gvLog.DataBind();
    }

    protected void Export_Click(object sender, EventArgs e)
    {
        gvLog.DataBind();
        SetupExcelExport();
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        //gvLog.DataBind();
    }

    private void SetupExcelExport()
    {
        int maxLength = AppSettings.ExportedSecurityReportMaximumLength;
        if (totalRowsCount > maxLength)
        {
            string msg = String.Format(Resources.TextMessages.ExportLimitMsg, maxLength);
            btnExport.OnClientClick = String.Format("alert('{0}'); return false;", msg);
            btnExport.HideWhenEmpty = false;
        }
        else
        {
            StringBuilder orderBy = new StringBuilder();

            if (!String.IsNullOrEmpty(gvLog.SortExpression))
            {
                orderBy.Append(gvLog.SortExpression);
            }

            if (gvLog.SortDirection == SortDirection.Descending)
            {
                orderBy.Append(" desc");
            }

            btnExport.IgnoredColumnsList = new string[] { "PKID", "SecurityEventID", "Metadata", "RowNumber", "Description", "HTMLDescription", "CategoryName" };
            btnExport.BoundedView = SecurityLog.GetReport(
                Convert.ToInt32(ddlCategory.SelectedValue),
                Convert.ToInt32(ddlEvent.SelectedValue), 
                dpStartDate.SelectedDate,
                dpEndDate.SelectedDate, 
                Roles.IsUserInRole(UserRoles.SuperAdministrator),
                orderBy.ToString()).Tables[0].AsDataView();
        }
    }


    [WebMethod]
    public static SecurityEventView[] GetEvents(int categoryID)
    {
        return SecurityLog.GetEvents(categoryID).ToArray();
    }

}
