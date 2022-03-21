using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Threading;

using Common.Logging;

using FrontDesk.Common.Debugging;
using FrontDesk.Common.Screening;
using FrontDesk.Configuration;
using FrontDesk.Kiosk.Controllers;
using FrontDesk.Kiosk.Screens;
using FrontDesk.Kiosk.Services;
using FrontDesk.Services;

namespace FrontDesk.Kiosk.Workflow
{
    public class VisualScreenController
    {
        private readonly ScreeningController _screeningWfController;

        private readonly IUserSessionTimeoutController _userSessionTimeoutController;

        private readonly IVisualScreenFactory _visualScreenFactory;

        private readonly IScreeningSectionAgeService _screeningSectionAgeService;

        private readonly IScreeningResultState _resultState;

        private readonly SectionQuestionsVisualScreenController _questionController = new SectionQuestionsVisualScreenController();

        private readonly ILog _logger = LogManager.GetLogger<VisualScreenController>();

        private readonly IPatientNameValidationService _patientNameValidationService;


        private bool IsPositiveResult { get; set; }

        #region Constructors

        public VisualScreenController(
                ScreeningController screeningWfController,
                IUserSessionTimeoutController userSessionTimeoutController,
                IVisualScreenFactory visualScreenFactory,
                IScreeningSectionAgeService screeningSectionAgeService,
                IScreeningResultState resultState,
                IPatientNameValidationService patientNameValidationService)
        {
            _screeningWfController = screeningWfController ?? throw new ArgumentNullException(nameof(screeningWfController));
            _userSessionTimeoutController = userSessionTimeoutController ?? throw new ArgumentNullException(nameof(userSessionTimeoutController));
            _visualScreenFactory = visualScreenFactory ?? throw new ArgumentNullException(nameof(visualScreenFactory));
            _screeningSectionAgeService = screeningSectionAgeService ?? throw new ArgumentNullException(nameof(screeningSectionAgeService));
            _resultState = resultState ?? throw new ArgumentNullException(nameof(resultState));
            _patientNameValidationService = patientNameValidationService ?? throw new ArgumentNullException(nameof(patientNameValidationService));
            
            _logger.Debug("Begin WF Step initialization.");
            InitWfSteps();
            _logger.Debug("End WF Step initialization.");
        }

        #endregion


        #region UI components
        /// <summary>
        /// Screen controls container
        /// </summary>
        Grid _screensContainer;


        public readonly ScreenWorkflowState CurrentState = new ScreenWorkflowState();
        public readonly ScreenWorkflowState NextState = new ScreenWorkflowState();


        private void InitWfSteps()
        {
            WorkflowSteps = new List<ScreeningStep>();
            WorkflowSteps.Add(ScreeningStep.Home);
            WorkflowSteps.Add(ScreeningStep.Welcome);

            WorkflowSteps.Add(ScreeningStep.PatientFirstName);
            WorkflowSteps.Add(ScreeningStep.PatientMiddleName);
            WorkflowSteps.Add(ScreeningStep.PatientLastName);
            WorkflowSteps.Add(ScreeningStep.PatientDateOfBirth);
            WorkflowSteps.Add(ScreeningStep.PatientStreet);
            WorkflowSteps.Add(ScreeningStep.PatientCity);
            WorkflowSteps.Add(ScreeningStep.PatientState);
            WorkflowSteps.Add(ScreeningStep.PatientZipCode);
            WorkflowSteps.Add(ScreeningStep.PatientPrimaryPhone);
            WorkflowSteps.Add(ScreeningStep.ScreeningSection);
            WorkflowSteps.Add(ScreeningStep.SendResult);

            WorkflowSteps.Add(ScreeningStep.DemographicsMessage);
            WorkflowSteps.Add(ScreeningStep.DemographicsGender);
            WorkflowSteps.Add(ScreeningStep.DemographicsRace);
            WorkflowSteps.Add(ScreeningStep.DemographicsTribe);
            WorkflowSteps.Add(ScreeningStep.DemographicsOnOffReservation);
            WorkflowSteps.Add(ScreeningStep.DemographicsCountyName);
            WorkflowSteps.Add(ScreeningStep.DemographicsCountyState);
            WorkflowSteps.Add(ScreeningStep.DemographicsEducationLevel);
            WorkflowSteps.Add(ScreeningStep.DemographicsMaritalStatus);
            WorkflowSteps.Add(ScreeningStep.DemographicsSexualOrientation);
            WorkflowSteps.Add(ScreeningStep.DemographicsMilitaryExperience);
            WorkflowSteps.Add(ScreeningStep.DemographicsMilitaryCombat);
            WorkflowSteps.Add(ScreeningStep.DemographicsSendResult);

            WorkflowSteps.Add(ScreeningStep.ThankYou);
        }

