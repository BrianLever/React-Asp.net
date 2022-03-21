using System;
using System.Collections.Generic;
using System.Data;

using FrontDesk.Common;
using FrontDesk.Common.Data;
using FrontDesk.Common.Extensions;

namespace FrontDesk.Kiosk.Repositories
{
    public class LookupValueDb : DBCompactDatabase
    {
        private readonly string _screenName;

        public LookupValueDb(string screenName) : base()
        {
            this._screenName = screenName;
        }

        public List<LookupValue> GetList()
        {
            List<LookupValue> result = new List<LookupValue>();

            ClearParameters();
            AddParameter("@Screen", DbType.String, 32).Value = _screenName;

            var sql = $"SELECT ID, Name FROM LookupValue WHERE Screen = @Screen ORDER BY OrderIndex ASC";
            try
            {
                using (var reader = RunSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        result.Add(new LookupValue
                        {
                            Id = reader.Get<int>("ID"),
                            Name = reader.Get<string>("Name")
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

        public void UpdateValues(ICollection<LookupValue> newValues, DateTime lastModifiedDateUTC)
        {

            var selectSql = $"SELECT 1 FROM LookupValue WHERE Screen = @Screen AND ID = @ID";
            var insertSql = $"INSERT INTO LookupValue(Screen, ID, Name, OrderIndex, LastModifiedDateUTC) VALUES (@Screen, @ID, @Name, @OrderIndex, @LastModifiedDateUTC)";
            var updateSql = $"UPDATE LookupValue " +
                $"SET Name = @Name, OrderIndex = @OrderIndex, LastModifiedDateUTC = @LastModifiedDateUTC " +
                $"WHERE Screen = @Screen AND ID = @ID";

            ClearParameters();
            AddParameter("@Screen", DbType.String, 32).Value = _screenName;
            AddParameter("@LastModifiedDateUTC", DbType.DateTime).Value = lastModifiedDateUTC;

            var idParam = AddParameter("@ID", DbType.Int32);
            var nameParam = AddParameter("@Name", DbType.String, 64);
            var orderIndexParam = AddParameter("@OrderIndex", DbType.Int32);

            try
            {

                foreach (var item in newValues)
                {
                    try
                    {
                        idParam.Value = item.Id;
                        nameParam.Value = item.Name;
                        orderIndexParam.Value = item.OrderIndex;

                        var exists = RunScalarQuery<int>(selectSql).HasValue;

                        if (exists)
                        {
                            var affectedRows = RunNonSelectQuery(updateSql);
                        }
                        else
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

        public void DeleteValues(ICollection<LookupValue> newValues)
        {
            var deleteSql = $"DELETE FROM LookupValue WHERE Screen = @Screen AND ID = @ID";

            ClearParameters();
            AddParameter("@Screen", DbType.String, 32).Value = _screenName;

            var idParam = AddParameter("@ID", DbType.Int32);
            try
            {

                foreach (var item in newValues)
                {
                    try
                    {
                        idParam.Value = item.Id;

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
