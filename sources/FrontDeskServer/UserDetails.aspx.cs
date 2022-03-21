using System;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using FrontDesk.Server;
using FrontDesk.Server.Logging;
using FrontDesk.Server.Membership;
using FrontDesk.Server.Screening.Services;
using FrontDesk.Server.Web;

public partial class UserDetailsForm : BaseManagementWebForm<FDUser, Int32>
{
    private readonly IBranchLocationService _branchLocationService = new BranchLocationService();

    public FDUser curentUser;


    protected void Page_Init(object sender, EventArgs e)
    {
        // Enable obfuscation for TypeName="...". This overloads class name from .aspx
        // Need to protect (select) method name from obfuscation: unable to determine obfuscated name of method.
        // Method names and parameters may stay in .aspx. If named parameters used, protect whole datasource class from obfuscation.
        odsState.TypeName = typeof(FrontDesk.State).FullName;
        odsrBranch.TypeName = typeof(FrontDesk.Server.Screening.Services.BranchLocationService).FullName;
        odsrUserBranch.TypeName = typeof(FrontDesk.Server.Screening.Services.BranchLocationService).FullName;
        odsrRoles.TypeName = typeof(FrontDesk.Server.FDUser).FullName;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        bool isIdFound = false;
        this.formObjectID = GetPageIDValue("id", out isIdFound);
        curentUser = FDUser.GetCurrentUser();
        EnsureFormObjectCreated();

        try
        {
            if (!IsNewInstance && curentUser.BranchLocationID != CurFormObject.BranchLocationID && !Roles.IsUserInRole(UserRoles.SuperAdministrator))
            {
                Response.Redirect("Default.aspx", false);
            }
            else
            {
                if (!IsPostBack)
                {
                    ddlState.DataBind();
                    ddlState.Items.Insert(0, new ListItem(Resources.TextMessages.DropDown_NotSelectedText, ""));

                    //odsrBranch.SelectMethod = User.IsInRole(UserRoles.SuperAdministrator.ToString()) ? "GetAll" : "GetForUserID";
                    ddlBranch.DataSourceID = User.IsInRole(UserRoles.SuperAdministrator.ToString()) ? "odsrBranch" : "odsrUserBranch";
                    odsrUserBranch.SelectParameters["userID"].DefaultValue = curentUser.UserID.ToString();
                    ddlBranch.DataBind();
                    ddlBranch.Items.Insert(0, new ListItem(Resources.TextMessages.DropDown_NotSelectedText, ""));

                    ddlGroup.DataBind();
                    ddlGroup.Items.Insert(0, new ListItem(Resources.TextMessages.DropDown_NotSelectedText, ""));
                    EditModeDataPrepare(this.formObjectID);
                }

                //add email regular expression
                vldRegEmail.ValidationExpression = BasePage.EmailRegularExpression;
                Master.PageHeaderText = "User Account Details";
            }
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            ErrorLog.Add(ex.Message, ex.StackTrace, null);
        }
    }

    protected override FDUser GetFormObjectByID(int objectID)
    {
        FDUser userDetails = null;
        try
        {
            userDetails = FDUser.GetUserByID(this.formObjectID);
        }
        catch (Exception ex)
        {
            ErrorLog.Add(ex.Message, ex.StackTrace, null);
        }
        return userDetails;
    }

    protected override void OnFormObjectIDNotFound(ref FDUser formObject)
    {
        formObject = new FDUser();
        this.formObjectID = 0;
    }

    protected override void EditModeDataPrepare(int objectID)
    {
        if (!IsNewInstance)
        {
            txtUsername.Text = CurFormObject.UserName;
            txtFirstName.Text = CurFormObject.FirstName;
            txtLastName.Text = CurFormObject.LastName;
            txtMiddleName.Text = CurFormObject.MiddleName;
            txtEmail.Text = CurFormObject.Email;
            txtCity.Text = CurFormObject.City;
            txtAddress1.Text = CurFormObject.AddressLine1;
            txtAddress2.Text = CurFormObject.AddressLine2;
            txtPhone.Text = CurFormObject.ContactPhone;
            txtComments.Text = CurFormObject.Comments;
            txtPostalCode.Text = CurFormObject.PostalCode;
            ddlState.SelectedValue = CurFormObject.StateCode;
            ddlBranch.SelectedValue = CurFormObject.BranchLocationID == null ? String.Empty : CurFormObject.BranchLocationID.ToString();
            ddlGroup.SelectedValue = CurFormObject.RoleName == null ? String.Empty : CurFormObject.RoleName;
            btnBlock.Text = CurFormObject.IsBlock ? "Unblock" : "Block";
            lblGroupValue.Text = CurFormObject.RoleName;
            lblBranchValue.Text = CurFormObject.BranchLocationID != null ? _branchLocationService.Get((int)CurFormObject.BranchLocationID).Name.ToString()
                : "N/A";
        }
    }

