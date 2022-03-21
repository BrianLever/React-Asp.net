using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Server.Screening
{
    public class DepressionScreeningScoring : ScreeningScoring
    {

        public DepressionScreeningScoring(ScreeningSectionResult result, ScreeningSection sectionInfo, IScreeningScoreLevelRepository screeningScoreLevelRepository) 
            : base(result, sectionInfo, screeningScoreLevelRepository) { }

        public override bool HasScores => true;

        public override ScreeningScoreRange[] SupportedScoreLevels => new ScreeningScoreRange[]
        {
            new ScreeningScoreRange{ ScoreLevel = 0, Left = 0, Right = 4},
            new ScreeningScoreRange{ ScoreLevel = 2, Left = 5, Right = 9},
            new ScreeningScoreRange{ ScoreLevel = 3, Left = 10, Right = 14},
            new ScreeningScoreRange{ ScoreLevel = 4, Left = 15, Right = 19},
            new ScreeningScoreRange{ ScoreLevel = 5, Left = 20, Right = 27}
        };

        public override ScreeningScoreLevel GetScoreLevel(int? score)
        {
            ScreeningScoreLevel levelOfRisk = null;

            if (!score.HasValue) return null;

            //get levels or risk
            var levels = _screeningScoreLevelRepository.GetScoreLevelsBySectionID(_sectionResult.ScreeningSectionID);
            if (levels.Count > 0)
            {
                //calculate
                //if (score == 0)
                //{
                //    levelOfRisk = levels[0];
                //}
                if (score <= 4)
                {
                    levelOfRisk = levels.First(x => x.ScoreLevel == 0); //[ 0 /*1*/]; //we have No depression vs. No + Mininal
                }
                else if (score <= 9)
                {
                    levelOfRisk = levels.First(x => x.ScoreLevel == 2);//levels[2];    
                }
                else if (score <= 14)
                {
                    levelOfRisk = levels.First(x => x.ScoreLevel == 3);//levels[3];
                }
                else if (score <= 19)
                {
                    levelOfRisk = levels.First(x => x.ScoreLevel == 4);///levels[4];
                }
                else if (score >= 20)
                {
                    levelOfRisk = levels.First(x => x.ScoreLevel == 5);//levels[5];
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
                    if (answer.QuestionID != 10) //ignore 10th question
                    {
                        score += answer.AnswerValue;
                    }
                }
            }

            return score;
        }

        
    }
}
