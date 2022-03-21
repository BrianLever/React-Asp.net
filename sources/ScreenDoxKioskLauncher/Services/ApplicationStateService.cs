using Common.Logging;
using System;
using System.Threading;

namespace ScreenDoxKioskLauncher.Services
{
    /// <summary>
    /// Application states
    /// </summary>
    public enum KioskApplicationState
    {
        /// <summary>
        /// Normal operational mode
        /// </summary>
        Normal,
        /// <summary>
        /// Kiosk upgrade is in progress
        /// </summary>
        Upgrading
    }

    /// <summary>
    /// Application state management service
    /// </summary>
    public class ApplicationStateService : IApplicationStateService, IDisposable
    {
        private readonly ILog _logger = LogManager.GetLogger<ApplicationStateService>();
        private ReaderWriterLockSlim _appStateLock = new ReaderWriterLockSlim();

        private KioskApplicationState _kioskAppState = KioskApplicationState.Normal;

        /// <summary>
        /// Get current application state
        /// </summary>
        /// <returns>State</returns>
        public KioskApplicationState GetState()
        {
            _appStateLock.EnterReadLock();
            try
            {
                return _kioskAppState;
            }
            finally
            {
                _appStateLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Update application state
        /// </summary>
        /// <param name="state"></param>
        public void SetState(KioskApplicationState state)
        {
            _appStateLock.EnterWriteLock();
            try
            {
                _kioskAppState = state;

                _logger.Info($"[STATE] Kiosk state has changed to {state}.");
            }
            finally
            {
                _appStateLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Returns True if application is in Normal state. Otherwise it's False
        /// </summary>
        /// <returns></returns>
        public bool IsInNormalState()
        {
            return GetState() == KioskApplicationState.Normal;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                if (_appStateLock != null) _appStateLock.Dispose();
                _appStateLock = null;

                disposedValue = true;
            }
        }

        ~ApplicationStateService()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
