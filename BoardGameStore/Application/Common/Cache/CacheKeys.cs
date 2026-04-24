using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Cache
{
    public static class CacheKeys
    {
        public const string CategoriesKey = "catalog:categories:all";
        public const string ProductList = "catalog:productList:all";
        public const string IpRateLimit = "rate_limit:{ip}";
        public const string IpWithApiRateLimit = "rate_limit:{ip}:{endpoint}";
        public static string JwtTokenBlackList(string jti) => $"jwt:blacklist:{jti}";
        public static string CartKeyToken(string token) => $"guest_cart:{token}";
    }
}
