using FrontDesk;
using FrontDesk.Common.Bhservice;
using FrontDesk.Common.Extensions;
using FrontDesk.Common.Messages;
using FrontDesk.Server;
using FrontDesk.Server.Descriptors;
using FrontDesk.Server.Logging;
using FrontDesk.Server.Membership;
using FrontDesk.Server.Printouts.Bhs;
using FrontDesk.Server.Screening.Services;
using FrontDesk.Server.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Web.UI;

public partial class BhsVisitForm : BaseManagementWebForm<BhsVisit, long>
{
    private readonly BhsVisitService _service = new BhsVisitService();
    private readonly IBhsFollowUpService _followUpService = new BhsFollowUpService();
    private readonly IScreeningDefinitionService _screeningDefinitionService = new ScreeningDefinitionService();
    protected bool ViewDetailsPermissions;

    protected override void SetPagePermissions()
    {
        base.SetPagePermissions();

        bool isAdminUser = false; //whether current user is admin
        ViewDetailsPermissions = true;

        var roles = new List<string>(Roles.GetRolesForUser());
        // Check Role-based permissions
        if (roles.Any(p => p == UserRoles.BranchAdministrator || p == UserRoles.SuperAdministrator))
        {
            isAdminUser = true;
            ReadPermission = true;
            WritePermission = true;
            PrintPermission = true;
        }
        else if (roles.Contains(UserRoles.LeadMedicalProfessionals))
        {
            ReadPermission = true;
            WritePermission = true;
            PrintPermission = true;
        }
        else if (roles.Contains(UserRoles.MedicalProfessionals))
        {
            ReadPermission = true;
            WritePermission = true;
            PrintPermission = true;
        }
        else if (roles.Contains(UserRoles.Staff))
        {
            ReadPermission = true;
            PrintPermission = false;
            WritePermission = false;
            ViewDetailsPermissions = false;
        }

        if (!HasReadPermission)
        {
            //do not have at least read permissions
            RedirectToErrorPage(Resources.TextMessages.AccessPermissionsErrorMessage);
        }


        //init
        BackPageUrl = "~/Bhs/BhsVisitList.aspx";

        if (!TryGetPageIDValue("id"))
        {
            // id not found
            Response.Redirect(BackPageUrl, true);
        }
        EnsureFormObjectCreated();
        if (IsNewInstance)
        {
            Response.Redirect(BackPageUrl, true);
        }

        //Check location based permissions
        if (!isAdminUser)
        {
            var curUser = FDUser.GetCurrentUser();

            var location = CurFormObject.LocationID;

            if (curUser.BranchLocationID.Value != location)
            {
                ReadPermission = false;
            }
        }
        

        if (!HasReadPermission)
        {

            //do not have at least read permissions
            //we disable session - so no alert
            //SetRedirectAlert(Resources.TextMessages.AccessPermissionsErrorMessage);
            RedirectToErrorPage(Resources.TextMessages.AccessPermissionsErrorMessage);
            //Response.Redirect("~/Default.aspx", true); 

        }

    }


    protected override BhsVisit GetFormObjectByID(long objectID)
    {
        return _service.Get(objectID);
    }

    protected void Page_Init(object sender, EventArgs e)
    {

        Page.ClientScript.RegisterClientScriptInclude("quill.js", ResolveClientUrl("~/scripts/quill.js"));
        Page.ClientScript.RegisterClientScriptInclude("pageEvents.js", ResolveClientUrl("~/scripts/controls/pageEvents.js"));



        ucVisit.HideContent = !ViewDetailsPermissions;
        btnPrint.Visible = HasPrintPermission;
        btnSaveChanges.Visible = HasWritePermission;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Master.PageHeaderText = "Visit Report";

        btnBack.OnClientClick = btnReturn.OnClientClick = string.Format("location.href='{0}'; return false;", ResolveClientUrl(BackPageUrl));



        if (!IsPostBack)
        {
            //SecurityLog.Add(new SecurityLog(SecurityEvents.ViewBHR,
            //    this.formObjectID, this.CurFormObject.LocationID));


        }
        EditModeDataPrepare(formObjectID);

    }


