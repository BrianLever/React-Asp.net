using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrontDesk.Common.Extensions.JsonContractResolvers;
using Newtonsoft.Json;

namespace FrontDesk.Common.Extensions
{
	public static class JsonExtensions
	{
		public static string ToJson<T>(this T model)
		{
			return JsonConvert.SerializeObject(model);
		}


        /// <summary>
        /// Guarantees that only properties of the target class and its base clases appears in target JSON string.
        /// </summary>
        /// <typeparam name="T">Target type that uses as a filter for properties serialization</typeparam>
        /// <param name="model">model to serialize</param>
        /// <returns>JSON string</returns>
        public static string ToJsonStrict<T>(this T model)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new ContextTypeOnlyJsonContractResolver<T>()
            };

            return JsonConvert.SerializeObject(model, settings);
        }
    }
}
