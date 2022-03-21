using FrontDesk;
using FrontDesk.Server.Extensions;
using FrontDesk.Server.Licensing;
using FrontDesk.Server.Logging;
using FrontDesk.Server.Screening.Services;
using FrontDesk.Server.Web;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ScreenProfileMinimalAgeForm : BaseManagementWebForm<ScreeningProfile, int>
{
    private readonly IScreeningProfileMinimalAgeService _service = new ScreeningProfileMinimalAgeService();
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

    protected void Page_Init(object sender, EventArgs e)
    {
        btnSave.Click += new EventHandler(btnSave_Click);
        btnReset.Click += new EventHandler(btnReset_Click);
        rptAgeParams.ItemDataBound += new RepeaterItemEventHandler(rptAgeParams_ItemDataBound);

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




    void btnReset_Click(object sender, EventArgs e)
    {
        RedirectToRefresh();
    }

    void btnSave_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            var ageParams = GetAgeFormData();
            try
            {
                _service.UpdateMinimalAgeSettings(formObjectID, ageParams);

                SetRedirectSuccessAlert(FrontDesk.Common.Messages.CustomMessage.GetUpdateMessage("Minimum Age Settings"));

                RedirectToRefresh();
            }
            catch (Exception ex)
            {
                ErrorLog.Add(ex.Message, ex.StackTrace, null);
                SetErrorAlert(FrontDesk.Common.Messages.CustomError.GetUpdateMessage("Minimum Age Settings"), this.GetType());
            }
        }
    }

    protected IList<ScreeningSectionAge> GetAgeFormData()
    {
        IList<ScreeningSectionAge> list = _service.GetSectionMinimalAgeSettings(formObjectID);
        HiddenField hdnID;
        TextBox txtAge;
        CheckBox chkEnabled;

        ScreeningSectionAge repositoryItem = null;
        foreach (RepeaterItem item in rptAgeParams.Items)
        {
            if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
            {
                hdnID = item.FindControl("hdnID") as HiddenField;
                txtAge = item.FindControl("txtAge") as TextBox;
                chkEnabled = item.FindControl("chkEnabled") as CheckBox;

                if (hdnID != null && txtAge != null && chkEnabled != null)
                {
                    repositoryItem = list.FirstOrDefault(x => x.ScreeningSectionID == hdnID.Value);
                    if (repositoryItem == null)
                    {
                        list.Add(new ScreeningSectionAge
                        {
                            ScreeningSectionID = hdnID.Value,
                            MinimalAge = Byte.Parse(txtAge.Text),
                            IsEnabled = chkEnabled.Checked,
                            LastModifiedDateUTC = DateTime.UtcNow
                        });
                    }
                    else
                    {
                        repositoryItem.MinimalAge = Byte.Parse(txtAge.Text);
                        repositoryItem.IsEnabled = chkEnabled.Checked;
                    }

                    //if DAST-10 feature is disabled - IsEnabled always false
                    if (hdnID.Value == ScreeningSectionDescriptor.SubstanceAbuse && !PasswordProtectedFeaturesProvider.IsDast10Enabled)
                    {
                        repositoryItem.IsEnabled = false;
                    }
                }

            }
        }
        return list;

    }


    void rptAgeParams_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        var item = e.Item;
        if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
        {
            var dataItem = (ScreeningSectionAge)item.DataItem;

            if (dataItem.ScreeningSectionID == ScreeningSectionDescriptor.SubstanceAbuse && !PasswordProtectedFeaturesProvider.IsDast10Enabled)
            {
                var chkEnabled = item.FindControl("chkEnabled") as CheckBox;
                if (chkEnabled == null)
                {
                    throw new InvalidOperationException("chkEnabled not found and couldn't be disabled");
                }
                if (!chkEnabled.Checked) //allow to turn off if section is enabled
                {
                    chkEnabled.Enabled = false;
                }

            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Master.PageHeaderText = "Profile \"{0}\" - Screening Measure Minimum Age Parameters".FormatWith(CurFormObject.Name);
        Page.ClientScript.RegisterClientScriptInclude("iphone-style-checkboxes.js", ResolveClientUrl("~/scripts/plugins/iphone-style-checkboxes.js"));
        Page.ClientScript.RegisterClientScriptInclude("minimalAgeCheckboxController.js", ResolveClientUrl("~/scripts/controls/minimalAgeCheckboxController.js"));


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
        var dataSource = _service.GetSectionMinimalAgeSettings(formObjectID);
        rptAgeParams.DataSource = dataSource;
        rptAgeParams.DataBind();
    }

    protected string GetScreeningSectionGroupsAsJson()
    {
        return ScreeningSectionAge.GetScreeningMinimalAgeGroups().AsJson();
    }
}
