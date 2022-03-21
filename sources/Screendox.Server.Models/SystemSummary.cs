namespace ScreenDox.Server.Models
{
    /// <summary>
    /// Actual number of screens done and current number of active locations and kiosk
    /// </summary>
    public class SystemSummary
    {
        /// <summary>
        /// Total number of patient screens done
        /// </summary>
        public long CheckInRecordCount { get; set; }
        /// <summary>
        /// Number of active locations
        /// </summary>
        public int BranchLocationCount { get; set; }
        /// <summary>
        /// Number of active kiosks
        /// </summary>
        public int KioskCount { get; set; }

    }
}
