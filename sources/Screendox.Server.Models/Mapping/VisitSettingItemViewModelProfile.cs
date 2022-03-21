using AutoMapper;

using FrontDesk.Configuration;

namespace ScreenDox.Server.Models.Mapping
{
    public class VisitSettingItemViewModelProfile : Profile
    {
        public VisitSettingItemViewModelProfile()
        {
            CreateMap<VisitSettingItem, VisitSettingItemViewModel>();

            CreateMap<VisitSettingItemViewModel, VisitSettingItem>();
        }
    }
}
