using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace FrontDesk.Common.Configuration
{
    public abstract class AppSettingsProxy
    {
        public static string GetStringValue(string key, string defaultValue)
        {
            string result;
            string value = ConfigurationManager.AppSettings[key];
            result = String.IsNullOrEmpty(value) ? defaultValue : value;

            return result;
        }
        public static int GetIntValue(string key, int defaultValue)
        {
            int settingValue = defaultValue;
            try
            {
                string value = ConfigurationManager.AppSettings[key];
                if (!Int32.TryParse(value, out settingValue))
                    settingValue = defaultValue;
            }
            catch
            {
                settingValue = defaultValue;
            }

            return settingValue;
        }

        public static double GetDoubleValue(string key, double defaultValue)
        {
            double settingValue = defaultValue;
            try
            {
                string value = ConfigurationManager.AppSettings[key];
                if (!Double.TryParse(value, out settingValue))
                    settingValue = defaultValue;
            }
            catch
            {
                settingValue = defaultValue;
            }

            return settingValue;
        }

        public static decimal GetDecimalValue(string key, decimal defaultValue)
        {
            decimal settingValue = defaultValue;
            try
            {
                string value = ConfigurationManager.AppSettings[key];
                if (!Decimal.TryParse(value, out settingValue))
                    settingValue = defaultValue;
            }
            catch
            {
                settingValue = defaultValue;
            }

            return settingValue;
        }

        public static Int64 GetDecimalValue(string key, Int64 defaultValue)
        {
            Int64 settingValue = defaultValue;
            try
            {
                string value = ConfigurationManager.AppSettings[key];
                if (!Int64.TryParse(value, out settingValue))
                    settingValue = defaultValue;
            }
            catch
            {
                settingValue = defaultValue;
            }

            return settingValue;
        }

        public static bool GetBooleanValue(string key, bool defaultValue)
        {
            bool settingValue = defaultValue;
            try
            {
                string value = ConfigurationManager.AppSettings[key];
                if (!Boolean.TryParse(value, out settingValue))
                {
                    settingValue = defaultValue;
                }
            }
            catch
            {
                settingValue = defaultValue;
            }
            return settingValue;
        }

        public static bool HasValue(string key)
        {
            return ConfigurationManager.AppSettings[key] != null;
        }
    }
}
