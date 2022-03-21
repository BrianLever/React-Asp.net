using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Server.Screening
{
    /// <summary>
    /// Calculate Problem Gambling section scoring and level of risk
    /// </summary>
    public class ProblemGamblingScreeningScoring : ScreeningScoring
    {
        public ProblemGamblingScreeningScoring(ScreeningSectionResult result, ScreeningSection screeningInfo, IScreeningScoreLevelRepository screeningScoreLevelRepository)
            : base(result, screeningInfo, screeningScoreLevelRepository) { }

        public override bool HasScores => true;

        public override ScreeningScoreRange[] SupportedScoreLevels => new ScreeningScoreRange[]
        {
            new ScreeningScoreRange{ ScoreLevel = 0, Left = 0, Right = 0},
            new ScreeningScoreRange{ ScoreLevel = 1, Left = 1, Right = 3},
        };


        public override ScreeningScoreLevel GetScoreLevel(int? score)
        {
            ScreeningScoreLevel levelOfRisk = null;

            if (!score.HasValue) return null;

            //get levels or risk
            var levels = _screeningScoreLevelRepository.GetScoreLevelsBySectionID(_sectionResult.ScreeningSectionID);
            if (levels.Count > 0 && levels.Count == 2)
            {
                //calculate
                if (score == 0)
                {
                    levelOfRisk = levels[0];
                }
                else if (score >= 1)
                {
                    levelOfRisk = levels[1];
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
