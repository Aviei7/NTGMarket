using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Cache
{
    public static class CacheTTL
    {
        public static readonly TimeSpan Categories = TimeSpan.FromHours(1);
        public static readonly TimeSpan ProductList = TimeSpan.FromMinutes(5);
        public static readonly TimeSpan IpRateLimit = TimeSpan.FromMinutes(1);
    }
}
