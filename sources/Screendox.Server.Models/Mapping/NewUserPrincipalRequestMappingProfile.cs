using AutoMapper;

using ScreenDox.Server.Models.Security;

namespace ScreenDox.Server.Models.Mapping
{
    public class NewUserPrincipalRequestMappingProfile : Profile
    {
        public NewUserPrincipalRequestMappingProfile()
        {
            CreateMap<NewUserPrincipalRequest, UserPrincipalAccount>();

            CreateMap<UserPrincipalRequest, UserPrincipalAccount>();
        }
    }
}
