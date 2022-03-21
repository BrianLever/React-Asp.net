using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FrontDesk.Server.Web.Controls;
using FrontDesk;
using FrontDesk.StateObjects;
using EhrInterface;
using RPMS.Common.Models;

public partial class ExportParametersAtGlance : BaseUserControl
{
    public ScreeningResult ScreeningResult { get; set; }

    public PatientSearch EhrPatient { get; set; }
    public int? EhrVisitRowID { get; set; }


    private Patient _patientRecord;
    private Visit _visitRecord;


    /// <summary>
    /// Selected Patient record from EHR
    /// </summary>
    public Patient PatientRecord
    {
        get
        {
            if (_patientRecord == null && EhrPatient != null)
            {
                _patientRecord = EhrInterfaceProxy.Instance.GetPatientRecord(EhrPatient);
            }
            return _patientRecord;
        }
        set
        {
            _patientRecord = value;
        }
    }

    /// <summary>
    /// Selected visit record from EHR
    /// </summary>
    public Visit VisitRecord
    {
        get
        {
            if (_visitRecord == null && EhrVisitRowID.HasValue) //not thread safe
            {
                _visitRecord = EhrInterfaceProxy.Instance.GetScheduledVisit(EhrVisitRowID.Value, EhrPatient);
            }
            return _visitRecord;
        }
        set
        {
            _visitRecord = value;
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        BindFormControls();
    }


    public override void ApplyTabIndexToControl(ref short startTabIndex)
    {

    }


    private void BindFormControls()
    {
        var patRecord = PatientRecord;
        if (patRecord != null)
        {
            lblEhrHRN.Text = patRecord.EHR;
            lblEhrName.Text = patRecord.FullName();
            lblEhrBirthday.Text = patRecord.DateOfBirth.ToString("MM/dd/yyyy");
        }

        if (this.ScreeningResult != null)
        {
            lblScreeningId.Text = this.ScreeningResult.ID.ToString();
            lblScreeningDate.Text = this.ScreeningResult.CreatedDate.ToString();
            lblFdName.Text = this.ScreeningResult.FullName;
            lblFdBirthday.Text = this.ScreeningResult.Birthday.ToString("MM/dd/yyyy");
        }

        var visitRecord = VisitRecord;
        if (visitRecord != null)
        {
            lblVisitDate.Text = "{0:MM'/'dd'/'yyyy' 'HH':'mm}".FormatWith(visitRecord.Date);
            lblVisitCategory.Text = visitRecord.ServiceCategory;
            lblVisitLocation.Text = visitRecord.Location.Name;
        }

    }
}