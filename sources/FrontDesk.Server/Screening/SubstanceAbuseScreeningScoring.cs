using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Server.Screening
{
    /// <summary>
    /// Calculate Alcohol section scoring and level of risk
    /// </summary>
    public class SubstanceAbuseScreeningScoring : ScreeningScoring
    {
        public SubstanceAbuseScreeningScoring(ScreeningSectionResult result, ScreeningSection screeningInfo, IScreeningScoreLevelRepository screeningScoreLevelRepository)
            : base(result, screeningInfo, screeningScoreLevelRepository) { }

        public override bool HasScores => true;

        public override ScreeningScoreRange[] SupportedScoreLevels => new ScreeningScoreRange[]
        {
            new ScreeningScoreRange{ ScoreLevel = 0, Left = 0, Right = 0},
            new ScreeningScoreRange{ ScoreLevel = 1, Left = 1, Right = 2},
            new ScreeningScoreRange{ ScoreLevel = 2, Left = 3, Right = 5},
            new ScreeningScoreRange{ ScoreLevel = 3, Left = 6, Right = 8},
            new ScreeningScoreRange{ ScoreLevel = 4, Left = 9, Right = 10}
        };


        public override ScreeningScoreLevel GetScoreLevel(int? score)
        {
            ScreeningScoreLevel levelOfRisk = null;

            if (!score.HasValue) return null;

            //get levels or risk
            var levels = _screeningScoreLevelRepository.GetScoreLevelsBySectionID(_sectionResult.ScreeningSectionID);
            if (levels.Count > 0 && levels.Count == 5)
            {
                //calculate
                if (score == 0)
                {
                    levelOfRisk = levels[0];
                }
                else if (score <= 2)
                {
                    levelOfRisk = levels[1];
                }
                else if (score <= 5)
                {
                    levelOfRisk = levels[2];
                }
                else if (score <= 8)
                {
                    levelOfRisk = levels[3];
                }
                else if (score > 8)
                {
                    levelOfRisk = levels[4];
                }

            }
            return levelOfRisk;
        }

        protected override int? GetScore()
        {
            int score = 0;
            if (_sectionResult.AnswerValue == 1) //if yes
            {
                score = 1; // answer on main question adds 1 to score
                foreach (var answer in _sectionResult.GetAnswersOnSpecificQuestions(_scoringQuestions))
                {
                    if (answer.QuestionID != 2)
                    {
                        score += answer.AnswerValue;
                    }
                    else
                    {
                        //for Question 2: Are you always able to stop using drugs when you want to?
                        // No is positive result
                        if (answer.AnswerValue == 0) score++;
                    }
                }
            }
            return score;
        }
    }
}
