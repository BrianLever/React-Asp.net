using System;
using System.Web.UI.WebControls;
using FrontDesk.Server;
using FrontDesk.Server.Logging;
using FrontDesk.Server.Screening.Services;
using FrontDesk.Server.Web;

public partial class UserProfile : BaseManagementWebForm<FDUser, Int32>
{
    private readonly IBranchLocationService _branchLocationService = new BranchLocationService();

    protected void Page_Init(object sender, EventArgs e)
    {
        // Enable obfuscation for TypeName="...". This overloads class name from .aspx
        // Need to protect (select) method name from obfuscation: unable to determine obfuscated name of method.
        // Method names and parameters may stay in .aspx. If named parameters used, protect whole datasource class from obfuscation.
        odsState.TypeName = typeof(FrontDesk.State).FullName;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Master.PageHeaderText = "My Profile";
        this.formObjectID = FDUser.CurrentUserID;
        if (!IsPostBack)
        {
            ddlState.DataBind();
            ddlState.Items.Insert(0, new ListItem(Resources.TextMessages.DropDown_NotSelectedText, ""));

            EditModeDataPrepare(this.formObjectID);
        }
        //add email regular expression
        vldRegEmail.ValidationExpression = BasePage.EmailRegularExpression;
    }

    protected override FDUser GetFormObjectByID(int objectID)
    {
        FDUser userDetails = null;
        try
        {
            userDetails = FDUser.GetCurrentUser();
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
        lblGroupValue.Text = CurFormObject.RoleName;
        lblBranchLocationValue.Text = CurFormObject.BranchLocationID != null ?  _branchLocationService.Get(Convert.ToInt32(CurFormObject.BranchLocationID)).Name
            : "N/A";

    }

    protected override FDUser GetFormData()
    {
        FDUser userDetails = CurFormObject;
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

        return userDetails;
    }

    public override void SetTabIndex()
    {
        if (!IsPostBack)
        {
            txtFirstName.Focus();
        }
    }

    #region page events

    protected void btnSave_Click(object sender, EventArgs e)
    {
        var user = GetFormData();
        try
        {
            FDUser.Update(user);
            SetSuccessAlert(FrontDesk.Common.Messages.CustomMessage.GetUpdateMessage("Profile"), this.GetType());
        }   
        catch (Exception ex)
        {
            ErrorLog.Add(ex.Message, ex.StackTrace, null);
            SetErrorAlert(FrontDesk.Common.Messages.CustomError.GetUpdateMessage("profile"), this.GetType());
        }
    }

    #endregion

}
