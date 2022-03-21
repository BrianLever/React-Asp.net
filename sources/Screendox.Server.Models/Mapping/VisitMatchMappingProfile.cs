using AutoMapper;

using RPMS.Common.Models;

using ScreenDox.Server.Models.ViewModels;

namespace ScreenDox.Server.Models.Mapping
{
    public class VisitMatchMappingProfile : Profile
    {
        public VisitMatchMappingProfile()
        {
            CreateMap<Visit, VisitMatch>();

        }
    }
}
