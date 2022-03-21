using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk
{
    public class AnswerScaleCacheManager
    {
        #region singleton pattern

        private static object _lockObj = new object();

        private static AnswerScaleCacheManager _instance = null;
        /// <summary>
        /// Get instance of Cache manager
        /// </summary>
        public static AnswerScaleCacheManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObj)
                    {
                        if (_instance == null) _instance = new AnswerScaleCacheManager();
                    }
                }
                return _instance;
            }
        }

       
        /// <summary>
        /// If true user SQL Compact database provider. False by default
        /// </summary>
        public bool UseLocalDatabaseConnection
        {
            get { return AnswerScale.UseLocalDatabaseConnection; }
            set { AnswerScale.UseLocalDatabaseConnection = value; }
        }

        #endregion

        private AnswerScaleCacheManager() { }

        /// <summary>
        /// Get Answer option list
        /// </summary>
        /// <param name="answerScaleID"></param>
        /// <returns></returns>
        public List<AnswerScaleOption> GetAnswerOptions(int answerScaleID)
        {
            List<AnswerScaleOption> options;
            AnswerScale answerScale = null;

            var dicAnswerScales = GetDataSource();
            if (dicAnswerScales != null && dicAnswerScales.TryGetValue(answerScaleID, out answerScale))
            {
                options = answerScale.Options;
            }
            else { options = new List<AnswerScaleOption>(); }

            return options;

        }

        Dictionary<int, AnswerScale> _dataSource = null;

        /// <summary>
        /// get datasource from database and put in into a cache
        /// </summary>
        /// <returns></returns>
        private Dictionary<int, AnswerScale> GetDataSource()
        {
            if (_dataSource == null)
            {
                if (System.Web.HttpContext.Current != null)// if hosted by ASP.NET
                {
                    string key = "FrontDesk_AnswerScaleCacheManager_Cache";
                    var cache = System.Web.HttpContext.Current.Cache;
                    if (cache != null && cache[key] != null)
                    {
                        _dataSource = (Dictionary<int, AnswerScale>)cache[key];
                    }
                    else
                    {
                        _dataSource = AnswerScale.GetAnswerOptions();
                        if (_dataSource != null)
                        {
                            cache.Add(key, _dataSource, null, System.Web.Caching.Cache.NoAbsoluteExpiration,
                                TimeSpan.FromHours(1), System.Web.Caching.CacheItemPriority.Normal, null);

                        }
                    }
                }
                else
                {
                    //use _dataSource as cache storage
                    if (_dataSource == null) _dataSource = AnswerScale.GetAnswerOptions();
                }
            }
            return _dataSource;
        }
    }
}
