using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Auth
{
    public interface ICookieService
    {
        public string? GetTokenFromRequest(string name);
        public void SetJwtCookie(string token, CookieOptions options, string tokenName);
        public void ClearJwtCookie(string tokenName, string path);
    }
}
