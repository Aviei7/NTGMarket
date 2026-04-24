using Domain.Models.UserModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Auth
{
    public interface IJWTProvider
    {
        public string GenerateToken(UsersModel user);

        public string? GetJtiFromToken(HttpContext httpContext);

        public DateTime? GetExpUtcFromToken(string token);

        public TimeSpan GetTtlRemToken(string token);
    }
}
