using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using FrontDesk.Kiosk.Controllers;

namespace FrontDesk.Kiosk.Screens
{
    /// <summary>
    /// Interaction logic for Welcome.xaml
    /// </summary>
    public partial class ScreeningSectionPanel : UserControl, IVisualScreen, IVisualSectionScreen
    {
        private ScreenComponentHelper _helper;

        protected Dictionary<int, string> _answerButtonStyleMapping;
        protected const string _defaultButtonStyle = "ButtonYesNoStyle";

        //copyright strings
        protected Dictionary<string, string> _sectionCopyrights;

        public ScreeningSectionPanel()
        {
            InitializeComponent();
            _helper = new ScreenComponentHelper(this);


            //map answer type to button styles
            _answerButtonStyleMapping = new Dictionary<int, string>();
            _answerButtonStyleMapping.Add(1, "ButtonYesNoStyle");
            _answerButtonStyleMapping.Add(2, "DepressionButtonStyle");
            _answerButtonStyleMapping.Add(3, "DepressionButtonStyle");
            _answerButtonStyleMapping.Add(4, "ViolenceButtonStyle");
            _answerButtonStyleMapping.Add(5, "DrugOfChoiceButtonStyle");
            _answerButtonStyleMapping.Add(6, "DrugOfChoiceButtonStyle");


            _sectionCopyrights = new Dictionary<string, string>(6);
            _sectionCopyrights.Add("SIH", string.Empty);
            _sectionCopyrights.Add("TCC", string.Empty);
            _sectionCopyrights.Add("CAGE", FrontDesk.Kiosk.Resources.Copyrights.CAGE);
            _sectionCopyrights.Add("DAST", FrontDesk.Kiosk.Resources.Copyrights.DAST10);
            _sectionCopyrights.Add("PHQ-9", FrontDesk.Kiosk.Resources.Copyrights.PHQ9);
            _sectionCopyrights.Add("HITS", FrontDesk.Kiosk.Resources.Copyrights.HITS);
            _sectionCopyrights.Add("GAD-7", FrontDesk.Kiosk.Resources.Copyrights.GAD7);
            _sectionCopyrights.Add("BBGS", FrontDesk.Kiosk.Resources.Copyrights.BBGS);


        }

        #region IVisualScreen Members
        /// <summary>
        /// User selected the answer
        /// </summary>
        public event EventHandler<NextScreenClickedEventArg> NextScreenClicked;

        private ScreeningSection _screenSection = null;
        /// <summary>
        /// Bound Section Yes/No question
        /// </summary>
        public ScreeningSection ScreenSection
        {
            get
            {
                return _screenSection;
            }
            set
            {
                if (_screenSection != value) //if data changed
                {
                    _screenSection = value;
                    if (_isInitializing)
                    {
                        _isDataChanged = true;
                    }
                    else
                    {
                        UpdateUI(); //force update UI
                    }
                }
            }
        }

        private ScreeningSectionQuestion _screenSectionQuestion = null;
        /// <summary>
        /// Bound Section Question with answer options
        /// </summary>
        public ScreeningSectionQuestion ScreenSectionQuestion
        {
            get
            {
                return _screenSectionQuestion;
            }
            set
            {

                if (_screenSectionQuestion != value) //if data changed
                {
                    _screenSectionQuestion = value;
                    if (_isInitializing)
                    {
                        _isDataChanged = true;
                    }
                    else
                    {
                        UpdateUI(); //force update UI
                    }
                }
            }
        }

        public ScreeningResult UserSessionResult { get; set; }

        bool _isInitializing = false;
        bool _isDataChanged = false;

        #region ISupportInitialize Members

        /// <summary>
        /// Begin initialization
        /// </summary>
        public void BeginDataBinding()
        {
            _isInitializing = true;
        }
        /// <summary>
        /// update UI if data have been changed during initialization
        /// </summary>
        public void EndDataBinding()
        {
            if (_isDataChanged)
            {
                UpdateUI();
            }
            _isDataChanged = false;
            _isInitializing = false;

        }

        #endregion


        #endregion

