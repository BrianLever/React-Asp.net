using System;

namespace FrontDesk.Kiosk.Services
{
    public class PatientNameValidationException: Exception
    {
        public PatientNameValidationException(string message, Exception innerException): base(message, innerException)
        {

        }
    }
}
