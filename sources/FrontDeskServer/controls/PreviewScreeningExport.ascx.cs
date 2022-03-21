using System;

using FrontDesk;
using FrontDesk.Server.Web.Controls;
using FrontDesk.StateObjects;

using RPMS.Common.Models;

public partial class PreviewScreeningExportControl : BaseUserControl
{
    public PatientSearch SelectedPatientRecord { get; set; }
    public int? SelectedVisitID { get; set; }

    public ScreeningResult ExportingScreeningResult { get; set; }


    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (ExportingScreeningResult != null)
        {
            ExportTask preview = EhrInterfaceProxy.Instance.PreviewExportResult(ExportingScreeningResult, SelectedPatientRecord, SelectedVisitID ?? 0);
            if (preview != null)
            {
                if (preview.Errors != null && preview.Errors.Count > 0)
                {
                    rptErrors.DataSource = preview.Errors;
                }
                else
                {
                    rptErrors.Visible = false;
                }

                if (preview.PatientRecordModifications.Count > 0)
                {
                    rptPatientRecordChanges.DataSource = preview.PatientRecordModifications;
                }
                else
                {
                    rptPatientRecordChanges.Visible = false;
                }


                if (preview.HealthFactors.Count > 0)
                {
                    rptHealthFactors.DataSource = preview.HealthFactors;
                }
                else
                {
                    rptHealthFactors.Visible = false;
                }

                if (preview.Exams.Count > 0)
                {
                    rptExams.DataSource = preview.Exams;
                }
                else
                {
                    rptExams.Visible = false;
                }

                if (preview.CrisisAlerts.Count > 0)
                {
                    rptCrisisAlerts.DataSource = preview.CrisisAlerts;
                }
                else
                {
                    rptCrisisAlerts.Visible = false;
                }

                if (preview.ScreeningSections.Count > 0)
                {
                    rptSections.DataSource = preview.ScreeningSections;
                }
                else
                {
                    rptSections.Visible = false;
                }
            }

            //run data bind
            this.DataBind();
        }


    }




    public override void ApplyTabIndexToControl(ref short startTabIndex)
    {
    }

}