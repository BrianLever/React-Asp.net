using FrontDesk.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Common
{
    public class LookupValue<T> where T : struct
    {
        public T Id { get; set; }
        public string Name { get; set; }
        public int OrderIndex { get; set; }
    }


    public class LookupValue : LookupValue<int>
    {
       
    }

    public static class LookupValueExtensions
    {
        public static LookupValue AsLookupValue(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            return new LookupValue
            {
                Id = value.As<int>()
            };
        }


        public static string ToSqlCsv<T>(this IEnumerable<LookupValue<T>> list) where T : struct
        {
            return list.ToCsv(x => x.Id);
        }

        public static string ToCsv<TId, TProperty>(this IEnumerable<LookupValue<TId>> list, Func<LookupValue<TId>, TProperty> prop) 
            where TId : struct
        {
            return list.ToCsv(prop, ",");
        }

        public static string ToCsv<TId, TProperty>(
                this IEnumerable<LookupValue<TId>> list, 
                Func<LookupValue<TId>, TProperty> prop,
                string delimiter)
            where TId : struct
        {
            if (list == null) return string.Empty;

            return string.Join(delimiter, list.Select(prop));
        }
    }


   
}
