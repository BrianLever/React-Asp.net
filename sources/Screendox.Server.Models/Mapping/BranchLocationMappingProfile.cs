using AutoMapper;

using FrontDesk.Server;


namespace ScreenDox.Server.Models.Mapping
{
    public class BranchLocationMappingProfile : Profile
    {
        public BranchLocationMappingProfile()
        {
            CreateMap<BranchLocation, BranchLocationRequest>();

            CreateMap<BranchLocationRequest, BranchLocation>();

        }
    }
}