        /// <summary>
        /// Initialize UI components and start Workflow
        /// </summary>
        /// <param name="screensContainer"></param>
        public void Initialize(Grid screensContainer, IScreenControl outOfServiceNotification, IScreenControl outOfServiceFullScreen, IScreenControl ucSessionExpiringSoon)
        {
            _screensContainer = screensContainer;

            _logger.Debug("WF Initialization is started.");

            _logger.Debug("Screening Question initialization is starting...");
            _screeningWfController.InitializeScreeningQuestions();
            _logger.Debug("Screening Question initialization completed.");

            Restart();

            _logger.Debug("WF State has been restarted.");

            _ucSessionExpireSoonScreen = ucSessionExpiringSoon;
            //subscribe to user's session expired event
            _userSessionTimeoutController.UserSessionExpired += new EventHandler(OnUserSessionExpired);
            _userSessionTimeoutController.UserSessionExpiring += new EventHandler(OnUserSessionExpiring);
            _ucOutOfServiceNotification = outOfServiceNotification;
            _ucOutOfServiceFullScreen = outOfServiceFullScreen;

            //subscriber to Out of service events
            OutOfServiceController.Instance.ServerConnectionLost += new EventHandler(OnServerConnectionLost);
            OutOfServiceController.Instance.ServerConnectionEstablished += new EventHandler(OnServerConnectionEstablished);

            _logger.Debug("WF Initialization completed.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public bool IsLastWfStep(ScreeningStep step)
        {
            var index = WorkflowSteps.IndexOf(step);
            return index == WorkflowSteps.Count - 1;
        }


        /// <summary>
        /// Get next step for workflow
        /// </summary>
        /// <returns></returns>
        public ScreeningStep GetNextWfStep(ScreeningStep step)
        {
            var index = WorkflowSteps.IndexOf(step) + 1;
            if (index >= WorkflowSteps.Count) index = 0;

            return WorkflowSteps[index];
        }

        /// <summary>
        /// Worflow steps
        /// </summary>
        public List<ScreeningStep> WorkflowSteps;
        /// <summary>
        /// Visual screen controls cache by step type.
        /// </summary>
        /// <remarks>Because of ScreeningSection can be next and current, we need to keep 2 controls for this step</remarks>
        private Dictionary<ScreeningStep, List<IVisualScreen>> _visualScreenCache = new Dictionary<ScreeningStep, List<IVisualScreen>>();

        /// <summary>
        /// Create a screen for a step
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        protected IVisualScreen CreateScreenForStep(ScreeningStep step)
        {
            IVisualScreen screen = _visualScreenFactory.CreateScreenForStep(step, _screeningWfController.CurrentState.Section, _resultState);
            screen.NextScreenClicked += new EventHandler<NextScreenClickedEventArg>(screen_nextScreenClicked);
            return screen;
        }

        /// <summary>
        /// Create or get from cache screen control for CurrentState.Step value. Screen control is assigned to the CurrentState.Screen field.
        /// </summary>
        /// <returns>New NextState.Screen value</returns>
        protected IVisualScreen GetScreenForCurrentStep()
        {
            List<IVisualScreen> screens;

            _visualScreenCache.TryGetValue(CurrentState.Step, out screens);
            if (screens != null)
            {
                if (CurrentState.Step == ScreeningStep.ScreeningSection)
                {
                    if (screens.Count == 2)
                    {
                        CurrentState.Screen = screens[0] == CurrentState.Screen ? screens[1] : screens[0];
                    }
                    else
                    {
                        CurrentState.Screen = screens[0];
                    }
                }
                else
                {
                    CurrentState.Screen = screens[0];
                }
                CurrentState.Screen.Reset();
            }
            else
            {
                screens = new List<IVisualScreen>(1);
                CurrentState.Screen = CreateScreenForStep(CurrentState.Step);

                screens.Add(CurrentState.Screen);
                _visualScreenCache.Add(CurrentState.Step, screens);
            }

            return CurrentState.Screen;
        }

        /// <summary>
        /// Create or get from cache screen control for NextState.Step value. Screen control is assigned to the NextState.Screen field.
        /// </summary>
        /// <returns>New NextState.Screen value</returns>
        protected IVisualScreen GetScreenForNextStep()
        {
            List<IVisualScreen> screens;
            _visualScreenCache.TryGetValue(NextState.Step, out screens);


            if (screens == null)
            {
                screens = new List<IVisualScreen>(1);
                NextState.Screen = CreateScreenForStep(NextState.Step);

                screens.Add(NextState.Screen);
                _visualScreenCache.Add(NextState.Step, screens);
            }
            else
            {

                if (NextState.Step != ScreeningStep.ScreeningSection)
                {

                    NextState.Screen = screens[0];
                    NextState.Screen.Reset();

                }
                else
                {
                    //create screen for Section Panel
                    //if this is first step we can create or use only signle instance
                    _visualScreenCache.TryGetValue(NextState.Step, out screens);
                    Debug.Assert(screens != null, "screens != null");
                    if (screens.Count < 2)
                    {

                        NextState.Screen = CreateScreenForStep(NextState.Step);
                        screens.Add(NextState.Screen);
                    }
                    else  //if we have 2 screens in the cache - take second
                    {
                        NextState.Screen = screens[0] == CurrentState.Screen ? screens[1] : screens[0];
                    }
                }

            }

            return NextState.Screen;
        }

        /// <summary>
        /// This method need to be executed in Background thread
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNextScreenClickedHandler(object sender, NextScreenClickedEventArg e)
        {
            if (CurrentState.Step != ScreeningStep.SendResult && CurrentState.Step != ScreeningStep.ThankYou)
            {
                _screensContainer.Dispatcher.BeginInvoke((ThreadStart)delegate
                {
                    //play button click sound
                    SoundController.Instance.PlayButtonSound();
                });
            }

            OnNextScreenClicked(e);

            _screensContainer.Dispatcher.BeginInvoke((ThreadStart)delegate
            {
                //#if DEBUG
                //                UIElement visibleElement = null;
                //                foreach (UIElement item in _screensContainer.Children)
                //                {
                //                    if (item.Visibility == Visibility.Visible)
                //                    {
                //                        visibleElement = item;
                //                        break;
                //                    }
                //                }
                //                Debug.Assert(visibleElement != null, "At least one screen should be visible");
                //#endif
            }, DispatcherPriority.Background);
        }
        /// <summary>
        /// Collect data from screen and validate it before moving to the next step
        /// </summary>
        void screen_nextScreenClicked(object sender, NextScreenClickedEventArg e)
        {
            Task.Run(delegate
                {
                    OnNextScreenClickedHandler(sender, e);
                }
            );
        }

        public void OnNextScreenClicked(NextScreenClickedEventArg e)
        {
            bool forceSkipSectionQuestion = false;  // for section screening if patient answered 'No' on section's question 
                                                    //we need to skip this section's screening and go to the next sction
            bool isValid = true;   //Whether step's input is valid or not. If false we need to show validation error message

            // collect data
            if (CurrentState.Step != ScreeningStep.ScreeningSection)
            {
                if (CurrentState.Step == ScreeningStep.SendResult)
                {
                    var isPositiveScreeningResult = (bool?)(e.Value);
                    if (!isPositiveScreeningResult.HasValue) //results not saved / failed - need to repeat check-in
                    {
                        //restart wf
                        Restart();
                        return;
                    }
                    //save results
                    IsPositiveResult = isPositiveScreeningResult.Value;

                    //Copy Screening data to Demographics

                    _resultState.Demographics.Init(_resultState.Result);

                    //save Is Positive result for the last step
                    _resultState.IsPositiveScoreScreening = IsPositiveResult;

                    _resultState.ResetScreeningResult();
                }
                else if (CurrentState.Step == ScreeningStep.DemographicsSendResult)
                {
                    var succceed = (bool)(e.Value);
                    if (!succceed) //results not saved - need to repeat check-in
                    {
                        //restart wf
                        Restart();
                        return;
                    }

                    IsPositiveResult = _resultState.IsPositiveScoreScreening ?? false;

                    _resultState.ResetResult();
                }
                //collect data for patient information screens
                try
                {
                    if (CurrentState.Step == ScreeningStep.Home)
                    {
                        //start time measuring
                        _resultState.ScreeningTimeLog.StartPatientScreeningRecording();
                    }
                    else if (CurrentState.Step == ScreeningStep.PatientFirstName)
                    {
                        _resultState.Result.FirstName = Convert.ToString(e.Value);
                    }
                    else if (CurrentState.Step == ScreeningStep.PatientMiddleName)
                    {
                        _resultState.Result.MiddleName = Convert.ToString(e.Value);
                    }
                    else if (CurrentState.Step == ScreeningStep.PatientLastName)
                    {
                        _resultState.Result.LastName = Convert.ToString(e.Value);
                    }
                    else if (CurrentState.Step == ScreeningStep.PatientDateOfBirth)
                    {
                        _resultState.Result.Birthday = Convert.ToDateTime(e.Value);

                        //we've collected all patient screening identity object fields, let's load patient's screening statistics from server
                        _screeningWfController.FrequencySpecification.LoadPatientScreeningsStatistics();

                        //start tracking CIF section screening time
                        _resultState.ScreeningTimeLog.StartSectionTimeRecording(ScreeningSectionDescriptor.ContactInfo);
                    }
                    else if (CurrentState.Step == ScreeningStep.PatientStreet)
                    {
                        _resultState.Result.StreetAddress = Convert.ToString(e.Value);
                    }
                    else if (CurrentState.Step == ScreeningStep.PatientState)
                    {
                        _resultState.Result.StateID = Convert.ToString(e.Value);
                    }
                    else if (CurrentState.Step == ScreeningStep.PatientCity)
                    {
                        _resultState.Result.City = Convert.ToString(e.Value);
                    }
                    else if (CurrentState.Step == ScreeningStep.PatientZipCode)
                    {
                        _resultState.Result.ZipCode = Convert.ToString(e.Value);
                    }
                    else if (CurrentState.Step == ScreeningStep.PatientPrimaryPhone)
                    {
                        _resultState.Result.Phone = Convert.ToString(e.Value);

                        _resultState.ScreeningTimeLog.StopSectionTimeRecording(ScreeningSectionDescriptor.ContactInfo);
                    }

                    else if (CurrentState.Step == ScreeningStep.DemographicsMessage)
                    {
                        //start tracking Demographics section screening time
                        _resultState.ScreeningTimeLog.StartSectionTimeRecording(ScreeningSectionDescriptor.Demographics);
                    }
                    else if (CurrentState.Step == ScreeningStep.DemographicsGender)
                    {
                        _resultState.Demographics.GenderId = (int)e.Value;
                    }
                    else if (CurrentState.Step == ScreeningStep.DemographicsRace)
                    {
                        _resultState.Demographics.RaceId = Convert.ToInt32(e.Value);
                    }
                    else if (CurrentState.Step == ScreeningStep.DemographicsTribe)
                    {
                        _resultState.Demographics.TribalAffiliation = Convert.ToString(e.Value);
                    }

                    else if (CurrentState.Step == ScreeningStep.DemographicsOnOffReservation)
                    {
                        _resultState.Demographics.LivingOnReservationId = Convert.ToInt32(e.Value);
                    }
                    else if (CurrentState.Step == ScreeningStep.DemographicsCountyName)
                    {
                        _resultState.Demographics.CountyNameOfResidence = Convert.ToString(e.Value);
                    }
                    else if (CurrentState.Step == ScreeningStep.DemographicsCountyState)
                    {
                        _resultState.Demographics.CountyStateOfResidence = Convert.ToString(e.Value);
                    }
                    else if (CurrentState.Step == ScreeningStep.DemographicsEducationLevel)
                    {
                        _resultState.Demographics.EducationLevelId = Convert.ToInt32(e.Value);
                    }
                    else if (CurrentState.Step == ScreeningStep.DemographicsMaritalStatus)
                    {
                        _resultState.Demographics.MaritalStatusId = Convert.ToInt32(e.Value);
                    }
                    else if (CurrentState.Step == ScreeningStep.DemographicsSexualOrientation)
                    {
                        _resultState.Demographics.SexualOrientationId = Convert.ToInt32(e.Value);
                    }
                    else if (CurrentState.Step == ScreeningStep.DemographicsMilitaryExperience)
                    {
                        var val = Convert.ToInt32(e.Value);
                        _resultState.Demographics.MilitaryExperience.Add(val);
                    }
                    else if (CurrentState.Step == ScreeningStep.DemographicsMilitaryCombat)
                    {
                        int val = Convert.ToInt32(e.Value);

                        if (val > 0)
                        {
                            _resultState.Demographics.MilitaryExperience.Add(val);
                        }
                    }

                }
                catch (Exception ex)
                {
                    Debug.Assert(true, "Invalid restuned value from screen.");
                    DebugLogger.TraceException(ex, string.Format("Invalid restuned value from screen. Step: {0}, value: '{1}'.", CurrentState.Step, e.Value));

                    return;
                }
            }
            else //screening section step
            {
                _questionController.ProcessNextScreenClicked(e, _resultState, ref forceSkipSectionQuestion);
            }

            if (!isValid)
            {

            }
            else
            {
                //go to the next step
                GoToNextScreen(forceSkipSectionQuestion);

            }
        }

        /// <summary>
        /// Restart worflow from the first step
        /// </summary>
        private void Restart()
        {

            _resultState.ResetResult();
            IsPositiveResult = false;

            if (_screeningWfController != null)
            {
                _screeningWfController.RestartScreening();
            }

            if (NextState.Screen != null) NextState.Screen.Hide();
            if (CurrentState.Screen != null) CurrentState.Screen.Hide();

            CurrentState.Step = ScreeningStep.Home;
            NextState.Step = GetNextWfStep(CurrentState.Step);

            CurrentState.Screen = null;
            NextState.Screen = null;

            if (_ucSessionExpireSoonScreen != null)
            {
                _ucSessionExpireSoonScreen.Hide();
            }


            InitializeScreensFromStart();
        }
        /// <summary>
        /// Initialize screens layout at start-up
        /// </summary>
        private void InitializeScreensFromStart()
        {
            _screensContainer.BeginInit();
            _screensContainer.Children.Clear();


            var visibleScreen = GetScreenForCurrentStep() as UserControl;
            _screensContainer.Children.Add(visibleScreen);

            var nextHiddenScreen = GetScreenForNextStep() as UserControl;
            nextHiddenScreen.Visibility = Visibility.Collapsed;
            _screensContainer.Children.Add(nextHiddenScreen);

            _screensContainer.EndInit();

            CurrentState.Screen.Show(false);
        }

        #endregion

        #region Workflow
        /// <summary>
        /// show next screen
        /// </summary>
        private void GoToNextScreen(bool forceSkipSectionQuestions)
        {
            bool isNormalWfHasChanged = false; //true when some steps in WF was skipped

            var hidingScreen = CurrentState.Screen; //screen will be removed
            var hidingStep = CurrentState.Step;

            //skip address questions of patient has already passed in with required frequency
            if (CurrentState.Step == ScreeningStep.PatientDateOfBirth)
            {
                OnAfterDateOfBirthQuestion(ref isNormalWfHasChanged);
            }
            else if (CurrentState.Step == ScreeningStep.SendResult)
            {

                //if need to skip Demographics

                ScreeningSectionAge ageSetting = _screeningSectionAgeService.GetMinimalAgeForScreeningSection(ScreeningSectionDescriptor.Demographics);

                if (!ageSetting.IsEnabled
                        || _resultState.Demographics.Age < ageSetting.MinimalAge
                        || _screeningWfController.FrequencySpecification.IsSkipRequiredForSection(ScreeningSectionDescriptor.Demographics)

                        )
                {

                    NextState.Step = ScreeningStep.ThankYou;
                    isNormalWfHasChanged = true;
                }
            }
            else if (CurrentState.Step == ScreeningStep.DemographicsMilitaryExperience)
            {
                var militaryExperience = _resultState.Demographics.MilitaryExperience;

                if (militaryExperience.Contains(1)) /* None */
                {
                    NextState.Step = ScreeningStep.DemographicsSendResult;
                    isNormalWfHasChanged = true;
                }

            }
            else if (CurrentState.Step == ScreeningStep.DemographicsRace)
            {

                //if need to skip Tribal Affilliation of not Native

                if (!DemographicsRaceDiscriptor.IsNativeRace(_resultState.Demographics.RaceId))
                {

                    NextState.Step = GetNextWfStep(ScreeningStep.DemographicsTribe);
                    isNormalWfHasChanged = true;
                }
            }

            if(_screensContainer == null) // unit test
            {
                GoToNextScreenStateChange(forceSkipSectionQuestions, isNormalWfHasChanged, hidingScreen, hidingStep);
                return;
            }
            

            // run UI update code into UI thread
            _screensContainer.Dispatcher.BeginInvoke((ThreadStart)delegate
            {
                GoToNextScreenStateChange(forceSkipSectionQuestions, isNormalWfHasChanged, hidingScreen, hidingStep);
            });
        }

        private void GoToNextScreenStateChange(
            bool forceSkipSectionQuestions, 
            bool isNormalWfHasChanged, 
            IVisualScreen hidingScreen, 
            ScreeningStep hidingStep)
        {
            CurrentState.Step = NextState.Step;
            CurrentState.Screen = GetScreenForCurrentStep();


            NextState.Step = GetNextWfStep(CurrentState.Step); //show next WF screen


            if (CurrentState.Step == ScreeningStep.ScreeningSection)
            {
                if (ProccessScreeningSectionsWorkflow(forceSkipSectionQuestions)) //if we do not complete Screening Section Workflow, do not change WF step for next screen
                {
                    NextState.Step = CurrentState.Step;
                }
                else
                {
                    //exit from section screening workflow
                    CurrentState.Step = GetNextWfStep(CurrentState.Step);
                    GetScreenForCurrentStep();
                    NextState.Step = GetNextWfStep(CurrentState.Step);
                }

                var sectionScreen = CurrentState.Screen as IVisualSectionScreen;
                if (sectionScreen != null)
                {
                    BindSectionScreen(sectionScreen);
                }
            }
            else if (CurrentState.Step == ScreeningStep.ThankYou)
            {
                //this screen is Thank You after we completed section screening.

                var thankYouScreen = CurrentState.Screen as IVisualThankYouScreen;
                Debug.Assert(thankYouScreen != null, "thankYouScreen != null");
                thankYouScreen.IsPositiveResult = this.IsPositiveResult;
            }



            GetScreenForNextStep();
            NextState.Screen.Hide();
            var nextHiddenScreen = NextState.Screen as UserControl;

            if (isNormalWfHasChanged ||
                    (hidingStep == ScreeningStep.ScreeningSection && CurrentState.Step != ScreeningStep.ScreeningSection)
            )
            {

                ReplaceNextScreenInScreenContainer(CurrentState.Screen);
            }


            //if not unit test
            if (_screensContainer != null)
            {
                _screensContainer.BeginInit();

                //hide current and show next
                hidingScreen.Hide();
                //show current
                CurrentState.Screen.Show(true);
                _screensContainer.EndInit();
                _screensContainer.UpdateLayout();

                //then create new control , add it to the children collection and remove hidden step
                _screensContainer.BeginInit();

                //animate
                _screensContainer.Children.Remove(hidingScreen as UserControl);
                if (!_screensContainer.Children.Contains(nextHiddenScreen))
                {
                    Debug.Assert(nextHiddenScreen != null, "nextHiddenScreen != null");
                    _screensContainer.Children.Add(nextHiddenScreen);
                }
                _screensContainer.EndInit();
            }
        }

        /// <summary>
        /// Handles logic after user completed Date of Birth question
        /// </summary>
        /// <param name="isNormalWfHasChanged">True when normal workflow has been modified.</param>
        private void OnAfterDateOfBirthQuestion(ref bool isNormalWfHasChanged)
        {
            // step 1: check the patient name is valid for the export to EHR

            ValidatePatientName();

            if (
                !_resultState.PatientNameValidatedOnServer.Value // patient name validation has failed
                && !_resultState.IsRepeatingPatientNameAfterValidationFailed // only when the first pass
                )
            {
                /* 
                 * when validation has failed first time, return workflow back to patient name screens for repeat entry
                during the second pass after initial validation has failed continue screening even when validation has failed again.
                */
                _resultState.IsRepeatingPatientNameAfterValidationFailed = true; // starts the second pass

                NextState.Step = ScreeningStep.PatientFirstName;
                isNormalWfHasChanged = true;

                return; // break
            }

            // step 2: check the patient name is valid for the export to EHR
            ScreeningSectionAge ageSetting = _screeningSectionAgeService.GetMinimalAgeForScreeningSection(ScreeningFrequencyDescriptor.ContactFrequencyID);

            if (!ageSetting.IsEnabled
                    || _resultState.Result.Age < ageSetting.MinimalAge
                    || _screeningWfController.FrequencySpecification.IsSkipRequiredForSection(ScreeningFrequencyDescriptor.ContactFrequencyID)

                    )
            {

                NextState.Step = ScreeningStep.ScreeningSection;
                isNormalWfHasChanged = true;
            }
        }

        /// <summary>
        /// Bind data to session screening UI screen
        /// </summary>
        /// <param name="screen"></param>
        private void BindSectionScreen(IVisualSectionScreen screen)
        {
            screen.UserSessionResult = _resultState.Result;

            screen.BeginDataBinding();
            screen.ScreenSection = _screeningWfController.CurrentState.Section;
            screen.ScreenSectionQuestion = _screeningWfController.CurrentState.SectionQuestion;

            DebugLogger.WriteTraceMessage(string.Format("Binding question to the current screen. Section {0}. Question: {1}", screen.ScreenSection.ScreeningSectionName, screen.ScreenSectionQuestion.QuestionText));
            screen.EndDataBinding();

        }

        /// <summary>
        /// Replace next screen in the Screens Container Children collection with a new control.
        /// </summary>
        /// <param name="nextScreenReplaceWith">Changed next visible screen</param>
        /// <remarks>Call this method when normal workflow has been changed and Next screen has been updated</remarks>
        private void ReplaceNextScreenInScreenContainer(IVisualScreen nextScreenReplaceWith)
        {
            //if not unit test
            if (_screensContainer != null)
            {
                var nextScreen = nextScreenReplaceWith as UserControl;
                _screensContainer.BeginInit();
                //remove 2nd screen and add this screen
                Debug.Assert(_screensContainer.Children.Count == 2);
                _screensContainer.Children.RemoveAt(1);
                if (!_screensContainer.Children.Contains(nextScreen))
                    _screensContainer.Children.Add(nextScreen);
                _screensContainer.EndInit();
            }

        }

        #endregion


        #region Screening Section Workflow

        /// <summary>
        /// Change current Screening WF section or question
        /// </summary>
        /// <param name="forceNextSection"></param>
        /// <returns>false if  wf step changed (either go to next section or to a new question</returns>
        private bool ProccessScreeningSectionsWorkflow(bool forceNextSection)
        {

            if (_screeningWfController.CurrentState.Section == null)
            {
                _screeningWfController.GoToNextSection(); //enter to first section
            }
            else if (forceNextSection) //user click on "No" for the section. We need to miss section's question and show next section
            {
                _screeningWfController.GoToNextSection();
            }
            else //go through wf
            {
                var nextQuestion = _screeningWfController.GoToNextQuestion();
                if (nextQuestion == null)
                {
                    _screeningWfController.GoToNextSection();
                }

            }

            return _screeningWfController.CurrentState.Section != null;
        }


        #endregion

        #region Automated Test Helpers

        public void SimulateUserClick(Button defaultButton)
        {
            ButtonAutomationPeer peer =
                            new ButtonAutomationPeer(defaultButton);

            IInvokeProvider invokeProv =
                peer.GetPattern(PatternInterface.Invoke)
                as IInvokeProvider;

            invokeProv.Invoke();

        }

        #endregion

        #region Server Connection monitoring

        IScreenControl _ucOutOfServiceNotification;
        IScreenControl _ucOutOfServiceFullScreen;
        object _outOfServiceSyncObj = new object();

        /// <summary>
        /// Handles connection established event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnServerConnectionEstablished(object sender, EventArgs e)
        {
            if (_ucOutOfServiceNotification == null || _ucOutOfServiceFullScreen == null)
                return;

            lock (_outOfServiceSyncObj)
            {
                _screensContainer.Dispatcher.BeginInvoke((ThreadStart)delegate
                {
                    if (_ucOutOfServiceFullScreen.IsVisible)
                    {
                        if (CurrentState.Screen != null) CurrentState.Screen.Show(false);
                        _ucOutOfServiceFullScreen.Hide();
                    }
                    _ucOutOfServiceNotification.Hide();
                }, DispatcherPriority.Normal);
            }
        }


        /// <summary>
        /// Handles Connection Lost event
        /// </summary>
        void OnServerConnectionLost(object sender, EventArgs e)
        {
            if (_ucOutOfServiceNotification == null || _ucOutOfServiceFullScreen == null)
                return;

            lock (_outOfServiceSyncObj)
            {

                DebugLogger.WriteTraceMessage("Showing connection error message.");

                _screensContainer.Dispatcher.BeginInvoke((ThreadStart)delegate
                {
                    if (CurrentState.Step == WorkflowSteps[0]) //we are on 1st Home page
                    {
                        if (!_ucOutOfServiceFullScreen.IsVisible)
                        {
                            //hide notification
                            _ucOutOfServiceNotification.Hide();
                            //full screen is not shown - show it
                            _ucOutOfServiceFullScreen.Show();


                            if (CurrentState.Screen != null) CurrentState.Screen.Hide();

                            //play error sound
                            SoundController.Instance.PlayErrorSound();
                        }
                    }
                    else
                    {
                        if (!_ucOutOfServiceNotification.IsVisible)
                        {
                            //show notification
                            _ucOutOfServiceNotification.Show();
                            _ucOutOfServiceFullScreen.Hide();
                            if (CurrentState.Screen != null) CurrentState.Screen.Show(false);

                            //play error sound
                            SoundController.Instance.PlayErrorSound();
                        }
                    }
                }, DispatcherPriority.Normal);
            }
        }

        #endregion

        #region User Session Timeout notification

        private IScreenControl _ucSessionExpireSoonScreen;

        /// <summary>
        /// Handler user's session expired event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnUserSessionExpired(object sender, EventArgs e)
        {
            if (CurrentState.Step != WorkflowSteps[0])
            {
                _screensContainer.Dispatcher.BeginInvoke((ThreadStart)delegate
                {
                    Restart();
                });
            }
        }

        void OnUserSessionExpiring(object sender, EventArgs e)
        {
            if (CurrentState.Step != WorkflowSteps[0])
            {
                if (_ucSessionExpireSoonScreen != null)
                {
                    _screensContainer.Dispatcher.BeginInvoke((ThreadStart)delegate
                    {
                        //play more time sound
                        SoundController.Instance.PlayMoreTimeSound();

                        _ucSessionExpireSoonScreen.Show();
                    });
                }
            }
        }

        #endregion

        #region Patient Name Validation FLow

        protected bool ValidatePatientName()
        {
            bool existsInEhr = false;
            try
            {
                var validationResult = _patientNameValidationService.Validate(_resultState.Result);

                if (validationResult == null) // patient info not found in EHR database
                {
                    existsInEhr = false;
                }
                else
                {
                    existsInEhr = true;

                    // update patient name using data from EHR
                    _resultState.Result.LastName = validationResult.LastName;
                    _resultState.Result.FirstName = validationResult.FirstName;
                    _resultState.Result.MiddleName = validationResult.MiddleName;
                    _resultState.Result.Birthday = validationResult.DateOfBirth;

                }
            }
            catch (PatientNameValidationException)
            {
                // in case of the connectivity issues, skip this step
                existsInEhr = true;

                _logger.Error("Skiped name validation step.");
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to call Patient Name Validation API. Uknown exception. Skipping name validation step.", ex);

                existsInEhr = true;
            }

            _resultState.PatientNameValidatedOnServer = existsInEhr;

            return existsInEhr;
        }

        #endregion
    }
}
