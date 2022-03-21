using System;
using System.Collections.Generic;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;

using FrontDesk;
using FrontDesk.Common.Entity;
using FrontDesk.Server;
using FrontDesk.Server.Extensions;
using FrontDesk.Server.Logging;
using FrontDesk.Server.Screening;
using FrontDesk.Server.Screening.Mappers;
using FrontDesk.Server.Screening.Services;
using FrontDesk.Server.Web;
using FrontDesk.StateObjects;

using EhrInterface;

using Resources;

using RPMS.Common.Models;

public partial class ExportWizard : BaseManagementWebForm<ScreeningResult, long>
{

    private readonly IScreeningResultService _screeningResultService = new ScreeningResultService();
    private readonly IScreeningDefinitionService _screeningInfoService = new ScreeningDefinitionService();

    private ExportMetaInfo _exportMeta;

    #region Wizard parameters
    //public int? SelectedRpmsPatientID { get; set; }
    public PatientSearch SelectedRpmsPatient { get; set; }
    public int? SelectedRpmsVisitID { get; set; }


    ExportWizardSteps currentWizardStep = ExportWizardSteps.SelectPatient;

    #endregion

    #region Step Names and Descriptions

    string[] stepTitles = new string[]{
        FormTexts.ExportWizard_PatientRecordStepTitle,
        FormTexts.ExportWizard_FindAppointmentTitle,
        //FormTexts.ExportWizard_ConfirmationTitle,
        FormTexts.ExportWizard_ResultsTitle,
    };

	readonly string[] stepDescriptions = new string[]{
        FormTexts.ExportWizard_PatientRecordStepDescription,
        FormTexts.ExportWizard_FindAppointmentDescription,
        //FormTexts.ExportWizard_ConfirmationDescription,
        FormTexts.ExportWizard_ResultsDescription,
    };



    #endregion

    protected void Page_Init(object sender, EventArgs e)
    {

        for (int i = 0; i < exportWizard.WizardSteps.Count; i++)
        {
            exportWizard.WizardSteps[i].Title = stepTitles[i];
        }

        exportWizard.NextButtonClick += new WizardNavigationEventHandler(exportWizard_NextButtonClick);
        exportWizard.PreviousButtonClick += new WizardNavigationEventHandler(exportWizard_PreviousButtonClick);
        exportWizard.FinishButtonClick += new WizardNavigationEventHandler(exportWizard_NextButtonClick);
        exportWizard.SideBarButtonClick += new WizardNavigationEventHandler(exportWizard_PreviousButtonClick);
    }



    protected void Page_PreLoad(object sender, EventArgs e)
    {
        InitWizardStepResults();

        _exportMeta = EhrInterfaceProxy.Instance.GetMeta();

        EditModeDataPrepare(this.formObjectID);

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Master.PageHeaderText = "Export Screening Results to " + _exportMeta.GetExternalEhrSystemLabel();
    }

    protected override ScreeningResult GetFormObjectByID(long objectID)
    {
        return ScreeningResultHelper.GetScreeningResult(objectID);
    }

    protected override void EditModeDataPrepare(long objectID)
    {
        ctrlPatientRecord.ScreeningResult = this.CurFormObject;
        ctrlPatientRecord.RpmsPatient = this.SelectedRpmsPatient;

        ctrlVisits.ScreeningDate = this.CurFormObject.CreatedDate.Date;
        ctrlVisits.SelectedPatientRecord = this.SelectedRpmsPatient;
        
        ctrlPreviewExportResult.SelectedPatientRecord = this.SelectedRpmsPatient;
        ctrlPreviewExportResult.SelectedVisitID = this.SelectedRpmsVisitID;
        ctrlPreviewExportResult.ExportingScreeningResult = this.CurFormObject;
    }

    protected override ScreeningResult GetFormData()
    {
        return this.CurFormObject;
    }


    protected void Page_PreRender(object sender, EventArgs args)
    {
        // render on click confirmations
        if (exportWizard.ActiveStep.StepType == WizardStepType.Finish)
        {

            //(exportWizard.FindControlInTemplate(
            //    WizardNavigationTempContainer.FinishNavigationTemplateContainerID,
            //    "btnFinish") as Button)
            //    .OnClientClick = string.Format("if(!confirm('{0}')){{return false;}}",
            //        Resources.TextMessages.ConfirmBeginExport);
        }
        else if (exportWizard.ActiveStep.StepType == WizardStepType.Complete)
        {
            if (!CurFormObject.ExportDate.HasValue && SelectedRpmsVisitID.HasValue && SelectedRpmsPatient != null)
            {
                //execute export
                DoExport();
            }

        }
    }


