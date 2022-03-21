using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace FrontDesk.Common.Mailing
{
    public class EmailManager
    {
        public const string ValidationExpression = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        /// <summary>
        /// Compacts the HTML text.
        /// See http://www.codeproject.com/KB/aspnet/WhitespaceFilter.aspx for 
        /// example of more complex rules.
        /// </summary>
        /// <param name="text">The source html text.</param>
        /// <returns>Equivalent html text</returns>
        public static string CompactHtml(StringBuilder text)
        {            
            text.Replace("\t", null);
            text.Replace("\r", null);
            text.Replace("\n", null);

            while (text.ToString().IndexOf("  ") != -1)
            {
                text.Replace("  ", " ");
            }

            return text.ToString();
        }

        /// <summary>
        /// Compacts the HTML text.
        /// </summary>
        /// <param name="text">The source html text.</param>
        /// <returns>Equivalent html text</returns>
        public static string CompactHtml(string text)
        {
            return CompactHtml(new StringBuilder(text));
        }


        /// <summary>
        /// Get the single string created from mail collection 
        /// </summary>
        public static string TransformMailCollectionToSingleString(MailAddressCollection mails, string separator)
        {
            StringBuilder mailString = new StringBuilder(String.Empty);

            foreach (var mail in mails)
            {
                mailString.Append(mail.Address + (String.IsNullOrEmpty(separator) ? String.Empty : separator));
            }

            if (mailString.Length > 0)
            {
                mailString.Remove(mailString.Length - 1, 1); // remove last comma
            }
            return mailString.ToString();
        }
    }
}
