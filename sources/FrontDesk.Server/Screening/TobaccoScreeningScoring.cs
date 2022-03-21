namespace FrontDesk.Server.Screening
{
    public class TobaccoScreeningScoring : ScreeningScoring
    {
        public TobaccoScreeningScoring(ScreeningSectionResult result, ScreeningSection sectionInfo, IScreeningScoreLevelRepository screeningScoreLevelRepository) 
            : base(result, sectionInfo, screeningScoreLevelRepository) { }


        public override bool HasScores => false;

        public override ScreeningScoreRange[] SupportedScoreLevels => new ScreeningScoreRange[]
        {
            new ScreeningScoreRange{ ScoreLevel = 0, Left = 0, Right = 0},
            new ScreeningScoreRange{ ScoreLevel = 1, Left = 1, Right = 1},
        };

       
        // this section does not have scoring
        public override ScreeningScoreLevel GetScoreLevel(int? score)
        {
            ScreeningScoreLevel levelOfRisk = null;

            if (!score.HasValue) return null;

            //get levels or risk
            var levels = _screeningScoreLevelRepository.GetScoreLevelsBySectionID(_sectionResult.ScreeningSectionID);
            if (levels.Count > 0 && levels.Count == 2)
            {
                //calculate
                if (score < 1)
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

        // this section does not have scoring
        protected override int? GetScore()
        {
            int score = 0;
            if (_sectionResult.AnswerValue == 1) //if yes
            {
                score = 1;
            }
            return score;
        }
    }
}
