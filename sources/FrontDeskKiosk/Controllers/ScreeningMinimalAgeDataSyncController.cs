using System;
using System.Collections.Generic;
using System.Timers;
using Common.Logging;
using FrontDesk.Common.Debugging;
using FrontDesk.Services;
using Newtonsoft.Json;


namespace FrontDesk.Kiosk
{
    internal class ScreeningMinimalAgeDataSyncController
    {

        private readonly IScreeningSectionAgeService _screeningSectionAgeService;
        private readonly ILog _logger = LogManager.GetLogger<ScreeningMinimalAgeDataSyncController>();

        #region Singleton constructor
        private static object _syncObject = new object();

        private static ScreeningMinimalAgeDataSyncController _instance = null;


        /// <summary>
        /// Get instance of the 
        /// </summary>
        public static ScreeningMinimalAgeDataSyncController Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncObject)
                    {
                        if (_instance == null)
                        {

                            _instance = new ScreeningMinimalAgeDataSyncController(new ScreeningSectionAgeService());
                        }
                    }
                }

                return _instance;
            }
        }


        private ScreeningMinimalAgeDataSyncController(IScreeningSectionAgeService screeningSectionAgeService)
        {

            if (screeningSectionAgeService == null) throw new ArgumentNullException("screeningSectionAgeService");

            _screeningSectionAgeService = screeningSectionAgeService;

            _timer = new Timer();
            _timer.Interval = TimeSpan.FromSeconds(Settings.AppSettings.ServerMinAgeDataUpdateIntervalInSeconds).TotalMilliseconds;
            _timer.Elapsed += new ElapsedEventHandler(_timer_Tick);
            _timer.Stop();
        }



        #endregion

        #region Timer and Parameters

        private Timer _timer;

        void _timer_Tick(object sender, EventArgs e)
        {
            ReadDataFromServer();
        }
        /// <summary>
        /// start monitoring server connection
        /// </summary>
        public void Start()
        {
            ReadDataFromServer(); //run immidiatelly
            _timer.Start();
        }
        /// <summary>
        /// stop monitoring server connection
        /// </summary>
        public void Stop()
        {
            _timer.Stop();
        }


        #endregion

        #region Ping Server Connection

        bool _isRequestInProgress = false;

        /// <summary>
        /// Call Ping method on server
        /// </summary>
        protected void ReadDataFromServer()
        {
            if (!_isRequestInProgress)
            {


                _isRequestInProgress = true;
                var client = new KioskEndpointService.KioskEndpointClient();
                try
                {
                    System.Net.ServicePointManager.ServerCertificateValidationCallback =
                            ((sender, certificate, chain, sslPolicyErrors) => true);

                    var lastDate = _screeningSectionAgeService.GetMaxAgeSettingsModifiedDateUTC() ?? new DateTime(2000, 1, 1);
                    var changes = client.GetModifiedAgeSettings_v2(Settings.AppSettings.KioskID, lastDate);

                    List<ScreeningSectionAge> ageSettings = new List<ScreeningSectionAge>();
                    foreach (var item in changes)
                    {
                        ageSettings.Add(new ScreeningSectionAge(item.ScreeningSectionID, item.MinimalAge, item.IsEnabled, item.LastModifiedDateUTC));
                    }

                    if (changes.Length > 0)
                    {
                        _screeningSectionAgeService.UpdateAgeSettings(ageSettings);

                        _logger.InfoFormat("Age settings has been updated. New values: {0}.", JsonConvert.SerializeObject(ageSettings));
                    }

                    client.Close();

                }
                catch (Exception ex)
                {
                    _logger.Error("Failed to ping WCF service on server side.", ex);
                    client.Abort();
                }
                finally
                {

                    _isRequestInProgress = false;
                }
            }

        }
        #endregion

    }


}
