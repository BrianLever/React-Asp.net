using AutoMapper;

using FrontDesk.Configuration;

namespace ScreenDox.Server.Models.Mapping
{
    public class ScreeningFrequencyItemViewModelProfile : Profile
    {
        public ScreeningFrequencyItemViewModelProfile()
        {
            CreateMap<ScreeningFrequencyItem, ScreeningFrequencyItemViewModel>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.Frequency, opt => opt.MapFrom(src => src.Frequency))
                .ForMember(dest => dest.LastModifiedDateUTC, opt => opt.MapFrom(src => src.LastModifiedDateUTC))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => ScreeningFrequencyDescriptor.GetName(src.ID)));


            CreateMap<ScreeningFrequencyItemViewModel, ScreeningFrequencyItem>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.Frequency, opt => opt.MapFrom(src => src.Frequency))
                .ForMember(dest => dest.LastModifiedDateUTC, opt => opt.MapFrom(src => src.LastModifiedDateUTC));

        }
    }
}
