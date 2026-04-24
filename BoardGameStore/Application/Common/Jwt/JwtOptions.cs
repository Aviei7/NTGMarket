using Microsoft.AspNetCore.Http;

namespace Infrastructure.Auth
{
    public class JwtOptions
    {
        public string JwtCookieName { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public int ExpiresHours { get; set; } = 2;
        public string TokenName { get; set; } = "guest_cart";
        public string Path { get; set; } = "/";
        public bool HttpOnly { get; set; } = true;
        public bool Secure { get; set; } = true;
        public SameSiteMode SameSite { get; set; } = SameSiteMode.None;
    }
}
