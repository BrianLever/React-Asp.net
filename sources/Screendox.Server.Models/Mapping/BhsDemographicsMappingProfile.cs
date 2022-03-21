using AutoMapper;

using FrontDesk.Common.Bhservice;

using ScreenDox.Server.Models.Factory;

namespace ScreenDox.Server.Models.Mapping
{
    public class BhsDemographicsMappingProfile : Profile
    {
        public BhsDemographicsMappingProfile()
        {
            CreateMap<BhsDemographics, DemographicsResponse>()
                .ForMember(dest => dest.Result,
                    opt => opt.MapFrom(src => ScreeningResultResponseFactory.CreateSlim(src.ToScreeningResultModel()))
                )
                .ForMember(dest => dest.StaffNameCompleted,
                    opt => opt.MapFrom(src => src.BhsStaffNameCompleted)
                );

            CreateMap<DemographicsRequest, BhsDemographics>();

        }
    }
}
