using FrontDesk.Common.Data;
using FrontDesk.Configuration;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;

namespace FrontDesk.Server.Data.ScreenngProfile
{
    /// <summary>
    /// Server-side implementation of IScreeningFrequencyRepository
    /// </summary>
    public class ScreeningProfileFrequencyDb : DBDatabase, IScreeningProfileFrequencyRepository
    {
        #region constructors

        public ScreeningProfileFrequencyDb(string connectionString)
            : base(connectionString) { }

        public ScreeningProfileFrequencyDb()
            : base(ConfigurationManager.ConnectionStrings[0].ConnectionString)
        { }

        public ScreeningProfileFrequencyDb(DbConnection sharedConnection)
            : base(sharedConnection)
        { }

        #endregion

        public void Save(int screeningProfileId, IEnumerable<ScreeningFrequencyItem> items)
        {

            if (items == null)
                throw new ArgumentNullException("items");

            //update only those records which have been changed
            var sql = @"[dbo].[uspUpdateScreeningProfileFrequency]";

            CommandObject.Parameters.Clear();
            AddParameter("@ScreeningProfileID", DbType.Int32).Value = screeningProfileId;

            var parID = AddParameter("@ScreeningSectionID", DbType.AnsiString);
            var parFrequency = AddParameter("@Frequency", DbType.Int32);
            var parLastModifiedDateUTC = AddParameter("@LastModifiedDateUTC", DbType.DateTime);


            try
            {
                BeginTransaction();

                foreach (var item in items)
                {
                    parID.Value = SqlParameterSafe(item.ID);
                    parFrequency.Value = item.Frequency;
                    parLastModifiedDateUTC.Value = item.LastModifiedDateUTC;

                    RunProcedureNonSelectQuery(sql);
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

        public ScreeningFrequencyItem Get(int screeningProfileId, string ScreeningSectionID)
        {
            if (string.IsNullOrWhiteSpace(ScreeningSectionID))
                throw new ArgumentNullException(nameof(ScreeningSectionID));

            ScreeningFrequencyItem result = null;

            string sqlText = @"
SELECT ScreeningSectionID, Frequency, LastModifiedDateUTC 
FROM dbo.ScreeningProfileFrequency 
WHERE ScreeningProfileID = @ScreeningProfileID AND ScreeningSectionID = @ScreeningSectionID";

            CommandObject.Parameters.Clear();
            AddParameter("@ScreeningProfileID", DbType.Int32).Value = screeningProfileId;
            AddParameter("@ScreeningSectionID", DbType.AnsiString).Value = ScreeningSectionID;

            try
            {

                Connect();

                using (var reader = RunSelectQuery(sqlText))
                {

                    if (reader.Read())
                    {
                        result = new ScreeningFrequencyItem
                        {
                            ID = reader.GetString(0).TrimEnd(),
                            Frequency = reader.GetInt32(1),
                            LastModifiedDateUTC = reader.GetDateTime(2)
                        };
                    }
                }

            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                base.Disconnect();
            }
            return result;
        }

        public IEnumerable<ScreeningFrequencyItem> GetAll(int screeningProfileId)
        {
            List<ScreeningFrequencyItem> result = new List<ScreeningFrequencyItem>();

            string sqlText = @"
SELECT f.ScreeningSectionID, f.Frequency, f.LastModifiedDateUTC 
FROM dbo.ScreeningProfileFrequency f 
    LEFT JOIN dbo.ScreeningSection sc ON f.ScreeningSectionID = sc.ScreeningSectionID 
WHERE f.ScreeningProfileID = @ScreeningProfileID
ORDER BY ISNULL(OrderIndex, 0) ASC, ScreeningSectionID ASC";


            try
            {
                CommandObject.Parameters.Clear();
                AddParameter("@ScreeningProfileID", DbType.Int32).Value = screeningProfileId;

                Connect();

                using (var reader = RunSelectQuery(sqlText))
                {

                    while (reader.Read())
                    {
                        result.Add(new ScreeningFrequencyItem
                        {
                            ID = reader.GetString(0).TrimEnd(),
                            Frequency = reader.GetInt32(1),
                            LastModifiedDateUTC = reader.GetDateTime(2)
                        });
                    }
                }

            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                base.Disconnect();
            }
            return result;
        }

    }
}
