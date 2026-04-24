using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Cart
{
    public interface ICartTokenService
    {
        public CookieOptions BuildCartCookieOptions(DateTimeOffset expiresAtUtc);

        public string CreateCartToken(DateTimeOffset expiresAtUtc);

        public TimeSpan GetRemainingCartTokenTtl(string? cartToken);
    }
}
