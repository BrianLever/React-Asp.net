using ScreenDox.Server.Common.Data.Configuration;
using ScreenDox.Server.Common.Infrastructure;

using System;
using System.Collections.Generic;
using System.Linq;

namespace ScreenDox.Server.Common.Configuration
{
    public interface ISystemSettingService
    {
        List<SystemSettings> GetAll();

        SystemSettings GetByKey(string key);

        string GetByKey(string key, string defaultValueWhenNull);


        bool UpdateSystemSettingsValue(string key, string value);

    }

    public class SystemSettingService : ISystemSettingService
    {
        private readonly ISystemSettingsRepository _repository;
        private string CacheKey = "SystemSettings";
        private TimeSpan CacheTimeout = TimeSpan.FromMinutes(30);
        private readonly IDataCacheService<List<SystemSettings>> _cacheService;


        public SystemSettingService(ISystemSettingsRepository repository,
            IDataCacheService<List<SystemSettings>> cacheService)
        {
            _repository = repository ?? throw new System.ArgumentNullException(nameof(repository));
            _cacheService = cacheService ?? throw new System.ArgumentNullException(nameof(cacheService));
        }

        public SystemSettingService() : this(new SystemSettingsDatabase(),
            new DataCacheService<List<SystemSettings>>())
        {

        }

        public List<SystemSettings> GetAll()
        {
            var items = _cacheService.Get(CacheKey);

            if (items == null || items.Any())
            {
                items = _repository.GetSystemSettings();

                //set to cache
                _cacheService.Add(CacheKey, items, CacheTimeout);
            }

            return items;
        }

        private void ClearCache()
        {
            _cacheService.Remove(CacheKey);
        }

        public SystemSettings GetByKey(string key)
        {
            return GetAll().FirstOrDefault(x => string.Compare(x.Key, key, true) == 0);
        }
        /// <summary>
        /// Returns setting value. When value is missing or null, returns defaultValueWhenNull instead.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValueWhenNull"></param>
        /// <returns></returns>
        public string GetByKey(string key, string defaultValueWhenNull)
        {
            return GetByKey(key)?.Value ?? defaultValueWhenNull; 
        }

        public bool UpdateSystemSettingsValue(string key, string value)
        {
            ClearCache();
         
            return _repository.UpdateSystemSettingsValue(key, value);
        }

        
    }
}