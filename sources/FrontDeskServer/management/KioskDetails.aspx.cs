using FrontDesk.Common;
using FrontDesk.Common.Messages;
using FrontDesk.Server;
using FrontDesk.Server.Logging;
using FrontDesk.Server.Membership;
using FrontDesk.Server.Screening.Services;
using FrontDesk.Server.Web;
using ScreenDox.Server.Common.Models;
using ScreenDox.Server.Common.Services;
using System;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class KioskManagement_KioskDetails : BaseManagementWebForm<Kiosk, short>
{
    private readonly IBranchLocationService _branchLocationService = new BranchLocationService();
    private readonly IKioskService _kioskService = new KioskService();

    protected void Page_Init(object sender, EventArgs e)
    {
        // Enable obfuscation for TypeName="...". This overloads class name from .aspx
        // Need to protect (select) method name from obfuscation: unable to determine obfuscated name of method.
        // Method names and parameters may stay in .aspx. If named parameters used, protect whole datasource class from obfuscation.
        odsrBranch.TypeName = typeof(FrontDesk.Server.Screening.Services.BranchLocationService).FullName;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        bool isIdFound = false;
        this.formObjectID = GetPageIDValue("id", out isIdFound);
        EnsureFormObjectCreated();
        try
        {
            if (!IsPostBack)
            {
                if (CurFormObject.KioskID == 0 && isIdFound)
                {
                    // non-existing id requested
                    Response.Redirect("KioskList.aspx");
                }
                FDUser user = FDUser.GetCurrentUser();
                if (user != null)
                {
                    if (!IsNewInstance && !Roles.IsUserInRole(UserRoles.SuperAdministrator) /*&& Kiosk.GetKioskCount(CurFormObject.KioskID, "", "", user.UserID) == 0*/)
                    {
                        Response.Redirect("KioskList.aspx");
                    }
                    else if (!Roles.IsUserInRole(UserRoles.SuperAdministrator))
                    {
                        odsrBranch.SelectMethod = "GetForUserID";
                        odsrBranch.SelectParameters.Add(new Parameter("userID", System.Data.DbType.Int32, user.UserID.ToString()));
                    }
                }

                ddlBranch.DataBind();
                ddlBranch.Items.Insert(0, new ListItem(Resources.TextMessages.DropDown_NotSelectedText, ""));
                EditModeDataPrepare(this.formObjectID);
            }
            Master.PageHeaderText = IsNewInstance ? "New Kiosk" : "Kiosk Details";
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            ErrorLog.AddServerException(CustomError.GetFormMessage("Kiosk"), ex);
        }
    }

    protected override Kiosk GetFormObjectByID(short objectID)
    {
        Kiosk kiosk = null;
        try
        {
            kiosk = _kioskService.GetByID(this.formObjectID);
        }
        catch (Exception ex)
        {
            ErrorLog.Add(ex.Message, ex.StackTrace, null);
        }
        return kiosk;
    }

    protected override void OnFormObjectIDNotFound(ref Kiosk formObject)
    {
        formObject = new Kiosk();
        this.formObjectID = 0;
    }



    protected override void EditModeDataPrepare(short objectID)
    {
       
        if (!IsNewInstance)
        {
            var model = CurFormObject;

            txtName.Text = model.Name;
            txtDescription.Text = model.Description;
            lblKeyValue.Text = TextFormatHelper.FormatWithGroupsInt16(CurFormObject.KioskID);
            ddlBranch.SelectedValue = model.BranchLocationID.ToString();
            txtSecret.Text = model.SecretKey;
            lblStatus.Text = "Kiosk is disabled";
            lblScreeningProfileName.Text = model.ScreeningProfileName;
        }
    }

    protected override Kiosk GetFormData()
    {
        Kiosk kiosk = CurFormObject;
        kiosk.Name = txtName.Text.Trim();
        kiosk.Description = txtDescription.Text.Trim();
        kiosk.SecretKey = txtSecret.Text.Trim();
        kiosk.CreatedDate = IsNewInstance ? DateTimeOffset.Now : CurFormObject.CreatedDate;
        kiosk.BranchLocationID = Convert.ToInt32(ddlBranch.SelectedValue);

        return kiosk;
    }


    public override void SetTabIndex()
    {
        base.SetTabIndex();
        if (!IsPostBack)
        {
            txtName.Focus();
        }
    }

    #region page permissions

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

    protected override void SetControlsEnabledState()
    {
        base.SetControlsEnabledState();

        btnAdd.Visible = IsNewInstance;
        btnSave.Visible = !IsNewInstance;
        btnDelete.Visible = !IsNewInstance;
        phKioskKey.Visible = !IsNewInstance;
        btnEnabled.Visible = !IsNewInstance;
        btnEnabled.Text = CurFormObject.Disabled ? "Enable" : "Disable";
        lblStatus.Visible = CurFormObject.Disabled;

        if (!IsNewInstance && CurFormObject.Disabled)
        {
            txtName.ReadOnly = true;
            txtDescription.ReadOnly = true;
            ddlBranch.Enabled = false;
            btnSave.Visible = false;
        }



    }


    #endregion


    #region page events

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;


        try
        {
            Kiosk kioskDetails = GetFormData();
            //check that kiosk is not inserted into disabled location

            var branch = _branchLocationService.Get(kioskDetails.BranchLocationID);
            if (branch.Disabled)
            {
                SetErrorAlert(Resources.TextMessages.Kiosk_CannotAddKioskToBlockedLocation, this.GetType());
            }
            else
            {
                formObjectID = KioskHelper.Add(kioskDetails);
                SecurityLog.Add(new SecurityLog(SecurityEvents.NewKioskRegistered,
                    String.Format("{0}~{1}", this.formObjectID, this.CurFormObject.Name)));

                SetRedirectSuccessAlert(FrontDesk.Common.Messages.CustomMessage.GetAddMessage("Kiosk"));
                Response.Redirect(String.Format("{0}?id={1}", Request.Path, this.formObjectID), false);
            }
        }
        catch (ApplicationException ex)
        {
            SetErrorAlert(ex.Message, this.GetType());
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            ErrorLog.Add(ex.Message, ex.StackTrace, null);
            SetErrorAlert(FrontDesk.Common.Messages.CustomError.GetAddMessage("Kiosk"), this.GetType());
        }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid && !IsNewInstance) return;

        try
        {
            Kiosk kioskDetails = GetFormData();

            var branch = _branchLocationService.Get(kioskDetails.BranchLocationID);
            if (branch.Disabled && !CurFormObject.Disabled)
            {
                SetErrorAlert(Resources.TextMessages.Kiosk_CannotAddKioskToBlockedLocation, this.GetType());
            }
            else
            {
                _kioskService.Update(kioskDetails);
                SetRedirectSuccessAlert(FrontDesk.Common.Messages.CustomMessage.GetUpdateMessage("Kiosk"));
                Response.Redirect(String.Format("{0}?id={1}", Request.Path, this.formObjectID), false);
            }
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            ErrorLog.Add(ex.Message, ex.StackTrace, null);
            SetErrorAlert(FrontDesk.Common.Messages.CustomError.GetUpdateMessage("Kiosk"), this.GetType());
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            _kioskService.Delete(this.formObjectID);

            SecurityLog.Add(new SecurityLog(SecurityEvents.KioskRemoved, this.CurFormObject.Name));
            SetRedirectSuccessAlert(FrontDesk.Common.Messages.CustomMessage.GetDeleteMessage("Kiosk"));
            Response.Redirect("KioskList.aspx", false);
        }
        catch (ApplicationException)
        {
            SetErrorAlert(Resources.TextMessages.Kiosk_UnableToDelete, this.GetType());
        }
        catch (Exception ex)
        {
            ErrorLog.Add(ex.Message, ex.StackTrace, null);
            SetErrorAlert(FrontDesk.Common.Messages.CustomError.GetDeleteMessage("Kiosk"), this.GetType());
        }
    }

    protected void btnEnabled_Click(object sender, EventArgs e)
    {
        if (!IsNewInstance)
        {
            try
            {
                if (CurFormObject.Disabled) //we are about to enable
                {
                    //check if branch location is enabled
                    var branch = _branchLocationService.Get(CurFormObject.BranchLocationID);
                    if (branch != null)
                    {
                        if (branch.Disabled)
                        {
                            SetErrorAlert(Resources.TextMessages.Kiosk_UnableToEnableBecauseLocationIsDisabled, this.GetType());
                            return;
                        }
                    }
                }



                KioskHelper.SetDisabledStatus(formObjectID, !CurFormObject.Disabled);

                SetRedirectSuccessAlert(String.Format("Kiosk has been {0} successfully.", CurFormObject.Disabled ? "enabled" : "disabled"));
                Response.Redirect(String.Format("{0}?id={1}", Request.Path, this.formObjectID), false);

            }
            catch (ApplicationException ex)
            {
                ErrorLog.Add(ex.Message, ex.StackTrace, null);
                SetErrorAlert(ex.Message, this.GetType());
                return;
            }
            catch (Exception ex)
            {
                ErrorLog.Add(ex.Message, ex.StackTrace, null);
                SetErrorAlert(CustomError.GetMessageForCustomOperation(CurFormObject.Disabled ? "enable" : "disable", "kiosk"), this.GetType());
                return;
            }
        }

    }

    #endregion
}
