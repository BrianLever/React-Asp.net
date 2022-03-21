using Common.Logging;

using System;
using System.Linq;

namespace FrontDesk.Server.Screening
{
    public abstract class ScreeningScoring
    {
        private readonly ILog _logger = LogManager.GetLogger<ScreeningScoring>();

        // ReSharper disable once InconsistentNaming
        protected ScreeningSectionResult _sectionResult = null;
        protected ScreeningSection _sectionInfo = null;

        protected readonly int[] _scoringQuestions;

        // ReSharper disable once InconsistentNaming
        protected IScreeningScoreLevelRepository _screeningScoreLevelRepository = null;

        private int? _score = null;
        private ScreeningScoreLevel _scoreLevel = null;


        /// <summary>
        /// Constructor with section result object
        /// </summary>
        /// <param name="sectionResult"></param>
        /// <exception cref="System.ArgumentNullException">sectionResult is null</exception>
        internal ScreeningScoring(
            ScreeningSectionResult sectionResult,
            ScreeningSection sectionInfo,
            IScreeningScoreLevelRepository screeningScoreLevelRepository)
        {
            if (sectionResult == null) throw new ArgumentNullException("sectionResult");
            if (sectionInfo == null) throw new ArgumentNullException("sectionInfo");

            if (screeningScoreLevelRepository == null) throw new ArgumentNullException("screeningScoreLevelRepository");

            _sectionResult = sectionResult;
            _sectionInfo = sectionInfo;
            _screeningScoreLevelRepository = screeningScoreLevelRepository;

            _scoringQuestions = _sectionInfo.NotMainSectionQuestions.Select(x => x.QuestionID).ToArray();
        }

        #region Metadata

        /// <summary>
        /// True if screening tool has scores. Otherwise it's binary - Negative = 0 and Positive = 1
        /// </summary>
        public abstract bool HasScores { get; }


        public string GetScoreRangeLabel(int scoreLevelLevel)
        {
            _logger.DebugFormat("Callling GetScoreRangeLabel. Passed scoreLevelIndex: {0}", scoreLevelLevel);

            var range = SupportedScoreLevels.First(x => x.ScoreLevel == scoreLevelLevel);

            if (range.Right.HasValue)
            {
                if (range.Right != range.Left) return "{0}-{1}".FormatWith(range.Left == 0 ? 1 : range.Left, range.Right);
                else return "{0}".FormatWith(range.Left);
            }
            else
            {
                return "{0} or greater".FormatWith(range.Left);
            }
        }

        public abstract ScreeningScoreRange[] SupportedScoreLevels { get; }



        #endregion

        /// <summary>
        /// Get score
        /// </summary>
        public int? Score
        {
            get
            {
                if (_score == null) _score = GetScore();
                return _score.GetValueOrDefault();
            }
        }
        /// <summary>
        /// Get score level
        /// </summary>
        public ScreeningScoreLevel ScoreLevel
        {
            get
            {
                if (_scoreLevel == null) _scoreLevel = GetScoreLevel(Score);
                return _scoreLevel;
            }
        }



        protected abstract int? GetScore();

        public abstract ScreeningScoreLevel GetScoreLevel(int? score);


    }

    public struct ScreeningScoreRange
    {
        public int ScoreLevel;
        public int Left;
        public int? Right;
    }

}
