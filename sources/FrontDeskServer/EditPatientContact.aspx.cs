using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common.Logging;
using FrontDesk;
using FrontDesk.Common.Extensions;
using FrontDesk.Configuration;
using FrontDesk.Server.Controllers;
using FrontDesk.Server.Extensions;
using FrontDesk.Server.Logging;
using FrontDesk.Server.Membership;
using FrontDesk.Server.Screening;
using FrontDesk.Server.Screening.Services;
using FrontDesk.Server.Web;

using RPMS.Common.GlobalConfiguration;
using RPMS.Common.Security;

public partial class EditPatientContact : BaseManagementWebForm<ScreeningResult, long>
{
    private readonly RpmsExportController exportController;
    private readonly ScreeningProfileMinimalAgeService _minimalAgeService;
    private ILog _logger = LogManager.GetLogger("EditPatientContact");
    protected string[] _userRoles = new string[0];

    protected bool ExportPermission = true;


    public EditPatientContact()
    {
        exportController = new RpmsExportController(
                new DefaultDateService(),
                new RpmsCredentialsService(new RpmsCredentialsRepository(), new CryptographyService()),
                new GlobalSettingsService()
                );

        _minimalAgeService = new ScreeningProfileMinimalAgeService();
    }


    protected void Page_Init(object sender, EventArgs e)
    {
        odsState.TypeName = typeof(FrontDesk.State).FullName;

        ClientScript.RegisterClientScriptInclude(this.GetType(), "dateFormat.min.js", ResolveClientUrl("~/scripts/plugins/dateFormat.min.js"));
        ClientScript.RegisterClientScriptInclude(this.GetType(), "editPatientContactCtrl.js", ResolveClientUrl("~/scripts/controls/editPatientContactCtrl.js"));
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Master.PageHeaderText = "Edit Patient Contact Info";
        bool isIdFound = false;
        this.formObjectID = GetPageIDValue("id", out isIdFound);
        if (isIdFound)
        {
            EnsureFormObjectCreated();

            btnBack.OnClientClick = string.Format("location.href='PatientCheckIn.aspx?id={0}'; return false;", formObjectID);

            if (!IsPostBack)
            {
                ddlState.DataBind();
                ddlState.Items.Insert(0, new ListItem(Resources.TextMessages.DropDown_NotSelectedText, ""));
                EditModeDataPrepare(this.formObjectID);
                BindPatientList(CurFormObject);
            }

            
        }
        else
        {
            SetRedirectFailureAlert(Resources.TextMessages.EditPatientContact_Error);
            Response.Redirect("PatientCheckIn.aspx", false);
        }
    }

    protected void BindPatientList(ScreeningResult patient)
    {
        ctrlMatchedPatientList.CurrentPatient = CurFormObject;
        ctrlMatchedPatientList.Update();
    }

    protected override ScreeningResult GetFormObjectByID(long objectID)
    {
        return ScreeningResultHelper.GetScreeningResult(objectID);
    }

    protected override void EditModeDataPrepare(long objectID)
    {
        txtFirstName.Text = CurFormObject.FirstName;
        txtLastName.Text = CurFormObject.LastName;
        txtMiddleName.Text = CurFormObject.MiddleName;
        txtBirthday.SelectedDate = CurFormObject.Birthday;
        ccPhone.Value = CurFormObject.Phone;
        txtAddress.Text = CurFormObject.StreetAddress;
        txtCity.Text = CurFormObject.City;
        ddlState.SelectedValue = CurFormObject.StateID;
        txtZipCode.Text = CurFormObject.ZipCode;
    }

    protected override ScreeningResult GetFormData()
    {
        ScreeningResult patient = CurFormObject;
        CurFormObject.FirstName = txtFirstName.Text.ToUpperInvariant();
        CurFormObject.LastName = txtLastName.Text.ToUpperInvariant();
        CurFormObject.MiddleName = txtMiddleName.Text.ToUpperInvariant();

        CurFormObject.Birthday = txtBirthday.SelectedDate.Value;
        CurFormObject.Phone = ccPhone.Value;
        CurFormObject.StreetAddress = txtAddress.Text.ToUpperInvariant();
        CurFormObject.City = txtCity.Text.ToUpperInvariant();
        CurFormObject.StateID = ddlState.SelectedValue;
        CurFormObject.ZipCode = txtZipCode.Text;

        return patient;
    }

    public override void SetTabIndex()
    {
        if (!IsPostBack)
        {
            txtFirstName.Focus();
        }
    }

    #region page permissions

