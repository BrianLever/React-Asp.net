using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

using FrontDesk.Common;
using FrontDesk.Common.Data;
using FrontDesk.Common.Extensions;

namespace FrontDesk.Server.Data.BhsVisits
{
    public class LookupValueDb : DBDatabase, ILookupValueSource
    {
        private readonly string _tableName;

        public LookupValueDb(string tableName) : base(0)
        {
            this._tableName = tableName;
        }

        internal LookupValueDb(DbConnection sharedConnection) : base(sharedConnection) { }

        public List<LookupValue> GetList()
        {
            List<LookupValue> result = new List<LookupValue>();

            var sql = $"SELECT ID, Name, OrderIndex FROM dbo.[{_tableName}] ORDER BY OrderIndex ASC";
            try
            {
                using (var reader = RunSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        result.Add(new LookupValue
                        {
                            Id = reader.Get<int>("ID"),
                            Name = reader.Get<string>("Name"),
                            OrderIndex = reader.Get<int>("OrderIndex")
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



        public List<LookupValue> GetModifiedValues(DateTime lastModifiedDateUTC)
        {
            List<LookupValue> result = new List<LookupValue>();

            var sql = $"SELECT ID, Name, OrderIndex FROM dbo.[{_tableName}] WHERE LastModifiedDateUTC > @LastModifiedDateUTC ORDER BY OrderIndex ASC";
            try
            {

                ClearParameters();
                AddParameter("@LastModifiedDateUTC", DbType.DateTime).Value = SqlParameterSafe(lastModifiedDateUTC);

                using (var reader = RunSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        result.Add(new LookupValue
                        {
                            Id = reader.Get<int>("ID"),
                            Name = reader.Get<string>("Name"),
                            OrderIndex = reader.Get<int>("OrderIndex")
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

    }
}
