using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Server.Screening.Models
{
    [Serializable]
    public class ScreeningResultByProblemFilter
    {
        public List<ScreeningResultByProblemFilterItem> Filters = new List<ScreeningResultByProblemFilterItem>();

        public const string AnySectionName = "Any";
    }
    [Serializable]
    public struct ScreeningResultByProblemFilterItem
    {
        public string ScreeningSection;
        public int MinScoreLevel;
    }
}
