using AutoMapper;

using ScreenDox.Server.Common.Models;

namespace ScreenDox.Server.Models.Mapping
{
    public class KioskMappingProfile : Profile
    {
        public KioskMappingProfile()
        {
            CreateMap<Kiosk, KioskRequest>();

            CreateMap<KioskRequest, Kiosk>();

        }
    }
}
