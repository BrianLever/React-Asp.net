using System;
using System.Timers;
using FrontDesk.Common.Debugging;
using FrontDesk.Kiosk.Discovery;
using FrontDesk.Kiosk.KioskEndpointService;

namespace FrontDesk.Kiosk.Controllers
{
    internal class OutOfServiceController
    {

        private readonly ISelfDiscoveryService _selfDiscoveryService = new SelfDiscoveryService();

        #region Singleton constructor

        private static object _syncObject = new object();

        private static OutOfServiceController _instance = null;
        /// <summary>
        /// Get instance of the 
        /// </summary>
        public static OutOfServiceController Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncObject)
                    {
                        if (_instance == null)
                        {

                            _instance = new OutOfServiceController();
                        }
                    }
                }

                return _instance;
            }
        }


        private OutOfServiceController()
        {
            this._timer = new Timer();
            _timer.Interval = TimeSpan.FromSeconds(Settings.AppSettings.ServerConnectionPingIntervalInSeconds).TotalMilliseconds;
            _timer.Elapsed += new ElapsedEventHandler(_timer_Tick);
            _timer.Stop();

            MaxFailedPingRequestSequenceLength = Settings.AppSettings.MaxFailedPingRequestSequenceLength;
            SucceedEventsProbLength = Settings.AppSettings.SucceedEventsProbLength;
        }



        #endregion

        #region Timer and Parameters

        private Timer _timer;

        void _timer_Tick(object sender, EventArgs e)
        {
            Ping();
        }
        /// <summary>
        /// start monitoring server connection
        /// </summary>
        public void Start()
        {
            Ping();
            _timer.Start();
        }
        /// <summary>
        /// stop monitoring server connection
        /// </summary>
        public void Stop()
        {
            _timer.Stop();
        }
        /// <summary>
        /// Get or set the max sequence length of unsuccessful ping requests after which server connection is considered to be unavailable
        /// </summary>
        public int MaxFailedPingRequestSequenceLength { get; set; }
        public int SucceedEventsProbLength { get; set; }


        #endregion


        #region Ping Server Connection

        bool _isRequestInProgress = false;
        int _numberOfFailedRequests = 0;
        int _numberOfSucceedRequests = 0;
        private object _requestSyncObject = new object();
        private object _failedCountSyncObject = new object();
        private object _succeedCountSyncObject = new object();

        /// <summary>
        /// Call Ping method on server
        /// </summary>
        protected void Ping()
        {
            bool succeed = false;
            if (!_isRequestInProgress)
            {
                lock (_requestSyncObject)
                {
                    if (!_isRequestInProgress)
                    {
                        _isRequestInProgress = true;

                        var message = new KioskPingMessage1
                        {
                            KioskID = Settings.AppSettings.KioskID,
                            KioskAppVersion = _selfDiscoveryService.GetAppVersion(),
                            IpAddress = _selfDiscoveryService.GetIpAddress()
                        };

                        try
                        {
                            succeed = KioskEndpointServiceClientFactory.Execute(c => c.Ping_v3(message));
                        }
                        catch (Exception ex)
                        {
                            DebugLogger.TraceException(ex, "Failed to ping WCF service on server side.");
                        }
                        finally
                        {
                            _isRequestInProgress = false;
                        }
                    }
                }
            }

            if (succeed)
            {
                IncrementSucceedRequestCount();
            }
            else
            {
                IncrementFailedRequestCount();
            }
        }

        /// <summary>
        /// Increase number of failed requests
        /// </summary>
        private void IncrementFailedRequestCount()
        {

            System.Threading.Interlocked.Increment(ref _numberOfFailedRequests);
            _numberOfSucceedRequests = 0;


            if (_numberOfFailedRequests > MaxFailedPingRequestSequenceLength)
            {
                _numberOfFailedRequests = 0;


                ConnectionState = ServerConnectionState.Disconnected;

                if (ServerConnectionLost != null)
                {
                    ServerConnectionLost(this, new EventArgs());
                }

            }

        }

        /// <summary>
        /// Increase number of succeed requests
        /// </summary>
        private void IncrementSucceedRequestCount()
        {

            System.Threading.Interlocked.Increment(ref _numberOfSucceedRequests);

            if (_numberOfSucceedRequests >= SucceedEventsProbLength)
            {
                ResetFailedRequestCount();
            }

        }


        public void SetConnectionFailedState()
        {
            _numberOfFailedRequests = MaxFailedPingRequestSequenceLength + 1;

            IncrementFailedRequestCount();

        }


        /// <summary>
        /// Reset failed requests count after connection has been established
        /// </summary>
        private void ResetFailedRequestCount()
        {
            _numberOfFailedRequests = 0;
            _numberOfSucceedRequests = 0;

            if (ConnectionState != ServerConnectionState.Connected)
            {
                var prevState = ConnectionState;
                ConnectionState = ServerConnectionState.Connected;

                if (prevState == ServerConnectionState.Disconnected)
                {
                    if (ServerConnectionEstablished != null)
                    {
                        ServerConnectionEstablished(this, new EventArgs());
                    }
                }
            }
        }

        #endregion

        #region events

        /// <summary>
        /// Occurs when the max length of failed requests sequence exceeded the threshold value
        /// </summary>
        public event EventHandler ServerConnectionLost;
        /// <summary>
        /// Occurs when server connection is established after it has been lost
        /// </summary>
        public event EventHandler ServerConnectionEstablished;

        #endregion

        public enum ServerConnectionState
        {
            Unknown,
            Connected,
            Disconnected
        }

        public ServerConnectionState ConnectionState { get; private set; }
    }


}
