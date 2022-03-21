using System;
using FrontDesk.Server.Configuration;

namespace ScreenDox.Server.Common.Infrastructure
{
    public interface IDataCacheService<T> where T : class
    {
        void Add(string key, T data);
        void Add(string key, T data, TimeSpan absolutionExpiration);
        T Get(string key);
        void Remove(string key);
    }
}