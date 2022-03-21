using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

using Common.Logging;

using FrontDesk;
using FrontDesk.Common;
using FrontDesk.Common.Extensions;
using FrontDesk.Server.Screening.Models;
using FrontDesk.Server.Web.Controls;

namespace UI
{
    public partial class ProblemScoreFilterControl : BaseUserControl
    {
        private ILog _logger = LogManager.GetLogger<ProblemScoreFilterControl>();

        public ProblemScoreFilterControl()
        {
            GroupName = "scorefilter";
        }

        public string GroupName
        {
            get; set;
        }


        public ScreeningResultByProblemFilter GetModel()
        {
            var selectedValues = GetSelectedOptions();

            if (!selectedValues.Any()) GetDefaultModel();

            var filterItems = new List<ScreeningResultByProblemFilterItem>();

            //parse value as rtp_ScreeningTool_Index
            foreach (var val in selectedValues)
            {
                var matches = val.Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries);

                Debug.Assert(matches.Length == 3, "Selected option should contain 3 groups");

                if (matches.Length < 3) continue;

                filterItems.Add(new ScreeningResultByProblemFilterItem
                {
                    ScreeningSection = matches[1],
                    MinScoreLevel = matches[2].As<Int32>()
                });

            }

            if (filterItems.Any())
            {

                return new ScreeningResultByProblemFilter
                {
                    Filters = filterItems
                };
            }
            else
            {
                return GetDefaultModel();
            }
        }

        private ScreeningResultByProblemFilter GetDefaultModel()
        {
            //set default value to any positive section
            return new ScreeningResultByProblemFilter
            {
                Filters = new List<ScreeningResultByProblemFilterItem>
                {
                    new ScreeningResultByProblemFilterItem
                    {
                        ScreeningSection = ScreeningResultByProblemFilter.AnySectionName,
                        MinScoreLevel = 1
                    }
                }
            };
        }

        private IList<string> GetSelectedOptions()
        {
            var result = new List<string>();

            foreach (var ctrl in this.Controls)
            {
                var cb = ctrl as CheckBox;
                if (cb != null && cb.Checked && !cb.ID.EndsWith("all"))
                {
                    result.Add(cb.ID);
                }
            }

            foreach (var ctrl in dtlDochItems.Controls)
            {
                var di = (DataListItem)ctrl;
                
                foreach (var diCtrl in di.Controls)
                {
                    var cb = diCtrl as CheckBox;

                    if (cb != null && cb.Checked && !cb.ID.EndsWith("all"))
                    {
                        result.Add(cb.ID);
                        _logger.TraceFormat("[ProblemScoreFilterControl] [DOCH] Selected ID: {0}", cb.ID);
                    }
                }
            }

            return result;
        }

        private IEnumerable<CheckBox> GetOptionControls()
        {
            foreach (var ctrl in this.Controls)
            {
                var rb = ctrl as CheckBox;
                if (rb != null)
                {
                    yield return rb;
                }
            }

            foreach (var ctrl in dtlDochItems.Controls)
            {
                var di = (DataListItem)ctrl;

                foreach (var diCtrl in di.Controls)
                {
                    var cb = diCtrl as CheckBox;

                    if (cb != null)
                    {
                        yield return cb;
                    }
                }
            }
        }


        public void SetCountsByFilterOptions(ScreeningsByScoreLevelCountResult model)
        {
            //apply count to filters
            foreach (var rb in GetOptionControls())
            {
                var selectedValue = rb.ID;

                //parse value as rtp_ScreeningTool_Index
                var matches = selectedValue.Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries);

                Debug.Assert(matches.Length == 3, "Selected option should contain 3 groups");

                if (matches.Length < 3 || matches[2] == "all") continue;

                var screeningSection = matches[1];
                var targetScoreLevel = matches[2].As<Int32>();

                if (screeningSection == VisitSettingsDescriptor.Depression)
                {
                    screeningSection = ScreeningSectionDescriptor.Depression;
                }
                else if (screeningSection == ScreeningSectionDescriptor.AnxietyAllQuestions)
                {
                    screeningSection = ScreeningSectionDescriptor.Anxiety;
                }

                var stats = model.GetSection(screeningSection);

                int count = 0;

                if (stats != null)
                {
                    int keyValue;
                    if (stats.ScoreLevelCount.TryGetValue(targetScoreLevel, out keyValue))
                    {
                        count = keyValue;
                    }
                }

                Regex re = new Regex(@"\(\d{1,4}\)$");

                rb.Text = re.Replace(rb.Text, "({0})".FormatWith(count));
            }

        }

        protected void Page_Init(object sender, EventArgs e)
        {
            foreach (var ctrl in this.Controls)
            {
                var rb = ctrl as RadioButton;
                if (rb != null)
                {
                    var idSections = rb.ID.Split(new[] { '_' });
                    if (idSections.Length > 2)
                    {
                        var screeningName = idSections[idSections.Length - 2];

                        if (screeningName.StartsWith("TCC"))
                        {
                            screeningName = "TCC";
                        }
                        rb.GroupName = this.GroupName + screeningName;
                    }
                }
            }
            //dtlDochItems.DataSource = _lookupLists.GetDrugOfChoice().Where(x => x.Id > 0).ToArray();
            dtlDochItems.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (IsPostBack)
            {
                GetValuesFromForm();
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            BindUserControls();

            Page.ClientScript.RegisterClientScriptInclude("multipleSelectCheckbox.js", ResolveClientUrl("~/scripts/controls/multipleSelectCheckbox.js"));

        }

        #region State management methods

        private void GetValuesFromForm()
        {
        }
        public void Reset()
        {
            BindUserControls();

        }


        #endregion

        private void BindUserControls()
        {

        }


        public override void ApplyTabIndexToControl(ref short startTabIndex)
        {
        }

        protected void dtlDochItems_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item ||
             e.Item.ItemType == ListItemType.AlternatingItem)
            {
                _logger.TraceFormat("[ProblemScoreFilterControl] dtlDochItems_ItemDataBound called");

                var checkBox = e.Item.FindControl("chk") as CheckBox;
                var data = e.Item.DataItem as LookupValue;

                if (checkBox != null && data != null)
                {
                    checkBox.ID = "rbt_DOCH_" + data.Id;

                    checkBox.Checked = Request.Form[checkBox.UniqueID] == "on";
                }

            }
        }

        protected void odsDrugsOfChoice_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            var data = e.ReturnValue as List<LookupValue>;
            if(data != null && data[0].Id == 0)
            {
                data.RemoveAt(0);
            }
        }

    }
}