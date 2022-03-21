using FrontDesk.Server.Screening.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrontDesk.Server.Screening.Scoring
{
    public class ScreeningSectionScoreHintCreator
    {
        /// <summary>
        /// Screening Tool ID
        /// </summary>
        public string ScreeningSectionID { get; set; }

        /// <summary>
        /// List of score levels
        /// </summary>
        public List<ScreeningScoreLevel> ScoreLevels { get; set; }
        /// <summary>
        /// Screening score object for reading metadata
        /// </summary>
        public ScreeningScoring ScreeningScore { get; set; }


        public ScreeningSectionScoreHint CreateScoreHint()
        {
            if (ScreeningScore == null) throw new ArgumentNullException(nameof(ScreeningScore));
            if (ScoreLevels == null) throw new ArgumentNullException(nameof(ScoreLevels));


            return new ScreeningSectionScoreHint
            {
                ScreeningSectionID = ScreeningSectionID,
                ScoreHint = ScreeningScore.HasScores ? GenerateScoreHintText() : String.Empty
            };
        }

        private string GenerateScoreHintText()
        {
            List<string> hints = new List<string>(ScoreLevels.Count);

            for(int index = 0; index < ScoreLevels.Count; index++)
            {
                var level = ScoreLevels[index];

                if (index == 0)
                {
                    var minimalScoreLevelRange = ScreeningScore.SupportedScoreLevels[0];
                    if(minimalScoreLevelRange.Left == minimalScoreLevelRange.Right)
                    {
                        // skip rendering negative score levels when it's only Score = 0
                        continue;
                    }
                }

                hints.Add("{0} = {1}".FormatWith(ScreeningScore.GetScoreRangeLabel(level.ScoreLevel), level.Label));
            }

            return string.Join(", ", hints);
        }
    }
}
