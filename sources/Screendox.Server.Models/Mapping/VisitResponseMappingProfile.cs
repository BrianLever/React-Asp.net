using AutoMapper;

using FrontDesk.Common.Bhservice;

using ScreenDox.Server.Models.Factory;

namespace ScreenDox.Server.Models.Mapping
{
    public class BhsVisitMappingProfile : Profile
    {
        public BhsVisitMappingProfile()
        {
            CreateMap<BhsVisit, VisitResponse>()
                .ForMember(dest => dest.Result,
                    opt => opt.MapFrom(src => ScreeningResultResponseFactory.CreateSlim(src.Result))
                )
                .ForMember(dest => dest.StaffNameCompleted,
                    opt => opt.MapFrom(src => src.BhsStaffNameCompleted)
                );

            CreateMap<VisitRequest, BhsVisit>();

        }
    }
}
