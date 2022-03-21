using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using FrontDesk.Common.Data;
using FrontDesk.Configuration;

namespace FrontDesk.Server.Data.Configuration
{
    [Obsolete]
    /// <summary>
    /// Server-side implementation of IScreeningFrequencyRepository
    /// </summary>
    public class ScreeningFrequencyDatabase : DBDatabase, IScreeningFrequencyRepository
    {
        #region constructors

        public ScreeningFrequencyDatabase(string connectionString)
            : base(connectionString) { }

        public ScreeningFrequencyDatabase()
            : base(ConfigurationManager.ConnectionStrings[0].ConnectionString)
        { }

        public ScreeningFrequencyDatabase(DbConnection sharedConnection)
            : base(sharedConnection)
        { }

        #endregion

        #region IScreeningFrequencyRepository Members


        public void Save(IEnumerable<ScreeningFrequencyItem> items)
        {

             if (items == null) 
                 throw new ArgumentNullException("items");

           //update only those records which have been changed
            var sql = @"
IF NOT EXISTS(SELECT NULL FROM dbo.ScreeningFrequency WHERE ID = @ID)
    INSERT INTO dbo.ScreeningFrequency(ID, Frequency, LastModifiedDateUTC)
    VALUES(@ID, @Frequency, @LastModifiedDateUTC)
ELSE
    UPDATE dbo.ScreeningFrequency
        SET Frequency = @Frequency, LastModifiedDateUTC = @LastModifiedDateUTC
    WHERE ID = @ID AND Frequency <> @Frequency
";

            CommandObject.Parameters.Clear();
            var parID = AddParameter("@ID", DbType.AnsiString);
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

                    RunNonSelectQuery(sql);
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

        public ScreeningFrequencyItem Get(string ID)
        {
            if (string.IsNullOrWhiteSpace(ID))
                throw new ArgumentNullException("ID");

            ScreeningFrequencyItem result = null;

            string sqlText = @"SELECT ID, Frequency, LastModifiedDateUTC FROM dbo.ScreeningFrequency WHERE ID = @ID";

            CommandObject.Parameters.Clear();
            AddParameter("@ID", DbType.AnsiString).Value = ID;

            try
            {

                Connect();

                using (var reader = RunSelectQuery(sqlText))
                {

                    if (reader.Read())
                    {
                        result = new ScreeningFrequencyItem
                        {
                            ID = reader.GetString(0),
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

        public IEnumerable<ScreeningFrequencyItem> GetAll()
        {
            List<ScreeningFrequencyItem> result = new List<ScreeningFrequencyItem>();

            string sqlText = @"
SELECT f.ID, f.Frequency, f.LastModifiedDateUTC 
FROM dbo.ScreeningFrequency f LEFT JOIN dbo.ScreeningSection sc ON f.ID = sc.ScreeningSectionID 
ORDER BY ISNULL(OrderIndex, 0) ASC, ID ASC";


            try
            {
                CommandObject.Parameters.Clear();

                Connect();

                using (var reader = RunSelectQuery(sqlText))
                {

                    while (reader.Read())
                    {
                        result.Add(new ScreeningFrequencyItem { 
                            ID = reader.GetString(0),
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


        #endregion

    }
}