    protected string GetStepDescription()
    {
        return stepDescriptions[exportWizard.ActiveStepIndex];
    }


    #region Vaidate current step and Init global form objects

    //read selected EHR records and validate current progress
    public void InitWizardStepResults()
    {
        //read wizard step
        {
            int step;
            if (TryGetPageIDValue<int>(UrlParametersDescriptor.WizardStepParameter, out step))
            {
                exportWizard.ActiveStepIndex = step;
            }
        }


        //read selected patient record
        {
            string rpmsPatientId;
            TryGetPageIDValue<string>(UrlParametersDescriptor.RpmsPatientRowIdParameter, out rpmsPatientId);
            if (!string.IsNullOrWhiteSpace( rpmsPatientId))
                SelectedRpmsPatient = rpmsPatientId.FromJson<PatientSearch>();
        }

        //read selected visit param
        {
            int rpmsVisitId;
            TryGetPageIDValue<int>(UrlParametersDescriptor.RpmsVisitRowIdParameter, out rpmsVisitId);
            if (rpmsVisitId > 0)
                SelectedRpmsVisitID = rpmsVisitId;
        }

        //validate export wizard step
        currentWizardStep = (ExportWizardSteps)exportWizard.ActiveStepIndex;



        if (currentWizardStep >= ExportWizardSteps.SelectVisit && SelectedRpmsPatient == null)
        {
            currentWizardStep--;
        }


        //read screening result
        TryGetPageIDValue(UrlParametersDescriptor.ScreeningResultIDParameter);

        this.EnsureFormObjectCreated();
        if (this.IsNewInstance)
        {
            RedirectToErrorPage(TextMessages.ExportWiz_ScreeningResultNotSelected);
        }

        if (this.CurFormObject.ExportDate.HasValue && currentWizardStep != ExportWizardSteps.Completed)
        {
            SetRedirectSuccessAlert(TextMessages.ExportWizard_ReportHasBeenExportedAlready);
            Response.Redirect(GetRedirectToPageUrl("~/PatientCheckIn.aspx", new Dictionary<string, object>{
                {"id", this.formObjectID}
            }));
        }

        //override step
        exportWizard.ActiveStepIndex = (int)currentWizardStep;

        CheckAndRedirectToSelectPatientPage(exportWizard.ActiveStepIndex);

    }

