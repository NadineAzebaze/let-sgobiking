using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Proxy
{
    internal class ProxyCache<T> where T : class
    {
        ObjectCache cache = MemoryCache.Default;
        public DateTimeOffset dt_default;

        public ProxyCache()
        {
            dt_default = ObjectCache.InfiniteAbsoluteExpiration;
        }

        public T Get(string CacheItemName)
        {
            if (this.cache.Get(CacheItemName) == null || !this.cache.Contains(CacheItemName))
            {
                T obj = (T)Activator.CreateInstance(typeof(T), CacheItemName);
                CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
                cacheItemPolicy.AbsoluteExpiration = dt_default;
                this.cache.Add(CacheItemName, obj, cacheItemPolicy);
            }
            return (T)this.cache.Get(CacheItemName);

        }


        public T Get(string CacheItemName, double dt_seconds)
        {
            if (this.cache.Get(CacheItemName) == null || !this.cache.Contains(CacheItemName))
            {
                T obj = (T)Activator.CreateInstance(typeof(T), CacheItemName);
                CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
                cacheItemPolicy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(dt_seconds);
                this.cache.Add(CacheItemName, obj, cacheItemPolicy);
            }
            return (T)this.cache.Get(CacheItemName);
        }

        public T Get(string CacheItemName, DateTimeOffset dt)
        {
            if (this.cache.Get(CacheItemName) == null || !this.cache.Contains(CacheItemName))
            {
                T obj = (T)Activator.CreateInstance(typeof(T), CacheItemName);
                CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
                cacheItemPolicy.AbsoluteExpiration = dt;
                this.cache.Add(CacheItemName, obj, cacheItemPolicy);
            }
            return (T)this.cache.Get(CacheItemName);
        }

    }
}

