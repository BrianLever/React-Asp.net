using FrontDesk.Common.Extensions;
using FrontDesk.Common.Messages;
using FrontDesk.Server;
using FrontDesk.Server.Logging;
using FrontDesk.Server.Membership;
using FrontDesk.Server.Screening.Services;
using FrontDesk.Server.Web;
using FrontDesk.Server.Web.Extensions;
using System;
using System.Web.Security;

public partial class management_BranchLocationDetails : BaseManagementWebForm<BranchLocation, int>
{
    private readonly IBranchLocationService _branchLocationService = new BranchLocationService();

    protected void Page_Init(object sender, EventArgs e)
    {
        ddlScreeningProfile.AddDefaultNotSelectedItem();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        bool pageIdSpecified = false;
        int branchLocationID = GetPageIDValue("id", out pageIdSpecified);
        if (pageIdSpecified)
        {
            this.formObjectID = branchLocationID;
            this.EnsureFormObjectCreated();

            if (CurFormObject == null)
            {
                // non-existing id requested
                Response.Redirect("BranchLocationList.aspx");
            }
        }

        try
        {
            if (!IsPostBack)
            {
                if (!IsNewInstance)
                {
                    EditModeDataPrepare(this.formObjectID);
                    btnSave.Text = "Save Changes";
                    btnEnabled.Text = CurFormObject.Disabled ? "Enable" : "Disable";
                    lblStatus.Visible = CurFormObject.Disabled;
                }
                else
                {
                    // adding new
                    btnSave.Text = "Create";
                    btnDelete.Visible = false;
                    btnEnabled.Visible = false;
                }
            }
            Master.PageHeaderText = !IsNewInstance ? String.Format("Branch Location: {0}", CurFormObject.Name) : "New Branch Location";

        }
        catch (Exception ex)
        {
            ErrorLog.Add(ex.Message, ex.StackTrace, null);
            RedirectToErrorPage(CustomError.GetInternalErrorMessage());
        }
    }

    protected override BranchLocation GetFormObjectByID(int objectID)
    {
        return _branchLocationService.Get(objectID);
    }

    protected override void EditModeDataPrepare(int objectID)
    {
        if (!IsNewInstance)
        {
            txtName.Text = CurFormObject.Name;
            txtDescription.Text = CurFormObject.Description;
            lblStatus.Text = "Branch location is disabled";
            ddlScreeningProfile.DataBind();
            ddlScreeningProfile.SetValueOrDefault(CurFormObject.ScreeningProfileID);
        }        
    }

    protected override BranchLocation GetFormData()
    {
        BranchLocation formData = new BranchLocation(formObjectID)
        {
            Name = txtName.Text,
            Description = txtDescription.Text,
            ScreeningProfileID = ddlScreeningProfile.SelectedValue.As<int>()
        };
        
        return formData;        
    }

    public override void SetTabIndex()
    {
        if (!IsPostBack)
        {
            txtName.Focus();
        }
    }

    private bool Save()
    {
        try
        {
            BranchLocation location = this.GetFormData();
            if (IsNewInstance)
            {
                formObjectID = _branchLocationService.Add(location);


                SecurityLog.Add(new SecurityLog(SecurityEvents.NewBranchLocationAdded,
                    String.Format("{0}~{1}", location.BranchLocationID, location.Name)));
            }
            else
            {
                _branchLocationService.Update(location);
            }
        }
        catch (ApplicationException ex)
        {
            ErrorLog.Add(ex.Message, ex.StackTrace, null);
            SetErrorAlert(ex.Message, this.GetType());
            return false;
        }
        catch (Exception ex)
        {
            ErrorLog.Add(ex.Message, ex.StackTrace, null);
            RedirectToErrorPage(CustomError.GetInternalErrorMessage());
        }
        return true;
    }

    private bool Delete()
    {
        bool success = false;
        try
        {            
            if (!IsNewInstance)
            {
                success = _branchLocationService.Delete(this.formObjectID);
                if (!success)
                {
                    SetErrorAlert("Cannot delete Branch Location while user or kiosk are assigned to it.", this.GetType());
                }
            }            
        }
        catch (Exception ex)
        {
            ErrorLog.Add(ex.Message, ex.StackTrace, null);
            RedirectToErrorPage(CustomError.GetInternalErrorMessage());
        }

        return success;
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("BranchLocationList.aspx", false);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        this.Validate();
        if (this.IsValid)
        {
            if (this.Save())
            {
                SetRedirectSuccessAlert(CustomMessage.GetUpdateMessage("Branch location"));
              

                Response.Redirect(Request.Path + "?id=" + formObjectID);
            }
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        // client javascript will ask for confirmation
        try
        {
            if (this.Delete())
            {
                SecurityLog.Add(new SecurityLog(SecurityEvents.BranchLocationDeleted, this.CurFormObject.Name));

                Response.Redirect("BranchLocationList.aspx", false);
            }
        }
        catch (System.Threading.ThreadAbortException) { }

    }

    protected void btnEnabled_Click(object sender, EventArgs e)
    {
        try
        {
            if (!_branchLocationService.HasActiveKiosk(formObjectID))
            {
                _branchLocationService.SetDisabledStatus(formObjectID, !CurFormObject.Disabled);
                SetRedirectSuccessAlert(String.Format("Branch location has been {0} successfully.", CurFormObject.Disabled ? "enabled" : "disabled"));
            }
            else
            {
                SetRedirectFailureAlert(Resources.TextMessages.Location_Disabled_Failed);
            }
            Response.Redirect(String.Format("{0}?id={1}", Request.Path, this.formObjectID), false);
        }
        catch (ApplicationException ex)
        {
            ErrorLog.Add(ex.Message, ex.StackTrace, null);
            SetErrorAlert(ex.Message, this.GetType());
            return;
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            ErrorLog.Add(ex.Message, ex.StackTrace, null);
            SetErrorAlert(String.Format("Failed to {0} branch location. Error has occurred.", CurFormObject.Disabled ? "enable" : "disable"), this.GetType());
            return;
        }
    }

    protected override void SetControlsEnabledState()
    {
        base.SetControlsEnabledState();

        btnDelete.Visible = !IsNewInstance;
        btnEnabled.Visible = !IsNewInstance;
        if (!IsNewInstance && CurFormObject.Disabled)
        {
            txtName.ReadOnly = true;
            txtDescription.ReadOnly = true;
            btnSave.Visible = false;
        }
    }

    protected override void SetPagePermissions()
    {
        base.SetPagePermissions();

        if (Roles.IsUserInRole(UserRoles.SuperAdministrator))
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
