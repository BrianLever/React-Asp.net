using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using FrontDesk.Common.Data;
using FrontDesk.Common.Extensions;
using FrontDesk.Server.Screening;

namespace FrontDesk.Server.Data
{

    public interface IScreeningScoreRepository
    {
        List<ScreeningScoreLevel> GetAllScoreLevels();
        List<ScreeningScoreLevel> GetScoreLevelsToQuestion(string screeningSectionID, int questionID);
        List<ScreeningScoreLevel> GetScoreLevelsBySectionID(string sectionID);
    }

    public class ScreeningScoreDb : DBDatabase, IScreeningScoreRepository
    {
        #region Constructors
        public ScreeningScoreDb() : base(0) { }

        public ScreeningScoreDb(DbConnection sharedConnection) : base(sharedConnection) { }

        #endregion


        /// <summary>
        /// Get list of score levels for particular sections
        /// </summary>
        /// <param name="sectionID"></param>
        /// <returns>List of all score levels sorted by screening ID and Score Level ID</returns>
        public List<ScreeningScoreLevel> GetAllScoreLevels()
        {
            var result = new List<ScreeningScoreLevel>();
            string sql = "[dbo].[uspGetAllScoreLevels]";

            ClearParameters();
            try
            {
                using (var reader = RunProcedureSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        result.Add(new ScreeningScoreLevel(reader));
                    }
                }
            }
            finally
            {
                Disconnect();
            }
            return result;
        }



        /// <summary>
        /// Get list of answers to specific questions in the format of score levels. Used for Visit Settings pge
        /// </summary>
        /// <param name="sectionID"></param>
        /// <returns>ist of answers to specific questions in the format of score levels</returns>
        public List<ScreeningScoreLevel> GetScoreLevelsToQuestion(string screeningSectionID, int questionID)
        {
            var result = new List<ScreeningScoreLevel>();
            string sql = "[dbo].[uspGetQuestionPositiveScoreLevels]";

            ClearParameters();
            AddParameter("@ScreeningSectionID", DbType.AnsiString, 5).Value = screeningSectionID;
            AddParameter("@QuestionID", DbType.Int32).Value = questionID;

            try
            {
                using (var reader = RunProcedureSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        result.Add(new ScreeningScoreLevel
                        {
                            ScreeningSectionID = reader.GetString(0),
                            ScoreLevel = reader.Get<int>(1, true),
                            Name = reader.GetString(2),
                            Label = reader.GetString(3)
                        });
                    }
                }
            }
            finally
            {
                Disconnect();
            }
            return result;
        }


        /// <summary>
        /// Get list of score levels for particular sections
        /// </summary>
        /// <param name="sectionID"></param>
        /// <returns></returns>
        public List<ScreeningScoreLevel> GetScoreLevelsBySectionID(string sectionID)
        {
            List<ScreeningScoreLevel> list = new List<ScreeningScoreLevel>();
            string sql = @"
SELECT ScreeningSectionID, ScoreLevel, Name, Label
FROM dbo.ScreeningScoreLevel
WHERE ScreeningSectionID = @ScreeningSectionID
ORDER BY ScoreLevel ASC
";
            CommandObject.Parameters.Clear();
            AddParameter("@ScreeningSectionID", DbType.AnsiString, 5).Value = SqlParameterSafe(sectionID);
           
            try
            {
                using (var reader = RunSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        list.Add(new ScreeningScoreLevel(reader));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Disconnect();
            }
            return list;
        }
        
        /// <summary>
        /// Get all screening section levels for particular screening and order by screening section id and scoreLevel
        /// </summary>
        /// <param name="screeningID"></param>
        /// <returns></returns>
        public DataSet GetAllScoreLevelsByScreeningID(string screeningID)
        {
            DataSet ds = null;
            
            string sql = @"
SELECT l.ScreeningSectionID, l.ScoreLevel, l.Name, l.Label
FROM dbo.ScreeningScoreLevel l INNER JOIN 
    dbo.ScreeningSection s ON l.ScreeningSectionID = s.ScreeningSectionID
WHERE s.ScreeningID = @ScreeningID
ORDER BY s.ScreeningSectionID,  l.ScoreLevel ASC
";
            CommandObject.Parameters.Clear();
            AddParameter("@ScreeningID", DbType.AnsiString, 4).Value = SqlParameterSafe(screeningID);

            try
            {
                ds = GetDataSet(sql);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Disconnect();
            }
            return ds;
        }
    }
}
