using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Common.Messages
{
    public static class CustomError
    {
        public static string FormErrorTemplate = "Failed to read #form_name# info.";
        public static string AddErrorTemplate = "Failed to add new #form_name#.";
        public static string UpdateErrorTemplate = "Failed to update #form_name#. Error has occurred.";
        public static string DeleteErrorTemplate = "Failed to delete #form_name#. Error has occurred.";
        public static string FormatErrorTemplate = "Input data has wrong format.";
        public static string FieldFormatErrorTemplate = "Field #field_name# data has wrong format.";
        public static string CustomActionErrorTemplate = "Failed to {0} {1}. Error has occurred.";
        
        public static string InternalErrorTemplate = "Sorry, but page couldn't be displayed properly due to internal server error.";


        /// <summary>
        /// Return Form read data error message
        /// </summary>
        /// <param name="entityName">Form name</param>
        /// <returns>Error message string</returns>
        public static string GetInternalErrorMessage()
        {
            return InternalErrorTemplate;
        }

        /// <summary>
        /// Return Form read data error message
        /// </summary>
        /// <param name="entityName">Form name</param>
        /// <returns>Error message string</returns>
        public static string GetFormMessage(string entityName)
        {
            return FormErrorTemplate.Replace("#form_name#", entityName);
        }

        /// <summary>
        /// Return Add Form message
        /// </summary>
        /// <param name="entityName">Form Name</param>
        public static string GetAddMessage(string entityName)
        {
            return AddErrorTemplate.Replace("#form_name#", entityName);
        }
        /// <summary>
        /// Return Update Form error message
        /// </summary>
        /// <param name="entityName">Form Name</param>
        public static string GetUpdateMessage(string entityName)
        {
            return UpdateErrorTemplate.Replace("#form_name#", entityName);
        }
        /// <summary>
        /// Return Delete Form error message
        /// </summary>
        /// <param name="entityName">Form Name</param>
        public static string GetDeleteMessage(string entityName)
        {
            return DeleteErrorTemplate.Replace("#form_name#", entityName);
        }

        /// <summary>
        /// Return Input data wrong format error message
        /// </summary>
        public static string GetFormatMessage()
        {
            return FormatErrorTemplate;
        }

        /// <summary>
        /// Return Field data has wrong format
        /// </summary>
        /// <param name="entityName">Form Name</param>
        public static string GetFormatMessage(string entityName)
        {
            return FieldFormatErrorTemplate.Replace("#field_name#", entityName);
        }

        /// <summary>
        /// Get error message for any custom operation
        /// </summary>
        /// <param name="actionName">Operation name, I.E. "delete", "synchronize", etc,</param>
        /// <param name="entityName">Entity name</param>
        public static string GetMessageForCustomOperation(string actionName, string entityName)
        {
            return string.Format(CustomActionErrorTemplate, actionName, entityName);
        }
    }
}
