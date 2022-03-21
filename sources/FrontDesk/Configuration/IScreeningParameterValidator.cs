using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Configuration
{
    public interface IScreeningParameterValidator<TParameter>
    {
        bool Validate(TParameter value);

        /// <summary>
        /// Validation errors after Validate method call
        /// </summary>
        List<string> Errors { get; }

        /// <summary>
        /// Validation error string after Validate method call
        /// </summary>
        string Error { get; }
    }
}
