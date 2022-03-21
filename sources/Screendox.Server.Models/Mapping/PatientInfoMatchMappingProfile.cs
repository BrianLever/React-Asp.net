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
    public class PatientInfoMatchMappingProfile : Profile
    {
        public PatientInfoMatchMappingProfile()
        {
            CreateMap<PatientInfo, PatientInfoMatch>()
             ;
            CreateMap<Patient, PatientInfoMatch>()
                .ForMember(dest => dest.ID,
                     opt => opt.MapFrom(src => src.ID)
                )
                .ForMember(dest => dest.NotMatchesFields,
                    opt => new List<string>()
                )
                .ForMember(dest => dest.Phone,
                    opt => opt.MapFrom(src => src.PhoneHome)
                )
                .ForMember(dest => dest.StateName,
                    opt => opt.MapFrom(src => src.StateID)
                )
                .ForMember(dest => dest.Birthday,
                    opt => opt.MapFrom(src => src.DateOfBirth)
                )
                .ForMember(dest => dest.Age,
                    opt => opt.MapFrom(src => ScreeningResult.GetAge(src.DateOfBirth, DateTime.Today))
                );
        }
    }
}
