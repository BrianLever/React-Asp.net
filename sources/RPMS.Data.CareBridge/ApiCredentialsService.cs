using System;
using System.Collections.Generic;
using System.Text;
using RPMS.Common.Security;
using RPMS.Common.GlobalConfiguration;
using System.Runtime.Caching;
using Newtonsoft.Json;

namespace RPMS.Data.CareBridge
{
    public class ApiCredentialsService : IApiCredentialsService
    {
        private readonly IGlobalSettingsRepository _repository;
        private static ObjectCache _cache = MemoryCache.Default;

        public ApiCredentialsService(IGlobalSettingsRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }


        public BasicAuthCredentials GetCredentials()
        {
            BasicAuthCredentials result = null;

            result = GetCredentialsCached();

            if (result == null)
            {
                var jsonCreds = _repository.Get(GlobalSettingsDescriptor.CareBridgeCredentials)?.Value;
                if (!string.IsNullOrEmpty(jsonCreds))
                {
                    result = JsonConvert.DeserializeObject<BasicAuthCredentials>(jsonCreds);
                }

                if(result != null)
                {
                    _cache.Add(GlobalSettingsDescriptor.CareBridgeCredentials, result, DateTimeOffset.Now.AddMinutes(30));
                }
            }

            return result ?? new BasicAuthCredentials();
        }

        public BasicAuthCredentials GetCredentialsCached()
        {
            return _cache.Get(GlobalSettingsDescriptor.CareBridgeCredentials) as BasicAuthCredentials;
        }



    }
}
