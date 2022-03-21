using AutoMapper;

using FrontDesk;
using FrontDesk.Common.Bhservice;

using RPMS.Common.Models;

using ScreenDox.Server.Models.Factory;
using ScreenDox.Server.Models.ViewModels;

using System;
using System.Collections.Generic;

namespace ScreenDox.Server.Models.Mapping
{
    public class PatientAddressRequestMappingProfile : Profile
    {
        public PatientAddressRequestMappingProfile()
        {

            CreateMap<PatientAddressRequest, ScreeningResult>()
                .ForMember(dest => dest.ExportedToHRN,
                     opt => opt.MapFrom(src => src.PatientID)
                )
                .ForMember(dest => dest.StreetAddress,
                     opt => opt.MapFrom(src => src.StreetAddress.ToUpperInvariant())
                )
                .ForMember(dest => dest.ZipCode,
                     opt => opt.MapFrom(src => src.ZipCode.Trim())
                )
                .ForMember(dest => dest.City,
                     opt => opt.MapFrom(src => src.City.ToUpperInvariant())
                )
                .ForMember(dest => dest.ExportedToHRN,
                     opt => opt.MapFrom(src => src.PatientID)
                )
                .ForMember(dest => dest.ExportedToHRN,
                     opt => opt.MapFrom(src => src.PatientID)
                );
        }
    }
}
