using Application.Interfaces.DBContext;
using Application.Services.Catalog;
using Infrastucture.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Infrastucture
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BoardStoreContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IBoardStoreContext>(provider => provider.GetRequiredService<BoardStoreContext>());

            return services;
        }
    }
}
