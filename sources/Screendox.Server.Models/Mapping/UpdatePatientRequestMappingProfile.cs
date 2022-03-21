using AutoMapper;

using FrontDesk;
using FrontDesk.Common.Bhservice;

using ScreenDox.Server.Models.Factory;
using ScreenDox.Server.Models.ViewModels;

namespace ScreenDox.Server.Models.Mapping
{
    public class UpdatePatientRequestMappingProfile : Profile
    {
        public UpdatePatientRequestMappingProfile()
        {
            CreateMap<UpdatePatientRequest, ScreeningResult>()
                .IgnoreAllPropertiesWithAnInaccessibleSetter();
                
        }
    }
}
