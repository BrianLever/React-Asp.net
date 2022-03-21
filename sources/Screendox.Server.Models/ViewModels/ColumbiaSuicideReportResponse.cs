using FrontDesk.Common.Extensions;

using ScreenDox.Server.Models.ColumbiaReports;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenDox.Server.Models.ViewModels
{
    public class ColumbiaSuicideReportResponse : ColumbiaSuicideReport
    {

        public string CreatedDateFormatted
        {
            get { return CreatedDate.FormatAsDateWithTime(); }
        }

        public string CompleteDateFormatted
        {
            get { return CompleteDate.FormatAsDateWithTime(); }
        }

        /// <summary>
        /// Alias for BhsStaffNameCompleted for UI components reusability
        /// </summary>
        public string StaffNameCompleted
        {
            get
            {
                return BhsStaffNameCompleted;
            }
            set
            {
                BhsStaffNameCompleted = value;
            }
        }
    }
}
