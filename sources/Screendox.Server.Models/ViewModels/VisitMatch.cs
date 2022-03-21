using FrontDesk;
using FrontDesk.Common;
using FrontDesk.Common.Extensions;

using RPMS.Common.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ScreenDox.Server.Models.ViewModels
{

    /// <summary>
    /// DTO for rendering Matched Scheduled Visit list from EHR system
    /// </summary>
    public class VisitMatch : Visit
    {
        /// <summary>
        /// Date of Visit formatted
        /// </summary>
        [DataMember]
        public string DateFormatted
        {
            get
            {
                return this.Date.FormatAsDateWithTime();
            }
        }

    }
}
