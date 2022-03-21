using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

namespace FrontDesk.Common.Entity
{
    /// <summary>
    /// Base class which implements IValidable interface
    /// </summary>
    [DataContract(Name = "BaseValidableEntity", Namespace = "http://www.frontdeskhealth.com")]
    [Obfuscation(Feature = "renaming", Exclude = true, ApplyToMembers = true)]  // class name, method names AND method arguments are not renamed
    public partial class BaseValidableEntity : IValidable
    {
        public BaseValidableEntity() { }

        #region IValidatable Members

        /// <summary>
        /// Validation errors list
        /// </summary>
        protected List<string> errorMessages = new List<string>();

        /// <summary>
        /// Get last error message after the validation
        /// </summary>
        public virtual string Error
        {
            get { return GetError(); }
        }
        /// <summary>
        /// Returns error message
        /// </summary>
        /// <returns>Error message string devided by \n character</returns>
        protected virtual string GetError()
        {
            System.Text.StringBuilder lastErrorMessage = new System.Text.StringBuilder();
            if (errorMessages != null && errorMessages.Count > 0)
            {
                for (int i = 0; i < errorMessages.Count; i++)
                {
                    lastErrorMessage.AppendFormat("{0}\n", errorMessages[i]);
                }
                lastErrorMessage.Length--;
            }
            return lastErrorMessage.ToString();
        }
        /// <summary>
        /// Add error string to the error list
        /// </summary>
        /// <param name="lastErrorString"></param>
        protected virtual void SetError(string lastErrorString)
        {
            errorMessages.Add(lastErrorString);
        }
        /// <summary>
        /// Clear error list
        /// </summary>
        public void ClearError()
        {
            errorMessages = new List<string>();
        }
        /// <summary>
        /// Validate object. True if object's data passed validation successfully
        /// </summary>
        public virtual bool IsValid
        {
            get { return Validate(); }
        }
        /// <summary>
        /// Validate object
        /// </summary>
        protected virtual bool Validate()
        {
            ClearError();

            return true;
        }
        #endregion


    }
}