        /// <summary>
        /// Update UI
        /// </summary>
        protected void UpdateUI()
        {

            // DrugOfChoice
            if (_screenSection != null && _screenSection.ScreeningSectionID == ScreeningSectionDescriptor.DrugOfChoice)
            {
                txtPleaseChooseOnlyOnly.Text = " (Please Choose Only One)";
                pnlMultiLineAnswerOptions.Visibility = Visibility.Visible;
                pnlQuestionWrapper.Orientation = Orientation.Horizontal; //this render the text in single line
                pnlAnswerOptions.Visibility = Visibility.Collapsed;
                grdRootGrid.Margin = new Thickness(0, 100, 0, 210);
                grdOptionsInRow.Margin = new Thickness(0, 0, 0, 10);
                brdQuestionText.Style = (Style)this.FindResource("question_space_drugs");
            }
            else
            {
                txtPleaseChooseOnlyOnly.Text = String.Empty;
                pnlMultiLineAnswerOptions.Visibility = Visibility.Collapsed;
                pnlQuestionWrapper.Orientation = Orientation.Vertical; //this activates question text wrapping
                pnlAnswerOptions.Visibility = Visibility.Visible;
                grdRootGrid.Margin = new Thickness(0, 250, 0, 210);
                grdOptionsInRow.Margin = new Thickness(0, 0, 0, 45);
                brdQuestionText.Style = (Style)this.FindResource("question_space");
            }




            if (_screenSectionQuestion != null)
            {
                _helper.FormatTextAndSetToUI(txtPreambulaText, _screenSectionQuestion.PreambleText);
                _helper.FormatTextAndSetToUI(txtQuestionText, _screenSectionQuestion.QuestionText);

                if (_screenSectionQuestion.IsMainQuestion &&
                        (_screenSectionQuestion.ScreeningSectionID == ScreeningSectionDescriptor.Alcohol
                        || _screenSectionQuestion.ScreeningSectionID == ScreeningSectionDescriptor.PartnerViolence))
                {
                    ucBackgound.CopyrightText = string.Empty;
                }
                else
                {
                    ucBackgound.CopyrightText = GetCopyrightStringForSection(_screenSectionQuestion.ScreeningSectionID);
                }
                if ((_screenSectionQuestion.ScreeningSectionID == ScreeningSectionDescriptor.Depression && _screenSectionQuestion.QuestionID != 10)
                        || _screenSectionQuestion.ScreeningSectionID == ScreeningSectionDescriptor.PartnerViolence
                        || _screenSectionQuestion.ScreeningSectionID == ScreeningSectionDescriptor.SubstanceAbuse
                        )
                {
                    txtQuestionText.FontWeight = FontWeights.SemiBold;
                }
                else
                {
                    txtQuestionText.FontWeight = FontWeights.Normal;
                }
                CreateQuestionAnswerOptions(_screenSectionQuestion.AnswerScaleID);
            }
            else if (_screenSection != null)
            {
                txtPreambulaText.Text = string.Empty;

                if (_screenSection.ScreeningSectionID == "DAST") //show copyright
                {
                    ucBackgound.CopyrightText = GetCopyrightStringForSection(_screenSection.ScreeningSectionID);
                }

                CreateQuestionAnswerOptions(AnswerScale.DefaultYesNoScaleID);
            }
            else
            {
                txtPreambulaText.Text = string.Empty;
                txtQuestionText.Text = string.Empty;
                CreateQuestionAnswerOptions(-1); //clear buttons
            }

            if (string.IsNullOrEmpty(txtPreambulaText.Text) && !(_screenSectionQuestion != null && _screenSectionQuestion.QuestionID == 10))
            {
                brdPreambulaText.MinHeight = 0;
            }
            else
            {
                brdPreambulaText.MinHeight = 120;
            }

        }

        private Panel GetActiveAnswerPanel()
        {
            if (pnlAnswerOptions.Visibility == Visibility.Visible)
            {
                return pnlAnswerOptions;
            }
            else
            {
                return pnlMultiLineAnswerOptions;
            }
        }


