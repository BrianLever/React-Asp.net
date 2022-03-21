namespace FrontDesk.Server.Data.ScreeningProfile
{
    using FrontDesk.Common.Data;
    using FrontDesk.Common.Extensions;

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;


    public interface IScreeningProfileMinimalAgeRepository
    {
        void UpdateMinimalAgeSettings(int screeningProfileId, IEnumerable<ScreeningSectionAge> ageSettings);
        IList<ScreeningSectionAge> GetSectionMinimalAgeSettings(int screeningProfileId, string screeningSectionID);
    }


    public interface IKioskMinimalAgeRepository
    {
        void UpdateMinimalAgeSettings(int screeningProfileId, IEnumerable<ScreeningSectionAge> ageSettings);
        IList<ScreeningSectionAge> GetModifiedSectionMinimalAgeSettingsForKiosk(short kioskId, DateTime lastModifiedDateUTC);

    }

    public class ScreeningProfileMinimalAgeDatabase : DBDatabase, IScreeningProfileMinimalAgeRepository, IKioskMinimalAgeRepository
    {
        public ScreeningProfileMinimalAgeDatabase() : base(0)
        {

        }

        public ScreeningProfileMinimalAgeDatabase(DbConnection sharedConnection) : base(sharedConnection)
        {

        }

        /// <summary>
        /// Save changes to the Section Minimal Age parameters
        /// </summary>
        /// <param name="ageSettings"></param>
        public void UpdateMinimalAgeSettings(int screeningProfileId, IEnumerable<ScreeningSectionAge> ageSettings)
        {
            if (ageSettings == null) throw new ArgumentNullException("ageSettings");

            var sql = "[dbo].[uspUpdateScreeningProfileAgeSettings]";

            CommandObject.Parameters.Clear();
            var parScreeningProfileID = AddParameter("@ScreeningProfileID", DbType.Int32).Value = screeningProfileId;

            var parScreeningSectionID = AddParameter("@ScreeningSectionID", DbType.AnsiStringFixedLength, 5);
            var parMinimalAge = AddParameter("@MinimalAge", DbType.Byte);
            var parIsEnabled = AddParameter("@IsEnabled", DbType.Boolean);
            var parLastModifiedDateUTC = AddParameter("@LastModifiedDateUTC", DbType.DateTime);


            try
            {
                BeginTransaction();

                foreach (var item in ageSettings)
                {
                    parScreeningSectionID.Value = SqlParameterSafe(item.ScreeningSectionID);
                    parMinimalAge.Value = item.MinimalAge;
                    parIsEnabled.Value = item.IsEnabled;
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


        /// <summary>
        /// Get Minimal Age settings for screening sections
        /// </summary>
        public IList<ScreeningSectionAge> GetSectionMinimalAgeSettings(int screeningProfileId, string screeningSectionID)
        {
            List<ScreeningSectionAge> list = new List<ScreeningSectionAge>();

            var sql = new QueryBuilder(@"
SELECT 
    s.ScreeningSectionID, 
    s.ScreeningSectionName, 
    ISNULL(a.MinimalAge,0), 
    ISNULL(a.IsEnabled, 1),
    ISNULL(a.LastModifiedDateUTC, GETUTCDATE()),
    ISNULL(a.AgeIsNotConfigurable, 0) as AgeIsNotConfigurable
FROM dbo.ScreeningSection s
     LEFT JOIN dbo.ScreeningProfileSectionAge a ON a.ScreeningSectionID = s.ScreeningSectionID
");

            CommandObject.Parameters.Clear();

            sql.AppendWhereCondition("a.ScreeningProfileID = @ScreeningProfileID", ClauseType.And);
            AddParameter("@ScreeningProfileID", DbType.Int32).Value = screeningProfileId;

            sql.AppendOrderCondition("s.OrderIndex", OrderType.Asc);
            if (!string.IsNullOrEmpty(screeningSectionID))
            {
                sql.AppendWhereCondition("s.ScreeningSectionID = @ScreeningSectionID", ClauseType.And);
                AddParameter("@ScreeningSectionID", DbType.AnsiStringFixedLength, 5).Value = screeningSectionID;
            }

            try
            {
                using (var reader = RunSelectQuery(sql.ToString()))
                {
                    while (reader.Read())
                    {
                        list.Add(new ScreeningSectionAge(
                                Convert.ToString(reader[0]).TrimEnd(),
                                Convert.ToString(reader[1]).TrimEnd(),
                                Convert.ToByte(reader[2]),
                                Convert.ToBoolean(reader[3]),
                                reader.Get<int>(5, true) == 1,
                                reader.Get<DateTime>(4, true)
                        ));
                    }
                }
            }
            catch (Exception) { throw; }
            finally { Disconnect(); }

            return list;
        }

        public IList<ScreeningSectionAge> GetModifiedSectionMinimalAgeSettingsForKiosk(short kioskId, DateTime lastModifiedDateUTC)
        {
            IList<ScreeningSectionAge> list = new List<ScreeningSectionAge>();

            var sql = @"dbo.uspGetModifiedSectionMinimalAgeSettingsForKiosk";

            CommandObject.Parameters.Clear();
            AddParameter("@LastModifiedDateUTC", DbType.DateTime).Value = lastModifiedDateUTC;
            AddParameter("@KioskID", DbType.Int16).Value = kioskId;

            try
            {
                using (var reader = RunProcedureSelectQuery(sql.ToString()))
                {
                    while (reader.Read())
                    {
                        list.Add(new ScreeningSectionAge()
                        {
                            ScreeningSectionID = Convert.ToString(reader[0]).TrimEnd(),
                            MinimalAge = Convert.ToByte(reader[1]),
                            IsEnabled = Convert.ToBoolean(reader[2]),
                            LastModifiedDateUTC = Convert.ToDateTime(reader[3])
                        });
                    }
                }
            }
            catch (Exception) { throw; }
            finally { Disconnect(); }
            return list;
        }

    }
}
