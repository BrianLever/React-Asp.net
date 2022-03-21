using FrontDesk;
using FrontDesk.Common.InfrastructureServices;
using FrontDesk.Configuration;
using FrontDesk.Server.Data;
using FrontDesk.Server.Data.ScreenngProfile;
using FrontDesk.Server.Logging;
using FrontDesk.Server.Screening;
using FrontDesk.Server.Screening.Services;
using FrontDesk.Server.Web;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ScreenProfileFrequencySettingsForm : BaseManagementWebForm<ScreeningProfile, int>
{

    private readonly IScreeningProfileFrequencyService _service;
    private readonly IScreeningProfileService _profileService = new ScreeningProfileService();

    protected override ScreeningProfile GetFormObjectByID(int objectID)
    {
        return _profileService.Get(objectID);
    }

    protected override void EditModeDataPrepare(int objectID)
    {
        //do nothing
    }

    protected override ScreeningProfile GetFormData()
    {
        return CurFormObject;
    }

    public ScreenProfileFrequencySettingsForm()
    {
        _service = new ScreeningProfileFrequencyService(
            new ScreeningProfileFrequencyDb(),
            new TimeService()
            );
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        btnSave.Click += new EventHandler(btnSave_Click);
        btnReset.Click += new EventHandler(btnReset_Click);
        rptAgeParams.ItemCreated += new RepeaterItemEventHandler(rptAgeParams_ItemCreated);

        TryGetPageIDValue("id");

        EnsureFormObjectCreated();

        if (CurFormObject == null)
        {
            Master.RedirectToList();
        }


        if (IsPostBack)
        {
            BindDataSource();
        }
    }


    void rptAgeParams_ItemCreated(object sender, RepeaterItemEventArgs e)
    {
        var item = e.Item;

        if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
        {
            var cmbFreq = item.FindControl("cmbFrequency") as DropDownList;
            if (cmbFreq != null)
            {
                cmbFreq.Items.AddRange(new ListItem[]{
                    new ListItem(Resources.TextMessages.Frequency_EveryVisit, "0"),
                    new ListItem(Resources.TextMessages.Frequency_Daily, "1"),
                    new ListItem(Resources.TextMessages.Frequency_Weekly, "7"),
                    new ListItem(Resources.TextMessages.Frequency_Monthly, "100"), 
                    new ListItem(Resources.TextMessages.Frequency_BiMonthly, "200"), 
                    new ListItem(Resources.TextMessages.Frequency_Quarterly, "300"), 
                    new ListItem(Resources.TextMessages.Frequency_Annually, "1200"), 
                    new ListItem(Resources.TextMessages.Frequency_Once, "240000"), 
                });

                string frequencyVal = ((ScreeningFrequencyItemViewModel)item.DataItem).Frequency.ToString();
                var selectedItem = cmbFreq.Items.FindByValue(frequencyVal);
                if (selectedItem != null)
                {
                    cmbFreq.SelectedValue = selectedItem.Value;
                }
            }
        }
    }

    void btnReset_Click(object sender, EventArgs e)
    {
        RedirectToRefresh();
    }

    void btnSave_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            var freqParams = GetFrequencyData();
            try
            {
                _service.Save(formObjectID, freqParams);

                SetRedirectSuccessAlert(FrontDesk.Common.Messages.CustomMessage.GetUpdateMessage("Screening Frequency Settings"));

                RedirectToRefresh();
            }
            catch (Exception ex)
            {
                ErrorLog.Add(ex.Message, ex.StackTrace, null);
                SetErrorAlert(FrontDesk.Common.Messages.CustomError.GetUpdateMessage("Screening Frequency Settings"), this.GetType());
            }
        }
    }

    private IList<ScreeningFrequencyItem> GetFrequencyData()
    {
        IList<ScreeningFrequencyItem> list = new List<ScreeningFrequencyItem>();
        HiddenField hdnID;
        DropDownList cmbFreq;

        foreach (RepeaterItem item in rptAgeParams.Items)
        {
            if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
            {
                hdnID = item.FindControl("hdnID") as HiddenField;
                cmbFreq = item.FindControl("cmbFrequency") as DropDownList;

                if (hdnID != null && cmbFreq != null)
                {
                    list.Add(new ScreeningFrequencyItem
                    {
                        ID = hdnID.Value,
                        Frequency = Int32.Parse(cmbFreq.SelectedValue)
                    });

                }
            }
        }
        return list;

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Master.PageHeaderText = "Profile \"{0}\" - Screening Frequency Settings".FormatWith(CurFormObject.Name);
    }


    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindDataSource();
        }
    }

    protected void BindDataSource()
    {

        List<ScreeningFrequencyItemViewModel> screeningFrequencyList = _service.GetAll(formObjectID)
            .Select(x => AutoMapper.Mapper.Map<ScreeningFrequencyItemViewModel>(x)).ToList();

        var dataSource = screeningFrequencyList;
        rptAgeParams.DataSource = dataSource;
        rptAgeParams.DataBind();
    }
}
