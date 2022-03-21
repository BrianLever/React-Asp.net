namespace ScreenDox.Server.Models.Configuration
{
    public interface ISystemSettingItem
    {
        string Description { get; set; }
        bool IsExposed { get; set; }
        string Key { get; set; }
        string Name { get; set; }
        string RegExp { get; set; }
        string Value { get; set; }
    }
}