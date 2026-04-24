using Application.Common.Cache;
using Application.Exceptions;
using Application.Interfaces.Cache;

namespace WebApp.Middleware
{
    public class IPMiddleware : IMiddleware
    {

        private readonly ILogger<IPMiddleware> _logger;
        private readonly ICacheService _cacheService;

        public IPMiddleware(ILogger<IPMiddleware> logger, ICacheService cacheService)
        {
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (ShouldSkipRateLimit(context))
            {
                await next(context);
                return;
            }

            var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            if (ip == "::1")
            {
                ip = "localhost";
            }

            var key = CacheKeys.IpWithApiRateLimit
                .Replace("{ip}", ip)
                .Replace("{endpoint}", BuildEndpointKey(context.Request));

            await WriteIpToCache(key);

            await next(context);
        }


        private async Task WriteIpToCache(string key)
        {
            var count = await _cacheService.SetIpToCache(key, CacheTTL.IpRateLimit);

            if (count > 300)
            {
                throw new TooManyRequestsException(retryAfter: TimeSpan.FromSeconds(60));
            }

        }

        private static bool ShouldSkipRateLimit(HttpContext context)
        {
            if (HttpMethods.IsOptions(context.Request.Method))
            {
                return true;
            }

            var path = context.Request.Path.Value?.ToLowerInvariant() ?? "/";

            return path.StartsWith("/api/server/status");
        }

        private static string BuildEndpointKey(HttpRequest request)
        {
            var path = request.Path.Value?.Trim('/').ToLowerInvariant() ?? "root";

            if (string.IsNullOrWhiteSpace(path))
            {
                path = "root";
            }

            path = path.Replace('/', ':');

            return $"{request.Method.ToLowerInvariant()}:{path}";
        }
    }
}
