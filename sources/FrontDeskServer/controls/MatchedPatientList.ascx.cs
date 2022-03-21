using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FrontDesk.Server.Web.Controls;
using FrontDesk;
using FrontDesk.Server.Screening;
using System.Data;
using FrontDesk.StateObjects;
using EhrInterface;
using FrontDesk.Common;
using Controls;
using RPMS.Common.Models;
using System.Diagnostics;
using Common.Logging;
using FrontDesk.Common.Extensions;
using FrontDesk.Server.Extensions;
using FrontDesk.Server.Web;

public partial class MatchedPatientList : PagedItemsUserControl
{
    private ILog _logger = LogManager.GetLogger<MatchedPatientList>();

    public ScreeningResult CurrentPatient
    {
        get
        {
            if (ViewState["CurrentPatient"] == null)
            {
                ViewState["CurrentPatient"] = null;
            }
            return (ScreeningResult)ViewState["CurrentPatient"];
        }
        set { ViewState["CurrentPatient"] = value; }
    }

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

    public override string SelectItemButtonText
    {
        get { return "Select EHR Record"; }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        this.PreRender += MatchedPatientList_PreRender;

    }

    public void Update()
    {
        updRoot.Update();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.PreRender += MatchedPatientList_PreRender;

    }

    private void MatchedPatientList_PreRender(object sender, EventArgs e)
    {
        if (!IsPostBack || ScriptManager.GetCurrent(this.Page).IsInAsyncPostBack)
        {
            try
            {
                //Get count of candidats for export from EHR database
                RowsCount = EhrInterfaceProxy.Instance.GetPatientCount(CurrentPatient);

                BindListData();
            }
            catch (Exception ex)
            {
                LogManager.GetLogger<MatchedPatientList>().Warn(ex);

                RenderErrorMessage(Resources.TextMessages.RpmsPatientsAccessErrorMessage);

            }
        }
    }

    /// <summary>
    /// Get candidats for export from EHR database
    /// </summary>
    protected override void BindListData()
    {
        if (RowsCount > 0)
        {
            PagetControl.RowsCount = RowsCount;
            rptMatchedPatient.DataSource = EhrInterfaceProxy.Instance.GetMatchedPatients(CurrentPatient, StartRow, 5);
            rptMatchedPatient.DataBind();
            phNoMatches.Visible = false;
            rptMatchedPatient.Visible = true;
        }
        else
        {
            phNoMatches.Visible = true;
            rptMatchedPatient.Visible = false;
            SelectedPatientRecord = null;
        }
    }

    /// <summary>
    /// All non-matched fields in the selected matched results set in red color
    /// </summary>
    /// <param name="item"></param>
    private void SetNonMatchedFields(RepeaterItem item)
    {
        var patient = item.DataItem as Patient;
        //Comparison first name
        if (patient != null)
        {
            var txtFirstName = (Label)item.FindControl("txtFirstName");
            if (string.Compare(CurrentPatient.FirstName, patient.FirstName, StringComparison.OrdinalIgnoreCase) != 0)
            {
                txtFirstName.ForeColor = System.Drawing.Color.Red;
            }
            //Compression last name
            var txtLastName = (Label)item.FindControl("txtLastName");
            if (String.Compare(CurrentPatient.LastName, patient.LastName, StringComparison.OrdinalIgnoreCase) != 0)
            {
                txtLastName.ForeColor = System.Drawing.Color.Red;
            }
            //Compressin middle name
            var txtMiddleName = (Label)item.FindControl("txtMiddleName");
            if (String.Compare(CurrentPatient.MiddleName, patient.MiddleName, StringComparison.OrdinalIgnoreCase) != 0)
            {
                txtMiddleName.ForeColor = System.Drawing.Color.Red;
            }
            //Compressin birthday
            var txtBirthday = (Label)item.FindControl("txtBirthday");
            if (CurrentPatient.Birthday != (DateTime)patient.DateOfBirth)
            {
                txtBirthday.ForeColor = System.Drawing.Color.Red;
            }
            //Compressin middle name
            var txtPhone = (Label)item.FindControl("txtPhone");
            if (string.Compare(CurrentPatient.Phone.AsRawPhoneNumber(), patient.PhoneHome.AsRawPhoneNumber(), StringComparison.OrdinalIgnoreCase) != 0)
            {
                txtPhone.ForeColor = System.Drawing.Color.Red;
            }
            //Compressin middle name
            var txtStreet = (Label)item.FindControl("txtStreet");
            if (String.Compare(CurrentPatient.StreetAddress, patient.StreetAddress, StringComparison.OrdinalIgnoreCase) != 0)
            {
                txtStreet.ForeColor = System.Drawing.Color.Red;
            }
            //Compressin middle name
            var txtCity = (Label)item.FindControl("txtCity");
            if (String.Compare(CurrentPatient.City, patient.City, StringComparison.OrdinalIgnoreCase) != 0)
            {
                txtCity.ForeColor = System.Drawing.Color.Red;
            }
            //Compressin middle name
            var txtState = (Label)item.FindControl("txtState");
            if (String.Compare(CurrentPatient.StateID, patient.StateID, StringComparison.OrdinalIgnoreCase) != 0)
            {
                txtState.ForeColor = System.Drawing.Color.Red;
            }
            //Compressin middle name
            var txtZipCode = (Label)item.FindControl("txtZipCode");
            if (String.Compare(CurrentPatient.ZipCode, patient.ZipCode, StringComparison.OrdinalIgnoreCase) != 0)
            {
                txtZipCode.ForeColor = System.Drawing.Color.Red;
            }
        }
    }


    protected override void OnItemSelected(string[] commandArgs)
    {
        base.OnItemSelected(commandArgs);

        if (commandArgs.Length < 4)
        {
            _logger.WarnFormat("[Select Patient] Incorrect number of parameters. Expected 4, but received: [{0}]", String.Join("|", commandArgs));
        }

        SelectedPatientRecord = new PatientSearch
        {
            ID = commandArgs[0].As<int>(),
            LastName = commandArgs[1].Trim(),
            FirstName = commandArgs[2].Trim(),
            MiddleName = commandArgs[3].Trim()
        };


    }

    #region Page event

    protected void rptMatchedPatient_Bound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            if (CurrentPatient != null)
            {
                SetNonMatchedFields(e.Item);

                var btnSelect = (Button)e.Item.FindControl("btnSelect");
                btnSelect.Visible = CurrentPatient.ExportDate == null;

                AddJsonPayload(e.Item);
            }
        }
    }


    protected void AddJsonPayload(RepeaterItem item)
    {
        var jsonDataCtrl = item.FindControl("hdnItemData") as HiddenField;
        Debug.Assert(jsonDataCtrl != null, "not found hdnItemData");
        var patient = item.DataItem as Patient;

        jsonDataCtrl.Value = Newtonsoft.Json.JsonConvert.SerializeObject(patient);
    }
    #endregion

    protected override Repeater ItemsContainer
    {
        get { return rptMatchedPatient; }
    }

    protected override Pager PagetControl
    {
        get { return ctrlPaging; }
    }

}