    protected override FDUser GetFormData()
    {
        FDUser userDetails = CurFormObject;
        userDetails.UserName = txtUsername.Text;
        userDetails.NewPasswordValue = txtPassword1.Text;
        userDetails.FirstName = txtFirstName.Text;
        userDetails.LastName = txtLastName.Text;
        userDetails.MiddleName = txtMiddleName.Text;
        userDetails.Email = txtEmail.Text;
        userDetails.City = txtCity.Text;
        userDetails.AddressLine1 = txtAddress1.Text;
        userDetails.AddressLine2 = txtAddress2.Text;
        userDetails.ContactPhone = txtPhone.Text;
        userDetails.Comments = txtComments.Text;
        userDetails.StateCode = ddlState.SelectedValue;
        userDetails.PostalCode = txtPostalCode.Text;

        // branch can be null for su
        if (!string.IsNullOrEmpty(ddlBranch.SelectedValue))
        {
            userDetails.BranchLocationID = ddlBranch.SelectedValue == "0" ? (int?)null : Convert.ToInt32(ddlBranch.SelectedValue);
        }
        userDetails.RoleName = ddlGroup.SelectedValue;

        return userDetails;
    }

    public override void SetTabIndex()
    {
        if (!IsPostBack)
        {
            if (IsNewInstance)
            {
                txtUsername.Focus();
            }
            else
            {
                txtFirstName.Focus();
            }
        }
    }

    #region page permissions

    protected override void SetPagePermissions()
    {
        //Trace.Warn("SetPagePermissions");

        base.SetPagePermissions();

        if (Roles.IsUserInRole(UserRoles.SuperAdministrator) || Roles.IsUserInRole(UserRoles.BranchAdministrator))
        {
            ReadPermission = true;
            WritePermission = true;
            DeletePermission = true;
        }
        else if (Roles.IsUserInRole(UserRoles.Staff))
        {
            ReadPermission = true;
            WritePermission = false;
            DeletePermission = false;
        }
        else
        {
            ReadPermission = false;
            WritePermission = false;
            DeletePermission = false;

            Response.Redirect("Default.aspx");
        }
    }

    protected override void SetControlsEnabledState()
    {
        //Trace.Warn("SetControlsEnabledState");

        phPassword.Visible = IsNewInstance;
        txtUsername.ReadOnly = !IsNewInstance;
        btnSave.Visible = IsNewInstance && HasWritePermission;
        btnUpdate.Visible = !IsNewInstance && HasWritePermission;
        btnDelete.Visible = !IsNewInstance && HasWritePermission &&
            CurFormObject.RoleName != UserRoles.SuperAdministrator && CurFormObject.UserID != curentUser.UserID;
        btnBlock.Visible = !IsNewInstance && HasWritePermission &&
            CurFormObject.RoleName != UserRoles.SuperAdministrator && CurFormObject.UserID != curentUser.UserID;

        btnUnLock.Visible = CurFormObject.IsLockedOut;

        lblBlockError.Visible = CurFormObject.IsBlock;
        lblLockedError.Visible = CurFormObject.IsLockedOut;
    }

    protected override void ApplyPagePermissions()
    {
        phBranchLocation.Visible = CurFormObject.RoleName != UserRoles.SuperAdministrator &&
                CurFormObject.UserID != curentUser.UserID;
        phOwnerBranchLocation.Visible = CurFormObject.RoleName == UserRoles.SuperAdministrator ||
            CurFormObject.UserID == curentUser.UserID;
        phOwnerGroup.Visible = CurFormObject.RoleName == UserRoles.SuperAdministrator ||
            CurFormObject.UserID == curentUser.UserID;
        phGroup.Visible = HasWritePermission && (CurFormObject.RoleName != UserRoles.SuperAdministrator && CurFormObject.UserID != curentUser.UserID);
    }

