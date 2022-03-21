namespace ScreenDoxKioskLauncher.Controllers
{
    using Common.Logging;
    using System;
    using System.Timers;

    /// <summary>
    /// Controller that executes action on timer event
    /// </summary>
    public abstract class TimerEventController: IController
    {
        /// <summary>
        /// Access to logger class
        /// </summary>
        protected readonly ILog Logger = LogManager.GetLogger<TimerEventController>(); 

        /// <summary>
        /// Timer
        /// </summary>
        private Timer _timer;

        /// <summary>
        /// Timer interval
        /// </summary>
        protected abstract TimeSpan TimerInterval { get; }

        /// <summary>
        /// Name of controller to appear in log messages
        /// </summary>
        protected abstract string ControllerName { get; }
       
        /// <summary>
        /// Action that executes on timer event
        /// </summary>
        public abstract void OnTimerTickAction();

        private void OnTimerEvent(object sender, EventArgs e)
        {
            Logger.DebugFormat($"{ControllerName} timer event. Starting OnTimerTickAction");

            OnTimerTickAction();
        }

        /// <summary>
        /// Init timer
        /// </summary>
        protected void InitTimer()
        {
            if (_timer == null)
            {
                _timer = new Timer
                {
                    Interval = TimerInterval.TotalMilliseconds
                };
                _timer.Elapsed += new ElapsedEventHandler(OnTimerEvent);
            }
        }

        /// <summary>
        /// start monitoring server connection
        /// </summary>
        public virtual void Start()
        {
            InitTimer();

            _timer.Start();

            OnTimerTickAction(); // run immidiatelly first time.
        }
        /// <summary>
        /// stop monitoring server connection
        /// </summary>
        public virtual void Stop()
        {
            InitTimer();
            _timer.Stop();
        }
    }
}
