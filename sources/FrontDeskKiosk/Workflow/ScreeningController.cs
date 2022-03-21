using System;
using System.Linq;

using Common.Logging;

using FrontDesk.Services;

namespace FrontDesk.Kiosk.Workflow
{
    /// <summary>
    /// Manage section's screening workflow data and keep results
    /// </summary>
    public class ScreeningController
    {
        private static readonly object _screeningLock = new object();
        private readonly ILog _logger = LogManager.GetLogger<ScreeningController>();

        /// <summary>
        /// Implements logic for workflow step flow
        /// </summary>
        private readonly IScreeningFrequencySpecification _wfFrequencySpec;

        private readonly IScreeningSectionAgeService _screeningSectionAgeService;

        public IScreeningFrequencySpecification FrequencySpecification
        {
            get
            {
                return _wfFrequencySpec;
            }
        }

        private readonly IScreeningResultState _resultState;

        public ScreeningState CurrentState = new ScreeningState();



        #region Constructors

        public ScreeningController(IScreeningFrequencySpecification wfFrequencySpec, IScreeningResultState resultState, IScreeningSectionAgeService screeningSectionAgeService)
        {
            if (wfFrequencySpec == null)
                throw new ArgumentNullException("wfFrequencySpec");

            if (resultState == null)
                throw new ArgumentNullException("resultState");

            if (screeningSectionAgeService == null)
                throw new ArgumentNullException("screeningSectionAgeService");

            _logger.Debug("Initializing ScreeningController.");

            _wfFrequencySpec = wfFrequencySpec;

            _resultState = resultState;
            _screeningSectionAgeService = screeningSectionAgeService;

            _logger.Debug("Initialized ScreeningController.");
        }


        #endregion

        #region Questions Initialization


        private Screening _screening;

        /// <summary>
        /// Reset internal workflow 
        /// </summary>
        public void InitializeScreeningQuestions()
        {
            InitializeScreeningQuestions(false);
        }

        /// <summary>
        /// Read screning questions from local database
        /// </summary>
        public void InitializeScreeningQuestions(bool forceDbRead)
        {
            lock (_screeningLock)
            {
                ClearScreeningData();

                //get current screening questions
                if (_screening == null || forceDbRead)
                {
                    if (forceDbRead)
                    {
                        _resultState.ResetScreeningMetaData();
                    }
                    _screening = _resultState.ScreeningMetaData;
                }
            }
        }
        /// <summary>
        /// Clear screening data
        /// </summary>
        private void ClearScreeningData()
        {
            CurrentState.Reset();

        }

        /// <summary>
        /// If screening questions have not been initialized, initialize them
        /// </summary>
        private void EnsureScreeningQuestionsInitialized()
        {
            if (_screening == null)
            {
                lock (_screeningLock)
                {
                    if (_screening == null)
                    {
                        InitializeScreeningQuestions();
                    }
                }
            }
        }

        #endregion

        #region Workflow Helper Methods





        /// <summary>
        /// Get first section
        /// </summary>
        /// <returns></returns>
        public ScreeningSection GetFirstSection()
        {
            ScreeningSection section = null;
            EnsureScreeningQuestionsInitialized();

            if (_screening.Sections.Count > 0)
            {
                section = _screening.Sections[0];
            }
            return section;
        }


        /// <summary>
        /// Get following section
        /// </summary>
        /// <returns>Returns next section. If this is the last section in the screening, returns null</returns>
        public ScreeningSection GetNextSection(string currentSectionId)
        {
            ScreeningSection section = null;
            EnsureScreeningQuestionsInitialized();

            if (_screening.Sections.Count > 0)
            {
                int index = 0;
                for (; index < _screening.Sections.Count; index++)
                {
                    if (_screening.Sections[index].ScreeningSectionID == currentSectionId)
                    {
                        index++; //get next
                        break;
                    }
                }
                //if there is next session - return it
                if (index < _screening.Sections.Count)
                {
                    section = _screening.Sections[index];
                }

                //if section equal to DrugOfChoice, skip if result of Substance Abuse is Negative
                if (section != null && section.ScreeningSectionID == ScreeningSectionDescriptor.DrugOfChoice)
                {
                    //check that Substance Abuse is negatve
                    var substanceAbuseSection = _resultState.Result.FindSectionByID(ScreeningSectionDescriptor.SubstanceAbuse);

                    if (substanceAbuseSection == null)
                    {
                        //if drug section has been skipped, skip the Drug of Chouce
                        section = GetNextSection(section.ScreeningSectionID);
                    }
                    else
                    {
                        if (substanceAbuseSection.Answers.Count(x => x.AnswerValue > 0) == 0)
                        {
                            //if all answers where negative, skip drug of choice
                            section = GetNextSection(section.ScreeningSectionID);
                        }
                    }
                }

            }
            return section;
        }


        /// <summary>
        /// Get first question in the section
        /// </summary>
        /// <returns>Returns question if found or null</returns>
        public ScreeningSectionQuestion GetFirstSectionQuestion(string sectionId)
        {
            ScreeningSectionQuestion question = null;
            EnsureScreeningQuestionsInitialized();

            ScreeningSection section = _screening.FindSectionByID(sectionId);


            if (section != null && section.Questions.Count > 0)
            {
                question = section.Questions[0];
            }
            return question;
        }

