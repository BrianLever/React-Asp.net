using System;
using FrontDesk.Common.Bhservice;
using FrontDesk.Common.Extensions;
using FrontDesk.Server.Web.Extensions;
using FrontDesk.Common;

namespace FrontDesk.Server.Web.Controls
{

    public partial class FollowUpControl : BaseWebFormUserControl<BhsFollowUp, long>
    {


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //ddlFollowUpContactOutcome.AddDefaultNotSelectedItem();
            ddlPatientAttendedVisit.AddDefaultNotSelectedItem();
            ddlNewVisitRecommendation.AddDefaultNotSelectedItem();
            ddlNewVisitReferralRecommendationAccepted.AddDefaultNotSelectedItem();

 
        }

        public bool HideContent { get; set; }

        public void SetReadOnlyFields(BhsFollowUp model)
        {
            //patient info
            ucPatientDetails.SetModel(model.Result);
            txtVisitReferralRecommendation.Text = model.VisitRefferalRecommendation;
            txtVisitDate.Text = model.ScheduledVisitDate.FormatAsDate();
            txtFollowUpDate.Text = model.ScheduledFollowUpDate.FormatAsDate();
            txtStaffName.Text = !string.IsNullOrEmpty(model.BhsStaffNameCompleted) ? model.BhsStaffNameCompleted : FDUser.GetCurrentUser().FullName;
            txtCompleteDate.Text = model.CompleteDate.FormatAsDateWithTime();

        }

        public override void SetModel(BhsFollowUp model)
        {
            SetReadOnlyFields(model);

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



            if (model.PatientAttendedVisit != null)
            {
                ddlPatientAttendedVisit.SetValueOrDefault(model.PatientAttendedVisit.Id);
            }

            if (model.FollowUpContactOutcome != null)
            {
                ddlFollowUpContactOutcome.SetValueOrDefault(model.FollowUpContactOutcome.Id);
            }

            dtFollowUpContactDate.SelectedDateTimeOffset = model.FollowUpContactDate;

            if (model.NewVisitReferralRecommendation != null)
            {
                ddlNewVisitRecommendation.SetValueOrDefault(model.NewVisitReferralRecommendation.Id);
                txtNewVisitRecommendationDescription.Text = model.NewVisitReferralRecommendation.Description;
            }
        }

        public override BhsFollowUp GetModel()
        {
            if (!Page.IsValid) return null;

            var model = Page.CurFormObject;

            model.NewVisitReferralRecommendationAccepted = ddlNewVisitReferralRecommendationAccepted.SelectedValue.AsLookupValue();

            model.ReasonNewVisitReferralRecommendationNotAccepted = ddlReasonNewVisitReferralRecommendationNotAccepted.SelectedValue.AsLookupValue();

            model.NewVisitDate = dtNewVisitDate.SelectedDateTimeOffset;

            model.Discharged = ddlDischarged.SelectedValue.AsLookupValue();
            model.FollowUpDate = dtFollowUpDate.SelectedDateTimeOffset ;

            model.PatientAttendedVisit = ddlPatientAttendedVisit.SelectedValue.AsLookupValue();
            model.FollowUpContactOutcome = ddlFollowUpContactOutcome.SelectedValue.AsLookupValue();
            model.FollowUpContactDate = dtFollowUpContactDate.SelectedDateTimeOffset;

            if (!string.IsNullOrEmpty(ddlNewVisitRecommendation.SelectedValue))
            {
                model.NewVisitReferralRecommendation = new Common.LookupValueWithDescription
                {
                    Id = ddlNewVisitRecommendation.SelectedValue.AsNullable<int>() ?? 0,
                    Description = txtNewVisitRecommendationDescription.Text
                };
            }
            else
            {
                model.NewVisitReferralRecommendation = null;
            }
            model.Notes = txtNotes.Value;

            return model;
        }

        public override void ApplyTabIndexToControl(ref short startTabIndex)
        {
            //do nothing

        }

        protected override void EditModeDataPrepare(BhsFollowUp model)
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

            if(Page.IsPostBack)
            {
                
            }

            Page.ClientScript.RegisterClientScriptInclude(this.GetType(), "nextVisitSchedule.js", ResolveClientUrl("~/scripts/controls/nextVisitSchedule.js"));
            Page.ClientScript.RegisterClientScriptInclude("richTextNotes.js", ResolveClientUrl("~/scripts/controls/richTextNotes.js"));

        }
    }
}
