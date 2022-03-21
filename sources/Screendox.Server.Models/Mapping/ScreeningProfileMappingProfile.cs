using AutoMapper;

using FrontDesk;


namespace ScreenDox.Server.Models.Mapping
{
    public class ScreeningProfileMappingProfile : Profile
    {
        public ScreeningProfileMappingProfile()
        {
            CreateMap<ScreeningProfile, ScreeningProfileRequest>();

            CreateMap<ScreeningProfileRequest, ScreeningProfile>();
        }
    }
}
