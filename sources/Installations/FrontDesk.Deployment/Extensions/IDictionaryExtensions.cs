using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace FrontDesk.Deployment.Extensions
{
    public static class IDictionaryExtensions
    {
        public static void AddOrUpdate(this IDictionary dictionary, object key, object value)
        {
            if (dictionary.Contains(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }
        }

    }
}