    protected override void EditModeDataPrepare(long objectID)
    {
        lblRecordID.Text = CurFormObject.ID.ToString();
        lblCreatedDate.Text = CurFormObject.ScreeningDate.FormatAsDateWithTime();
        lblLocation.Text = !string.IsNullOrEmpty(CurFormObject.Result.LocationLabel) ? CurFormObject.Result.LocationLabel : "N/A";
        lnkScreening.NavigateUrl = "~/PatientCheckIn.aspx?id=" + CurFormObject.ScreeningResultID;
        lnkScreening.Text = CurFormObject.ScreeningResultID.ToString();

        if (!IsPostBack)
        {
            ucVisit.DataBind();
            ucVisit.SetModel(CurFormObject);
        }
        else
        {
            ucVisit.SetReadOnlyFields(CurFormObject);
        }

        var followUpReports = _followUpService.GetFollowUpsForVisit(formObjectID);
        if (followUpReports.Count > 0)
        {
            phFollowUpReports.Visible = true;
            rptFollowUpVisits.DataSource = followUpReports;
            rptFollowUpVisits.DataBind();
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        Print();
    }

    protected void btnSaveChanges_Click(object sender, EventArgs e)
    {
        Page.Validate();
        if (Page.IsValid)
        {

            //TO DO: validate
            var model = GetFormData();
            try
            {
                var curUser = FDUser.GetCurrentUser();
                //Update screening result
                _service.Update(model, curUser);
                SecurityLog.Add(new SecurityLog(SecurityEvents.UpdateBhsVisitInformation, "{0}~{1}~{2}".FormatWith(this.formObjectID, CurFormObject.Result.FullName, CurFormObject.Result.Birthday.FormatAsDate()), this.CurFormObject.LocationID));

                _service.UpdateDrugOfChoiceInResult(model, curUser);

                SetRedirectSuccessAlert(FrontDesk.Common.Messages.CustomMessage.GetUpdateMessage("Visit"));
                Response.Redirect(BhsWebPagesDescriptor.VisitPageUrlTemplate.FormatWith(this.formObjectID), false);

            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                ErrorLog.Add(ex.Message, ex.StackTrace, null);
                SetErrorAlert(FrontDesk.Common.Messages.CustomError.GetUpdateMessage("Visit"), this.GetType());
            }
        }
    }

    protected void btnFindAddress_Click(object sender, EventArgs e)
    {
        if (CurFormObject.Result.IsEmptyContactInfo())
        {
            Response.Redirect("~/FindPatientAddress.aspx?id={0}&backUrl={1}".FormatWith(CurFormObject.ScreeningResultID, "BhsVisit.aspx?id={0}".FormatWith(CurFormObject.ID)), true);
        }
    }

    protected override void ApplyPagePermissions()
    {
        base.ApplyPagePermissions();

        btnFindAddress.Visible = CurFormObject.Result.IsEmptyContactInfo();
    }

    protected override void SetControlsEnabledState()
    {
        base.SetControlsEnabledState();
    }

    protected override BhsVisit GetFormData()
    {
        var model = ucVisit.GetModel();


        return model;
    }


    protected void Print()
    {
        try
        {
            //Print report into context
            if (CurFormObject != null)
            {
                var pdfRender = new BhsVisitPdfPrintout(CurFormObject, _screeningDefinitionService);
                pdfRender.CreatePDF(System.Web.HttpContext.Current, "BhsVisitReport_{0}.pdf".FormatWith(CurFormObject.ID));
            }

            SecurityLog.Add(new SecurityLog(SecurityEvents.PrintBhsVisit, this.formObjectID, this.CurFormObject.LocationID));

        }
        catch (Exception ex)
        {
            SetErrorAlert(CustomError.GetInternalErrorMessage(), this.GetType());
            var msg = Resources.TextMessages.BhsVisitPrintingFailed.FormatWith(formObjectID);
            ErrorLog.AddServerException(msg, ex);
        }
    }

}