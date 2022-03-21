using System;

namespace ScreenDoxKioskLauncher.Services
{
    /// <summary>
    /// Exception related to issues with downloading and extracting installation package file
    /// </summary>
    [Serializable]
    public class DownloadPackageException: Exception
    {

        /// <summary>
        /// Default contructor
        /// </summary>
        /// <param name="message"></param>
        public DownloadPackageException(string message): base(message)
        {

        }

        /// <summary>
        /// Default contructor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public DownloadPackageException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
