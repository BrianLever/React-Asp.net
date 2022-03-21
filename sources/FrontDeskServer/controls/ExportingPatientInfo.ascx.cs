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

public partial class ExportingPatientInfo : BaseUserControl
{
    public ScreeningResult ScreeningResult { get; set; }

    //public int? RpmsPatientRowID { get; set; }
    public PatientSearch RpmsPatient { get; set; }

    private Patient _patientRecord;

    /// <summary>
    /// Selected Patient record from EHR
    /// </summary>
    public Patient PatientRecord
    {
        get
        {
            if (_patientRecord == null && RpmsPatient != null)
            {
                _patientRecord = EhrInterfaceProxy.Instance.GetPatientRecord(RpmsPatient);
            }
            return _patientRecord;
        }
        set
        {
            _patientRecord = value;
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
            lblEhrPhoneHome.Text = patRecord.PhoneHome + " (Home)";
            lblEhrPhoneOffice.Text = patRecord.PhoneOffice + " (Work)";
            lblEhrBirthday.Text = patRecord.DateOfBirth.ToString("MM/dd/yyyy");
            lblEhrStreetAddress.Text = patRecord.StreetAddress;
            lblEhrCity.Text = patRecord.City;
            lblEhrStateID.Text = patRecord.StateID;
            lblEhrZipCode.Text = patRecord.ZipCode;
        }

        if (this.ScreeningResult!= null)
        {
            lblScreeningId.Text = this.ScreeningResult.ID.ToString();
            lblScreeningDate.Text = this.ScreeningResult.CreatedDate.ToString();
            lblFdName.Text = this.ScreeningResult.FullName;
            lblFdBirthday.Text = this.ScreeningResult.Birthday.ToString("MM/dd/yyyy");
            lblFdPhone.Text = this.ScreeningResult.Phone;
            lblFdAddress.Text = this.ScreeningResult.StreetAddress;
            lblFdCity.Text = this.ScreeningResult.City;
            lblFdStateID.Text = this.ScreeningResult.StateID;
            lblFdZipCode.Text = this.ScreeningResult.ZipCode;
        }
    }
}