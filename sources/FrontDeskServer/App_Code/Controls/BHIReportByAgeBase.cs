using FrontDesk.Common.Screening;
using FrontDesk.Server.Configuration;
using FrontDesk.Server.Screening.Services;

using System.Text;
using System.Web.UI;

namespace FrontDeskServer.Controls
{
    public class BHIReportByAgeBase : UserControl
    {
        protected readonly IScreeningAgesSettingsProvider _ageGroupsProvider = new ScreeningAgesDbProvider();
        protected readonly IBranchLocationService branchLocationService = new BranchLocationService();

        public int? ApplyPaddingForItemsStartingFromLine { get; set; }

        protected int _incrementalLineNumber = 0;

        protected int AgeColumnCount
        {
            get { return _ageGroupsProvider.AgeGroupsLabels.Length; }
        }

        public string GetAgesColumnHeaders(bool doNotRenderValues = false)
        {
            var html = new StringBuilder();

            foreach (var range in _ageGroupsProvider.AgeGroupsLabels)
            {
                html.AppendFormat(@"<th class=""bhi-age-column"">{0}</th>", doNotRenderValues ? string.Empty : range);
            }

            html.AppendFormat(@"<th class=""bhi-age-column"">{0}</th>", doNotRenderValues ? string.Empty : "Total");

            return html.ToString();
        }


    }
}