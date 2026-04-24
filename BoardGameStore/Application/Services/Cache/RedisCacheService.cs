using Application.Common.Cache;
using Application.Interfaces.Cache;
using StackExchange.Redis;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services.Cache
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDatabase _redis;

        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _redis = redis.GetDatabase();
        }

        public async Task<T?> GetAsync<T>(string key, CancellationToken ct = default)
        {
            var cached = await _redis.StringGetAsync(key);
            if (!cached.HasValue)
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(cached!);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan ttl, CancellationToken ct = default)
        {
            var json = JsonSerializer.Serialize(value);
            await _redis.StringSetAsync(key, json, ttl);
        }

        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan ttl, CancellationToken ct = default)
        {
            var cached = await GetAsync<T>(key, ct);
            if (cached is not null)
            {
                return cached;
            }

            var result = await factory();
            await SetAsync(key, result, ttl, ct);

            return result;
        }

        public async Task<long> SetIpToCache(string key, TimeSpan ttl, CancellationToken ct = default)
        {
            var count = await _redis.StringIncrementAsync(key);

            if (count == 1)
            {
                await _redis.KeyExpireAsync(key, ttl);
            }

            return count;
        }

        public async Task<bool> ExistsKeyAsync(string key)
        {
            return await _redis.KeyExistsAsync(key);
        }

        public Task BlacklistJtiAsync(string jti, TimeSpan ttl)
        {
            var key = CacheKeys.JwtTokenBlackList(jti);
            return _redis.StringSetAsync(key, "1", ttl);
        }
    }
}
