using DocumentFormat.OpenXml.Office2010.ExcelAc;
using System.Collections.Generic;
using System.Diagnostics;

namespace FrontDesk.Server.Screening.Models
{
    /// <summary>
    /// Text hint for all available score levels and their score. Initially created for Visit Settings page
    /// </summary>
    [DebuggerDisplay("ID: {ScreeningSectionID}, Hint: {ScoreHint}")]
    public class ScreeningSectionScoreHint
    {
        /// <summary>
        /// Screening Tool ID
        /// </summary>
        public string ScreeningSectionID { get; set; }

        /// <summary>
        /// Text description of all available score levels with scores for specific Screening Tool
        /// </summary>
        public string ScoreHint { get; set; }

    }
}
