using System;

namespace FrontDesk.Kiosk
{
	public class KioskConfigurationException : Exception
    {
        public KioskConfigurationException(string errorMessage) : base(errorMessage) { }

        public KioskConfigurationException(string errorMessage, Exception innerException) : base(errorMessage, innerException) { }
    }
}
