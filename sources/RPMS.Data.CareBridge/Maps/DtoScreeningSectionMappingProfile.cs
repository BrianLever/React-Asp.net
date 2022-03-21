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
    public class DtoScreeningSectionMappingProfile : Profile
    {
        public DtoScreeningSectionMappingProfile()
        {
            CreateMap<ScreeningResultRecord, ScreeningSectionRequest>();
        }
    }
}
