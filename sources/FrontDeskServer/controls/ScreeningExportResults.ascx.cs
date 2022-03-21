using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FrontDesk.Server.Web.Controls;
using FrontDesk.StateObjects;
using FrontDesk;
using EhrInterface;
using RPMS.Common.Models;

public partial class ScreeningExportResultsControl : BaseUserControl
{
    public int? SelectedPatientRecordID { get; set; }
    public int? SelectedVisitID { get; set; }

    public IEnumerable<ExportResult> ExportResults { get; set; }

    public string UnhandledExceptionMessage { get; set; }

    public string Header { get; set; }
    public ExportOperationStatus ExportStatus { get; set; }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (this.ExportResults != null)
        {
            rptResults.DataSource = this.ExportResults;
            ProcessOperationResults();

        }

        if (!string.IsNullOrEmpty(UnhandledExceptionMessage))
        {
            phUnhandledException.Visible = true;
        }
        //run data bind
        this.DataBind();
    }

    private void ProcessOperationResults()
    {
        if (ExportResults != null)
        {
            ExportStatus = this.ExportResults.GetExportOperationStatus();
            switch (ExportStatus)
            {
                case ExportOperationStatus.AllSucceed:
                    Header = Resources.TextMessages.ExportWizard_ExportCompletedSuccessfully;
                    break;
                case ExportOperationStatus.AllFailed:
                    Header = Resources.TextMessages.ExportWizard_ExportFailed;
                    break;
                case ExportOperationStatus.SomeOperationsFailed:
                    Header = Resources.TextMessages.ExportWizard_ExportCompletedWithErrors;
                    break;

            }
        }
    }



    public override void ApplyTabIndexToControl(ref short startTabIndex)
    {
    }

}