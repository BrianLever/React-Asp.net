using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using FrontDesk.Common.Data;


namespace RPMS.Common.GlobalConfiguration
{
    public class GlobalSettingsDatabase : DBDatabase, IGlobalSettingsRepository
    {
        public GlobalSettingsDatabase() : base()
        {
            this.ConnectionString = ConfigurationManager.ConnectionStrings["LocalConnection"].ConnectionString;

        }

        public GlobalSettingsDatabase(DbConnection sharedConnection)
        : base(sharedConnection)
        {
        }


        public virtual List<GlobalSettingsItem> GetAll()
        {
            var resut = new List<GlobalSettingsItem>();
            const string sql =
                 @"SELECT [Key], [Value] FROM dbo.[SystemSettings] ORDER BY [Key] ASC";

            ClearParameters();

            try
            {
                using (var reader = RunSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        resut.Add(new GlobalSettingsItem
                        {
                            Key = reader.GetString(0),
                            Value = reader.GetString(1),
                        });
                    }

                }
            }
            finally
            {
                Disconnect();
            }

            return resut;
        }

        public GlobalSettingsItem Get(string key)
        {
            const string sql =
                 @"SELECT [Key], [Value] FROM dbo.[SystemSettings] WHERE [Key] = @Key";

            ClearParameters();
            AddParameter("@Key", System.Data.DbType.AnsiString).Value = key;

            try
            {
                using (var reader = RunSelectQuery(sql))
                {
                    if (reader.Read())
                    {
                        return new GlobalSettingsItem
                        {
                            Key = reader.GetString(0),
                            Value = reader.GetString(1),
                        };
                    }

                }
            }
            finally
            {
                Disconnect();
            }

            return null;
        }
    }
}