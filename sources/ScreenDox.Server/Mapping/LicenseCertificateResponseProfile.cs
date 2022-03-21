using AutoMapper;

using FrontDesk.Server.Licensing.Services;

using ScreenDox.Server.Models.ViewModels;

namespace ScreenDox.Server.Models.Mapping
{
    public class LicenseCertificateResponseProfile : Profile
    {
        public LicenseCertificateResponseProfile()
        {
            CreateMap<LicenseCertificate, LicenseCertificateResponse>()
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ForMember(dest => dest.LicenseString, opt => opt.MapFrom(src => src.License.LicenseString))
                .ForMember(dest => dest.RegisteredDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.MaxBranchLocations, opt => opt.MapFrom(src => src.License.MaxBranchLocations))
                .ForMember(dest => dest.MaxKiosks, opt => opt.MapFrom(src => src.License.MaxKiosks))
                .ForMember(dest => dest.DurationInYears, opt => opt.MapFrom(src => src.License.Years))
                .ForMember(dest => dest.ActivatedDate, opt => opt.MapFrom(src => src.ActivationKey != null ? src.ActivatedDate : null))
                .ForMember(dest => dest.ExpirationDate, opt => opt.MapFrom(src => src.ActivationKey != null ? src.ExpirationDate : null));

        }
    }
}
