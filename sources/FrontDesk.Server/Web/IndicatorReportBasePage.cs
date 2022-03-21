using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;
using FrontDesk.Common.Messages;
using FrontDesk.Server.Logging;
using FrontDesk.Server.Membership;
using FrontDesk.Server.Screening;
using FrontDesk.Server.Screening.Services;

namespace FrontDesk.Server.Web
{
    public abstract class IndicatorReportBasePage : BasePage
    {
        protected readonly IBranchLocationService branchLocationService = new BranchLocationService();

        #region Filters

        protected string SelectedBranchLocationID
        {
            get { return Convert.ToString(ViewState["LocationID"]); }
            set { ViewState["LocationID"] = value; }
        }

        protected string SelectStartDate
        {
            get { return Convert.ToString(ViewState["StartDate"]); }
            set { ViewState["StartDate"] = value; }
        }

        protected string SelectEndDate
        {
            get { return Convert.ToString(ViewState["EndDate"]); }
            set { ViewState["EndDate"] = value; }
        }

        protected bool SelectedReportTypeIsUniquePatientsFilter
        {
            get { return Convert.ToInt32(ViewState["ReportType"]) > 0? false: true; }
            set { ViewState["ReportType"] = value; }
        }


        public DateTime? minStartDate { get; set; }
        public DateTime endDate { get; set; }
        public BranchLocation currentLocation { get; set; }

        #endregion

        //common controls
        protected abstract Button PrintButton { get; }
        protected abstract DropDownList LocationsDropDownList { get; }
        protected abstract Label MyLocationLabel { get; }


        protected DateTime? GetMinStartDate()
        {
            DateTime? startDate = null;

            DateTimeOffset? dbMinDate = ScreeningResultHelper.GetMinDate();
            if (dbMinDate.HasValue)
            {
                startDate = dbMinDate.Value.DateTime;
            }
            else
            {
                startDate = null;
            }
            return startDate;
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            PrintReport();

            Response.Flush();
            Response.End();
        }

        [PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = UserRoles.BranchAdministrator)]
        [PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = UserRoles.SuperAdministrator)]
        [PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = UserRoles.LeadMedicalProfessionals)]
        protected abstract void PrintReport();
       

        protected override void SetPagePermissions()
        {
            if (Roles.IsUserInRole(UserRoles.SuperAdministrator) || Roles.IsUserInRole(UserRoles.BranchAdministrator))
            {
                ReadPermission = true;
                WritePermission = true;
                PrintPermission = true;
                DeletePermission = true;
            }
            else if (Roles.IsUserInRole(UserRoles.LeadMedicalProfessionals))
            {
                ReadPermission = true;
                PrintPermission = true;
                WritePermission = false;
                DeletePermission = false;
            }
            else
            {
                ReadPermission = true;
                WritePermission = false;
                PrintPermission = false;
                DeletePermission = false;
            }
        }

        protected override void SetControlsEnabledState()
        {
            PrintButton.Visible = HasPrintPermission;
            LocationsDropDownList.Visible = HasWritePermission;
            MyLocationLabel.Visible = !HasWritePermission;
        }
    }
}