    private void CheckAndRedirectToSelectPatientPage(int currentStepIndex)
    {
        try
        {
            if (currentStepIndex == (int)ExportWizardSteps.SelectPatient)
            {
                Response.Redirect(GetRedirectToPageUrl("~/EditPatientContact.aspx", new Dictionary<string, object>{
                {"id", this.formObjectID}
            }));
            }
        }
        catch (ThreadAbortException) { }
    }


    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/", true);
        }
        catch (ThreadAbortException) { }
    }

    protected void btnPreviousOnError_Click(object sender, EventArgs e)
    {
        exportWizard_PreviousButtonClick(this,
            new WizardNavigationEventArgs((int)ExportWizardSteps.Completed, (int)ExportWizardSteps.SelectVisit));
    }

    void exportWizard_PreviousButtonClick(object sender, WizardNavigationEventArgs e)
    {
        string redirectUrl = string.Empty;
        if (e.CurrentStepIndex == (int)ExportWizardSteps.SelectVisit)
        {
            CheckAndRedirectToSelectPatientPage(e.CurrentStepIndex - 1);
        }
        else
        {
            int nextStepIndex = e.NextStepIndex;
            if (nextStepIndex == e.CurrentStepIndex)
                --nextStepIndex;

            redirectUrl = GetRedirectToPageUrl(Request.Url.ToString(), new Dictionary<string, object>{
                {UrlParametersDescriptor.WizardStepParameter, nextStepIndex}, 
                {"id", this.formObjectID},
                {UrlParametersDescriptor.RpmsPatientRowIdParameter, this.SelectedRpmsPatient.AsJson()}, 
                {UrlParametersDescriptor.RpmsVisitRowIdParameter,this.SelectedRpmsVisitID}
            });
        }
        try
        {
            Response.Redirect(redirectUrl);
        }
        catch (ThreadAbortException) { }
    }

    void exportWizard_NextButtonClick(object sender, WizardNavigationEventArgs e)
    {
        if (exportWizard.ActiveStepIndex == (int)ExportWizardSteps.SelectVisit)
        {
            this.SelectedRpmsVisitID = ctrlVisits.SelectedItemID;

            if (!this.SelectedRpmsVisitID.HasValue)
            {
                e.Cancel = true;

                SetRedirectWarningAlert(Resources.TextMessages.ExportWizard_PleaseSelectVisit);
            }

        }


        string redirectUrl = GetRedirectToPageUrl((Request.UrlReferrer ??  Request.Url).ToString(), new Dictionary<string, object>{
            {UrlParametersDescriptor.WizardStepParameter, e.Cancel? e.CurrentStepIndex : e.NextStepIndex}, 
            {"id", this.formObjectID},
            {UrlParametersDescriptor.RpmsPatientRowIdParameter, this.SelectedRpmsPatient.AsJson()}, 
            {UrlParametersDescriptor.RpmsVisitRowIdParameter,this.SelectedRpmsVisitID}
        }
        );
        try
        {
            Response.Redirect(redirectUrl);
        }
        catch (ThreadAbortException) { }
    }



    #endregion

    private void DoExport()
    {
        var screeningResult = this.CurFormObject;
        ExportOperationStatus status = ExportOperationStatus.Unknown;

        var proxy = EhrInterfaceProxy.Instance;
        try
        {
            var rpmsPatient = proxy.GetPatientRecord(this.SelectedRpmsPatient);
            var rpmsVisit = proxy.GetScheduledVisit(this.SelectedRpmsVisitID.Value, SelectedRpmsPatient);
           
          
            var results = proxy.CommitExportTask(rpmsPatient.ID, rpmsVisit.ID,
                proxy.PreviewExportResult(screeningResult, rpmsPatient, rpmsVisit.ID));
            
            results.AddRange(proxy.CommitExportResult(rpmsPatient.ID, rpmsVisit.ID, this.CurFormObject, _screeningInfoService.Get()));

            ctrlScreeningExportResults.ExportResults = results;

            status = results.GetExportOperationStatus();


            _screeningResultService.UpdateExportInfo(screeningResult, status, rpmsPatient, rpmsVisit, FDUser.GetCurrentUser().UserID);
           
            if (status == ExportOperationStatus.AllSucceed || status == ExportOperationStatus.SomeOperationsFailed)
            {
                
                BhsPatientAddressmapper.ImportPatientAddressFromEhr(screeningResult, rpmsPatient);
                new BhsVisitService().FullfilPatientAddress(screeningResult);


                SecurityLog.Add(new SecurityLog(SecurityEvents.EditPatientContactInformation,
                    String.Format("#{0}, {1} => EHR:{2}, Address: {3}",
                        screeningResult.ID,
                        screeningResult.FullName,
                        screeningResult.ExportedToHRN,
                        screeningResult.AddressToString()
                        ),
                    screeningResult.LocationID));
            }

        
        }
        catch (NonValidEntityException ex)
        {

            ctrlScreeningExportResults.UnhandledExceptionMessage =
                ErrorLog.AddServerException(TextMessages.FailedToUpdateFDExportDetails, ex).ErrorMessage;

            status = status == ExportOperationStatus.Unknown? //exception happned before getting export status
                ExportOperationStatus.AllFailed: 
                ExportOperationStatus.SomeOperationsFailed;

        }
        catch (Exception ex)
        {
            ctrlScreeningExportResults.UnhandledExceptionMessage =
                ErrorLog.AddServerException(TextMessages.ExportFailed, ex).ErrorMessage;

            status = status == ExportOperationStatus.Unknown ? //exception happned before getting export status
               ExportOperationStatus.AllFailed :
               ExportOperationStatus.SomeOperationsFailed;

        }
        finally
        {
            SetResultPageControlVisibility(status);
        }
    }

    private void SetResultPageControlVisibility(ExportOperationStatus status)
    {
        plhSucceed.Visible = false;
        btnPreviousOnError.Visible = false;
        btnClose.Visible = false;

        if (status == ExportOperationStatus.AllSucceed)
        {
            plhSucceed.Visible = true;
            
        }
        else if (status == ExportOperationStatus.AllFailed)
        {
            btnPreviousOnError.Visible = true;
            btnClose.Visible = true;
        }
        else
        {
            btnClose.Visible = true;
        }

        if (!btnClose.Visible)
        {
            btnClose.Visible = true;
            btnClose.Style.Add(HtmlTextWriterStyle.Visibility, "hidden");
        }
    }
}
