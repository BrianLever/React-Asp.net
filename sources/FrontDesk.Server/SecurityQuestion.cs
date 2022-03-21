using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using FrontDesk.Server.Data;

namespace FrontDesk.Server
{
    public class SecurityQuestion
    {
        public int ID { get; set; }
        public string Text { get; set; }

        private static SecurityQuestionDb DbObject { get { return new SecurityQuestionDb(); } }

        /// <summary>
        /// Get question array
        /// </summary>
        [Obfuscation(Feature = "renaming", Exclude = true)] // used in data binding by name
        public static List<string> GetQuestions()
        {
            return DbObject.GetQuestions();            
        }
    }
}
