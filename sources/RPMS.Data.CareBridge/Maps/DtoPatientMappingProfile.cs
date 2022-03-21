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
    public class DtoPatientMappingProfile : Profile
    {
        public DtoPatientMappingProfile()
        {
            CreateMap<PatientRecord, Patient>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.RowID))
                .ForMember(dest => dest.EHR, opt => opt.MapFrom(src => src.HealthRecordNumber))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.PatientName.LastName ?? string.Empty))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.PatientName.FirstName ?? string.Empty))
                .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(src => src.PatientName.MiddleName ?? string.Empty))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.Birthday))
                .ForMember(dest => dest.StateID, opt => opt.MapFrom(src => src.StreetAddress.State))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.StreetAddress.City))
                .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => src.StreetAddress.Zip))
                .ForMember(dest => dest.StreetAddressLine1, opt => opt.MapFrom(src => src.StreetAddress.Address1))
                .ForMember(dest => dest.StreetAddressLine2, opt => opt.MapFrom(src => src.StreetAddress.Address2))
                .ForMember(dest => dest.PhoneHome, opt => opt.MapFrom(src => StringFormat.AsPhone(src.HomePhone)))
                .ForMember(dest => dest.PhoneOffice, opt => opt.MapFrom(src => StringFormat.AsPhone(src.OfficePhone)))
                ;

            CreateMap<PatientSearch, PatientName>();
        }
    }
}
