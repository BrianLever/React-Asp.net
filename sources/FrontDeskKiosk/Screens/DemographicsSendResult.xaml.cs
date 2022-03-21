using System;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using FrontDesk.Common.Debugging;
using FrontDesk.Kiosk.Workflow;
using System.Threading;
using System.Windows.Threading;
using FrontDesk.Kiosk.Settings;

namespace FrontDesk.Kiosk.Screens
{
    /// <summary>
    /// Interaction logic for SendResult.xaml
    /// </summary>
    public partial class DemographicsSendResult : UserControl, IVisualScreen
    {

        private ScreenComponentHelper _helper;

        #region timers

        DispatcherTimer _onErrorTimeoutTimer = null;

        #endregion

        public DemographicsSendResult()
        {
            InitializeComponent();

            _helper = new ScreenComponentHelper(this);
            grdFailedToSend.Visibility = Visibility.Collapsed;
            grdSendingResults.Visibility = Visibility.Visible;
            _onErrorTimeoutTimer = new DispatcherTimer();
            _onErrorTimeoutTimer.Interval = TimeSpan.FromSeconds(Settings.AppSettings.SaveResultErrorPageTimeoutInSeconds);
            _onErrorTimeoutTimer.Tick += new EventHandler(OnErrorTimeoutTimer_Tick);
            _onErrorTimeoutTimer.IsEnabled = false;


        }

        public event EventHandler<NextScreenClickedEventArg> NextScreenClicked;
        protected virtual void OnNextScreenClicked(IVisualScreen screen, NextScreenClickedEventArg e)
        {
            if (NextScreenClicked != null)
            {
                NextScreenClicked(screen, e);
            }
        }

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
        public bool SendResults()
        {

            bool result = SendDemographicsResults(0);

            _isInSavingProgress = false;

            return result;
        }
        /// <summary>
        /// Save results to server
        /// </summary>
        /// <returns></returns>
        public bool SendDemographicsResults(int attempNumber)
        {
            Debug.WriteLine("Step into DemographicsSendResult");

            bool operationResult = false;

            var client = new KioskEndpointService.KioskEndpointClient();


            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                        ((sender, certificate, chain, sslPolicyErrors) => true);

                var timeLog = ScreeningResultState.Instance.ScreeningTimeLog;
                timeLog.StopSectionTimeRecording(ScreeningSectionDescriptor.Demographics);
                timeLog.StopPatientScreeningRecording();

                var demographics = ScreeningResultState.Instance.Demographics;


                operationResult = client.SaveDemographicsResults(
                    demographics,
                    timeLog.GetLogRecords().ToArray()
                    );
                DebugLogger.WriteTraceMessage("Saved demographics with result: " + operationResult);

                client.Close();

            }
            catch (Exception ex)
            {
                //log error
                DebugLogger.TraceException(ex, "Unhandled exception occured on sending demographics result to the server.");
            }
            finally
            {
                client.Abort();
            }

            if (!operationResult && attempNumber < 3) //try to resend 3 times with interval
            {
                var sleepInterval = AppSettings.SendResultsRetryIntervalInMilliseconds * (attempNumber + 1);
                Thread.Sleep(sleepInterval);
                //try to resend
                operationResult = SendDemographicsResults(attempNumber + 1);

            }


            Debug.WriteLine("Step out DemographicsSendResult");
            return operationResult;
        }

        /// <summary>
        /// Do saving result and handle error
        /// </summary>
        public void SaveResult()
        {
            bool saveOperationResult = false;
            lock (_syncObject)
            {
                _isInSavingProgress = true;
            }

            try
            {
                saveOperationResult = SendResults();
            }
            catch (Exception ex)
            {
                DebugLogger.TraceException(ex);
            }
            finally
            {

                if (saveOperationResult)
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


        public void GoToNextStep(bool succeed)
        {
            OnNextScreenClicked(this, new NextScreenClickedEventArg(null, succeed));
        }


        void OnErrorTimeoutTimer_Tick(object sender, EventArgs e)
        {
            _onErrorTimeoutTimer.Stop(); //stop timer
            OnNextScreenClicked(this, new NextScreenClickedEventArg(null, false)); //raise next step and mark that result is 'False'

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