    #endregion

    #region page event

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            var userDetails = GetFormData();
            try
            {
                this.formObjectID = FDUser.Add(userDetails);
                SecurityLog.Add(new SecurityLog(SecurityEvents.NewUserCreated,
                    String.Format("{0}~{1}", this.formObjectID, this.CurFormObject.UserName)));
                SetRedirectSuccessAlert(FrontDesk.Common.Messages.CustomMessage.GetAddMessage("User"));
                Response.Redirect(String.Format("~/UserDetails.aspx?id={0}", this.formObjectID), false);
            }
            catch (System.Threading.ThreadAbortException) { }
            catch (ApplicationException ex)
            {
                SetErrorAlert(ex.Message, this.GetType());
            }
            catch (Exception ex)
            {
                ErrorLog.Add(ex.Message, ex.StackTrace, null);
                SetErrorAlert(FrontDesk.Common.Messages.CustomError.GetAddMessage("User"), this.GetType());
            }
        }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        var userDetails = GetFormData();
        try
        {
            FDUser.Update(userDetails);
            SetRedirectSuccessAlert(FrontDesk.Common.Messages.CustomMessage.GetUpdateMessage("User details"));
            Response.Redirect(String.Format("~/UserDetails.aspx?id={0}", this.formObjectID),false);
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (ApplicationException ex)
        {
            SetErrorAlert(ex.Message, this.GetType());
        }
        catch (Exception ex)
        {
            ErrorLog.Add(ex.Message, ex.StackTrace, null);
            SetErrorAlert(FrontDesk.Common.Messages.CustomError.GetUpdateMessage("User details"), this.GetType());
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            FDUser.DeleteUser(this.formObjectID);
            Response.Redirect("UserList.aspx", false);
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (SecurityLogException)
        {
            SetErrorAlert(Resources.TextMessages.SecurityLog_UserDeleteError, this.GetType());
        }
        catch (Exception ex)
        {
            ErrorLog.Add(ex.Message, ex.StackTrace, null);
            SetErrorAlert(FrontDesk.Common.Messages.CustomError.GetDeleteMessage("User"), this.GetType());
        }
    }

    protected void btnBlock_Click(object sender, EventArgs e)
    {
        try
        {
            FDUser.Block(this.formObjectID, !CurFormObject.IsBlock);
            if (!CurFormObject.IsBlock)
            {
                SetRedirectSuccessAlert("User has been blocked successfully.");
            }
            else
            {
                SetRedirectSuccessAlert("User has been unblocked successfully.");
            }
            Response.Redirect(String.Format("~/UserDetails.aspx?id={0}", this.formObjectID), false);
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            ErrorLog.Add(ex.Message, ex.StackTrace, null);
            SetErrorAlert(FrontDesk.Common.Messages.CustomError.GetMessageForCustomOperation(CurFormObject.IsBlock ? "unblock" : "block", "User"), this.GetType());
        }
    }

    protected void btnUnlock_Click(object sender, EventArgs e)
    {
        try
        {
            CurFormObject.UnLock();
            SetRedirectSuccessAlert("User has been unloked successfully.");
            Response.Redirect(String.Format("~/UserDetails.aspx?id={0}", this.formObjectID), false);
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            ErrorLog.Add(ex.Message, ex.StackTrace, null);
            SetErrorAlert(FrontDesk.Common.Messages.CustomError.GetMessageForCustomOperation("unlock", "User"), this.GetType());
        }
    }

    /// <summary>
    /// Validate new password value
    /// Password can not contain user name
    /// </summary>
    public void ValidationNewPassword(object source, ServerValidateEventArgs args)
    {
        args.IsValid = !txtPassword1.Text.Contains(txtUsername.Text) &&
                       !txtPassword1.Text.Contains(txtUsername.Text.ToUpper()) &&
                       !txtPassword1.Text.Contains(txtUsername.Text.ToLower());
    }

    #endregion
}
