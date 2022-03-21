using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Common.Messages
{
    public static class ConfirmMessage
    {
        /// <summary>
        /// Get localized confirmation text for "Save Changes" operation
        /// </summary>
        /// <returns></returns>
        public static string GetSaveChangesConfirmText()
        {
            return FrontDesk.Server.Resources.TextMessages.SaveChangesConfirmText;
        }
        /// <summary>
        /// Get "Save Changes" operation text string
        /// </summary>
        public static string SaveChangesConfirmTemplateString { get { return FrontDesk.Server.Resources.TextMessages.SaveChangesConfirmText; } }


        /// <summary>
        /// Get localized confirmation text for "Delete" operation
        /// </summary>
        /// <param name="entityName">Entity Name which is about to be deleted</param>
        /// <returns></returns>
        public static string GetDeleteConfirmText(string entityName)
        {
            return string.Format(FrontDesk.Server.Resources.TextMessages.DeleteConfirmText, entityName);
        }

        /// <summary>
        /// Get "Delete" operation text string with placeholders
        /// </summary>
        public static string DeleteConfirmTemplateString { get { return FrontDesk.Server.Resources.TextMessages.DeleteConfirmText; } }


    }
}
