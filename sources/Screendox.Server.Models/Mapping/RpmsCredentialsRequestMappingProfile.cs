using AutoMapper;

using RPMS.Common.Security;

using ScreenDox.Server.Models;

namespace ScreenDox.Server.Models.Mapping
{
    public class RpmsCredentialsRequestMappingProfile : Profile
    {
        public RpmsCredentialsRequestMappingProfile()
        {
            CreateMap<RpmsCredentialsRequest, RpmsCredentials>()
                .IgnoreAllPropertiesWithAnInaccessibleSetter();

            CreateMap<RpmsCredentials, RpmsCredentialsResponse>()
                .IgnoreAllPropertiesWithAnInaccessibleSetter();
        }
    }
}
