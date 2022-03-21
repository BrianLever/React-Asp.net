using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Configuration
{
    public static class ScreeningFrequencyValidatorFactory
    {
        public static IScreeningParameterValidator<ScreeningFrequencyItem> Create(string screeningFrequencyID)
        {
            if (screeningFrequencyID == ScreeningFrequencyDescriptor.ContactFrequencyID)
            {
                return new FrequencyScreeningParameterValidator();
            }
            else
            {
                return new FrequencyScreeningParameterValidator();
            }
        }
    }
}
