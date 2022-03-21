
using Common.Logging;

using FrontDesk.Server.Data;
using FrontDesk.Server.Screening.Models;
using FrontDesk.Server.Screening.Scoring;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace FrontDesk.Server.Screening.Services
{
    public interface IScreeningScoreLevelService
    {
        List<ScreeningSectionScoreHint> GetAllScoreHints();
    }

    public class ScreeningScoreLevelService : IScreeningScoreLevelService
    {
        private readonly ILog _logger = LogManager.GetLogger<ScreeningScoreLevelService>();
        private readonly IScreeningScoreRepository _repository;


        public ScreeningScoreLevelService(IScreeningScoreRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public ScreeningScoreLevelService() : this(new ScreeningScoreDb())
        {

        }

        public List<ScreeningSectionScoreHint> GetAllScoreHints()
        {
            var result = new List<ScreeningSectionScoreHint>();

            // fill score levels

            var scoreHintCreatorList = new List<ScreeningSectionScoreHintCreator>();

            string currentSectionId = string.Empty;
            ScreeningSectionScoreHintCreator currentScoreCreator = null;

            foreach (var scoreLevel in _repository.GetAllScoreLevels())
            {
                if (string.IsNullOrEmpty(currentSectionId) || scoreLevel.ScreeningSectionID != currentSectionId)
                {
                    // start new section
                    currentSectionId = scoreLevel.ScreeningSectionID;
                    currentScoreCreator = InitScoreCreatorFromScoreLevel(scoreLevel);

                    scoreHintCreatorList.Add(currentScoreCreator);
                }
                else
                {
                    currentScoreCreator.ScoreLevels.Add(scoreLevel);
                }
            }


            result = scoreHintCreatorList.Select(x => x.CreateScoreHint()).ToList(); ;

            // add score hint for Suicide Identiation questions for Visit Creator
            var suicideAnswers = _repository.GetScoreLevelsToQuestion(
                ScreeningSectionDescriptor.Depression,
                ScreeningSectionDescriptor.DepressionThinkOfDeathQuestionID);

            // capitalize names
            var textInfo = new CultureInfo("en-US", false).TextInfo;

            result.Add(new ScreeningSectionScoreHint
            {
                ScreeningSectionID = VisitSettingsDescriptor.DepressionThinkOfDeath,
                ScoreHint = string.Join(", ", suicideAnswers.Select(x => "{0} = {1}".FormatWith(x.ScoreLevel, textInfo.ToTitleCase(x.Label))))
            });




            return result;
        }

        private ScreeningSectionScoreHintCreator InitScoreCreatorFromScoreLevel(ScreeningScoreLevel scoreLevel)
        {
            var result = new ScreeningSectionScoreHintCreator
            {
                ScreeningSectionID = scoreLevel.ScreeningSectionID,
                ScoreLevels = new List<ScreeningScoreLevel> { scoreLevel }
            };

            ScreeningSectionResult screeningSectionResult = new ScreeningSectionResult()
            {
                ScreeningSectionID = scoreLevel.ScreeningSectionID
            };

            result.ScreeningScore = ScreeningScoringFactory.CreateScreeningScoring(screeningSectionResult, new ScreeningSection());

            return result;
        }
    }

}
