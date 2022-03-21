using System;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using FrontDesk.Common.Debugging;
using FrontDesk.Kiosk.Workflow;
using System.Threading;
using System.Windows.Threading;
using FrontDesk.Kiosk.Settings;
using Common.Logging;

namespace FrontDesk.Kiosk.Screens
{
    /// <summary>
    /// Interaction logic for SendResult.xaml
    /// </summary>
    public partial class SendResult : UserControl, IVisualScreen
    {
        private ILog _logger = LogManager.GetLogger("SendResult");

        private ScreenComponentHelper _helper;

        #region timers

        DispatcherTimer _onErrorTimeoutTimer = null;

        #endregion

        public SendResult()
        {
            InitializeComponent();

            _helper = new ScreenComponentHelper(this);
            grdFailedToSend.Visibility = Visibility.Collapsed;
            grdSendingResults.Visibility = Visibility.Visible;
            _onErrorTimeoutTimer = new DispatcherTimer();
            _onErrorTimeoutTimer.Interval = TimeSpan.FromSeconds(Settings.AppSettings.SaveResultErrorPageTimeoutInSeconds);
            _onErrorTimeoutTimer.Tick += new EventHandler(OnErrorTimeoutTimer_Tick);
            _onErrorTimeoutTimer.IsEnabled = false;


            //this.IsVisibleChanged += new DependencyPropertyChangedEventHandler(OnIsVisibleChanged);
            //this.Loaded += new RoutedEventHandler(OnLoaded);
        }




        #region properties


        #endregion

        #region Raise event

        public event EventHandler<NextScreenClickedEventArg> NextScreenClicked;
        protected virtual void OnNextScreenClicked(IVisualScreen screen, NextScreenClickedEventArg e)
        {
            if (NextScreenClicked != null)
            {
                NextScreenClicked(screen, e);
            }
        }

        #endregion

        public Button GetDefaultButtonForTesting()
        {
            return null;
        }

        public void Reset()
        {
            //set sending message visible
            grdFailedToSend.Visibility = Visibility.Collapsed;
            grdSendingResults.Visibility = Visibility.Visible;
            lock (_syncObject)
            {
                _isInSavingProgress = false;
            }
        }

        object _syncObject = new object();



        #region Sending results

        private bool _isInSavingProgress = false;


        /// <summary>
        /// start sending results
        /// </summary>
        /// <returns></returns>
        public bool? SendScreeningResults()
        {
            var attempNumber = 0;

            ScreeningResultState.Instance.ScreeningTimeLog.StopPatientScreeningRecording();

            var operationResult = SendResultsToServer(attempNumber);

            while (!operationResult.HasValue && attempNumber < 3) //try to resend 3 times with interval 500ms
            {
                var sleepInterval = AppSettings.SendResultsRetryIntervalInMilliseconds * (attempNumber + 1);
                Thread.Sleep(sleepInterval);

                //try to resend
                operationResult = SendResultsToServer(++attempNumber);
            }


            _isInSavingProgress = false;

            return operationResult; // return if save was successfull or failed.
        }

        /// <summary>
        /// Return True/False - response from the server if Screening has any Positive score or all negative.
        /// Null means that screening has failed to save
        /// </summary>
        /// <param name="attempNumber"></param>
        /// <returns></returns>
        private bool? SendResultsToServer(int attempNumber)
        {
            _logger.DebugFormat("Entering SendScreeningResults. Attempt: {0}", attempNumber);
            bool? operationResult = null;

            var client = new KioskEndpointService.KioskEndpointClient();

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                        ((sender, certificate, chain, sslPolicyErrors) => true);


                operationResult = client.SaveScreeningResult_v2(
                    ScreeningResultState.Instance.Result,
                    ScreeningResultState.Instance.ScreeningTimeLog.GetLogRecords().ToArray()
                    );

                _logger.TraceFormat("Saved screening with result: {0}. Attempt: {1}", operationResult, attempNumber);

                client.Close();

            }
            catch (Exception ex)
            {
                //log error
                _logger.Error("Unhandled exception occured while sending screening result to the server.", ex);
                operationResult = null;
            }
            finally
            {
                client.Abort();
            }

            _logger.DebugFormat("Exit SendScreeningResults. Attempt: {0}", attempNumber);

            return operationResult;
        }


        /// <summary>
        /// Do saving result and handle error
        /// </summary>
        public void SaveResult()
        {
            bool? saveOperationResult = null;
            lock (_syncObject)
            {
                _isInSavingProgress = true;
            }

            try
            {
                saveOperationResult = SendScreeningResults();
            }
            catch (Exception ex)
            {
                _logger.Error("Unhandled Save Result exception.", ex);
            }
            finally
            {

                if (saveOperationResult.HasValue)
                {
                    //go to next step
                    this.Dispatcher.BeginInvoke((ThreadStart)delegate
                    {
                        GoToNextStep(saveOperationResult);
                    }, null);
                }
                else
                {
                    this.Dispatcher.BeginInvoke((ThreadStart)delegate
                {
                    //show error message 
                    ShowErrorMessage();
                }, null);
                }

                lock (_syncObject)
                {
                    _isInSavingProgress = false;
                }
            }

        }


        public void ShowErrorMessage()
        {
            grdSendingResults.Visibility = Visibility.Collapsed;
            grdFailedToSend.Visibility = Visibility.Visible;

            _onErrorTimeoutTimer.Start();
        }


        public void GoToNextStep(bool? isScreeningHasPositiveIndicator)
        {
            OnNextScreenClicked(this, new NextScreenClickedEventArg(null, isScreeningHasPositiveIndicator));
        }


        void OnErrorTimeoutTimer_Tick(object sender, EventArgs e)
        {
            _onErrorTimeoutTimer.Stop(); //stop timer
            OnNextScreenClicked(this, new NextScreenClickedEventArg(null, null)); //raise next step and mark that result is 'False'

        }


        public void Show(bool withAnimation)
        {

            _helper.Show(withAnimation);

            ucSpinner.Show();
            if (!_isInSavingProgress)
            {
                lock (_syncObject)
                {
                    if (!_isInSavingProgress)
                    {
                        ThreadPool.QueueUserWorkItem(delegate
                        {
                            SaveResult(); //start saving results in separate thread
                        });
                    }
                }
            }

        }

        public void Hide()
        {
            if (_onErrorTimeoutTimer.IsEnabled)
                _onErrorTimeoutTimer.Stop();


            _helper.Hide();
            ucSpinner.Hide();

        }

        #endregion


    }
}
