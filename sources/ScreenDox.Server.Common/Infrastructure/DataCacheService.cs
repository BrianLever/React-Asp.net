using Common.Logging;

using FrontDesk.Common.Extensions;
using System;
using System.Runtime.Caching;

namespace ScreenDox.Server.Common.Infrastructure
{
    public class DataCacheService<T> : IDataCacheService<T> where T: class
    {
        private ILog _logger = LogManager.GetLogger("DataCacheService");


        private ObjectCache GetCacheProvider()
        {
            return MemoryCache.Default;
        }

        private TimeSpan _absoluteExpirationPeriod = TimeSpan.FromSeconds(30);

        /// <summary>
        /// Contructor
        /// </summary>
        public DataCacheService()
        {

        }

        public DataCacheService(TimeSpan absoluteExpirationPeriod)
        {
            _absoluteExpirationPeriod = absoluteExpirationPeriod;
        }

        public void Add(string key, T data)
        {
            Add(key, data, _absoluteExpirationPeriod);
        }
        public void Add(string key, T data, TimeSpan absolutionExpiration)
        {
            var policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTimeOffset.Now.Add(absolutionExpiration);

            GetCacheProvider().Set(key, data, policy);
        }

        /// <summary>
        /// Remove cached value
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            GetCacheProvider().Remove(key);
        }

        public T Get(string key)
        {
            T result = null;
            object data = GetCacheProvider().Get(key);
            if(data != null)
            {
                result = data.As<T>();
            }

            return result;
        }
    }
}
