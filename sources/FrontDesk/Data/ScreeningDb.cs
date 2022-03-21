using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using FrontDesk.Common.Data;

namespace FrontDesk.Data
{
    internal class ScreeningDb : DBDatabase, IScreeningAnswersDb
    {
        #region Constructors
        internal ScreeningDb() : base(0) { }

        internal ScreeningDb(DbConnection sharedConnection) : base(sharedConnection) { }

        #endregion

        #region Answer Scale and Options

        /// <summary>
        /// Get all answer scales from the database.
        /// </summary>
        /// <returns>Dictionary, where AnswerScale ID is a key</returns>
        public Dictionary<int, AnswerScale> GetAnswerOptions()
        {
            Dictionary<int, AnswerScale> scales = new Dictionary<int, AnswerScale>();
            AnswerScale scale = null;
            AnswerScaleOption option = null;


            var sql = @"
SELECT AnswerScaleOptionID, AnswerScaleID, OptionText, OptionValue
FROM dbo.AnswerScaleOption
ORDER BY AnswerScaleID ASC, AnswerScaleOptionID ASC";

            try
            {
                Connect();
                using (var reader = RunSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        option = new AnswerScaleOption()
                        {
                            AnswerScaleOptionID = reader.GetInt32(0),
                            AnswerScaleID = reader.GetInt32(1),
                            Text = reader.GetString(2),
                            Value = reader.GetInt32(3)
                        };
                        if (!scales.TryGetValue(option.AnswerScaleID, out scale))
                        {
                            scale = new AnswerScale(option.AnswerScaleID);
                            scales.Add(scale.AnswerScaleID, scale);
                        }

                        //add option
                        scale.Options.Add(option);
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
            return scales;
        }
        /// <summary>
        /// Get all answer options for particular scale
        /// </summary>
        /// <returns>Dictionary, where AnswerScale ID is a key</returns>
        public List<AnswerScaleOption> GetAnswerOptions(int answerScaleID)
        {
            List<AnswerScaleOption> options = new List<AnswerScaleOption>();
       

            var sql = @"
SELECT AnswerScaleOptionID, AnswerScaleID, OptionText, OptionValue
FROM dbo.AnswerScaleOption
WHERE AnswerScaleID = @AnswerScaleID
ORDER BY AnswerScaleOptionID ASC";

            AddParameter("@AnswerScaleID", DbType.Int32).Value = answerScaleID;

            try
            {
                Connect();
                using (var reader = RunSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        options.Add(new AnswerScaleOption()
                        {
                            AnswerScaleOptionID = reader.GetInt32(0),
                            AnswerScaleID = reader.GetInt32(1),
                            Text = reader.GetString(2),
                            Value = reader.GetInt32(3)
                        });

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
            return options;
        }


        #endregion

        #region Screening Sections
        /// <summary>
        /// Get screening sections
        /// </summary>
        /// <param name="screeningID"></param>
        /// <returns></returns>
        internal List<ScreeningSection> GetScreeningSections(string screeningID)
        {
            List<ScreeningSection> sections = new List<ScreeningSection>();
            ScreeningSectionQuestion question = null;
            string sql = @"[dbo].[uspGetScreeningSections]";
            string sqlQuestions = @"[dbo].[uspGetScreeningSectionQuestions]";
            
            CommandObject.Parameters.Clear();
            AddParameter("@ScreeningID", DbType.String, 4).Value = screeningID;
            try
            {
                using (var reader = RunProcedureSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        sections.Add(new ScreeningSection()
                        {
                            ScreeningSectionID = reader.GetString(0).TrimEnd(),
                            ScreeningSectionName = reader.GetString(1),
                            //QuestionText = reader.GetString(2),
                            ScreeningSectionShortName = reader.GetString(3),
                            ScreeningID = screeningID
                        });
                    }
                }
                using (var reader = RunProcedureSelectQuery(sqlQuestions))
                {
                    while (reader.Read())
                    {

                        question = new ScreeningSectionQuestion()
                        {
                            QuestionID = reader.GetInt32(0),
                            ScreeningSectionID = reader.GetString(1).TrimEnd(),
                            PreambleText = Convert.ToString(reader[2]),
                            QuestionText = Convert.ToString(reader[3]),
                            AnswerScaleID = reader.GetInt32(4),
                            IsMainQuestion = !reader.IsDBNull(5) && reader.GetBoolean(5),
                            ShowOnlyWhenPossitiveScore = !reader.IsDBNull(6) && reader.GetBoolean(6)
                        };

                        var section = sections.FirstOrDefault(p => p.ScreeningSectionID == question.ScreeningSectionID);
                        if (section != null)
                        {
                            section.Questions.Add(question);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Disconnect();
            }
            return sections;
        }
        /// <summary>
        /// Get first available screening
        /// </summary>
        /// <returns></returns>
        internal string GetAvailableScreening()
        {
            string id = string.Empty;
            string sql = @"
SELECT TOP(1) ScreeningID
FROM dbo.Screening
";
            CommandObject.Parameters.Clear();

            try
            {
                id = Convert.ToString(RunScalarQuery(sql));


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Disconnect();
            }
            return id;
        }
        #endregion
    }
}
