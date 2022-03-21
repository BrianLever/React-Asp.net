using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrontDeskRpmsService.App_Start
{
    public static class AutoMapperConfiguration
    {
        public static void Configure()
        {
            AutoMapper.Mapper.Initialize(cfg => cfg.AddProfiles("RPMS.Data.CareBridge"));
            
        }
    }
}