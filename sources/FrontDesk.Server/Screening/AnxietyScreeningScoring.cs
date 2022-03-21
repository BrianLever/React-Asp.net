using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Server.Screening
{
    public class AnxietyScreeningScoring : ScreeningScoring
    {

        public AnxietyScreeningScoring(
            ScreeningSectionResult result, 
            ScreeningSection sectionInfo, 
            IScreeningScoreLevelRepository screeningScoreLevelRepository) 
            : base(result, sectionInfo, screeningScoreLevelRepository) { }

        public override bool HasScores => true;

        public override ScreeningScoreRange[] SupportedScoreLevels => new ScreeningScoreRange[]
        {
            new ScreeningScoreRange{ ScoreLevel = 0, Left = 0, Right = 4},
            new ScreeningScoreRange{ ScoreLevel = 1, Left = 5, Right = 9},
            new ScreeningScoreRange{ ScoreLevel = 2, Left = 10, Right = 14},
            new ScreeningScoreRange{ ScoreLevel = 3, Left = 15, Right = 21}
        };

        public override ScreeningScoreLevel GetScoreLevel(int? score)
        {
            ScreeningScoreLevel levelOfRisk = null;

            if (!score.HasValue) return null;

            //get levels or risk
            var levels = _screeningScoreLevelRepository.GetScoreLevelsBySectionID(_sectionResult.ScreeningSectionID);
            if (levels.Count > 0)
            {
                if (score <= 4)
                {
                    levelOfRisk = levels.First(x => x.ScoreLevel == 0); //None Mininal
                }
                else if (score <= 9)
                {
                    levelOfRisk = levels.First(x => x.ScoreLevel == 1); // MILD    
                }
                else if (score <= 14)
                {
                    levelOfRisk = levels.First(x => x.ScoreLevel == 2); // MODERATE
                }
                else if (score >= 15)
                {
                    levelOfRisk = levels.First(x => x.ScoreLevel == 3); // SEVERE
                }

            }
            return levelOfRisk;
        }

        protected override int? GetScore()
        {
            int score = 0;
            if (_sectionResult.Answers != null)
            {
                foreach (var answer in _sectionResult.Answers)
                {
                    if (answer.QuestionID != 8) //ignore 8th question
                    {
                        score += answer.AnswerValue;
                    }
                }
            }

            return score;
        }

        
    }
}