        /// <summary>
        /// Create buttons for question answers and bind event to them
        /// </summary>
        /// <param name="answerScaleID"></param>
        protected void CreateQuestionAnswerOptions(int answerScaleID)
        {
            //turn on Kiosk data mode
            AnswerScaleCacheManager.Instance.UseLocalDatabaseConnection = true;
            var options = AnswerScaleCacheManager.Instance.GetAnswerOptions(answerScaleID);


            var ctrlAnswers = GetActiveAnswerPanel();

            //get previous question for multi-step multi-option options

            var previousAnswers = new List<ScreeningSectionQuestionResult>();

            if (ScreenSection.ScreeningSectionID == ScreeningSectionDescriptor.DrugOfChoice)
            {
                previousAnswers = UserSessionResult.FindSectionByID(ScreenSection.ScreeningSectionID)?.Answers ?? new List<ScreeningSectionQuestionResult>();
            }

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
                if(ScreenSection.ScreeningSectionID == ScreeningSectionDescriptor.DrugOfChoice && previousAnswers.Count == 0)
                {
                    //this is the first question, so we do not display "None" option.
                    //Need to reserve a space for it
                    var btn = CreateAnswerOptionButton(answerScaleID, new AnswerScaleOption { }, true);
                    btn.Opacity = 0;
                    ctrlAnswers.Children.Add(btn);
                }

                foreach (var option in options)
                {

                    //disable option if previosly selected
                    bool isDisabled = option.Value > 0 &&
                        previousAnswers.Exists(x => option.Value == x.AnswerValue);

                    Button btn = CreateAnswerOptionButton(answerScaleID, option, isDisabled);

                    ctrlAnswers.Children.Add(btn);
                }
            }

            ctrlAnswers.EndInit();

        }

        private Button CreateAnswerOptionButton(int answerScaleID, AnswerScaleOption option, bool isDisabled)
        {
            //create button and add it to the stack panel
            Button btn = new Button();
            btn.Content = option.Text;
            btn.CommandParameter = option;

            if (!isDisabled)
            {
                btn.Click += OnAnswerButtonClick;
            }

            //apply styles depending on answer scale
            string style = string.Empty;
            if (!_answerButtonStyleMapping.TryGetValue(answerScaleID, out style))
            {
                style = _defaultButtonStyle;
            }
            //btn.Style = (Style)this.Resources[style]; 
            btn.Style = (Style)Application.Current.FindResource(style);
            btn.IsEnabled = !isDisabled;
            return btn;
        }

        /// <summary>
        /// Get copyright string for section
        /// </summary>
        /// <param name="sectionID"></param>
        /// <returns></returns>
        protected string GetCopyrightStringForSection(string sectionID)
        {
            string copyright;
            _sectionCopyrights.TryGetValue(sectionID, out copyright);
            return copyright;
        }

        protected virtual void OnNextScreenClicked(IVisualScreen screen, NextScreenClickedEventArg e)
        {
            if (NextScreenClicked != null)
            {
                NextScreenClicked(screen, e);
            }
        }

        private object _answerLockObject = new object();
        private AnswerScaleOption _selectedAnswer = null;

        private void OnAnswerButtonClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn != null)
            {
                //play sound
                SoundController.Instance.PlayButtonSound();

                lock (_answerLockObject)
                {
                    this._selectedAnswer = (AnswerScaleOption)btn.CommandParameter;
                    btnNext.IsEnabled = true;

                    btn.IsDefault = true;
                    var panel = GetActiveAnswerPanel();
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
                if (this._selectedAnswer != null)
                {
                    OnNextScreenClicked(this, new NextScreenClickedEventArg(this, _selectedAnswer));

                }
            }

        }


        #region IVisualScreen Members

        /// <summary>
        /// Get default button for UI testing
        /// </summary>
        /// <returns></returns>
        public Button GetDefaultButtonForTesting()
        {

            var ctrlAnswers = GetActiveAnswerPanel();

            if (ctrlAnswers.Children.Count > 0)
            {
                var rand = new Random();
                int index = rand.Next(ctrlAnswers.Children.Count);
                return ctrlAnswers.Children[index] as Button;
            }
            else
                throw new InvalidOperationException("There is not Answer options on the screen");
        }

        #endregion





        public void Reset()
        {
            _screenSectionQuestion = null;
            _screenSection = null;
            ucBackgound.CopyrightText = string.Empty;

            this.Visibility = Visibility.Collapsed;
            lock (_answerLockObject)
            {
                this._selectedAnswer = null;
                this.btnNext.IsEnabled = false;
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


    }
}
