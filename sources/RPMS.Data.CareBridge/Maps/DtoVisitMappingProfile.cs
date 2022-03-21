using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using RPMS.Common.Models;
using RPMS.Data.CareBridge.Dto;
using FrontDesk.Common.Extensions;
using RPMS.Data.CareBridge.Formatting;

namespace RPMS.Data.CareBridge.Maps
{
    public class DtoVisitMappingProfile : Profile
    {
        public DtoVisitMappingProfile()
        {
            CreateMap<PatientVisitRecord, Visit>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.VisitId))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.AdmitionDate))
                .ForMember(dest => dest.ServiceCategory, opt => opt.MapFrom(src => src.ServiceCategory ?? string.Empty))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => new Location
                {
                    Name = src.LocationName
                }));
        }
    }
}
