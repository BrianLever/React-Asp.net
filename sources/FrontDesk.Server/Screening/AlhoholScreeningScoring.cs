using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Server.Screening
{
    /// <summary>
    /// Calculate Alcohol section scoring and level of risk
    /// </summary>
    public class AlhoholScreeningScoring : ScreeningScoring
    {
        public AlhoholScreeningScoring(ScreeningSectionResult result, ScreeningSection sectionInfo, IScreeningScoreLevelRepository screeningScoreLevelRepository) 
            : base(result, sectionInfo, screeningScoreLevelRepository) { }

        public override bool HasScores => true;

        public override ScreeningScoreRange[] SupportedScoreLevels => new ScreeningScoreRange[]
        {
            new ScreeningScoreRange{ ScoreLevel = 0, Left = 0, Right = 0},
            new ScreeningScoreRange{ ScoreLevel = 1, Left = 1, Right = 1},
            new ScreeningScoreRange{ ScoreLevel = 2, Left = 2, Right = 2},
            new ScreeningScoreRange{ ScoreLevel = 3, Left = 3, Right = 4}
        };

        

        public override ScreeningScoreLevel GetScoreLevel(int? score)
        {
            ScreeningScoreLevel levelOfRisk = null;

            if (!score.HasValue) return null;

            //get levels or risk
            var levels = _screeningScoreLevelRepository.GetScoreLevelsBySectionID(_sectionResult.ScreeningSectionID);
            if (levels.Count > 0 && levels.Count == 4)
            {
                //calculate
                if (score == 0)
                {
                    levelOfRisk = levels[0];
                }
                else if (score == 1)
                {
                    levelOfRisk = levels[1];
                }
                else if (score == 2)
                {
                    levelOfRisk = levels[2];
                }
                else if (score >= 3)
                {
                    levelOfRisk = levels[3];
                }

            }
            return levelOfRisk;
        }


        protected override int? GetScore()
        {
            int score = 0;
            if (_sectionResult.AnswerValue == 1) //if yes
            {
                foreach (var answer in _sectionResult.GetAnswersOnSpecificQuestions(_scoringQuestions))
                {
                    score += answer.AnswerValue;
                }
            }
            return score;
        }
    }
}
