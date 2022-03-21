using ScreenDox.Server.Models.Configuration;

using System;
using System.Data;

namespace ScreenDox.Server.Common.Configuration
{
    public class SystemSettingsItemFactory
    {
        public ISystemSettingItem Create(IDataReader reader)
        {
            return new SystemSettingItem
            {
                Key = Convert.ToString(reader["Key"]),
                Value = Convert.ToString(reader["Value"]),
                Name = Convert.ToString(reader["Name"]),
                Description = Convert.ToString(reader["Description"]),
                RegExp = Convert.ToString(reader["RegExp"]),
                IsExposed = Convert.ToBoolean(reader["IsExposed"])
            };

        }
    }
}
