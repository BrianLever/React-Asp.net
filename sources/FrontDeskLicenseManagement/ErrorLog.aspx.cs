using System;
using FrontDesk.Common.Messages;
using FrontDesk.Server.LicenseManagerWeb;
using FrontDesk.Server.Logging;

public partial class ErrorLogForm : LMBaseManagementWebForm<ErrorLog, long>
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.PageHeaderText = "Error Log Item Details";

        if (!TryGetPageIDValue("id"))
        {
            Response.Redirect("ErrorLogList.aspx", true);
        }
        try
        {
            EnsureFormObjectCreated();
            EditModeDataPrepare(formObjectID);
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            ErrorLog.Add(ex.Message, ex.StackTrace, null);
            RedirectToErrorPage(CustomError.GetInternalErrorMessage());
        }
    }

    protected override ErrorLog GetFormObjectByID(long objectID)
    {
        return ErrorLog.Get(objectID);
    }

    protected override void EditModeDataPrepare(long objectID)
    {
        if (!IsNewInstance)
        {
            lblID.Text = CurFormObject.ErrorLogID.ToString();
            //lblKioskLabel.Text = CurFormObject.KioskLabel;
            lblCreatedDate.Text = CurFormObject.CreatedDate.ToString("MM/dd/yyyy HH:mm:ss zzz");
            lblErrorMessage.Text = CurFormObject.ErrorMessage;
            lblStackTrace.Text = CurFormObject.ErrorTraceLog;
        }
        else
        {
            Response.Redirect("ErrorLogList.aspx", true);
        }
    }

    protected override ErrorLog GetFormData()
    {
        return null;
    }

   
}
