using System;
using System.Collections.Generic;
using System.Data;

using FrontDesk.Common;
using FrontDesk.Common.Data;
using FrontDesk.Common.Extensions;

namespace FrontDesk.Server.Data.KioskDataValues
{
    public interface ILookupValuesDeleteLogDb
    {
        List<LookupValueDeleteLogItem> Get(DateTime lastModifiedDateUTC);
    }

    public class LookupValuesDeleteLogDb : DBDatabase, ILookupValuesDeleteLogDb
    {
        public LookupValuesDeleteLogDb(): base(0)
        {

        }


        public List<LookupValueDeleteLogItem> Get(DateTime lastModifiedDateUTC)
        {
            var result = new List<LookupValueDeleteLogItem>();

            var sql = $"SELECT TableName, ID, Name FROM [dbo].[LookupValuesDeleteLog] WHERE LastModifiedDateUTC > @LastModifiedDateUTC";
            try
            {
                ClearParameters();
                AddParameter("@LastModifiedDateUTC", DbType.DateTime).Value = SqlParameterSafe(lastModifiedDateUTC);

                using (var reader = RunSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        result.Add(new LookupValueDeleteLogItem
                        {
                            TableName = reader.Get<string>("TableName"),
                            Id = reader.GetNullable<int>("ID") ?? 0,
                            Name = reader.Get<string>("Name") ?? string.Empty
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