    protected override void SetPagePermissions()
    {
        //Trace.Warn("SetPagePermissions");

        base.SetPagePermissions();
        ExportPermission = true;
        _userRoles = Roles.GetRolesForUser();

        if (_userRoles.Contains(UserRoles.SuperAdministrator) || _userRoles.Contains(UserRoles.BranchAdministrator))
        {
            ReadPermission = true;
            WritePermission = true;
            PrintPermission = true;
            DeletePermission = true;
        }
        else if (_userRoles.Contains(UserRoles.LeadMedicalProfessionals))
        {
            ReadPermission = true;
            PrintPermission = true;
            WritePermission = true;
            DeletePermission = false;
        }
        else if (_userRoles.Contains(UserRoles.MedicalProfessionals))
        {
            ReadPermission = true;
            PrintPermission = false;
            WritePermission = false;
            DeletePermission = false;
        }

        else if (_userRoles.Contains(UserRoles.Staff))
        {
            ReadPermission = true;
            PrintPermission = false;
            WritePermission = false;
            DeletePermission = false;
            ExportPermission = false; //export disabled
        }
        else
        {
            ReadPermission = false;
            WritePermission = false;
            DeletePermission = false;
            ExportPermission = false;

            Response.Redirect("Default.aspx");
        }
    }

    protected bool CheckAddressFieldsIsDisplayed()
    {
        //check if address screening is off or screening does not have any information from address.
        var contactSetting = _minimalAgeService.GetSectionMinimalAgeSettings(ScreeningProfile.DefaultProfileID)
            .FirstOrDefault(x => x.ScreeningSectionID == ScreeningFrequencyDescriptor.ContactFrequencyID) ?? new ScreeningSectionAge { IsEnabled = true };

        return contactSetting.IsEnabled || !CurFormObject.IsEmptyContactInfo();
    }

    protected override void SetControlsEnabledState()
    {
        pnlExportEligibilityNotes.Visible = !CurFormObject.IsEligible4Export;

        btnExport.Enabled = ExportPermission && (!exportController.RpmsCredentialsAreExpired() ?? false);
        
        btnSave.Enabled = HasWritePermission;

        phAddressDetails.Visible = CheckAddressFieldsIsDisplayed();

        //logging
        if (_logger.IsDebugEnabled)
        {
            _logger.DebugFormat("[Export UI] ScreeningResultID: {0}. User Roles:{1}", CurFormObject.ID, string.Join(",", _userRoles));

            if (!btnExport.Visible)
            {
                _logger.DebugFormat("[Export UI] Button Export is hidden. ScreeningResultID: {0}. IsEligible4Export: {1}.", CurFormObject.ID, CurFormObject.IsEligible4Export);
            }

            if (!btnExport.Enabled)
            {
                _logger.DebugFormat("[Export UI] Button Export is disabled. ScreeningResultID: {0}. ExportPermission: {1}.", CurFormObject.ID, ExportPermission);
            }
        }
    }

    #endregion


    protected string GetAlreadyExportedWarning()
    {
        return FrontDesk.Server.Resources.TextMessages.ExportWizard_AlreadyExportedWarning.FormatWith(CurFormObject.ExportDate.FormatAsDate());
    }

    protected string GetPatientInfoAsJson()
    {
        return ((ScreeningPatientIdentityWithAddress)CurFormObject).ToJsonStrict();
    }

    #region page event
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            var patient = GetFormData();
            try
            {

                //Update screening result
                ScreeningResultHelper.Update(patient);

                SecurityLog.Add(new SecurityLog(SecurityEvents.EditPatientContactInformation, this.formObjectID, this.CurFormObject.LocationID));

                FrontDesk.StateObjects.EhrInterfaceProxy.Instance.ResetCache4GetMatchedPatients(this.CurFormObject);

                SetSuccessAlert(FrontDesk.Common.Messages.CustomMessage.GetUpdateMessage("Patient contact information"), this.GetType());
                //Response.Redirect(String.Format("~/EditPatientContact.aspx?id={0}", this.formObjectID), false);

                BindPatientList(patient);
               
            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                ErrorLog.Add(ex.Message, ex.StackTrace, null);
                SetErrorAlert(FrontDesk.Common.Messages.CustomError.GetUpdateMessage("Patient contact information"), this.GetType());
            }
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        if (!this.ctrlMatchedPatientList.SelectedItemID.HasValue)
        {
            SetWarningAlert(Resources.TextMessages.ExportWizard_PleaseSelectPatient, typeof(EditPatientContact));
            return;
        }

        string redirectUrl = GetRedirectToPageUrl("~/ExportWizard.aspx", new Dictionary<string, object>{
                                {UrlParametersDescriptor.ScreeningResultIDParameter, this.formObjectID},
                                {UrlParametersDescriptor.RpmsPatientRowIdParameter, this.ctrlMatchedPatientList.SelectedPatientRecord.AsJson()},
                                {UrlParametersDescriptor.WizardStepParameter, (int)ExportWizardSteps.SelectVisit}
                        });

        Response.Redirect(redirectUrl);
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        Response.Redirect(String.Format("{0}?id={1}", Request.Path, this.formObjectID), false);
    }

    /// <summary>
    /// Validate patient birthday
    /// </summary>
    protected void BirthdayValidate(object source, ServerValidateEventArgs args)
    {
        try
        {
            args.IsValid = (DateTime)txtBirthday.SelectedDate < DateTime.Now;
        }

        catch
        {
            args.IsValid = false;
        }

    }

    protected void ZipCodeValidate(object source, ServerValidateEventArgs args)
    {
        try
        {
            args.IsValid = txtZipCode.Text.Length == 5;
        }

        catch
        {
            args.IsValid = false;
        }

    }
    #endregion
}
