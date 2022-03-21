using System;
using System.Linq;
using System.Web.UI.WebControls;

using FrontDesk.Common.Bhservice;
using FrontDesk.Common.Extensions;
using FrontDesk.Server.Extensions;
using FrontDesk.Server.Screening.Models;
using FrontDesk.Server.Web.Extensions;

namespace FrontDesk.Server.Web.Controls
{

    public partial class BHSVisitReportControl : BaseWebFormUserControl<BhsVisit, long>
    {


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            ddlNewVisitRecommendation.AddDefaultNotSelectedItem();
            ddlNewVisitReferralRecommendationAccepted.AddDefaultNotSelectedItem();

            rptOtherTools.DataSource = Enumerable.Repeat(new ManualScreeningResultValue(), 4);
            rptTreatmentAction.DataSource = Enumerable.Repeat(new TreatmentAction(), 5);

            rptOtherTools.ItemDataBound += RptOtherTools_ItemDataBound;
            rptTreatmentAction.ItemDataBound += RptTreatmentAction_ItemDataBound;

        }

        public bool HideContent { get; set; }

        public void SetReadOnlyFields(BhsVisit model)
        {
            var screeningResult = model.Result;

            //patient info
            lblFirstname.Text = screeningResult.FirstName;
            lblLastname.Text = screeningResult.LastName;
            lblMiddlename.Text = screeningResult.MiddleName;
            lblBirthday.Text = screeningResult.Birthday.FormatAsDate();

            lblStreetAddress.Text = screeningResult.StreetAddress.FormatAsNullableString();
            lblCity.Text = screeningResult.City.FormatAsNullableString();
            lblState.Text = screeningResult.StateName.FormatAsNullableString().ToUpper();
            lblZipCode.Text = screeningResult.ZipCode.FormatAsNullableString();
            lblPhone.Text = screeningResult.Phone.FormatAsNullableString();
            lblRecordNo.Text = screeningResult.ID.ToString();
            lblRecordNo.NavigateUrl = ResolveUrl("~/PatientCheckIn.aspx?id={0}".FormatWith(screeningResult.ID));

            //screening result
            txtScreeningDate.Text = screeningResult.CreatedDate.FormatAsDate();
            chkTobacoExposureSmokerInHomeFlag.Checked = model.TobacoExposureSmokerInHomeFlag;
            chkTobacoExposureCeremonyUseFlag.Checked = model.TobacoExposureCeremonyUseFlag;
            chkTobacoExposureSmokingFlag.Checked = model.TobacoExposureSmokingFlag;
            chkTobacoExposureSmoklessFlag.Checked = model.TobacoExposureSmoklessFlag;

            if (model.AlcoholUseFlag != null)
            {
                txtAlcoholUseFlagScoreLevel.Text = model.AlcoholUseFlag.ScoreLevel.ToString();
                txtAlcoholUseFlagScoreLevelLabel.Text = model.AlcoholUseFlag.ScoreLevelLabel;
            }
            if (model.SubstanceAbuseFlag != null)
            {
                txtSubstanceAbuseFlagScoreLevel.Text = model.SubstanceAbuseFlag.ScoreLevel.ToString();
                txtSubstanceAbuseFlagScoreLevelLabel.Text = model.SubstanceAbuseFlag.ScoreLevelLabel;
            }

            if (model.AnxietyFlag != null)
            {
                txtAnxietyFlagScoreLevel.Text = model.AnxietyFlag.ScoreLevel.ToString();
                txtAnxietyFlagScoreLevelLabel.Text = model.AnxietyFlag.ScoreLevelLabel;
            }

            if (model.DepressionFlag != null)
            {
                txtDepressionFlagScoreLevel.Text = model.DepressionFlag.ScoreLevel.ToString();
                txtDepressionFlagScoreLevelLabel.Text = model.DepressionFlag.ScoreLevelLabel;
                txtDepressionThinkOfDeathAnswer.Text = model.DepressionThinkOfDeathAnswer;
            }

            if (model.PartnerViolenceFlag != null)
            {
                txtPartnerViolenceFlagScoreLevel.Text = model.PartnerViolenceFlag.ScoreLevel.ToString();
                txtPartnerViolenceFlagScoreLevelLabel.Text = model.PartnerViolenceFlag.ScoreLevelLabel;
            }

            if (model.ProblemGamblingFlag != null)
            {
                txtProblemGamblingFlagScoreLevel.Text = model.ProblemGamblingFlag.ScoreLevel.ToString();
                txtProblemGamblingFlagScoreLevelLabel.Text = model.ProblemGamblingFlag.ScoreLevelLabel;
            }

            txtStaffName.Text = !string.IsNullOrEmpty(model.BhsStaffNameCompleted) ? model.BhsStaffNameCompleted : FDUser.GetCurrentUser().FullName;
            txtCompleteDate.Text = model.CompleteDate.FormatAsDateWithTime();

        }

