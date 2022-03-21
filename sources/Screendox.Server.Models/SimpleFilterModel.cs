using System;

namespace FrontDesk.Server.Screening.Models
{
    /// <summary>
    /// Filter for screening result search includes location and period
    /// </summary>
    [Serializable]
    public class SimpleFilterModel
    {
        /// <summary>
        /// Location Id
        /// </summary>
        public int? Location;
        /// <summary>
        /// Start date (inclusive)
        /// </summary>
        public DateTime? StartDate;
        /// <summary>
        /// End date (inclusive)
        /// </summary>
        public DateTime? EndDate;

        public override string ToString()
        {
            return "{0:yyyymmdd}_{1:yyyymmdd}_{2}".FormatWith(StartDate, EndDate, Location);
        }
    }
}
