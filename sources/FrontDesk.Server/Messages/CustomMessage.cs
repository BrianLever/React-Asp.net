using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Common.Messages
{
    public static class CustomMessage
    {
        public static string AddSuccessfullyTemplate = "#form_name# has been created successfully.";
        public static string UpdateSuccessfullyTemplate = "#form_name# has been updated successfully.";
        public static string DeleteSuccessfullyTemplate = "#form_name# has been deleted successfully.";

        /// <summary>
        /// Return Add Form successfully message, the first char must be a capital letter 
        /// </summary>
        /// <param name="sFormName">Form Name</param>
        public static string GetAddMessage(string sFormName)
        {
            return AddSuccessfullyTemplate.Replace("#form_name#", sFormName);
        }
        /// <summary>
        /// Return Update Form successfully message, the first char must be a capital letter 
        /// </summary>
        /// <param name="sFormName">Form Name</param>
        public static string GetUpdateMessage(string sFormName)
        {
            return UpdateSuccessfullyTemplate.Replace("#form_name#", sFormName);
        }
        /// <summary>
        /// Return Delete Form successfully message, the first char must be a capital letter 
        /// </summary>
        /// <param name="sFormName">Form Name</param>
        public static string GetDeleteMessage(string sFormName)
        {
            return DeleteSuccessfullyTemplate.Replace("#form_name#", sFormName);
        }
    }
}
