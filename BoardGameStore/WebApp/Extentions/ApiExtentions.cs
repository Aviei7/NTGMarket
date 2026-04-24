using Application.Common.Cache;
using Application.Interfaces.Cache;
using Infrastructure.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic.FileIO;
using StackExchange.Redis;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace WebApp.Extentions
{
    public static class ApiExtentions
    {
        public static IServiceCollection AddApiAuthentication(
            this IServiceCollection services,
            IConfiguration config)
        {

            services.Configure<JwtOptions>(config.GetSection("JwtOptions"));

            var jwt = config.GetSection("JwtOptions").Get<JwtOptions>()!;
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;


            services.AddAuthentication().AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new()
                { 
                    ValidateAudience  = false,
                    ValidateIssuer = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.SecretKey))
                };

                options.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies[jwt.JwtCookieName];
                        return Task.CompletedTask;
                    },

                     OnTokenValidated = async ctx =>
                     {

                         // достаём jti
                         var jti = ctx.Principal?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
                         if (string.IsNullOrEmpty(jti))
                             return;

                         // Redis check
                         var cacheService = ctx.HttpContext.RequestServices.GetRequiredService<ICacheService>();

                         var isRevoked = await cacheService.ExistsKeyAsync(CacheKeys.JwtTokenBlackList(jti));
                         if (isRevoked)
                             ctx.Fail("Token revoked");
                     }
                };
            });

            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            services.AddAuthentication();
            return services;
        }
    }
}
