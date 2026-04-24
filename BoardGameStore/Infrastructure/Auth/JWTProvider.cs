using Application.Interfaces.Auth;
using Domain.Models.UserModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Infrastructure.Auth
{
    public class JWTProvider : IJWTProvider
    {
        private readonly JwtOptions _options;

        public JWTProvider(IOptions<JwtOptions> options)
        {
            _options = options.Value;
        }

        public string GenerateToken(UsersModel user)
        {
            var jti = Guid.NewGuid().ToString();

            Claim[] claims =
            [
                new Claim(JwtRegisteredClaimNames.Sub, user.UserID.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, jti),
            ];

            var singingCredantials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
                    SecurityAlgorithms.HmacSha256
                );

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: singingCredantials,
                expires: DateTime.UtcNow.AddHours(_options.ExpiresHours)
                );
            var handler = new JwtSecurityTokenHandler();

            var tokenValue = handler.WriteToken(token);
            return tokenValue;
        }

        public string? GetJtiFromToken(HttpContext httpContext)
        {
           return httpContext.User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
        }

        public TimeSpan GetTtlRemToken(string token)
        {

            var expUtc = GetExpUtcFromToken(token);
            if (!expUtc.HasValue)
                return TimeSpan.Zero;

            var ttl = expUtc.Value - DateTime.UtcNow;
            return ttl > TimeSpan.Zero ? ttl : TimeSpan.Zero;
        }

        public DateTime? GetExpUtcFromToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token)) return null;

            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            return jwt.ValidTo; 
        }
    }
}
