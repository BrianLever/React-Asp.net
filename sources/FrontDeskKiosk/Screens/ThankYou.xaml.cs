using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace FrontDesk.Kiosk.Screens
{
	/// <summary>
	/// Interaction logic for Welcome.xaml
	/// </summary>
	public partial class ThankYou : UserControl, IVisualScreen, IVisualThankYouScreen
    {

        private ScreenComponentHelper _helper;
        public bool IsPositiveResult
        {
            get
            {
                return _isPositiveResult;
            }
            set
            {
                if (_isPositiveResult != value) //if data changed
                {
                    _isPositiveResult = value;
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

        private bool _isPositiveResult = false;
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
        public ThankYou()
        {
            InitializeComponent();

            _helper = new ScreenComponentHelper(this);

            
            //init timer
            _timeoutTimer = new DispatcherTimer();
            _timeoutTimer.Interval = TimeSpan.FromSeconds(Settings.AppSettings.ThankYouPageTimeoutInSeconds);
            _timeoutTimer.Tick += new EventHandler(_timeoutTimer_Tick);
            _timeoutTimer.IsEnabled = false;

            UpdateUI();
        }

        protected void UpdateUI()
        {
            txtIsPositive.Visibility = _isPositiveResult ? Visibility.Visible : Visibility.Collapsed;
            txtIsNegative.Visibility = !_isPositiveResult ? Visibility.Visible : Visibility.Collapsed;
        }     

        #region IVisualScreen Members

        public event EventHandler<NextScreenClickedEventArg> NextScreenClicked;

        #endregion

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            OnNextScreenClicked(this, new NextScreenClickedEventArg(null, null));
        }

        protected virtual void OnNextScreenClicked(IVisualScreen screen, NextScreenClickedEventArg e)
        {
            NextScreenClicked?.Invoke(screen, e);
        }

        #region IVisualScreen Members

        public Button GetDefaultButtonForTesting()
        {
           return null;
        }

        public void Reset()
        {
            //do nothing
        }

        #endregion

       
        #region Screen Timeout

        private DispatcherTimer _timeoutTimer;
        
        //timeout event handler
        void _timeoutTimer_Tick(object sender, EventArgs e)
        {
            _timeoutTimer.Stop();
            //raise next button event
            OnNextScreenClicked(this, new NextScreenClickedEventArg(null, null));
        }

        public void Show(bool withAnimation)
        {
            _helper.Show(withAnimation);
            _timeoutTimer.Start();
           
        }

        public void Hide()
        {
            if (_timeoutTimer.IsEnabled)
            {
                _timeoutTimer.Stop();
            }

            _helper.Hide();
        }
        
        #endregion


    }
}
