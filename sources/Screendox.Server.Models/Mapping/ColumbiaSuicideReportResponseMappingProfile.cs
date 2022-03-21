using AutoMapper;

using ScreenDox.Server.Models.ColumbiaReports;
using ScreenDox.Server.Models.ViewModels;

namespace ScreenDox.Server.Models.Mapping
{
    public class ColumbiaSuicideReportResponseMappingProfile : Profile
    {
        public ColumbiaSuicideReportResponseMappingProfile()
        {
            CreateMap<ColumbiaSuicideReport, ColumbiaSuicideReportResponse>();


            CreateMap<ColumbiaSuicideReportUpdateRequest, ColumbiaSuicideReport > ()
                .IgnoreAllPropertiesWithAnInaccessibleSetter();

        }
    }
}
