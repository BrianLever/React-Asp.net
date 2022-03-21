using System;
using System.Web.UI.WebControls;

using Common.Logging;

using FrontDesk.Server.Extensions;
using FrontDesk.Server.Web;
using FrontDesk.Server.Web.Controls;
using FrontDesk.StateObjects;

using RPMS.Common.Models;

public partial class PatientScheduledVisits : Controls.PagedItemsUserControl
{
    //public int? SelectedPatientRecordId { get; set; }

    private ILog _logger = LogManager.GetLogger<PatientScheduledVisits>();

    public PatientSearch SelectedPatientRecord
    {
        get
        {
            bool isFound;
            var stringValue = GetIDValue<string>("SelectedPatientRecord", PageIDSource.ViewState, out isFound);

            return stringValue.FromJson<PatientSearch>();
        }
        set
        {
            ViewState["SelectedPatientRecord"] = value.AsJson();
        }
    }

    public DateTime ScreeningDate { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //Get count of candidats for export from EHR database
            RowsCount = EhrInterfaceProxy.Instance.GetScheduledVisitsByPatientCount(SelectedPatientRecord);

            BindListData();
        }
    }

    /// <summary>
    /// Bind visits for patient
    /// </summary>
    protected override void BindListData()
    {
        if (RowsCount > 0)
        {
            PagetControl.RowsCount = RowsCount;
            rptVisits.DataSource = EhrInterfaceProxy.Instance.GetScheduledVisitsByPatient(SelectedPatientRecord, StartRow, 5);
            rptVisits.DataBind();
            phNoMatches.Visible = false;

            _logger.InfoFormat("[UI][PatientScheduledVisits] Binding patient's visits. Record: {0}, Start Row: {1}, Rows Count: {2}", SelectedPatientRecord.ID, StartRow, RowsCount);
        }
        else
        {
            phNoMatches.Visible = true;
        }
    }

    protected override Repeater ItemsContainer
    {
        get { return rptVisits; }
    }

    protected override Pager PagetControl
    {
        get { return ctrlPaging; }
    }
}