using System;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

using Common.Logging;

using FrontDesk;
using FrontDesk.Common.Extensions;
using FrontDesk.Server.Controllers;
using FrontDesk.Server.Logging;
using FrontDesk.Server.Membership;
using FrontDesk.Server.Screening;
using FrontDesk.Server.Screening.Services;
using FrontDesk.Server.Web;

using RPMS.Common.GlobalConfiguration;
using RPMS.Common.Security;

public partial class FindPatientContact : BaseManagementWebForm<ScreeningResult, long>
{
    private readonly RpmsExportController exportController;
    private readonly BhsVisitService _bhsVisitService = new BhsVisitService();
    private readonly ILog _logger = LogManager.GetLogger<FindPatientContact>();

    public FindPatientContact()
    {
        exportController = new RpmsExportController(
                new DefaultDateService(),
                new RpmsCredentialsService(new RpmsCredentialsRepository(), new CryptographyService()),
                new GlobalSettingsService()
                );

    }


    protected void Page_Init(object sender, EventArgs e)
    {
        odsState.TypeName = typeof(FrontDesk.State).FullName;
        ClientScript.RegisterClientScriptInclude(this.GetType(), "findPatientAddressCtrl.js", ResolveClientUrl("~/scripts/controls/findPatientAddressCtrl.js"));

        SavePreviousPageUrl();

    }



    protected void Page_Load(object sender, EventArgs e)
    {
        Master.PageHeaderText = "Select Patient Address";
        bool isIdFound = false;
        this.formObjectID = GetPageIDValue("id", out isIdFound);
        if (isIdFound)
        {
            EnsureFormObjectCreated();

            //btnBack.OnClientClick = string.Format("location.href='PatientCheckIn.aspx?id={0}'; return false;", formObjectID);

            if (!IsPostBack)
            {
                ddlState.DataBind();
                ddlState.Items.Insert(0, new ListItem(Resources.TextMessages.DropDown_NotSelectedText, ""));
                ctrlMatchedPatientList.CurrentPatient = CurFormObject;
                EditModeDataPrepare(this.formObjectID);
            }
        }
        else
        {
            SetRedirectFailureAlert(Resources.TextMessages.EditPatientContact_Error);
            Response.Redirect("PatientCheckIn.aspx", false);
        }

        _logger.InfoFormat("[Find Patient] Referral page: {0}", PreviousPageUrl);

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

        CurFormObject.ExportedToHRN = hdnPatientEhr.Value.Trim();
        CurFormObject.Phone = ccPhone.Value;
        CurFormObject.StreetAddress = txtAddress.Text.ToUpperInvariant();
        CurFormObject.City = txtCity.Text.ToUpperInvariant();
        CurFormObject.StateID = ddlState.SelectedValue;
        CurFormObject.ZipCode = txtZipCode.Text.Trim();

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

        if (Roles.IsUserInRole(UserRoles.SuperAdministrator) || Roles.IsUserInRole(UserRoles.BranchAdministrator))
        {
            ReadPermission = true;
            WritePermission = true;
            PrintPermission = true;
            DeletePermission = true;
        }
        else if (Roles.IsUserInRole(UserRoles.LeadMedicalProfessionals) || Roles.IsUserInRole(UserRoles.Staff))
        {
            ReadPermission = true;
            PrintPermission = false;
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

    protected bool CheckAddressFieldsIsDisplayed()
    {
        return true;
        //check if address screening is off or screening does not have any information from address.
        //var contactSetting = _minimalAgeService.GetSectionMinimalAgeSettings().FirstOrDefault(x => x.ScreeningSectionID == ScreeningFrequencyDescriptor.ContactFrequencyID) ?? new ScreeningSectionAge { IsEnabled = true };

        //return contactSetting.IsEnabled || !CurFormObject.IsEmptyContactInfo;
    }

    protected override void SetControlsEnabledState()
    {
        btnSave.Enabled = !exportController.RpmsCredentialsAreExpired() ?? false;
        phAddressDetails.Visible = CheckAddressFieldsIsDisplayed();
    }

    #endregion

    #region page event
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            var patient = GetFormData();
            try
            {

                _bhsVisitService.FullfilPatientAddress(patient);

                SecurityLog.Add(new SecurityLog(SecurityEvents.PatientAddressHasBeenAddedFromBhsVisit,"{0}~{1}".FormatWith(CurFormObject.FullName, CurFormObject.Birthday.FormatAsDate()), this.CurFormObject.LocationID));

                SetRedirectSuccessAlert(FrontDesk.Common.Messages.CustomMessage.GetUpdateMessage("Patient address information"));
                _logger.InfoFormat("[Find Patient] Patient name updated for patient [{0}]. Redirecting to {1}", patient.ID, PreviousPageUrl);

                var backurl = PreviousPageUrl;
                Response.Redirect(!string.IsNullOrEmpty(backurl) ? "~/bhs/{0}".FormatWith(backurl) : "~/Default.aspx", false);


            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                ErrorLog.Add(ex.Message, ex.StackTrace, null);
                SetErrorAlert(FrontDesk.Common.Messages.CustomError.GetUpdateMessage("Patient contact information"), this.GetType());
            }
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