        /// <summary>
        /// Get first question in the section
        /// </summary>
        /// <returns>Returns question if found or null</returns>
        public ScreeningSectionQuestion GetNextSectionQuestion(string currentSectionId, int currentSectionQuestionID)
        {
            ScreeningSectionQuestion question = null;

            EnsureScreeningQuestionsInitialized();

            ScreeningSection section = _screening.FindSectionByID(currentSectionId);
            if (section != null)
            {
                var currentQuestion = section.FindQuestionByID(currentSectionQuestionID);

                if (currentQuestion != null)
                {
                    int index = section.Questions.IndexOf(currentQuestion) + 1;
                    if (index < section.Questions.Count)
                    {
                        question = section.Questions[index];
                    }


                    //Drug of Choice - 3rd questions is skipped if "None selected on the second answer
                    if (question != null)
                    {
                        if (section.ScreeningSectionID == ScreeningSectionDescriptor.DrugOfChoice)
                        {
                            if (question.QuestionID == 3 && _resultState.Result.FindSectionByID(currentSectionId).FindQuestionByID(currentSectionQuestionID).AnswerValue == 0)
                            {
                                question = null; //skip question #3
                            }
                        }
                        else if (question.ShowOnlyWhenPossitiveScore)
                        {
                            // skip if all previous questions where negative
                            if (!_resultState.Result.FindSectionByID(currentSectionId).HasPositiveAnswers())
                            {
                                question = null; // skip this and next questions
                            }

                        }
                    }
                }
            }

            return question;
        }



        #endregion

        #region Worfklow methods
        /// <summary>
        /// Restart screening workflow
        /// </summary>
        public void RestartScreening()
        {
            lock (_screeningLock)
            {
                ClearScreeningData();
            }
        }

        /// <summary>
        /// Go to the next section.
        /// </summary>
        /// <returns>Returns next section or null, if workflow completed</returns>
        public ScreeningSection GoToNextSection()
        {
            if (CurrentState.Section != null)
            {
                _logger.InfoFormat("[GoToNextSection] Finished section [{0}]", CurrentState.Section?.ScreeningSectionID);
                _resultState.ScreeningTimeLog.StopSectionTimeRecording(CurrentState.Section?.ScreeningSectionID);
            }

            EnsureScreeningQuestionsInitialized();
            lock (_screeningLock)
            {

                CurrentState.Section = PickNextSection();

                //check if this section should be presented to patient according to the minimal age settings
                if (CurrentState.Section != null)
                {
                    ScreeningSectionAge ageSetting = _screeningSectionAgeService
                        .GetMinimalAgeForScreeningSection(CurrentState.Section.ScreeningSectionID);

                    while (CurrentState.Section != null
                        && (!ageSetting.IsEnabled
                        || _resultState.Result.Age < ageSetting.MinimalAge
                        || _wfFrequencySpec.IsSkipRequiredForSection(CurrentState.Section.ScreeningSectionID))
                        )
                    {
                        CurrentState.Section = PickNextSection();
                        if (CurrentState.Section != null)
                        {
                            ageSetting = _screeningSectionAgeService.GetMinimalAgeForScreeningSection(CurrentState.Section.ScreeningSectionID);
                        }
                    }
                }

                //set the first main question
                CurrentState.SectionQuestion = null;
                if (CurrentState.Section != null)
                {
                    GoToNextQuestion();
                }
            }
            if (CurrentState.Section != null)
            {
                _logger.InfoFormat("[GoToNextSection] Started new section [{0}]", CurrentState.Section?.ScreeningSectionID);
                _resultState.ScreeningTimeLog.StartSectionTimeRecording(CurrentState.Section?.ScreeningSectionID);
            }

            return CurrentState.Section;
        }

        /// <summary>
        /// Get next section but do not change the current section
        /// </summary>
        /// <returns></returns>
        public ScreeningSection PickNextSection()
        {
            ScreeningSection section;
            EnsureScreeningQuestionsInitialized();
            lock (_screeningLock)
            {
                if (CurrentState.Section == null)
                {
                    section = GetFirstSection();
                }
                else
                {
                    section = GetNextSection(CurrentState.Section.ScreeningSectionID);
                }
            }
            return section;
        }

        /// <summary>
        /// Go to the next section.
        /// </summary>
        public ScreeningSectionQuestion GoToNextQuestion()
        {
            EnsureScreeningQuestionsInitialized();
            lock (_screeningLock)
            {
                if (CurrentState.Section == null)
                {
                    CurrentState.Section = GetFirstSection();
                }

                if (CurrentState.SectionQuestion == null)
                {
                    CurrentState.SectionQuestion = GetFirstSectionQuestion(CurrentState.Section.ScreeningSectionID);
                }
                else
                {
                    CurrentState.SectionQuestion = GetNextSectionQuestion(CurrentState.Section.ScreeningSectionID, CurrentState.SectionQuestion.QuestionID);
                }
            }
            return CurrentState.SectionQuestion;
        }

        /// <summary>
        /// Get next question but do not change the current selected question
        /// </summary>
        /// <returns></returns>
        /// <remarks>If current section is null method returns null</remarks>
        public ScreeningSectionQuestion PickNextQuestion()
        {
            ScreeningSectionQuestion question;
            EnsureScreeningQuestionsInitialized();
            lock (_screeningLock)
            {
                if (CurrentState.Section == null)
                {
                    return null;
                }

                if (CurrentState.SectionQuestion == null)
                {
                    question = GetFirstSectionQuestion(CurrentState.Section.ScreeningSectionID);
                }

                else
                {
                    question = GetNextSectionQuestion(CurrentState.Section.ScreeningSectionID, CurrentState.SectionQuestion.QuestionID);
                }
            }
            return question;
        }


        #endregion


    }
}
