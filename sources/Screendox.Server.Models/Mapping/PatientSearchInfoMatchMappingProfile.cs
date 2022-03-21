using AutoMapper;

using FrontDesk;

using RPMS.Common.Models;

using ScreenDox.Server.Models.ViewModels;

using System;
using System.Collections.Generic;

namespace ScreenDox.Server.Models.Mapping
{
    public class PatientSearchInfoMatchMappingProfile : Profile
    {
        public PatientSearchInfoMatchMappingProfile()
        {
            CreateMap<Patient, PatientSearchInfoMatch>()
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
               )
               .ForMember(dest => dest.IsEhrSource,
                   opt => opt.MapFrom(src => true)
               )
               .ForMember(dest => dest.ScreeningResultID,
                   opt => opt.MapFrom(src => (long?)null)
               )
               .ForMember(dest => dest.DemographicsID,
                   opt => opt.MapFrom(src => (long?)null)
               )
               .ForMember(dest => dest.EhrPatientId,
                   opt => opt.MapFrom(src => src.ID)
               );
        }
    }
}
