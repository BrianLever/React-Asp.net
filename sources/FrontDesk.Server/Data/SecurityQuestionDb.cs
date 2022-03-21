using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Common;
using FrontDesk.Common.Data;

namespace FrontDesk.Server.Data
{
    public interface ISecurityQuestionRepository
    {
        List<string> GetQuestions();
    }

    public class SecurityQuestionDb : DBDatabase, ISecurityQuestionRepository
    {
        #region Constructors

        // TODO: 1st connection string is used. Any other way to specify connection string, with parameterless constructor?
        public SecurityQuestionDb() : base(0) { }

        public SecurityQuestionDb(DbConnection sharedConnection)
            : base(sharedConnection)
        {
        }

        #endregion

        /// <summary>
        /// Get question array from db
        /// </summary>
        public List<string> GetQuestions()
        {
            string sql = "select [QuestionText] from dbo.[SecurityQuestion] order by [QuestionText]";

            List<string> list = new List<string>();

            try
            {
                using (var reader = RunSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        list.Add(reader["QuestionText"].ToString());
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                this.Disconnect();
            }

            return list;
        }
    }
}
