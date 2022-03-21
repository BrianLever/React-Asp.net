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
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class BhsFollowUpForm : BaseManagementWebForm<BhsFollowUp, long>
{
    private readonly BhsFollowUpService _service = new BhsFollowUpService();
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
            ReadPermission = false;
            ViewDetailsPermissions = false;
            WritePermission = false;
            PrintPermission = false;
        }
        else
        {
            ReadPermission = false ;
        }
       

        BackPageUrl = "~/bhs/FollowUpList.aspx";

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

            var location = CurFormObject.Visit.LocationID;

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

    protected override BhsFollowUp GetFormObjectByID(long objectID)
    {
        return _service.Get(objectID);
    }


    protected void Page_Init(object sender, EventArgs e)
    {
        
        Page.ClientScript.RegisterClientScriptInclude("quill.js", ResolveClientUrl("~/scripts/quill.js"));
        Page.ClientScript.RegisterClientScriptInclude("pageEvents.js", ResolveClientUrl("~/scripts/controls/pageEvents.js"));


        ucDetails.Visible = ViewDetailsPermissions;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Master.PageHeaderText = "Follow-Up Report";

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
        lblCreatedDate.Text = CurFormObject.Visit.CreatedDate.FormatAsDateWithTime();
        lblLocation.Text = !string.IsNullOrEmpty(CurFormObject.Result.LocationLabel) ? CurFormObject.Result.LocationLabel : "N/A";
        lnkScreening.NavigateUrl = "~/PatientCheckIn.aspx?id=" + CurFormObject.ScreeningResultID;
        lnkScreening.Text = CurFormObject.ScreeningResultID.ToString();
        lnkVisit.NavigateUrl = "~/bhs/BhsVisit.aspx?id=" + CurFormObject.Visit.ID;
        lnkVisit.Text = CurFormObject.Visit.ID.ToString();


        if (!IsPostBack)
        {
            ucDetails.DataBind();
            ucDetails.SetModel(CurFormObject);
        }
        else
        {
            ucDetails.SetReadOnlyFields(CurFormObject);
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

            var model = GetFormData();
            try
            {

                //Update screening result
                _service.Update(model, FDUser.GetCurrentUser());
                SecurityLog.Add(new SecurityLog(SecurityEvents.UpdateBhsThirtyDayFollowUpInformation, "{0}~{1}~{2}".FormatWith(this.formObjectID, CurFormObject.Result.FullName, CurFormObject.Result.Birthday.FormatAsDate()), this.CurFormObject.Visit.LocationID));


                SetRedirectSuccessAlert(FrontDesk.Common.Messages.CustomMessage.GetUpdateMessage("Follow-Up Report"));
                Response.Redirect(BhsWebPagesDescriptor.FollowUpPageUrlTemplate.FormatWith(this.formObjectID), false);

            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                ErrorLog.Add(ex.Message, ex.StackTrace, null);
                SetErrorAlert(FrontDesk.Common.Messages.CustomError.GetUpdateMessage("Follow-Up Report"), this.GetType());
            }
        }
    }



    protected override void ApplyPagePermissions()
    {
        base.ApplyPagePermissions();

        btnPrint.Visible = HasPrintPermission;
        btnSaveChanges.Visible = HasWritePermission;

    }

    protected override void SetControlsEnabledState()
    {
        base.SetControlsEnabledState();
    }

    protected override BhsFollowUp GetFormData()
    {
        var model = ucDetails.GetModel();


        return model;
    }


    protected void Print()
    {
        try
        {
            //Print report into context
            if (CurFormObject != null)
            {
                var pdfRender = new BhsFollowUpPdfPrintout(CurFormObject);
                pdfRender.CreatePDF(System.Web.HttpContext.Current, "BhsFollowUpReport_{0}.pdf".FormatWith(CurFormObject.ID));
            }

            SecurityLog.Add(new SecurityLog(SecurityEvents.PrintBhsFollowUp, this.formObjectID, this.CurFormObject.Result.LocationID));

        }
        catch (Exception ex)
        {
            SetErrorAlert(CustomError.GetInternalErrorMessage(), this.GetType());
            var msg = Resources.TextMessages.BhsFollowUpPrintingFailed.FormatWith(formObjectID);
            ErrorLog.AddServerException(msg, ex);
        }
    }

}