        protected void SetDrugOfChoiceFields(BhsVisit model)
        {
            var drugOfChoice = new DrugOfChoiceModel(model.Result);

            ddlDrugOfChoicePrimary.SetValueOrDefault(drugOfChoice.Primary);
            ddlDrugOfChoiceSecondary.SetValueOrDefault(drugOfChoice.Secondary);
            ddlDrugOfChoiceTertiary.SetValueOrDefault(drugOfChoice.Tertiary);
        }

        protected void GetUpdatedDrugOfChoiceFields(BhsVisit model)
        {
            var drugOfChoice = new DrugOfChoiceModel(model.Result);

            drugOfChoice.Primary = ddlDrugOfChoicePrimary.SelectedValue.AsNullable<int>() ?? 0;
            drugOfChoice.Secondary = ddlDrugOfChoiceSecondary.SelectedValue.AsNullable<int>() ?? 0;
            drugOfChoice.Tertiary = ddlDrugOfChoiceTertiary.SelectedValue.AsNullable<int>()?? 0;

            var section = drugOfChoice.GetSection();

            // if new answers has been added by the user, add them the the result model
            if (!model.Result.SectionAnswers.Contains(section))
            {
                model.Result.AppendSectionAnswer(section);
            }

        }

        public override void SetModel(BhsVisit model)
        {
            SetReadOnlyFields(model);

            SetDrugOfChoiceFields(model);

            if (model.NewVisitReferralRecommendation != null)
            {
                ddlNewVisitRecommendation.SetValueOrDefault(model.NewVisitReferralRecommendation.Id);
                txtNewVisitRecommendationDescription.Text = model.NewVisitReferralRecommendation.Description;
            }

            if (model.NewVisitReferralRecommendationAccepted != null)
            {
                ddlNewVisitReferralRecommendationAccepted.SetValueOrDefault(model.NewVisitReferralRecommendationAccepted.Id);
            }

            if (model.ReasonNewVisitReferralRecommendationNotAccepted != null)
            {
                ddlReasonNewVisitReferralRecommendationNotAccepted.SetValueOrDefault(model.ReasonNewVisitReferralRecommendationNotAccepted.Id);
            }

            dtNewVisitDate.SelectedDateTimeOffset = model.NewVisitDate;

            if (model.Discharged != null)
            {
                ddlDischarged.SetValueOrDefault(model.Discharged.Id);
            }

            ddlThirtyDatyFollowUpFlag.SetValueOrDefault(model.ThirtyDatyFollowUpFlag ? "1" : "0");

            if (model.NewVisitDate.HasValue)
            {
                ddlThirtyDatyFollowUpFlag.Enabled = false;
                vldNewVisitDate.Enabled = model.IsCompleted && model.ThirtyDatyFollowUpFlag;
            }
            dtFollowUpDate.SelectedDateTimeOffset = model.FollowUpDate;

            txtNotes.Value = model.Notes;
        }



        public override BhsVisit GetModel()
        {
            if (!Page.IsValid) return null;

            var model = Page.CurFormObject;

            GetUpdatedDrugOfChoiceFields(model);


            model.NewVisitReferralRecommendation = new Common.LookupValueWithDescription
            {
                Id = ddlNewVisitRecommendation.SelectedValue.AsNullable<int>() ?? 0,
                Description = txtNewVisitRecommendationDescription.Text
            };

            model.NewVisitReferralRecommendationAccepted = new Common.LookupValue
            {
                Id = ddlNewVisitReferralRecommendationAccepted.SelectedValue.AsNullable<int>() ?? 0,
            };

            model.ReasonNewVisitReferralRecommendationNotAccepted = new Common.LookupValue
            {
                Id = ddlReasonNewVisitReferralRecommendationNotAccepted.SelectedValue.As<int>()
            };
            model.NewVisitDate = dtNewVisitDate.SelectedDateTimeOffset;

            model.Discharged = new Common.LookupValue
            {
                Id = ddlDischarged.SelectedValue.As<int>()
            };
            model.FollowUpDate = dtFollowUpDate.SelectedDateTimeOffset;

            {
                //collect Other screening tools
                var rptOtherToolsUniqueId = rptOtherTools.UniqueID;
                model.OtherScreeningTools.Clear();
                for (int index = 0; index < 4; index++)
                {
                    var txtScoreUniqueId = "{0}{1}ctl{2:d2}{1}txtScoreOrResult".FormatWith(rptOtherToolsUniqueId, IdSeparator, index + 1);
                    var txtNameUniqueId = "{0}{1}ctl{2:d2}{1}txtNameOfTool".FormatWith(rptOtherToolsUniqueId, IdSeparator, index + 1);

                    var otherToolItem = new ManualScreeningResultValue
                    {
                        ScoreOrResult = Request.Form[txtScoreUniqueId].Trim(),
                        ToolName = Request.Form[txtNameUniqueId].Trim()
                    };

                    if (!string.IsNullOrWhiteSpace(otherToolItem.ScoreOrResult) ||
                        !string.IsNullOrWhiteSpace(otherToolItem.ToolName))
                    {
                        model.OtherScreeningTools.Add(otherToolItem);
                    }

                }
            }
            {
                //collect Treatment Actions
                var rptTreatmentActionsUniqueId = rptTreatmentAction.UniqueID;
                model.TreatmentActions.Clear();
                for (int index = 0; index < 5; index++)
                {
                    var ddlTreatmentActionUniqueId = "{0}{1}ctl{2:d2}{1}ddlTreatmentAction".FormatWith(rptTreatmentActionsUniqueId, IdSeparator, index + 1);
                    var txtTreatmentActionDescriptionUniqueId = "{0}{1}ctl{2:d2}{1}txtTreatmentActionDescription".FormatWith(rptTreatmentActionsUniqueId, IdSeparator, index + 1);


                    if (string.IsNullOrWhiteSpace(Request.Form[ddlTreatmentActionUniqueId])) continue;

                    var treatmentActionItem = new TreatmentAction
                    {
                        Id = Request.Form[ddlTreatmentActionUniqueId].ConvertToNullableType<int>() ?? 0,
                        Description = Request.Form[txtTreatmentActionDescriptionUniqueId].Trim()
                    };

                    if (treatmentActionItem.Id > 0)
                    {
                        model.TreatmentActions.Add(treatmentActionItem);
                    }
                }
            }

            model.Notes = txtNotes.Value;

            return model;
        }

