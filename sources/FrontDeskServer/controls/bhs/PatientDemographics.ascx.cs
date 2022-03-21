using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using FrontDesk.Common.Bhservice;
using FrontDesk.Common.Extensions;
using FrontDesk.Server.Extensions;
using FrontDesk.Server.Web.Extensions;

namespace FrontDesk.Server.Web.Controls
{

    public partial class PatientDemographicsControl : BaseWebFormUserControl<BhsDemographics, long>
    {


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            ddlRace.AddDefaultNotSelectedItem();
            ddlGender.AddDefaultNotSelectedItem();
            ddlSexualOrientation.AddDefaultNotSelectedItem();
            ddlMaritalStatus.AddDefaultNotSelectedItem();
            ddlEducationLevel.AddDefaultNotSelectedItem();
            ddlLivingOnReservation.AddDefaultNotSelectedItem();
            //rptMilitaryExperience.DataSource = Enumerable.Repeat(new ManualScreeningResultValue(), 4);

 

        }

        public bool HideContent { get; set; }

        public void SetReadOnlyFields(BhsDemographics model)
        {
            //patient info
            lblFirstname.Text = model.FirstName;
            lblLastname.Text = model.LastName;
            lblMiddlename.Text = model.MiddleName;
            lblBirthday.Text = model.Birthday.FormatAsDate();

            lblStreetAddress.Text = model.StreetAddress.FormatAsNullableString();
            lblCity.Text = model.City.FormatAsNullableString();
            lblState.Text = model.StateName.FormatAsNullableString().ToUpper();
            lblZipCode.Text = model.ZipCode.FormatAsNullableString();
            lblPhone.Text = model.Phone.FormatAsNullableString();
            lblRecordNo.Text = model.ExportedToHRN;
            
            txtStaffName.Text = !string.IsNullOrEmpty(model.BhsStaffNameCompleted) ? model.BhsStaffNameCompleted : FDUser.GetCurrentUser().FullName;
            txtCompleteDate.Text = model.CompleteDate.FormatAsDateWithTime();

        }

        public override void SetModel(BhsDemographics model)
        {
            SetReadOnlyFields(model);

            if (model.Race != null)
            {
                ddlRace.SetValueOrDefault(model.Race.Id);
            }

            if (model.Gender != null)
            {
                ddlGender.SetValueOrDefault(model.Gender.Id);
            }

            if (model.SexualOrientation != null)
            {
                ddlSexualOrientation.SetValueOrDefault(model.SexualOrientation.Id);
            }

            if (model.MaritalStatus != null)
            {
                ddlMaritalStatus.SetValueOrDefault(model.MaritalStatus.Id);
            }
            if (model.EducationLevel != null)
            {
                ddlEducationLevel.SetValueOrDefault(model.EducationLevel.Id);
            }
            if (model.LivingOnReservation != null)
            {
                ddlLivingOnReservation.SetValueOrDefault(model.LivingOnReservation.Id);
            }
            txtTribalAffiliation.Text = model.TribalAffiliation;
            txtCountyofResidence.Text = model.CountyOfResidence;



            if(model.MilitaryExperience.Count == 0) //set default to None
            {
                model.MilitaryExperience.Add(new Common.LookupValue { Id = 1 });
            }
            chlMilitaryExperience.SetValues(model.MilitaryExperience);
        }

        public override BhsDemographics GetModel()
        {
            if (!Page.IsValid) return null;

            var model = Page.CurFormObject;



            model.Race = new Common.LookupValue
            {
                Id = ddlRace.SelectedValue.AsNullable<int>() ?? 0
            };
            model.Gender = new Common.LookupValue
            {
                Id = ddlGender.SelectedValue.AsNullable<int>() ?? 0
            };
            model.SexualOrientation = new Common.LookupValue
            {
                Id = ddlSexualOrientation.SelectedValue.AsNullable<int>() ?? 0
            };
            model.TribalAffiliation = txtTribalAffiliation.Text.Trim();
            model.MaritalStatus = new Common.LookupValue
            {
                Id = ddlMaritalStatus.SelectedValue.AsNullable<int>() ?? 0
            };
            model.EducationLevel = new Common.LookupValue
            {
                Id = ddlEducationLevel.SelectedValue.AsNullable<int>() ?? 0
            };
            model.LivingOnReservation = new Common.LookupValue
            {
                Id = ddlLivingOnReservation.SelectedValue.AsNullable<int>() ?? 0
            };
            model.CountyOfResidence = txtCountyofResidence.Text.Trim();

            model.MilitaryExperience = new List<Common.LookupValue>();
            foreach(ListItem item in chlMilitaryExperience.Items)
            {
                if(item.Selected)
                {
                    model.MilitaryExperience.Add(new Common.LookupValue
                    {
                        Id = item.Value.AsNullable<int>() ?? 1
                    });
                }
            };

          
            return model;
        }

        public override void ApplyTabIndexToControl(ref short startTabIndex)
        {
            //do nothing

        }

        protected override void EditModeDataPrepare(BhsDemographics model)
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

            Page.ClientScript.RegisterClientScriptInclude("singleSelectCheckbox.js", ResolveClientUrl("~/scripts/controls/singleSelectCheckbox.js"));
            Page.ClientScript.RegisterClientScriptInclude("militaryServiceList.js", ResolveClientUrl("~/scripts/controls/militaryServiceList.js"));
            Page.ClientScript.RegisterClientScriptInclude("bhsDemographicCtrl.js", ResolveClientUrl("~/scripts/controls/bhsDemographicCtrl.js"));


        }
    }
}
