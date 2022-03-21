using FrontDesk.Common.Data;
using FrontDesk.Common.Extensions;
using FrontDesk.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace FrontDesk.Server.Data.Configuration
{
    public interface IVisitSettingsRepository
	{
		void Update(List<VisitSettingItem> settings);
		List<VisitSettingItem> Get(string filterById);
        List<VisitSettingItem> GetAll();
    }

    public class VisitSettingsDatabase : DBDatabase, IVisitSettingsRepository
    {
        public VisitSettingsDatabase() : base(0)
        {

        }

        internal VisitSettingsDatabase(DbConnection sharedConnection) : base(sharedConnection)
        {

        }

        public void Update(List<VisitSettingItem> settings)
        {
            if (settings == null) throw new ArgumentNullException("settings");

            //update only those records which have been changed
            var sql = @"
UPDATE dbo.VisitSettings SET 
IsEnabled = @IsEnabled, 
CutScore = @CutScore, 
LastModifiedDateUTC = @LastModifiedDateUTC
WHERE MeasureToolId = @MeasureToolId 
    AND (IsEnabled <> @IsEnabled OR CutScore <> @CutScore)
";
            CommandObject.Parameters.Clear();
            var parScreeningSectionID = AddParameter("@MeasureToolId", DbType.AnsiStringFixedLength, 5);
            var parIsEnabled = AddParameter("@IsEnabled", DbType.Boolean);
            var parCutScore = AddParameter("@CutScore", DbType.Int32);

            var parLastModifiedDateUTC = AddParameter("@LastModifiedDateUTC", DbType.DateTime);
            try
            {
                BeginTransaction();

                foreach (var item in settings)
                {
                    parScreeningSectionID.Value = SqlParameterSafe(item.Id);
                    parIsEnabled.Value = item.IsEnabled;
                    parCutScore.Value = item.CutScore;
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

        public List<VisitSettingItem> GetAll()
        {
            return Get(null);
        }



        public List<VisitSettingItem> Get(string filterById)
        {
            List<VisitSettingItem> list = new List<VisitSettingItem>();

            var sql = new QueryBuilder(@"
SELECT 
    vs.[MeasureToolId], 
    vs.Name, 
    vs.[IsEnabled],
    vs.CutScore,
    ISNULL(vs.LastModifiedDateUTC, GETUTCDATE())
FROM dbo.VisitSettings vs
");

            CommandObject.Parameters.Clear();

            sql.AppendOrderCondition("vs.OrderIndex", OrderType.Asc);
            if (!string.IsNullOrEmpty(filterById))
            {
                sql.AppendWhereCondition("vs.MeasureToolId = @Id", ClauseType.And);
                AddParameter("@Id", DbType.AnsiStringFixedLength, 5).Value = filterById;
            }

            try
            {
                using (var reader = RunSelectQuery(sql.ToString()))
                {
                    while (reader.Read())
                    {
                        list.Add(new VisitSettingItem
                        {
                            Id = Convert.ToString(reader[0]).TrimEnd(),
                            Name = Convert.ToString(reader[1]).TrimEnd(),
                            IsEnabled = Convert.ToBoolean(reader[2]),
                            CutScore = reader.Get<int>(3, false),
                            LastModifiedDateUTC = Convert.ToDateTime(reader[4])

                        });
                    }
                }
            }
            finally { Disconnect(); }
            return list;
        }
    }
}
