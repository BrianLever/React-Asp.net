using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using FrontDesk.Configuration;
using FrontDesk.Server.Screening;

namespace FrontDesk.Server.Mappers
{
    [Obsolete("migrated to screendox.server.models")]
    public class ScreeningMapCreator : Profile
    {

        public ScreeningMapCreator()
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
