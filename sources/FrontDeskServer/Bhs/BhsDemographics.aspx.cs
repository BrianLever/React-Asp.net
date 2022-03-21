using Common.Logging;
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

public partial class BhsDemographicsForm : BaseManagementWebForm<BhsDemographics, long>
{
    private readonly BhsDemographicsService _service = new BhsDemographicsService();
    private readonly ILog _logger = LogManager.GetLogger<BhsDemographicsForm>();

    private bool ViewDetailsPermissions;

    protected override void SetPagePermissions()
    {
        base.SetPagePermissions();

        var roles = new List<string>(Roles.GetRolesForUser());
        // Check Role-based permissions
        if (roles.Contains(UserRoles.Staff))
        {
            ReadPermission = true;
            ViewDetailsPermissions = false;
            WritePermission = false;
            PrintPermission = false;
        }
        else
        {
            ReadPermission = true;
            ViewDetailsPermissions = true;
            WritePermission = true;
            PrintPermission = true;
        }
    }

    protected override BhsDemographics GetFormObjectByID(long objectID)
    {
        return _service.Get(objectID);
    }


    protected void Page_Init(object sender, EventArgs e)
    {
        BackPageUrl = "~/Bhs/BhsVisitList.aspx";

        if (!TryGetPageIDValue("id"))
        {
            // id not found
            Response.Redirect(BackPageUrl, true);
        }
        EnsureFormObjectCreated();
        if (IsNewInstance)
        {
            _logger.InfoFormat("[BhsDemographicsForm] Id not found, redirecting to the visit list. ID: {0}", this.formObjectID);
            Response.Redirect(BackPageUrl, true);
        }

        ucDemographics.HideContent = !ViewDetailsPermissions;
        ucDemographics.Visible = ViewDetailsPermissions;

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Master.PageHeaderText = "Patient Demographics";

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
        lblLocation.Text = !string.IsNullOrEmpty(CurFormObject.LocationLabel) ? CurFormObject.LocationLabel : "N/A";

        if (!IsPostBack)
        {
            ucDemographics.DataBind();
            ucDemographics.SetModel(CurFormObject);
        }
        else
        {
            ucDemographics.SetReadOnlyFields(CurFormObject);
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        Print();
    }

    protected void btnFindAddress_Click(object sender, EventArgs e)
    {
        if (CurFormObject.IsEmptyContactInfo())
        {
            _logger.InfoFormat("[BhsDemographicsForm] Contact Info is empty. Redirecting to Patient Search page. ID: {0}", this.formObjectID);
            Response.Redirect("~/FindPatientAddress.aspx?id={0}&{2}={1}"
                .FormatWith(CurFormObject.ScreeningResultID, "BhsDemographics.aspx?id={0}".FormatWith(CurFormObject.ID), WebPageParametersDescriptor.BackUrlParam), true);
        }
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
                SecurityLog.Add(new SecurityLog(SecurityEvents.UpdateBhsDemographicsInformation, "{0}~{1}~{2}".FormatWith(this.formObjectID, CurFormObject.FullName, CurFormObject.Birthday.FormatAsDate()), this.CurFormObject.LocationID));


                SetRedirectSuccessAlert(FrontDesk.Common.Messages.CustomMessage.GetUpdateMessage("Patient Demographics"));
                Response.Redirect(BhsWebPagesDescriptor.DemographicsPageUrlTemplate.FormatWith(this.formObjectID), false);

            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                ErrorLog.Add(ex.Message, ex.StackTrace, null);
                SetErrorAlert(FrontDesk.Common.Messages.CustomError.GetUpdateMessage("Patient Demographics"), this.GetType());
            }
        }
    }



    protected override void ApplyPagePermissions()
    {
        base.ApplyPagePermissions();

        btnPrint.Visible = HasPrintPermission;
        btnSaveChanges.Visible = HasWritePermission;
        btnFindAddress.Visible = CurFormObject.IsEmptyContactInfo();
    }

    protected override void SetControlsEnabledState()
    {
        base.SetControlsEnabledState();
    }

    protected override BhsDemographics GetFormData()
    {
        var model = ucDemographics.GetModel();


        return model;
    }


    protected void Print()
    {
        try
        {
            //Print report into context
            if (CurFormObject != null)
            {
                var pdfRender = new BhsDemographicsPdfPrintout(CurFormObject);
                pdfRender.CreatePDF(System.Web.HttpContext.Current, "PatientDemographicsReport_{0}.pdf".FormatWith(CurFormObject.ID));
            }
          
            SecurityLog.Add(new SecurityLog(SecurityEvents.PrintBhsDemographics, this.formObjectID, this.CurFormObject.LocationID));

        }
        catch (Exception ex)
        {
            SetErrorAlert(CustomError.GetInternalErrorMessage(), this.GetType());
            var msg = Resources.TextMessages.BhsDemographicsPrintingFailed.FormatWith(formObjectID);
            ErrorLog.AddServerException(msg, ex);
        }
    }

}