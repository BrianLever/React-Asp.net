using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FrontDesk.Server.Screening;
using FrontDesk.Server.Web;
using FrontDesk;
using FrontDesk.Common.Messages;
using FrontDesk.Server.Logging;
using System.Security.Permissions;
using FrontDesk.Server.Membership;
using System.Web.Security;
using FrontDesk.Server;
using FrontDesk.Server.Reports;
using FrontDesk.Server.Screening.Services;
using FrontDesk.Common.Logging;
using Common.Logging;
using FrontDesk.Common.Extensions;
using FrontDesk.Server.Descriptors;
using ScreenDox.Server.Common.Services;

public partial class PatientCheckIn : BaseManagementWebForm<ScreeningResult, long>
{
    private ILog _logger = LoggerFactory.GetLogger();
    private BhsVisitService _service = new BhsVisitService();
    private BhsDemographicsService _demographicsService = new BhsDemographicsService();
    private readonly IKioskService _kioskService = new KioskService();

    protected Int64? PatientDemographicsID;

    protected string PatientDemographicsUrl
    {
        get
        {
            return ResolveClientUrl(BhsWebPagesDescriptor.DemographicsPageUrlTemplate.FormatWith(PatientDemographicsID ?? 0));
        }
    }

    #region Check Security Rules
    protected bool HasCreateVhsVisitPermission = true;

