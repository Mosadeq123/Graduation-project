using StackExchange.Redis;
using Store.G04.Core.Repositories.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.G04.Service.Services.Caches
{
    public class CacheService : ICacheService
    {
        private readonly IDatabase _database;
        public CacheService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task<string> GetCacheKeyAsync(string Key)
        {
            var cacheResponse = await _database.StringGetAsync(Key);
            if (cacheResponse.IsNullOrEmpty) return null;
            return cacheResponse.ToString();
        }

        public async Task SetCacheKeyAsync(string Key, object response, TimeSpan expireTime)
        {
            if (response is not null) return;
            var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            await _database.StringSetAsync(Key, JsonSerializer.Serialize(response), expireTime);
        }
    }
}
