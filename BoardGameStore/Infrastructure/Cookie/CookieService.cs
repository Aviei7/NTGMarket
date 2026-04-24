using Application.Interfaces.Cache;
using Infrastructure.Auth;
using Microsoft.AspNetCore.Http;
using Application.Interfaces.Cache;
using Application.Interfaces.Auth;

namespace Infrastructure
{
    public class CookieService : ICookieService
    {
        private readonly IHttpContextAccessor _http;

        public CookieService(IHttpContextAccessor http)
        {
            _http = http;
        }

        public string? GetTokenFromRequest(string name) => _http.HttpContext?.Request.Cookies[name];

        public void SetJwtCookie(string token, CookieOptions options, string tokenName)
        {
            var response = _http.HttpContext?.Response;

            response?.Cookies.Append(tokenName, token, options);
        }

        public void ClearJwtCookie(string tokenName, string path)
        {
            var response = _http.HttpContext?.Response;
            var isHttps = _http.HttpContext?.Request.IsHttps ?? false;

            response?.Cookies.Delete(tokenName, new CookieOptions
            {
                Path = path,
                Secure = isHttps,
                SameSite = isHttps ? SameSiteMode.None : SameSiteMode.Lax
            });
        }
    }
}
