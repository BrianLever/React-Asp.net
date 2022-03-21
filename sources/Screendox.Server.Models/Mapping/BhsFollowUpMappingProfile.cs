using AutoMapper;

using FrontDesk.Common.Bhservice;

using ScreenDox.Server.Models.Factory;

namespace ScreenDox.Server.Models.Mapping
{
    public class BhsFollowUpMappingProfile : Profile
    {
        public BhsFollowUpMappingProfile()
        {
            CreateMap<BhsFollowUp, FollowUpResponse>()
                .ForMember(dest => dest.Result,
                    opt => opt.MapFrom(src => ScreeningResultResponseFactory.CreateSlim(src.Result))
                )
                .ForMember(dest => dest.StaffNameCompleted,
                    opt => opt.MapFrom(src => src.BhsStaffNameCompleted)
                )
                .ForMember(dest => dest.Visit,
                    opt => opt.MapFrom(src => Mapper.Map<VisitResponse>(src.Visit))
                )
                ;

            CreateMap<FollowUpRequest, BhsFollowUp>();
        }
    }
}
