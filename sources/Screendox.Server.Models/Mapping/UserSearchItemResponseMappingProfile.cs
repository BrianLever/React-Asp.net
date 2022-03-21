using AutoMapper;

using ScreenDox.Server.Models.ViewModels;

namespace ScreenDox.Server.Models.Mapping
{
    public class UserSearchItemResponseMappingProfile : Profile
    {
        public UserSearchItemResponseMappingProfile()
        {
            CreateMap<UserPrincipal, UserSearchItemResponse>()
                .IgnoreAllPropertiesWithAnInaccessibleSetter();
        }
    }
}
