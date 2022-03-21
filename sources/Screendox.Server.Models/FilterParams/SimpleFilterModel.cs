using FrontDesk;
using FrontDesk.Server.Screening.Models;

namespace ScreenDox.Server.Models.FilterParams
{
    /// <summary>
    /// Filter for screening result search includes location and period
    /// </summary>
    public class BhiReportFilter : SimpleFilterModel
    {
        public bool RenderUniquePatientsReportType { get; set; } = false;

        public override string ToString()
        {
            return "{0}_{1}".FormatWith(base.ToString(), RenderUniquePatientsReportType);
        }
    }
}
