using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Server.Screening
{
    public class DrugOfChoiceScreeningScoring : ScreeningScoring
    {

        public DrugOfChoiceScreeningScoring(ScreeningSectionResult result, ScreeningSection sectionInfo, IScreeningScoreLevelRepository screeningScoreLevelRepository)
            : base(result, sectionInfo, screeningScoreLevelRepository)
        {

        }

        public override bool HasScores => false;

        public override ScreeningScoreRange[] SupportedScoreLevels => new ScreeningScoreRange[]
        {
            new ScreeningScoreRange{ ScoreLevel = 0, Left = 0, Right = 0},
            new ScreeningScoreRange{ ScoreLevel = 1, Left = 1, Right = 1}
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
                if (score > 0)
                {
                    levelOfRisk = levels[0];
                }
                else
                {
                    levelOfRisk = levels[1];
                }

            }
            return levelOfRisk;
        }

        protected override int? GetScore()
        {
            int score = 0;
            foreach (var answer in _sectionResult.GetAnswersOnSpecificQuestions(_scoringQuestions))
            {
                score++;
            }

            return score;
        }

       
    }
}
