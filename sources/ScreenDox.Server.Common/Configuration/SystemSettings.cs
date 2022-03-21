using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using ScreenDox.Server.Common.Infrastructure;
using ScreenDox.Server.Common.Data.Configuration;
using ScreenDox.Server.Models.Configuration;

namespace ScreenDox.Server.Common.Configuration
{
    public class SystemSettings : SystemSettingItem
    {

        private static string CacheKey = "SystemSettings";
        private static readonly IDataCacheService<List<SystemSettings>> _cache = new DataCacheService<List<SystemSettings>>();

        private static SystemSettingsDatabase GetRepository() => new SystemSettingsDatabase();

        #region constructor

        public SystemSettings() { }

        public SystemSettings(IDataReader reader)
        {
            this.Key = Convert.ToString(reader["Key"]);
            this.Value = Convert.ToString(reader["Value"]);
            this.Name = Convert.ToString(reader["Name"]);
            this.Description = Convert.ToString(reader["Description"]);
            this.RegExp = Convert.ToString(reader["RegExp"]);
            this.IsExposed = Convert.ToBoolean(reader["IsExposed"]);
        }

        #endregion


        /// <summary>
        /// Get all system settings and added default value "not selected" in index 0
        /// </summary>
        public static List<SystemSettings> GetSystemSettings()
        {
            List<SystemSettings> systemSettings = null;

            systemSettings = _cache.Get(CacheKey);

            if (systemSettings == null)
            {
                systemSettings = GetRepository().GetSystemSettings();

                //set to cache
                _cache.Add(CacheKey, systemSettings, TimeSpan.FromMinutes(30));
            }

            return systemSettings;
        }

        public static void ClearCache()
        {
            _cache.Remove(CacheKey);
        }


        /// <summary>
        /// Get system settings
        /// </summary>
        public static SystemSettings GetValue(string key)
        {
            return GetSystemSettings().FirstOrDefault(o => string.Compare(o.Key, key, true) == 0);
        }

        /// <summary>
        /// Update system settings value
        /// </summary>
        public static bool UpdateSystemSettingsValue(string key, string value)
        {
            _cache.Remove("SystemSettings");
            return GetRepository().UpdateSystemSettingsValue(key, value);
        }

        #region Methods for Admin Settings Web Page

        /// <summary>
        /// Get all system settings and added default value "not selected" in index 0
        /// </summary>
        public static List<SystemSettings> GetSystemSettingsForDisplay()
        {
            //get all system settings 
            var lstSettings = GetSystemSettings();

            var systemSettings = new SystemSettings()
            {
                Name = Resources.TextMessages.DropDown_NotSelectedText,
                Key = "0",
                RegExp = String.Empty
            };

            //adding default values
            lstSettings.Insert(0, systemSettings);


            return lstSettings;
        }


        /// <summary>
        /// Get system settings
        /// </summary>
        public static SystemSettings GetValueForDisplay(string key)
        {
            return GetSystemSettingsForDisplay().FirstOrDefault(o => o.Key.ToUpperInvariant() == key.ToString().ToUpperInvariant());
        }
        #endregion

        #region Get Typed Setting Value Methods

        /// <summary>
        /// Get Int value from settings table with default value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int GetIntValue(string key, int defaultValue)
        {
            int setingValue = defaultValue;
            try
            {
                string value = GetValue(key).Value;
                if (!Int32.TryParse(value, out setingValue))
                    setingValue = defaultValue;
            }
            catch
            {
                setingValue = defaultValue;
            }
            return setingValue;
        }

        /// <summary>
        /// Get Int value from settings table with default value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static long GetInt64Value(string key, long defaultValue)
        {
            Int64 setingValue = defaultValue;
            try
            {
                string value = SystemSettings.GetValue(key).Value;
                if (!Int64.TryParse(value, out setingValue))
                    setingValue = defaultValue;
            }
            catch
            {
                setingValue = defaultValue;
            }
            return setingValue;
        }

        /// <summary>
        /// Get Int value from settings table with default value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double GetDoubleValue(string key, double defaultValue)
        {
            double setingValue = defaultValue;
            try
            {
                string value = SystemSettings.GetValue(key).Value;
                if (!Double.TryParse(value, out setingValue))
                    setingValue = defaultValue;
            }
            catch
            {
                setingValue = defaultValue;
            }
            return setingValue;
        }


        /// <summary>
        /// Get Int value from settings table with default value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static decimal GetDecimalValue(string key, decimal defaultValue)
        {
            decimal setingValue = defaultValue;
            try
            {
                string value = SystemSettings.GetValue(key).Value;
                if (!Decimal.TryParse(value, out setingValue))
                    setingValue = defaultValue;
            }
            catch
            {
                setingValue = defaultValue;
            }
            return setingValue;
        }

        /// <summary>
        /// Get Int value from settings table with default value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetStringValue(string key, string defaultValue)
        {
            string setingValue = defaultValue;
            try
            {
                setingValue = SystemSettings.GetValue(key)?.Value;
            }
            catch
            {
                setingValue = defaultValue;
            }
            return setingValue;
        }

        #endregion
    }
}
