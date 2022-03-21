using System;
using System.Runtime.Caching;

namespace RPMS.Common.GlobalConfiguration
{
    public class GlobalSettingsService : IGlobalSettingsService
    {
        private readonly IGlobalSettingsRepository _globalSettingsRepository;
        private static ObjectCache _cache = MemoryCache.Default;

        public GlobalSettingsService(IGlobalSettingsRepository globalSettingsRepository)
        {
            _globalSettingsRepository = globalSettingsRepository ?? throw new ArgumentNullException(nameof(globalSettingsRepository));
        }

        public GlobalSettingsService() : this(new GlobalSettingsDatabase())
        {

        }

        public string ExternalEhrSystem
        {
            get
            {
                var result = (string)_cache.Get(GlobalSettingsDescriptor.ExternalEhrSystemKey);
                if (result == null)
                {
                    result = _globalSettingsRepository.Get(GlobalSettingsDescriptor.ExternalEhrSystemKey)?.Value ?? ExternalEhrSystemDescriptor.None;

                    _cache.Add(GlobalSettingsDescriptor.ExternalEhrSystemKey, result, DateTimeOffset.Now.AddMinutes(30));
                }

                return result;
            }
        }

        public bool IsRpmsMode
        {
            get
            {
                return ExternalEhrSystem == ExternalEhrSystemDescriptor.EhrRpms;
            }
        }
        public bool IsNextGenMode
        {
            get
            {
                return ExternalEhrSystem == ExternalEhrSystemDescriptor.NextGen;
            }
        }
    }
}