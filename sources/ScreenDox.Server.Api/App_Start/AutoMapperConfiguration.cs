using ScreenDox.Server.Models.Mapping;

namespace ScreenDox.Server.Api
{
    /// <summary>
    /// Register mapping configs.
    /// </summary>
    public static class AutoMapperConfiguration
    {
        /// <summary>
        /// Configure mapping
        /// </summary>
        public static void Configure()
        {
            AutoMapper.Mapper.Initialize(cfg => cfg.AddProfiles(
                typeof(BhsVisitMappingProfile),
                typeof(ErrorLogItemMappingProfile)
                ));
        }
    }
}