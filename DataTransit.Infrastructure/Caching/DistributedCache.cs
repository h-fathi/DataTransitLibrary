using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace DataTransit.Infrastructure.Caching
{
    public class DistributedCache : ICache
    {
        private readonly IDistributedCache distributedCache;
        public DistributedCache(IDistributedCache _distributedCache)
        {
            this.distributedCache = _distributedCache;
        }
        public async Task<T> GetAsync<T>(string cacheKey)
        {
            var json = await distributedCache.GetStringAsync(cacheKey);

            if (string.IsNullOrEmpty(json))
                return (default);

            return JsonConvert.DeserializeObject<T>(json);
        }

        public async Task SetAsync(string cacheKey, object data)
        {
            var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(DateTime.Now.AddMinutes(10)).SetSlidingExpiration(TimeSpan.FromMinutes(2));
            await distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(data), options);
        }

    }
}
