using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using FrontDesk.Common.Data;

namespace FrontDesk.Data
{
    internal class ScreeningKioskDb : DBCompactDatabase, FrontDesk.Data.IScreeningAnswersDb, IScreeningKioskDb
    {
        #region Constructors
        internal ScreeningKioskDb() : base() { }


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
FROM AnswerScaleOption
ORDER BY AnswerScaleID, AnswerScaleOptionID";

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
FROM AnswerScaleOption
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
                            Value = reader.GetInt32(4)
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
            string sql = @"
SELECT ScreeningSectionID, ScreeningSectionName
FROM ScreeningSection
WHERE ScreeningID = @ScreeningID
ORDER BY OrderIndex ASC
";
            string sqlQuestions = @"
SELECT q.QuestionID, q.ScreeningSectionID, q.PreambleText, q.QuestionText, q.AnswerScaleID, q.IsMainQuestion
FROM ScreeningSectionQuestion q INNER JOIN ScreeningSection s ON q.ScreeningSectionID = s.ScreeningSectionID
WHERE s.ScreeningID = @ScreeningID
ORDER BY s.OrderIndex ASC, q.OrderIndex ASC, q.QuestionID ASC
";
            CommandObject.Parameters.Clear();
            AddParameter("@ScreeningID", DbType.String, 4).Value = screeningID;
            try
            {
                using (var reader = RunSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        sections.Add(new ScreeningSection()
                        {
                            ScreeningSectionID = reader.GetString(0),
                            ScreeningSectionName = reader.GetString(1),
                            ScreeningID = screeningID
                        });
                    }
                }
                using (var reader = RunSelectQuery(sqlQuestions))
                {
                    while (reader.Read())
                    {

                        question = new ScreeningSectionQuestion()
                        {
                            QuestionID = reader.GetInt32(0),
                            ScreeningSectionID = reader.GetString(1),
                            PreambleText = Convert.ToString(reader[2]),
                            QuestionText = Convert.ToString(reader[3]),
                            AnswerScaleID = reader.GetInt32(4),
                            IsMainQuestion = Convert.ToBoolean(reader[5])

                        };

                        var section = sections.FirstOrDefault(p => p.ScreeningSectionID == question.ScreeningSectionID);
                        if (section != null)
                        {
                            section.Questions.Add(question);
                        }
                    }

                }
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
        public string GetAvailableScreening()
        {
            string id = string.Empty;
            string sql = @"
SELECT TOP(1) ScreeningID
FROM Screening
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

        #region Age Selections

        public ScreeningSectionAge GetMinimalAgeForScreeningSection(string screeningSectionID)
        {
            ScreeningSectionAge age = null;

            var sql = new QueryBuilder(@"
SELECT a.MinimalAge, a.IsEnabled, a.LastModifiedDateUTC, a.ScreeningSectionID  
FROM ScreeningSectionAge a 
");

            CommandObject.Parameters.Clear();

            sql.AppendWhereCondition("a.ScreeningSectionID = @ScreeningSectionID", ClauseType.And);
            AddParameter("@ScreeningSectionID", DbType.String, 5).Value = screeningSectionID;



            try
            {
                using (var reader = RunSelectQuery(sql.ToString()))
                {
                    if (reader.Read())
                    {
                        age = new ScreeningSectionAge
                        {
                            MinimalAge = Convert.ToByte(reader[0]),
                            IsEnabled = Convert.ToBoolean(reader[1]),
                            LastModifiedDateUTC = Convert.ToDateTime(reader[2]),
                            ScreeningSectionID = Convert.ToString(reader[3])
                        };
                    }
                }
            }
            catch (Exception) { throw; }
            finally { Disconnect(); }
            return age;
        }

        public void UpdateAgeSettings(ICollection<ScreeningSectionAge> ageSettings)
        {
            if (ageSettings == null) return;

            //update only those records which have been changed
            var sqlCount = "SELECT COUNT(*) FROM ScreeningSectionAge WHERE ScreeningSectionID = @ScreeningSectionID";
            var sqlInsert = @"
INSERT INTO ScreeningSectionAge(ScreeningSectionID, MinimalAge, IsEnabled, LastModifiedDateUTC)
VALUES(@ScreeningSectionID, @MinimalAge, @IsEnabled, @LastModifiedDateUTC)
";
            var sqlUpdate = @"
UPDATE ScreeningSectionAge
    SET MinimalAge = @MinimalAge, IsEnabled = @IsEnabled, LastModifiedDateUTC = @LastModifiedDateUTC
WHERE ScreeningSectionID = @ScreeningSectionID
";

            CommandObject.Parameters.Clear();
            var parScreeningSectionID = AddParameter("@ScreeningSectionID", DbType.String, 5);
            var parMinimalAge = AddParameter("@MinimalAge", DbType.Byte);
            var parIsEnabled = AddParameter("@IsEnabled", DbType.Boolean);

            var parLastModifiedDateUTC = AddParameter("@LastModifiedDateUTC", DbType.DateTime);
            int count;

            try
            {
                BeginTransaction();

                foreach (var item in ageSettings)
                {
                    parScreeningSectionID.Value = SqlParameterSafe(item.ScreeningSectionID.TrimEnd());
                    parMinimalAge.Value = item.MinimalAge;
                    parIsEnabled.Value = item.IsEnabled;
                    parLastModifiedDateUTC.Value = item.LastModifiedDateUTC;

                    count = Convert.ToInt32(RunScalarQuery(sqlCount));
                    if (count > 0) RunNonSelectQuery(sqlUpdate);
                    else RunNonSelectQuery(sqlInsert);
                }

                CommitTransaction();
            }
            catch (Exception)
            {
                RollbackTransaction();
                throw;
            }
            finally { Disconnect(); }
        }


        public DateTime? GetMaxAgeSettingsModifiedDateUTC()
        {
            DateTime? maxDate = null;

            //update only those records which have been changed
            var sql = "SELECT MAX(LastModifiedDateUTC) FROM ScreeningSectionAge";
            try
            {
                object max = RunScalarQuery(sql);
                if (max != null && !Convert.IsDBNull(max))
                {
                    maxDate = Convert.ToDateTime(max);
                }

            }
            catch (Exception)
            {
                throw;
            }
            finally { Disconnect(); }

            return maxDate;
        }

        #endregion
    }
}
