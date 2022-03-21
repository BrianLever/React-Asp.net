using AutoMapper;

using FrontDesk.Configuration;

namespace ScreenDox.Server.Models.Mapping
{
    public class UserPrincipalResponseProfile : Profile
    {
        public UserPrincipalResponseProfile()
        {
            CreateMap<UserPrincipal, UserPrincipalResponse>();
        }
    }
}
