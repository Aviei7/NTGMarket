using Application.Common.Cache;
using Application.Interfaces.Cache;

namespace Application.Services.Cache
{
    public class MemoryCacheService : ICacheService
    {
        private readonly Dictionary<string, CacheEntry> _cache = new(StringComparer.Ordinal);
        private readonly object _sync = new();

        public Task<T?> GetAsync<T>(string key, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            if (TryGetActiveEntry(key, out var entry) && entry.Value is T typedValue)
            {
                return Task.FromResult<T?>(typedValue);
            }

            return Task.FromResult<T?>(default);
        }

        public Task SetAsync<T>(string key, T value, TimeSpan ttl, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            lock (_sync)
            {
                _cache[key] = CreateEntry(value, ttl);
            }

            return Task.CompletedTask;
        }

        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan ttl, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            if (TryGetActiveEntry(key, out var entry) && entry.Value is T typedValue)
            {
                return typedValue;
            }

            var result = await factory();
            await SetAsync(key, result, ttl, ct);

            return result;
        }

        public Task<long> SetIpToCache(string key, TimeSpan ttl, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            lock (_sync)
            {
                if (TryGetActiveEntryUnsafe(key, out var existingEntry) && existingEntry.Value is long currentCount)
                {
                    var nextCount = currentCount + 1;
                    _cache[key] = existingEntry with { Value = nextCount };
                    return Task.FromResult(nextCount);
                }

                _cache[key] = CreateEntry(1L, ttl);
                return Task.FromResult(1L);
            }
        }

        public Task<bool> ExistsKeyAsync(string key)
        {
            return Task.FromResult(TryGetActiveEntry(key, out _));
        }

        public Task BlacklistJtiAsync(string jti, TimeSpan ttl)
        {
            var key = CacheKeys.JwtTokenBlackList(jti);

            lock (_sync)
            {
                _cache[key] = CreateEntry(true, ttl);
            }

            return Task.CompletedTask;
        }

        private bool TryGetActiveEntry(string key, out CacheEntry entry)
        {
            lock (_sync)
            {
                return TryGetActiveEntryUnsafe(key, out entry);
            }
        }

        private bool TryGetActiveEntryUnsafe(string key, out CacheEntry entry)
        {
            if (!_cache.TryGetValue(key, out entry))
            {
                return false;
            }

            if (entry.ExpiresAtUtc <= DateTimeOffset.UtcNow)
            {
                _cache.Remove(key);
                entry = default;
                return false;
            }

            return true;
        }

        private static CacheEntry CreateEntry(object? value, TimeSpan ttl)
        {
            return new CacheEntry(value, DateTimeOffset.UtcNow.Add(ttl));
        }

        private readonly record struct CacheEntry(object? Value, DateTimeOffset ExpiresAtUtc);
    }
}
