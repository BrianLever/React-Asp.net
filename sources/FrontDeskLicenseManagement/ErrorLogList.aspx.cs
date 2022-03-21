using System;
using System.Data;
using FrontDesk.Common.Messages;
using FrontDesk.Server.LicenseManagerWeb;
using FrontDesk.Server.Logging;

public partial class ErrorLogListForm : LMBasePage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        // Enable obfuscation for TypeName="...". This overloads class name from .aspx
        // Method name stays readable: unable to determine obfuscated name.
        // Method names and parameters may stay in .aspx.
        odsItems.TypeName = typeof(FrontDesk.Server.Logging.ErrorLog).FullName;

        RegisterGridViewForCustomPaging(gvItems);
    }
    
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.PageHeaderText = "Error Log";

        if (!IsPostBack)
        {
            dtEndDate.SelectedDate = DateTime.Today;
        }

        var dv = new DataView(ErrorLog.GetForExport(dtStartDate.SelectedDate, dtEndDate.SelectedDate, 0, 10000).Tables[0]);
        btnExport.BoundedView = dv;
    }

    protected void Search_Click(object sender, EventArgs e)
    {
        SetDataSourceParameters();
        gvItems.DataBind();
    }
    protected void Clear_Click(object sender, EventArgs e)
    {
        dtStartDate.SelectedDate = null;
        dtEndDate.SelectedDate = DateTime.Today;

        AddAjaxScriptStatement(GetControlClearStateScript(dtStartDate.ClientID, HTMLControlType.Textbox, dtStartDate.Text));
        AddAjaxScriptStatement(GetControlClearStateScript(dtEndDate.ClientID, HTMLControlType.Textbox, dtEndDate.Text));

        SetDataSourceParameters();
        gvItems.DataBind();
    }

    protected void btnDeleteAll_Click(object sender, EventArgs e)
    {
        try
        {
            ErrorLog.DeleteAll();
            gvItems.DataBind();
        }
        catch (Exception ex)
        {
            ErrorLog.Add(ex.Message, ex.StackTrace, null);
            SetErrorAlert(CustomError.GetMessageForCustomOperation("clear", "Error Log"), this.GetType());
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
        }
        catch (Exception ex)
        {
            ErrorLog.Add(ex.Message, ex.StackTrace, null);
            SetErrorAlert(CustomError.GetMessageForCustomOperation("export", "Error Log"), this.GetType());
        }
    }

    private void SetDataSourceParameters()
    {
        gvItems.PageIndex = 0;

        if (dtStartDate.SelectedDate.HasValue)
            odsItems.SelectParameters["startDate"].DefaultValue = dtStartDate.SelectedDate.Value.ToString();
        else
            odsItems.SelectParameters["startDate"].DefaultValue = null;

        if (dtEndDate.SelectedDate.HasValue)
            odsItems.SelectParameters["endDate"].DefaultValue = dtEndDate.SelectedDate.Value.ToString();
        else
            odsItems.SelectParameters["endDate"].DefaultValue = null;
    }
   
}
