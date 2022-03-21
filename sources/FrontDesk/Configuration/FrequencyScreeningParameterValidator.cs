using System.Collections.Generic;

namespace FrontDesk.Configuration
{
    public class FrequencyScreeningParameterValidator : IScreeningParameterValidator<ScreeningFrequencyItem>
    {

        private List<string> errorMessages = new List<string>();

        /// <summary>
        /// Validate Contact and Screening Frequency parameters
        /// </summary>
        /// <param name="value"></param>
        /// <param name="errorMessages"></param>
        /// <returns></returns>
        public bool Validate(ScreeningFrequencyItem value)
        {
            errorMessages.Clear();

            if (value.Frequency < 0 )
            {
                errorMessages.Add(Properties.Resources.ScreeningFrequency_FrequencyValidationError);
                return false;
            }
            return true;
        }


        #region IScreeningParameterValidator<ScreeningFrequencyItem> Members


        public List<string> Errors
        {
            get { return errorMessages; }
        }

        public string Error
        {
            get
            {
                return string.Join(". ", errorMessages);
            }
        }

        #endregion
    }
}
