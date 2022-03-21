using FrontDesk;
using FrontDesk.Common.Extensions;
using FrontDesk.Common.InfrastructureServices;
using FrontDesk.Configuration;
using FrontDesk.Server.Logging;
using FrontDesk.Server.Membership;
using FrontDesk.Server.Screening.Models;
using FrontDesk.Server.Screening.Services;
using FrontDesk.Server.Web;

using Resources;

using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.Caching;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class VisitSettingsForm : BasePage
{
    private readonly VisitSettingsService _service = new VisitSettingsService();
    private readonly IScreeningScoreLevelService _screeningScoreLevelService = new ScreeningScoreLevelService();
    private readonly ITimeService _timeService = new TimeService();

    // state
    private List<ScreeningSectionScoreHint> ScoreLevelHints = null;

    private List<ScreeningSectionScoreHint> GetScoreLevelHints()
    {
        const string key = "VisitSettingsForm_ScoreLevelHints";

        var cache = MemoryCache.Default;

        var cacheEntry = cache.Get(key);
        List<ScreeningSectionScoreHint> result;

        if (cacheEntry == null)
        {
            result = _screeningScoreLevelService.GetAllScoreHints();

            var cachePolicy = new CacheItemPolicy();
            cachePolicy.AbsoluteExpiration = _timeService.GetDateTimeOffsetNow().AddMinutes(5);
            cache.Add(key, result, cachePolicy);
        }
        else
        {
            result = (List<ScreeningSectionScoreHint>)cacheEntry ?? new List<ScreeningSectionScoreHint>();
        }

        return result;
    }


    protected void Page_Init(object sender, EventArgs e)
    {
        btnSave.Click += new EventHandler(btnSave_Click);
        btnReset.Click += new EventHandler(btnReset_Click);
        rptAgeParams.ItemDataBound += new RepeaterItemEventHandler(rptAgeParams_ItemDataBound);
        if (IsPostBack)
        {
            BindDataSource();
        }
    }

    protected override void SetPagePermissions()
    {
        base.SetPagePermissions();

        if (!User.IsInRole(UserRoles.SuperAdministrator))
        {
            RedirectToErrorPage(Resources.TextMessages.AccessPermissionsErrorMessage);
        }
    }


    void btnReset_Click(object sender, EventArgs e)
    {
        //set all to On
        var settings = GetFormData();
        settings.ForEach(x => x.IsEnabled = true);
        Save(settings);
    }

    void btnSave_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            var settings = GetFormData();
            Save(settings);
        }
    }

    private void Save(List<VisitSettingItem> settings)
    {
        try
        {
            _service.Update(settings);
            Response.Redirect(Request.Path, false);
        }
        catch (Exception ex)
        {
            ErrorLog.Add(ex.Message, ex.StackTrace, null);
        }
    }

    List<VisitSettingItem> GetFormData()
    {
        var list = _service.GetAll();
        HiddenField hdnID;
        CheckBox chkEnabled;
        TextBox txtCutScore;

        VisitSettingItem repositoryItem = null;
        foreach (RepeaterItem item in rptAgeParams.Items)
        {
            if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
            {
                hdnID = item.FindControl("hdnID") as HiddenField;
                chkEnabled = item.FindControl("chkEnabled") as CheckBox;
                txtCutScore = item.FindControl("txtCutScore") as TextBox;

                if (hdnID != null && chkEnabled != null)
                {
                    repositoryItem = list.FirstOrDefault(x => x.Id == hdnID.Value);
                    if (repositoryItem == null)
                    {
                        repositoryItem = new VisitSettingItem
                        {
                            Id = hdnID.Value,
                            IsEnabled = chkEnabled.Checked,
                            LastModifiedDateUTC = DateTime.UtcNow
                        };

                        list.Add(repositoryItem);
                    }
                    else
                    {
                        repositoryItem.IsEnabled = chkEnabled.Checked;
                    }
                }

                if (txtCutScore != null)
                {
                    repositoryItem.CutScore = txtCutScore.Text.AsNullable<int>() ?? 1;
                }
                else
                {
                    repositoryItem.CutScore = 1;
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
            var dataItem = (VisitSettingItem)item.DataItem;

            var chkEnabled = item.FindControl("chkEnabled") as CheckBox;
            if (chkEnabled == null)
            {
                throw new InvalidOperationException("chkEnabled not found and couldn't be disabled");
            }

            // bind Cut score
            var lblCutScore = item.FindControl("lblCutScore") as Label;
            var txtCutScore = item.FindControl("txtCutScore") as TextBox;
            var vldCutScore = item.FindControl("vldCutScore") as RequiredFieldValidator;
            var vldCutScoreRange = item.FindControl("vldCutScoreRange") as RangeValidator;

            // section id mapping
            var screeningSectionId = dataItem.Id;

            if (screeningSectionId == VisitSettingsDescriptor.Depression)
            {
                screeningSectionId = ScreeningSectionDescriptor.Depression;
            }

            lblCutScore.Text = GetScreeningSectionScoreHint(screeningSectionId);
            txtCutScore.Visible = !string.IsNullOrEmpty(lblCutScore.Text);
            vldCutScore.Visible = txtCutScore.Visible;
            if (vldCutScore != null)
            {
                vldCutScore.ErrorMessage = "{0}:{1}".FormatWith(dataItem.Name, TextMessages.VisitSettingsCutStore_EmptyValueError);
            }
            if (vldCutScoreRange != null)
            {
                vldCutScoreRange.ErrorMessage = "{0}:{1}".FormatWith(dataItem.Name, TextMessages.VisitSettingsCutScore_RangeError);
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Master.PageHeaderText = "Visit Settings";
        Page.ClientScript.RegisterClientScriptInclude("iphone-style-checkboxes.js", ResolveClientUrl("~/scripts/plugins/iphone-style-checkboxes.js"));
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
        ScoreLevelHints = GetScoreLevelHints();
        var dataSource = _service.GetAll();
        rptAgeParams.DataSource = dataSource;
        rptAgeParams.DataBind();
    }


    protected string GetScreeningSectionScoreHint(string screeningSectionID)
    {
        var scoreLevel = ScoreLevelHints.FirstOrDefault(x => x.ScreeningSectionID == screeningSectionID);
        return scoreLevel != null ? scoreLevel.ScoreHint : string.Empty;
    }
}