        //data binding
        private void RptOtherTools_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var index = e.Item.ItemIndex;
                var printIndex = index + 1;
                var lblTool = (FormLabel)e.Item.FindControl("lblTool");
                var txtScoreOrResult = (TextBox)e.Item.FindControl("txtScoreOrResult");
                var txtNameOfTool = (TextBox)e.Item.FindControl("txtNameOfTool");

                if (lblTool != null && txtScoreOrResult != null && txtNameOfTool != null)
                {
                    lblTool.Text = "Other screening tool " + printIndex;
                    if (Page.CurFormObject.OtherScreeningTools.Count > index)
                    {
                        var item = Page.CurFormObject.OtherScreeningTools[index];
                        txtScoreOrResult.Text = item.ScoreOrResult;
                        txtNameOfTool.Text = item.ToolName;
                    }
                }
            }
        }


        private void RptTreatmentAction_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var index = e.Item.ItemIndex;
                var printIndex = index + 1;
                var lblTool = (FormLabel)e.Item.FindControl("lblTool");
                var ddlTreatmentAction = (DropDownList)e.Item.FindControl("ddlTreatmentAction");
                var txtTreatmentActionDescription = (TextBox)e.Item.FindControl("txtTreatmentActionDescription");

                if (lblTool != null && ddlTreatmentAction != null && txtTreatmentActionDescription != null)
                {
                    lblTool.Text = "Treatment action {0} (delivered)".FormatWith(printIndex);
                    if (index == 0)
                    {
                        //when first item - it's mandatory
                        lblTool.Mandatory = true;
                        //and the first item is selected
                    }
                    else
                    {
                        //add None as default value
                        ddlTreatmentAction.AddDefaultNotSelectedItem("None", string.Empty);
                    }

                    if (Page.CurFormObject.TreatmentActions.Count > index)
                    {
                        var item = Page.CurFormObject.TreatmentActions[index];
                        ddlTreatmentAction.SetValueOrDefault(item.Id);
                        txtTreatmentActionDescription.Text = item.Description;
                    }
                }
            }
        }

        public override void ApplyTabIndexToControl(ref short startTabIndex)
        {
            //do nothing

        }

        protected override void EditModeDataPrepare(BhsVisit model)
        {

        }

        protected override void SetControlsEnabledState(bool isNewInstance)
        {
            base.SetControlsEnabledState(isNewInstance);
            phSecuritySection.Visible = !HideContent;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (Page.IsPostBack)
            {
                rptOtherTools.DataBind();
                rptTreatmentAction.DataBind();
            }

            Page.ClientScript.RegisterClientScriptInclude(this.GetType(), "nextVisitSchedule.js", ResolveClientUrl("~/scripts/controls/nextVisitSchedule.js"));
            Page.ClientScript.RegisterClientScriptInclude("richTextNotes.js", ResolveClientUrl("~/scripts/controls/richTextNotes.js"));
            Page.ClientScript.RegisterClientScriptInclude("bhsVisitReport.js", ResolveClientUrl("~/scripts/controls/bhsVisitReport.js"));
            Page.ClientScript.RegisterClientScriptInclude("drugOfChoiceMultistep.js", ResolveClientUrl("~/scripts/controls/drugOfChoiceMultistep.js"));


        }

    }
}
