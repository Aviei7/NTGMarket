using Application.DTO.Output.FilterDTO;
using Application.DTO.Output.Users;
using Application.DTO.Output.UsersDTO;
using Application.Exceptions;
using Application.Interfaces.Auth;
using Application.Interfaces.Cache;
using Application.Interfaces.DBContext;
using Application.Interfaces.Users;
using Application.Services.Users.UsersServices;
using Infrastructure.Auth;
using Infrastucture.Context;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Dynamic.Core.Tokenizer;
using System.Security.Claims;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersServices _userServices;
        private readonly IJWTProvider _jwtProvider;
        private readonly ICookieService _cookieService;
        private readonly ICacheService _cacheService;
        private readonly JwtOptions _jwtOptions;

        public UsersController(
            ICacheService cacheService,
            ICookieService cookieService,
            IOptions<JwtOptions> jwtOptions,
            IJWTProvider jwtProvider,
            IUsersServices userServices)
        {
            _userServices = userServices;
            _jwtProvider = jwtProvider;
            _cookieService = cookieService;
            _cacheService = cacheService;
            _jwtOptions = jwtOptions.Value;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDTO model)
        {
            var token = await _userServices.Register(model.FirstName, model.LastName, model.Password, model.Email, model.Phone);

            Console.WriteLine($"Token: {token}");

            var options = new CookieOptions
            {
                Path = _jwtOptions.Path,
                HttpOnly = _jwtOptions.HttpOnly,
                Secure = _jwtOptions.Secure,
                SameSite = _jwtOptions.SameSite,
                Expires = DateTimeOffset.UtcNow.AddHours(_jwtOptions.ExpiresHours)
            };

            _cookieService.SetJwtCookie(token, options,  _jwtOptions.JwtCookieName);

            return Ok(new { token });

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDTO loginUserDTO)
        {
            var token = await _userServices.Login(loginUserDTO.Email, loginUserDTO.Password);

            var options = new CookieOptions
            {
                Path = _jwtOptions.Path,
                HttpOnly = _jwtOptions.HttpOnly,
                Secure = _jwtOptions.Secure,
                SameSite = _jwtOptions.SameSite,
                Expires = DateTimeOffset.UtcNow.AddHours(_jwtOptions.ExpiresHours)
            };

            _cookieService.SetJwtCookie(token, options, _jwtOptions.JwtCookieName);

            return Ok(new { token });
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<UserInfoDTO>> Me(CancellationToken ct)
        {
            var userIdStr = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            if (!int.TryParse(userIdStr, out var userId))
                return Unauthorized();

            var dto = await _userServices.GetUserByIdAsync(userId, ct);
            if (dto is null) return NotFound();

            return Ok(dto);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult<UserInfoDTO>> Logout(CancellationToken ct)
        {
            var ctx = HttpContext;

            var token = _cookieService.GetTokenFromRequest(_jwtOptions.JwtCookieName);
            if (token is null) return NotFound();

            var jti = _jwtProvider.GetJtiFromToken(ctx);
            if (jti is null) return NotFound();

            var ttlToken = _jwtProvider.GetTtlRemToken(token);
            if (ttlToken == TimeSpan.Zero) return Ok();

            await _cacheService.BlacklistJtiAsync(jti, ttlToken);

            _cookieService.ClearJwtCookie(_jwtOptions.JwtCookieName, "/");

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<List<UserInfoDTO>>> GetUserList(CancellationToken ct)
        {
            var userList = await _userServices.GetUserList(ct);
            return Ok(userList);
        }
    }
}
