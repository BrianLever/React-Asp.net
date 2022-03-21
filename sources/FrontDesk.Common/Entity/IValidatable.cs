using System;

namespace FrontDesk.Common.Entity
{
    /// <summary>
    /// Interface for Validatable business object
    /// </summary>
    public interface IValidable
    {
        /// <summary>
        /// Clear errors
        /// </summary>
        void ClearError();

        /// <summary>
        /// Get if object data is valid
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// Get error message
        /// </summary>
        string Error { get; }
        

    }
}