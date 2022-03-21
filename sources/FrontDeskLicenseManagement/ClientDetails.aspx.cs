using System;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using FrontDesk.Common.Messages;
using FrontDesk.Server.LicenseManagerWeb;
using FrontDesk.Server.Licensing.Management;
using FrontDesk.Server.Logging;

public partial class ClientDetails : LMBaseManagementWebForm<Client, Int32>
{
    protected void Page_Init(object sender, EventArgs e)
    {
        // Enable obfuscation for TypeName="...". This overloads class name from .aspx
        // Protect method name from obfuscation: unable to determine obfuscated name of method.
        // Method names and parameters may stay in .aspx. If named parameters used, protect whole class from obfuscation.
        odsStates.TypeName = typeof(FrontDesk.State).FullName;
        odsrLicense.TypeName = typeof(FrontDesk.Server.Licensing.Management.LicenseEntityHelper).FullName;            
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            bool isIdFound = false;
            this.formObjectID = GetPageIDValue("id", out isIdFound);
            EnsureFormObjectCreated();

            if (IsNewInstance)
            {
                Master.PageHeaderText = "Create New Client";
                this.btnUpdate.Visible = false;
                this.btnDelete.Visible = false;
            }
            else
            {
                Master.PageHeaderText = "Client Details";
                this.btnAdd.Visible = false;

                this.lnbAssignLicense.OnClientClick = string.Format("location.href='AssignLicense.aspx?id={0}'; return false;", this.formObjectID);
                this.lnbtnCreateLicense.OnClientClick = String.Format("location.href='LicenseDetails.aspx?client={0}'; return false", this.formObjectID);
            }

            if (!IsPostBack)
            {
                EditModeDataPrepare(this.formObjectID);
            }

            BindPageListControls();
        }
        catch (Exception ex)
        {
            ErrorLog.AddServerException(CustomError.GetFormMessage("client"), ex);
            RedirectToErrorPage(CustomError.GetInternalErrorMessage());
        }
    }

    protected override void BindPageListControls()
    {
        base.BindPageListControls();

        //bind state list and add "not selected" item
        if (!IsPostBack)
        {
            ddlState.Items.Clear();
            ddlState.DataBind();
            var emptyItem = new ListItem(Resources.TextMessages.DropDown_NotSelectedText, string.Empty);
            ddlState.Items.Insert(0, emptyItem);
        }
    }

    protected override Client GetFormObjectByID(int objectID)
    {
        return Client.GetByID(objectID);
    }

    protected override void EditModeDataPrepare(int objectID)
    {
        if (CurFormObject != null)
        {
            txtCompanyName.Text = CurFormObject.CompanyName;
            if (!string.IsNullOrEmpty(CurFormObject.StateCode))
            {
                ddlState.SelectedValue = CurFormObject.StateCode;
            }
            txtCity.Text = CurFormObject.City;
            txtAddress1.Text = CurFormObject.AddressLine1;
            txtAddress2.Text = CurFormObject.AddressLine2;
            txtPostalCode.Text = CurFormObject.PostalCode;
            txtEmail.Text = CurFormObject.Email;
            txtContactPerson.Text = CurFormObject.ContactPerson;
            txtContactPhone.Text = CurFormObject.ContactPhone;
            txtNotes.Text = CurFormObject.Notes;
        }
    }

    protected override Client GetFormData()
    {
        Client client = new Client();
        client.ClientID = (CurFormObject == null) ? 0 : CurFormObject.ClientID;
        client.CompanyName = txtCompanyName.Text.Trim();
        client.StateCode = ddlState.SelectedValue;
        client.City = txtCity.Text;
        client.AddressLine1 = txtAddress1.Text.Trim();
        client.AddressLine2 = txtAddress2.Text.Trim();

        client.PostalCode = txtPostalCode.Text.Trim();
        client.Email = txtEmail.Text.Trim();
        client.ContactPerson = txtContactPerson.Text.Trim();
        client.ContactPhone = txtContactPhone.Text.Trim();
        client.Notes = txtNotes.Text.Trim();

        return client;
    }

    public override void SetTabIndex()
    {
        if (!IsPostBack)
        {
            txtCompanyName.Focus();
        }
    }

    protected override void SetControlsEnabledState()
    {
        base.SetControlsEnabledState();
        phClientLicense.Visible = !IsNewInstance;
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid && IsNewInstance)
            {
                var client = GetFormData();

                int newId = Client.Add(client);
                SetRedirectAlert(FrontDesk.Common.Messages.CustomMessage.GetAddMessage("Client"));
                Response.Redirect(String.Format("~/ClientDetails.aspx?id={0}", newId), false);
            }
        }
        catch (ThreadAbortException)
        {
        }
        catch (ApplicationException ex)
        {
            SetErrorAlert(ex.Message, this.GetType());
        }
        catch (Exception ex)
        {
            ErrorLog.Add(ex.Message, ex.StackTrace, null);
            SetErrorAlert(FrontDesk.Common.Messages.CustomError.GetAddMessage("Client"), this.GetType());
        }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid && !IsNewInstance)
            {
                var client = GetFormData();

                client.Update();
                SetRedirectAlert(FrontDesk.Common.Messages.CustomMessage.GetUpdateMessage("Client"));
                Response.Redirect(String.Format("~/ClientDetails.aspx?id={0}", client.ClientID), false);
            }
        }
        catch (ThreadAbortException)
        {
        }
        catch (ApplicationException ex)
        {
            SetErrorAlert(ex.Message, this.GetType());
        }
        catch (Exception ex)
        {
            ErrorLog.Add(ex.Message, ex.StackTrace, null);
            SetErrorAlert(FrontDesk.Common.Messages.CustomError.GetUpdateMessage("Client"), this.GetType());
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            Client.Delete(this.formObjectID);
            SetRedirectAlert(FrontDesk.Common.Messages.CustomMessage.GetDeleteMessage("Client"));
            Response.Redirect(String.Format("~/Clients.aspx"), false);
        }
        catch (ThreadAbortException)
        {
        }
        catch (ApplicationException ex)
        {
            SetErrorAlert(ex.Message, this.GetType());
        }
        catch (Exception ex)
        {
            ErrorLog.Add(ex.Message, ex.StackTrace, null);
            SetErrorAlert(FrontDesk.Common.Messages.CustomError.GetDeleteMessage("Client"), this.GetType());
        }

    }

}
