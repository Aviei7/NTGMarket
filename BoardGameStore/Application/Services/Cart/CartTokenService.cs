using Application.Common.Cart;
using Application.Interfaces.Auth;
using Application.Interfaces.Cache;
using Application.Interfaces.Cart;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace Application.Services.Cart
{
    public class CartTokenService : ICartTokenService
    {
        private readonly CartCookieOptions _cartOptions;
        public CartTokenService(IOptions<CartCookieOptions> cartOptions)
        {
            _cartOptions = cartOptions.Value;
        }

        public CookieOptions BuildCartCookieOptions(DateTimeOffset expiresAtUtc)
        {
            return new CookieOptions
            {
                Path = _cartOptions.Path,
                HttpOnly = _cartOptions.HttpOnly,
                Secure = _cartOptions.Secure,
                SameSite = _cartOptions.SameSite,
                Expires = expiresAtUtc
            };
        }

        public string CreateCartToken(DateTimeOffset expiresAtUtc)
        {
            return $"{Guid.NewGuid():N}.{expiresAtUtc.ToUnixTimeSeconds()}";
        }

        public TimeSpan GetRemainingCartTokenTtl(string? cartToken)
        {
            if (string.IsNullOrWhiteSpace(cartToken))
            {
                return TimeSpan.Zero;
            }

            var tokenParts = cartToken.Split('.', 2, StringSplitOptions.RemoveEmptyEntries);
            if (tokenParts.Length != 2)
            {
                return TimeSpan.Zero;
            }

            if (!long.TryParse(tokenParts[1], NumberStyles.None, CultureInfo.InvariantCulture, out var expiresUnixSeconds))
            {
                return TimeSpan.Zero;
            }

            var expiresAtUtc = DateTimeOffset.FromUnixTimeSeconds(expiresUnixSeconds);
            var ttl = expiresAtUtc - DateTimeOffset.UtcNow;

            return ttl > TimeSpan.Zero ? ttl : TimeSpan.Zero;
        }
    }
}
