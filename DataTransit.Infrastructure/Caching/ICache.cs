using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataTransit.Infrastructure.Caching
{
    public interface ICache
    {
        Task<T> GetAsync<T>(string cacheKey);
        Task SetAsync(string cacheKey, object data);
    }


}
