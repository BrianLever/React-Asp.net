using FrontDesk.Common;
using FrontDesk.Common.Data;
using FrontDesk.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace FrontDesk.Server.Data.BhsVisits
{
    public class TypeaheadDb : DBDatabase, ITypeaheadDataSource
    {
        private readonly string _tableName;

        public TypeaheadDb(string tableName) : base(0)
        {
            this._tableName = tableName;
        }

        internal TypeaheadDb(DbConnection sharedConnection) : base(sharedConnection) { }

        public List<string> GetList(string query = null)
        {
            List<string> result = new List<string>();

            ClearParameters();

            var sql = new QueryBuilder($"SELECT VALUE FROM dbo.[{ _tableName }]");

            if (!string.IsNullOrEmpty(query))
            {
                sql.AppendWhereCondition("Value like @Filter", ClauseType.And);
                AddParameter("@filter", System.Data.DbType.String).Value = SqlLikeStringPrepeare(query, LikeCondition.Contains);
            }
            sql.AppendOrderCondition("1", OrderType.Asc);

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

        public List<string> GetModifiedValues(DateTime lastModifiedDateUTC)
        {
            List<string> result = new List<string>();

            string sql = $"SELECT VALUE FROM dbo.[{ _tableName }] WHERE LastModifiedDateUTC > @LastModifiedDateUTC ORDER BY VALUE";

            ClearParameters();
            AddParameter("@LastModifiedDateUTC", DbType.DateTime).Value = lastModifiedDateUTC;

            try
            {
                using (var reader = RunSelectQuery(sql))
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
    }
}
