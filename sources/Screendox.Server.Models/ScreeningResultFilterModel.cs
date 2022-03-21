using System;

namespace FrontDesk.Server.Screening.Models
{
    /// <summary>
    /// Extended filter for screening result search that includes patient name and Screendox ID with problem score filter
    /// </summary>
    [Serializable]
    public class ScreeningResultFilterModel : SimpleFilterModel
    {
        /// <summary>
        /// Patient's firt name
        /// </summary>
        public string FirstName;
        /// <summary>
        /// Patient's last name
        /// </summary>
        public string LastName;
        /// <summary>
        /// Screendox Id
        /// </summary>
        public long? ScreeningResultID;

        public ScreeningResultByProblemFilter ProblemScoreFilter;
    }
}
