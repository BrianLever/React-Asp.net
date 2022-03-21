using FrontDesk.Common.Bhservice;
using FrontDesk.Common.Extensions;
using FrontDesk.Server.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FrontDesk.Server.Web.Controls
{
    public partial class BhsPatientDetailsControl : BaseFormUserControl<ScreeningPatientIdentityWithAddress, long>
    {
        public override void SetModel(ScreeningPatientIdentityWithAddress model)
        {
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
        }

        public override ScreeningPatientIdentityWithAddress GetModel()
        {
            return null;
        }

        public override void ApplyTabIndexToControl(ref short startTabIndex)
        {
            
        }

     
    }
}