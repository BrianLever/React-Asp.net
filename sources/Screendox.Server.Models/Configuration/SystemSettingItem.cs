namespace ScreenDox.Server.Models.Configuration
{
    public class SystemSettingItem : ISystemSettingItem
    {
        /// <summary>
        /// Key value
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Regular validation expression 
        /// </summary>
        public string RegExp { get; set; }

        /// <summary>
        /// Is exposed
        /// </summary>
        public bool IsExposed { get; set; }

    }
}
