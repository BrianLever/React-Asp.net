using AutoMapper;

using FrontDesk;

namespace ScreenDox.Server.Models.Mapping
{
    public class ScreeningSectionAgeItemViewModelProfile : Profile
    {
        public ScreeningSectionAgeItemViewModelProfile()
        {
            CreateMap<ScreeningSectionAge, ScreeningSectionAgeItemViewModel>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.ScreeningSectionLabel)
                );

            CreateMap<ScreeningSectionAgeItemViewModel, ScreeningSectionAge>()
                .ForMember(dest => dest.ScreeningSectionLabel,
                    opt => opt.MapFrom(src => src.Name)
                ); ;

        }
    }
}
