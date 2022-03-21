namespace ScreenDox.Server.Common.Data.Configuration
{
    using FrontDesk.Common.Data;

    using ScreenDox.Server.Common.Configuration;

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public interface ISystemSettingsRepository
    {
        System.Collections.Generic.List<SystemSettings> GetSystemSettings();
        System.Collections.Generic.List<SystemSettings> GetSystemSettingsForDisplay();
        bool UpdateSystemSettingsValue(string key, string value);
    }

    public class SystemSettingsDatabase : DBDatabase, ISystemSettingsRepository
    {
        #region constructors

        public SystemSettingsDatabase(string connectionString)
            : base(connectionString) { }

        public SystemSettingsDatabase(): base(0)
        { }

        public SystemSettingsDatabase(DbConnection sharedConnection)
            : base(sharedConnection)
        { }

        #endregion

        #region GET

        /// <summary>
        /// Get system settings where is exposed true
        /// </summary>
        public List<SystemSettings> GetSystemSettings()
        {
            string sqlText = @"SELECT * FROM SystemSettings ORDER BY [Key] ASC";

            List<SystemSettings> allSystemSettings = new List<SystemSettings>();

            try
            {
                CommandObject.Parameters.Clear();

                Connect();

                using (var reader = RunSelectQuery(sqlText))
                {

                    while (reader.Read())
                    {
                        allSystemSettings.Add(new SystemSettings(reader));
                    }
                }

            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                base.Disconnect();
            }
            return allSystemSettings; ;
        }

        /// <summary>
        /// Get system settings where is exposed true
        /// </summary>
        public List<SystemSettings> GetSystemSettingsForDisplay()
        {
            string sqlText = @"SELECT * FROM SystemSettings WHERE IsExposed = 1 ORDER BY [Name] ASC";

            List<SystemSettings> allSystemSettings = new List<SystemSettings>();
            try
            {
                CommandObject.Parameters.Clear();

                Connect();

                using (var reader = RunSelectQuery(sqlText))
                {
                    while (reader.Read())
                    {
                        allSystemSettings.Add(new SystemSettings(reader));
                    }
                }

            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                base.Disconnect();
            }
            return allSystemSettings; ;
        }

        #endregion

        #region ADD/UPDATE/DELETE

        /// <summary>
        /// Update system settings value
        /// </summary>
        public bool UpdateSystemSettingsValue(string key, string value)
        {
            string sqlText = @"UPDATE SystemSettings SET Value = @Value WHERE [Key] = @Key And IsExposed = 1";
            bool isSuccess = false;

            try
            {
                CommandObject.Parameters.Clear();
                AddParameter("@Key", DbType.String, 128).Value = key;
                AddParameter("@Value", DbType.String, 255).Value = value;

                Connect();

                if (RunNonSelectQuery(sqlText) > 0)
                {
                    isSuccess = true;
                }
            }

            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                base.Disconnect();
            }
            return isSuccess;
        }

        #endregion
    }
}
