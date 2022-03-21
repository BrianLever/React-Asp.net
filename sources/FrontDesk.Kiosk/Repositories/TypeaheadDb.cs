using System;
using System.Collections.Generic;
using System.Data;
using FrontDesk.Common;
using FrontDesk.Common.Data;

namespace FrontDesk.Kiosk.Repositories
{
    public class TypeaheadDb : DBCompactDatabase
    {
        private readonly string _tableName;

        public TypeaheadDb(string tableName) : base()
        {
            _tableName = tableName;
        }

        internal TypeaheadDb(string tableName, string connectionString) : base(connectionString)
        {
            _tableName = tableName;
        }


        public List<string> GetList(string query = null)
        {
            List<string> result = new List<string>();

            ClearParameters();

            var sql = new QueryBuilder($"SELECT VALUE FROM { _tableName }");

            if (!string.IsNullOrEmpty(query))
            {
                sql.AppendWhereCondition("Value like @Filter", ClauseType.And);
                AddParameter("@filter", System.Data.DbType.String).Value = SqlLikeStringPrepeare(query, LikeCondition.Contains);
            }
            sql.AppendOrderCondition("VALUE", OrderType.Desc);

            try
            {
                using (var reader = RunSelectQuery(sql.ToString()))
                {
                    while (reader.Read())
                    {
                        result.Add(reader.GetString(0));
                    }
                }
            }
            finally
            {
                Disconnect();
            }
            return result;
        }


        public void UpdateValues(ICollection<LookupValue> newValues, DateTime lastModifiedDateUTC)
        {

            var selectSql = $"SELECT 1 FROM { _tableName } WHERE Value = @Value";
            var insertSql = $"INSERT INTO { _tableName }(Value, LastModifiedDateUTC) VALUES (@Value, @LastModifiedDateUTC)";
           

            ClearParameters();
            AddParameter("@LastModifiedDateUTC", DbType.DateTime).Value = lastModifiedDateUTC;
            var valueParam = AddParameter("@Value", DbType.String, 128);

            try
            {

                foreach (var item in newValues)
                {
                    try
                    {
                        valueParam.Value = item.Name;

                        var exists = RunScalarQuery<int>(selectSql).HasValue;

                        if (!exists)
                        {
                            RunNonSelectQuery(insertSql);
                        }


                    }
                    catch (Exception)
                    {
                        throw;
                    }

                }

            }
            finally
            {
                Disconnect();
            }
        }


        public void DeleteValues(ICollection<LookupValue> values)
        {
            var deleteSql = $"DELETE FROM { _tableName } WHERE Value = @Value";


            ClearParameters();
            var valueParam = AddParameter("@Value", DbType.String, 128);

            try
            {

                foreach (var item in values)
                {
                    try
                    {
                        valueParam.Value = item.Name;

                        RunNonSelectQuery(deleteSql);

                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            finally
            {
                Disconnect();
            }
        }

    }
}
