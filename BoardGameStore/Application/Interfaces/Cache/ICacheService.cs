using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Cache
{
    public interface ICacheService
    {
        public Task<T?> GetAsync<T>(string key, CancellationToken ct = default);
        public Task SetAsync<T>(string key, T value, TimeSpan ttl, CancellationToken ct = default);
        public Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan ttl, CancellationToken ct = default);

        public Task<long> SetIpToCache(string key, TimeSpan ttl, CancellationToken ct = default);

        public Task<bool> ExistsKeyAsync(string key);
        public Task BlacklistJtiAsync(string jti, TimeSpan ttl);
    }
}
