using System.Collections.Generic;

namespace FrontDesk.Server.Screening.Models
{
    public class SectionScoreLevelCountResult
    {
        public string SectionID { get; set; }

        public Dictionary<int, int> ScoreLevelCount { get; set; } = new Dictionary<int, int>();

        public override int GetHashCode()
        {
            return SectionID.GetHashCode();
        }

    }
}