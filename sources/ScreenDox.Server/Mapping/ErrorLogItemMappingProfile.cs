using AutoMapper;

using FrontDesk.Server.Logging;

namespace ScreenDox.Server.Models.Mapping
{
    public class ErrorLogItemMappingProfile: Profile
    {
        public ErrorLogItemMappingProfile()
        {
            CreateMap<ErrorLogItem, ErrorLog>();

            CreateMap<ErrorLog, ErrorLogItem>();
        }
    }
}
