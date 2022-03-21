using FrontDesk;
using FrontDesk.Common.Messages;
using FrontDesk.Server.Logging;
using FrontDesk.Server.Membership;
using FrontDesk.Server.Screening.Services;
using FrontDesk.Server.Web;

using System;
using System.Web.Security;

public partial class management_ScreenProfilePage : BaseManagementWebForm<ScreeningProfile, int>
{

    private readonly IScreeningProfileService _service = new ScreeningProfileService();

    protected void Page_Load(object sender, EventArgs e)
    {
        bool pageIdSpecified = false;
        formObjectID = GetPageIDValue("id", out pageIdSpecified);
        if (pageIdSpecified)
        {
            EnsureFormObjectCreated();

            if (CurFormObject == null)
            {
                // non-existing id requested
                Response.Redirect("ScreenProfileList.aspx");
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

                }
                else
                {
                    // adding new
                    btnSave.Text = "Create";
                    btnDelete.Visible = false;
                }
            }
            Master.PageHeaderText = !IsNewInstance ? String.Format("Screen Profile: {0}", CurFormObject.Name) : "New Screen Profile";

        }
        catch (Exception ex)
        {
            ErrorLog.Add(ex.Message, ex.StackTrace, null);
            RedirectToErrorPage(CustomError.GetInternalErrorMessage());
        }

        btnDelete.OnClientClick = "if(!confirm('Are you sure that you want to delete this Screen Profile?')){return false;}";
    }

    protected override ScreeningProfile GetFormObjectByID(int objectID)
    {
        return _service.Get(objectID);
    }

    protected override void EditModeDataPrepare(int objectID)
    {
        if (!IsNewInstance)
        {
            txtName.Text = CurFormObject.Name;
            txtDescription.Text = CurFormObject.Description;
        }
    }

    protected override ScreeningProfile GetFormData()
    {
        return new ScreeningProfile
        {
            ID = formObjectID,
            Name = txtName.Text,
            Description = txtDescription.Text
        };
    }

    public override void SetTabIndex()
    {
        short tabIndex = 1;

        txtName.TabIndex = tabIndex++;
        txtDescription.TabIndex = tabIndex++;
        btnSave.TabIndex = tabIndex++;
        btnDelete.TabIndex = tabIndex++;
        btnCancel.TabIndex = tabIndex++;

        if (!IsPostBack)
        {
            txtName.Focus();
        }

    }

    private bool Save()
    {
        try
        {
            var model = this.GetFormData();
            if (IsNewInstance)
            {
                formObjectID = _service.Add(model);

                SecurityLog.Add(new SecurityLog(SecurityEvents.NewBranchLocationAdded,
                    String.Format("{0}~{1}", model.ID, model.Name)));
            }
            else
            {
                _service.Update(model);
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
                success = _service.Delete(this.formObjectID);
                if (!success)
                {
                    SetErrorAlert("Cannot delete Screen Profile while branch location is using it.", this.GetType());
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
        Response.Redirect("ScreenProfileList.aspx", false);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        this.Validate();
        if (this.IsValid)
        {
            if (this.Save())
            {
                SetRedirectSuccessAlert(CustomMessage.GetUpdateMessage("Screen Profile"));
                //Response.Redirect("BranchLocationList.aspx");
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

                Response.Redirect("ScreenProfileList.aspx", false);
            }
        }
        catch (System.Threading.ThreadAbortException) { }

    }

    protected override void SetControlsEnabledState()
    {
        base.SetControlsEnabledState();

        btnDelete.Visible = !IsNewInstance;
        if (!IsNewInstance && CurFormObject.ID == ScreeningProfile.DefaultProfileID)
        {
            txtName.ReadOnly = true;
            btnDelete.Visible = false;
        }
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
