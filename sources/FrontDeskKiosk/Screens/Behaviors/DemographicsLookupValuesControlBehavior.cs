using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using FrontDesk.Common;
using FrontDesk.Kiosk.Controllers;

namespace FrontDesk.Kiosk.Screens.Behaviors
{
    public class DemographicsLookupValuesControlBehavior
    {
        protected ScreenComponentHelper _helper;

        protected Dictionary<string, string> _answerButtonStyleMapping;

        private IDemographicsLookupVisualScreen _control;

        protected const string _defaultButtonStyle = "ButtonYesNoStyle";

        public List<int> ExcludedItems { get; } = new List<int>();

        public DemographicsLookupValuesControlBehavior(IDemographicsLookupVisualScreen control)
        {
            _control = control;
            _helper = new ScreenComponentHelper(control);


            //map answer type to button styles
            _answerButtonStyleMapping = new Dictionary<string, string>();
            _answerButtonStyleMapping.Add("Gender", "DemographicsGenderButtonStyle");
            _answerButtonStyleMapping.Add("SexualOrientation", "DemographicsGenderButtonStyle");
            _answerButtonStyleMapping.Add("MaritalStatus", "DemographicsGenderButtonStyle");

            _answerButtonStyleMapping.Add("Race", "DemographicsRaceButtonStyle");
            _answerButtonStyleMapping.Add("EducationLevel", "DemographicsRaceButtonStyle");
            _answerButtonStyleMapping.Add("LivingOnReservation", "DemographicsRaceButtonStyle");
            _answerButtonStyleMapping.Add("MilitaryExperience", "DemographicsGenderButtonStyle");
            _answerButtonStyleMapping.Add("MilitaryCombat", "ButtonYesNoStyle");


            Init();
        }

        public void Init()
        {
            _control.NextButton.Click += btnNext_Click;
        }

        /// <summary>
        /// Update UI
        /// </summary>
        public void UpdateUI()
        {

            CreateQuestionAnswerOptions();

        }

        /// <summary>
        /// Create buttons for question answers and bind event to them
        /// </summary>
        protected void CreateQuestionAnswerOptions()
        {

            var options = _control.LookupService.GetAll();


            var ctrlAnswers = _control.AnswerOptionsPanel;

            //get previous question for multi-step multi-option options
            ctrlAnswers.BeginInit();

            if (ctrlAnswers.Children.Count > 0)
            {
                //unsubscribe from events
                foreach (Button button in ctrlAnswers.Children)
                {
                    if (button != null)
                    {
                        button.Click -= OnAnswerButtonClick;
                    }
                }
            }

            ctrlAnswers.Children.Clear();
            if (options != null)
            {
                foreach (var option in options)
                {

                    if (ExcludedItems.Contains(option.Id)) continue;

                    Button btn = CreateAnswerOptionButton(_control.LookupService.ScreenName, option);

                    ctrlAnswers.Children.Add(btn);
                }
            }

            ctrlAnswers.EndInit();

        }

        private Button CreateAnswerOptionButton(string screenName, LookupValue option)
        {
            //create button and add it to the stack panel
            Button btn = new Button();
            btn.Content = option.Name;
            btn.CommandParameter = option;
            btn.Click += OnAnswerButtonClick;


            //apply styles depending on answer scale
            string style = string.Empty;
            if (!_answerButtonStyleMapping.TryGetValue(screenName, out style))
            {
                style = _defaultButtonStyle;
            }
            btn.Style = (Style)Application.Current.FindResource(style);

            return btn;
        }

        private object _answerLockObject = new object();
        private LookupValue _selectedAnswer = null;

        private void OnAnswerButtonClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn != null)
            {
                //play sound
                SoundController.Instance.PlayButtonSound();

                lock (_answerLockObject)
                {
                    _selectedAnswer = (LookupValue)btn.CommandParameter;
                    _control.NextButton.IsEnabled = true;

                    btn.IsDefault = true;
                    var panel = _control.AnswerOptionsPanel;
                    foreach (Button otherButton in panel.Children)
                    {
                        if (otherButton != btn)
                        {
                            otherButton.IsDefault = false;
                        }
                    }
                }
            }
        }

        protected void btnNext_Click(object sender, RoutedEventArgs e)
        {

            lock (_answerLockObject)
            {
                if (_selectedAnswer != null)
                {
                    _control.TriggerNextScreenEvent(_selectedAnswer.Id);

                }
            }

        }


        public void Reset()
        {
            _helper.Hide();
            lock (_answerLockObject)
            {
                _selectedAnswer = null;
                _control.NextButton.IsEnabled = false;
                UpdateUI();
            }

        }

        public void Show(bool withAnimation)
        {
            _helper.Show(withAnimation);
        }

        public void Hide()
        {
            _helper.Hide();
        }

        /// <summary>
        /// Get default button for UI testing
        /// </summary>
        /// <returns></returns>
        public Button GetDefaultButtonForTesting()
        {

            var ctrlAnswers = _control.AnswerOptionsPanel;

            if (ctrlAnswers.Children.Count > 0)
            {
                var rand = new Random();
                int index = rand.Next(ctrlAnswers.Children.Count);
                return ctrlAnswers.Children[index] as Button;
            }
            else
                throw new InvalidOperationException("There is not Answer options on the screen");
        }

    }
}
