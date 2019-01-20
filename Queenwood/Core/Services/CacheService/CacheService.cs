using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;

namespace Queenwood.Core.Services.CacheService
{
    public class CacheService : ICacheService
    {
        private IMemoryCache _cache;
        private static List<String> _cacheKeys = new List<string>();

        public CacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        // Locking per object
        private static object _globalLockObject = new object();
        private static Dictionary<string, object> _keyNameLockObjects = new Dictionary<string, object>();
        private static object GetLockObj(string keyName)
        {
            lock (_globalLockObject)
            {
                if (!_keyNameLockObjects.ContainsKey(keyName))
                    _keyNameLockObjects.Add(keyName, new object());

                return _keyNameLockObjects[keyName];
            }
        }


        /// <summary>
        /// CacheHelper.Get(cacheKey, () => { return Model }, 60);
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keyName">Unique name of cache entry</param>
        /// <param name="creator">Function to generate data to be cached</param>
        /// <param name="expiryMins">Time in mins to keep entry in the cache</param>
        /// <returns></returns>
        public T Get<T>(string keyName, Func<T> creator, int expiryMins)
        {
            T obj = (T)_cache.Get(keyName);

            if (obj != null)
                return obj;

            lock (GetLockObj(keyName))
            {
                obj = (T)_cache.Get(keyName);

                if (obj != null)
                    return obj;

                obj = creator();

                if (obj != null)
                {
                    _cache.Set<T>(keyName, obj, DateTime.Now.AddMinutes(expiryMins));
                    _cacheKeys.Add(keyName);
                }

                return obj;
            }
        }

        /// <summary>
        /// Remove item from cache by unique name
        /// </summary>
        /// <param name="keyName"></param>
        public void Remove(string keyName)
        {
            _cacheKeys.Remove(keyName);
            _cache.Remove(keyName);
        }

        /// <summary>
        /// Remove all items from cache
        /// </summary>
        public void RemoveAll()
        {
            for (int i = _cacheKeys.Count - 1; i >= 0; i--)
            {
                _cache.Remove(_cacheKeys[i]);
                _cacheKeys.RemoveAt(i);
            }
        }
    }
}

