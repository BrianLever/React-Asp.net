using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FrontDesk.Common.Extensions
{
    public static class DataReaderExtensions
    {
        public static  T? Get<T>(this IDataReader reader, int columnIndex) where T: struct 
        {
            if (reader.IsDBNull(columnIndex))
            {
                return null;
            }

            return (T)Convert.ChangeType(reader[columnIndex], typeof (T));
        }

        #region IDataReader
        public static T Get<T>(this IDataReader dataRow, string columnName, T defaultValue)
        {
            var value = dataRow[columnName];
            return Convert.IsDBNull(value) ? defaultValue : value.ConvertToType<T>();
        }

        public static T Get<T>(this IDataReader dataRow, int ordinal, T defaultValue)
        {
            var value = dataRow[ordinal];
            return Convert.IsDBNull(value) ? defaultValue : value.ConvertToType<T>();
        }

        public static T? GetNullable<T>(this IDataReader dataRow, string columnName) where T : struct
        {
            var value = dataRow[columnName];
            if (Convert.IsDBNull(value)) value = null;

            return value.ConvertToNullableType<T>();
        }

        public static T? GetNullable<T>(this IDataReader dataRow, int ordinal) where T : struct
        {
            var value = dataRow[ordinal];
            if (Convert.IsDBNull(value)) value = null;

            return value.ConvertToNullableType<T>();
        }

        public static T Get<T>(this IDataReader dataRow, string columnName, bool throwOnNullValue = false)
        {
            if (throwOnNullValue && Convert.IsDBNull(dataRow[columnName]))
                throw new ArgumentException("Column {0} value is null but the column is not nullable.".FormatWith(columnName));

            var defValue = default(T);

            if (typeof(T) == typeof(String))
            {
                defValue = (T)Convert.ChangeType(string.Empty, typeof(T));
            }


            return dataRow.Get<T>(columnName, defValue);
        }

        public static T Get<T>(this IDataReader dataRow, int ordinal, bool throwOnNullValue = false)
        {
            if (throwOnNullValue && Convert.IsDBNull(dataRow[ordinal]))
                throw new ArgumentException("Column {0} value is null but the column is not nullable.".FormatWith(ordinal));

            var defValue = default(T);

            if (typeof(T) == typeof(String))
            {
                defValue = (T)Convert.ChangeType(string.Empty, typeof(T));
            }


            return dataRow.Get<T>(ordinal, defValue);
        }

        //public static List<int> GetAsCommaSeperatedList(this IDataReader dataRow, string columnName)
        //{
        //    var value = dataRow[columnName];

        //    if( Convert.IsDBNull(value) || value == null) return new List<int>();

        //    return ((string)value).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
        //        .Select(x => Int32.Parse(x)).ToList() ;
        //}

        public static List<T> GetFromCsv<T>(this IDataReader dataRow, string columnName)
        {
            var value = dataRow[columnName];

            if (Convert.IsDBNull(value) || value == null) return new List<T>();

            return ((string)value).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.As<T>()).ToList();
        }



        #endregion


        /// <summary>
        /// Get list of column names for searching for optional fields in the Business Object Class initializing
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static string[] GetColumnNames(this IDataReader reader)
        {
            var columnList = new string[reader.FieldCount];

            for (int i = 0; i < reader.FieldCount; columnList[i] = reader.GetName(i++)) ;
            return columnList;
        }
    }
}
