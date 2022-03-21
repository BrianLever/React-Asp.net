using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Caching;

namespace FrontDesk.Kiosk.Services
{
    public abstract class TypeaheadDataSourceBase<T> : ITypeaheadDataSourceBase<T>
        where T : class
    {
        #region Caching

        protected abstract string CacheKey { get; }

        private ObjectCache _localCache = MemoryCache.Default;

        /// <summary>
        /// Cache states
        /// </summary>
        public virtual void Refresh()
        {
            GetDataFromCache();
        }

        protected abstract string GetTextForFilter(T inputModel);


        protected abstract List<T> GetDataFromSource();

        protected List<T> GetDataFromCache()
        {
            List<T> value = (List<T>)_localCache.Get(CacheKey);

            if (value == null)
            {
                value = GetDataFromSource();

                _localCache.Set(CacheKey, value, new CacheItemPolicy
                {
                    AbsoluteExpiration = DateTimeOffset.Now.Date.AddDays(1) //expire next day
                });
            }

            return value;
        }

        #endregion

        public ReadOnlyCollection<T> GetAll()
        {
            return GetDataFromCache().AsReadOnly();
        }

        public T GetByName(string textInput)
        {
            textInput = textInput ?? string.Empty;

            return GetDataFromCache().FirstOrDefault(s => GetTextForFilter(s).ToLower() == textInput.ToLower());
        }

        /// <summary>
        /// Get the list of items that starts with "nameStartsWith" parameter.
        /// </summary>
        public virtual List<T> GetByPartOfName(string startsWithInput)
        {
            if (String.IsNullOrEmpty(startsWithInput))
            {
                return GetDataFromCache();
            }

            return GetDataFromCache().FindAll(s => GetTextForFilter(s).ToLower().StartsWith(startsWithInput.ToLower()));
        }

    }
}