    protected override void SetPagePermissions()
    {
        base.SetPagePermissions();

        bool isAdminUser = false; //whether current user is admin
        WritePermission = false;

        if (!TryGetPageIDValue("id"))
        {
            // id not found
            Response.Redirect("~/Default.aspx", true);
        }
        try
        {

            var roles = new List<string>(Roles.GetRolesForUser());
            // Check Role-based permissions
            if (roles.Any(p => p == UserRoles.BranchAdministrator || p == UserRoles.SuperAdministrator))
            {
                DeletePermission = true;
                ReadPermission = true;
                PrintPermission = true;
                isAdminUser = true;
                WritePermission = true;
            }
            else if (roles.Contains(UserRoles.LeadMedicalProfessionals))
            {
                ReadPermission = true;
                PrintPermission = true;
                DeletePermission = true; //updated on 10/15/2018
                WritePermission = true;

            }
            else if (roles.Contains(UserRoles.MedicalProfessionals))
            {
                ReadPermission = true;
                PrintPermission = true;
                DeletePermission = false;
                WritePermission = true;

            }
            else if (roles.Contains(UserRoles.Staff))
            {
                ReadPermission = true;
                PrintPermission = false;
                DeletePermission = false;
                WritePermission = false;
                HasCreateVhsVisitPermission = false;
            }

            if (!HasReadPermission)
            {
                //do not have at least read permissions
                RedirectToErrorPage(Resources.TextMessages.AccessPermissionsErrorMessage);
                //we disable session - so no alert

                //SetRedirectAlert(Resources.TextMessages.AccessPermissionsErrorMessage);
                //Response.Redirect("~/Default.aspx", true);
            }




            EnsureFormObjectCreated();
            if (IsNewInstance)
            {
                Response.Redirect("~/Default.aspx", true);
            }
            //Check location based permissions
            if (!isAdminUser)
            {
                var curUser = FDUser.GetCurrentUser();

                var kiosk = _kioskService.GetByID(CurFormObject.KioskID);
                if (kiosk != null)
                {
                    if (curUser.BranchLocationID.Value != kiosk.BranchLocationID)
                    {
                        ReadPermission = false;
                    }
                }
                else
                {
                    ReadPermission = false;
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

        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            ErrorLog.Add("PatientCheckIn page initialization error. " + ex.Message, ex.StackTrace, null);
            RedirectToErrorPage(CustomError.GetInternalErrorMessage());
        }
    }

    protected override void ApplyPagePermissions()
    {
        base.ApplyPagePermissions();

        btnDelete.Visible = HasDeletePermission;
        if (HasDeletePermission)
        {
            btnDelete.OnClientClick = string.Format("if(!confirm('{0}')){{return false;}}", Resources.TextMessages.DeleteCheckInConfirmMessage);
        }
        btnEdit.Visible = HasWritePermission;
        btnPrint.Visible = HasPrintPermission;
        ucReport.isStaff = HasReadPermission && User.IsInRole(UserRoles.Staff);
        btnCreateBhsVisit.Visible = HasCreateVhsVisitPermission;

    }

    protected override void SetControlsEnabledState()
    {
        base.SetControlsEnabledState();
        phValidationErrors.Visible = CurFormObject.WithErrors;


    }


    #endregion

    protected void Page_Init(object sender, EventArgs e)
    {
        BackPageUrl = "~/Default.aspx";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Master.PageHeaderText = "Patient Behavioral Health Screening Report";

        ucReport.ScreeningResult = this.CurFormObject;
        btnBack.OnClientClick = string.Format("location.href='{0}'; return false;", ResolveClientUrl(BackPageUrl));

        if (_service.FindByScreeningResultId(formObjectID).HasValue)
        {
            btnCreateBhsVisit.Text = "Open Visit";
        }
        else
        {
            btnCreateBhsVisit.Text = "Create Visit";
        }
        EditModeDataPrepare(formObjectID);

        if (!IsPostBack)
        {
            SecurityLog.Add(new SecurityLog(SecurityEvents.ViewBHR,
                this.formObjectID, this.CurFormObject.LocationID));
        }
    }


    protected override ScreeningResult GetFormObjectByID(long objectID)
    {
        return ScreeningResultHelper.GetScreeningResult(objectID);
    }

    protected override void EditModeDataPrepare(long objectID)
    {
        lblRecordID.Text = CurFormObject.ID.ToString();
        lblCreatedDate.Text = CurFormObject.CreatedDate.ToString("MM/dd/yyyy HH:mm zzz");
        lblLocation.Text = !string.IsNullOrEmpty(CurFormObject.LocationLabel) ? CurFormObject.LocationLabel : "N/A";

        if (CurFormObject.ExportDate.HasValue)
        {
            pnlExportInfo.Visible = true;
            lblExportDate.Text = CurFormObject.ExportDate.Value.ToString("MM/dd/yyyy HH:mm zzz");
            lblHRN.Text = CurFormObject.ExportedToHRN;
            lblLinkedVisitLocation.Text = CurFormObject.ExportedToVisitLocation;
            lblLinkedVisitTime.Text = CurFormObject.ExportedToVisitDate.Value.ToString("MM/dd/yyyy HH:mm");


            var exportOperator = FDUser.GetUserByID(CurFormObject.ExportedBy ?? 0);
            lblExportedBy.Text = exportOperator != null ? exportOperator.FullName : FrontDesk.Server.Resources.TextMessages.NA;

        }
        btnEdit.Visible = CurFormObject.IsEligible4Export;
        if (CurFormObject.WithErrors)
        {
            lblValdationErrors.Text = Resources.TextMessages.CheckInHasValidationErrors;

        }

        PatientDemographicsID = _demographicsService.Find(CurFormObject);
    }


    protected override ScreeningResult GetFormData()
    {
        return null;
    }

    //Handle delete check-in event
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (DeleteCheckInRecord())
        {
            Response.Redirect(BackPageUrl); //redirect if record has been deleted
        }
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        PrintCheckInRecord();
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        Response.Redirect(String.Format("EditPatientContact.aspx?id={0}", formObjectID));
    }

    protected void btnCreateBhsVisit_Click(object sender, EventArgs e)
    {
        long? visitId = null;
        //create bhs visit
        try
        {
            visitId = _service.FindByScreeningResultId(formObjectID);
            if (!visitId.HasValue)
            {
                _logger.InfoFormat("Creating new Visit from Patient Check-In page.");
                var visit = _service.Create(formObjectID);
                visitId = visit != null ? visit.ID : (long?)null;

                _logger.InfoFormat("Visit created from Patient Check-In page. Screening result id: {0}. Visit Id: {1}", formObjectID, visitId.Value);
                SecurityLog.Add(new SecurityLog(SecurityEvents.ManuallyCreateBhsVisitInformation,
                    String.Format("{0}~{1}", this.CurFormObject.FullName, this.CurFormObject.Birthday.FormatAsDate()),
                    this.CurFormObject.LocationID));
            }
        }
        catch (Exception ex)
        {
            SetErrorAlert(Resources.TextMessages.CheckInCreateBhsVisitFailed.FormatWith(formObjectID), this.GetType());
            ErrorLog.AddServerException(Resources.TextMessages.CheckInCreateBhsVisitFailed.FormatWith(formObjectID), ex);
        }

        if (visitId.HasValue)
        {
            Response.Redirect(String.Format("./Bhs/BhsVisit.aspx?id={0}", visitId.Value));
        }
    }

    [PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = UserRoles.BranchAdministrator)]
    [PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = UserRoles.SuperAdministrator)]
    [PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = UserRoles.LeadMedicalProfessionals)]

    protected bool DeleteCheckInRecord()
    {
        bool deleted = false;
        try
        {

            ScreeningResultHelper.Delete(this.formObjectID);
            SecurityLog.Add(new SecurityLog(SecurityEvents.BHRDeleted,
                String.Format("{0}~{1}", this.CurFormObject.FullName, this.CurFormObject.Birthday),
                this.CurFormObject.LocationID));
            deleted = true;
        }
        catch (ApplicationException ex)
        {
            SetErrorAlert(ex.Message, this.GetType());
        }
        catch (Exception ex)
        {
            SetErrorAlert(Resources.TextMessages.CheckInDeletionFailed, this.GetType());
            ErrorLog.AddServerException(Resources.TextMessages.CheckInDeletionFailed, ex);
        }

        return deleted;
    }

    [PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = UserRoles.BranchAdministrator)]
    [PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = UserRoles.SuperAdministrator)]
    [PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = UserRoles.LeadMedicalProfessionals)]
    [PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = UserRoles.MedicalProfessionals)]

    protected void PrintCheckInRecord()
    {
        try
        {
            //Print report into context
            if (CurFormObject != null)
            {
                BhsPdfReport BhsPdfReport = new BhsPdfReport(CurFormObject);
                BhsPdfReport.CreatePDF(HttpContext.Current, String.Format("{0} Report {1}.pdf", CurFormObject.ScreeningID, CurFormObject.ID));
            }

            SecurityLog.Add(new SecurityLog(SecurityEvents.PrintBHR, this.formObjectID, this.CurFormObject.LocationID));

        }
        catch (Exception ex)
        {
            SetErrorAlert(CustomError.GetInternalErrorMessage(), this.GetType());
            var msg = string.Format(Resources.TextMessages.CheckInPrintingFailed, formObjectID);
            ErrorLog.AddServerException(msg, ex);
        }
    }



}
