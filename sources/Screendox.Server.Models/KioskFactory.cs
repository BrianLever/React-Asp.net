namespace ScreenDox.Server.Common.Models
{
    using FrontDesk.Common.Extensions;

    using System;
    using System.Data;
    using System.Linq;

    public static class KioskFactory
    {
        public static Kiosk Create(IDataReader reader)
        {
            var result = new Kiosk
            {
                KioskID = Convert.ToInt16(reader["KioskID"]),
                Name = reader.Get<string>("KioskName"),
                Description = Convert.IsDBNull(reader["Description"]) ? string.Empty : reader["Description"].ToString(),
                CreatedDate = (DateTimeOffset)(reader["CreatedDate"]),
                LastActivityDate = Convert.IsDBNull(reader["LastActivityDate"])
                ? (DateTimeOffset?)null : (DateTimeOffset)(reader["LastActivityDate"]),
                BranchLocationID = Convert.ToInt32(reader["BranchLocationID"]),
                Disabled = Convert.IsDBNull(reader["Disabled"]) ? false : Convert.ToBoolean(reader["Disabled"]),
                IpAddress = reader.Get<string>("IpAddress") ?? string.Empty,
                KioskAppVersion = reader.Get<string>("KioskAppVersion") ?? string.Empty,
                
            };

            var columns = reader.GetColumnNames();
            if (columns.Contains("SecretKey"))
            {
                result.SecretKey = reader.Get<string>("SecretKey") ?? string.Empty;
            }

            if (columns.Contains("ScreeningProfileName"))
            {
                result.ScreeningProfileName = reader.Get<string>("ScreeningProfileName") ?? String.Empty;
            }

            return result;
        }
    }
}
