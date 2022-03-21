using System;

namespace FrontDesk.Kiosk
{
	/// <summary>
	/// Incapsulates auto hide logic. Singletone
	/// </summary>
	public class UserSessionTimeoutController : FrontDesk.Kiosk.IUserSessionTimeoutController
    {

        public UserSessionTimeoutController()
        {

        }

        //private DispatcherTimer _timer;
        private System.Timers.Timer _timer;
        private TimeSpan _sessionTimeoutPeriod = TimeSpan.FromSeconds(60);
        private TimeSpan _sessionTimeoutNotification = TimeSpan.FromSeconds(55);

        /// <summary>
        /// User session timeout. By default 60 seconds
        /// </summary>
        /// <exception cref="System.ArgumentException">SessionTimoutPeriod value is less than 5 seconds</exception>
        public TimeSpan SessionTimeoutPeriod
        {
            get { return _sessionTimeoutPeriod; }
            set
            {
                if (value.TotalSeconds < 5)
                {
                    throw new ArgumentException("SessionTimoutPeriod value cannot be less than 5 seconds");
                }
                _sessionTimeoutPeriod = value;

            }
        }

        /// <summary>
        /// User session is expiring timeout. By default 55 seconds
        /// </summary>
        /// <exception cref="System.ArgumentException">SessionExpiringNotificationTimeout value is less than 5 seconds</exception>
        public TimeSpan SessionExpiringNotificationTimeout
        {
            get { return _sessionTimeoutNotification; }
            set
            {
                if (value.TotalSeconds < 5)
                {
                    throw new ArgumentException("SessionExpiringNotificationTimeout value cannot be less than 5 seconds");
                }
                _sessionTimeoutNotification = value;

            }
        }

        public DateTime LastUsedTime { get; set; }
        private DateTime _lastCommitedUsedTimestamp = DateTime.MinValue;

        public void StartMonitoring()
        {
            //_timer = new DispatcherTimer();
            //_timer.Tick += new EventHandler(_timer_Tick);
            //_timer.Interval = TimeSpan.FromSeconds(0.8);
            //_timer.Start();

            _timer = new System.Timers.Timer();
            _timer.Elapsed += new System.Timers.ElapsedEventHandler(_timer_Tick);
            _timer.Interval = 500;
            _timer.Start();
        }

        public void StopMonitoring()
        {
            if (_timer != null)
            {
                _timer.Stop();
            }
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            
            
            if (LastUsedTime != _lastCommitedUsedTimestamp)
            {
                var now = DateTime.Now;
                //Debug.Print("{0:HH:mm:ss.fff} - User session timer tick event. Inactivity period {1} sec", now, now.Subtract(LastUsedTime).TotalSeconds);

                //if was any activity
                double unusedTimePeriod = now.Subtract(LastUsedTime).TotalSeconds;

                if (unusedTimePeriod >= _sessionTimeoutPeriod.TotalSeconds)
                {

                    //Debug.Print("{0:HH:mm:ss.fff} - User session expired. Inactivity duration: {1} sec", now, unusedTimePeriod);


                    //set commited timestamp
                    _lastCommitedUsedTimestamp = LastUsedTime;
                    try
                    {
                        if (UserSessionExpired != null)
                        {

                            //raise event
                            UserSessionExpired(this, new EventArgs());
                        }
                    }
                    catch { }
                    finally
                    {

                    }
                }
                else if (unusedTimePeriod >= _sessionTimeoutNotification.TotalSeconds)
                {
                    //Debug.Print("{0:HH:mm:ss.fff} - User session will be expired in 5 minutes. Inactivity duration: {1} sec", now, unusedTimePeriod);

                    //force to show session will expire soon notification
                    if (UserSessionExpiring != null)
                    {
                        //raise event
                        UserSessionExpiring(this, new EventArgs());
                    }
                }
            }

        }

        public event EventHandler UserSessionExpiring;
        public event EventHandler UserSessionExpired;
    }
}
