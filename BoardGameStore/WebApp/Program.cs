using Application.Common.Cart;
using Application.Interfaces;
using Application.Interfaces.Auth;
using Application.Interfaces.Cabinet;
using Application.Interfaces.Cache;
using Application.Interfaces.Cart;
using Application.Interfaces.Catalog;
using Application.Interfaces.CatalogFilter;
using Application.Interfaces.Checkout;
using Application.Interfaces.DBContext;
using Application.Interfaces.Loyalty;
using Application.Interfaces.Users;
using Application.Services.Cabinet;
using Application.Services.Cache;
using Application.Services.Cart;
using Application.Services.Catalog;
using Application.Services.Checkout;
using Application.Services.Loyalty;
using Application.Services.Pagination;
using Application.Services.Users.AuthValidationService;
using Application.Services.Users.UsersServices;
using Infrastructure;
using Infrastructure.Auth;
using Infrastructure.Auth.Users;
using Infrastructure.Context;
using Infrastructure.Repositories;
using Infrastucture.Context;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using StackExchange.Redis;
using WebApp.Extentions;
using WebApp.Middleware;

var builder = WebApplication.CreateBuilder(args);
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? ["http://localhost:4200"];
var isDevelopment = builder.Environment.IsDevelopment();
var databaseProvider = builder.Configuration["Database:Provider"]?.Trim() ?? "Sqlite";
var cacheProvider = builder.Configuration["Cache:Provider"]?.Trim() ?? "Memory";

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<BoardStoreContext>(options => ConfigureDatabase(options, builder.Configuration));

builder.Services.Configure<CartCookieOptions>(builder.Configuration.GetSection("CartCookieOptions"));
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.HttpOnly = HttpOnlyPolicy.Always;
    options.Secure = isDevelopment ? CookieSecurePolicy.None : CookieSecurePolicy.Always;
    options.MinimumSameSitePolicy = isDevelopment ? SameSiteMode.Lax : SameSiteMode.None;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("spa", policy =>
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials());
});

builder.Services.AddApiAuthentication(builder.Configuration);

builder.Services.AddScoped<ICatalogService, CatalogService>();
builder.Services.AddScoped<ICatalogReprository, CatalogRepository>();
builder.Services.AddScoped<IFilterService, FilterService>();
builder.Services.AddScoped<IFilterRepository, FilterRepository>();
builder.Services.AddScoped<IPaginationService, PaginationService>();
builder.Services.AddScoped<IDatabaseSeeder, DatabaseSeeder>();
builder.Services.AddScoped<IBoardStoreContext, BoardStoreContext>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IAuthValidationService, AuthValidationService>();
builder.Services.AddScoped<IUsersServices, UsersServices>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IJWTProvider, JWTProvider>();
builder.Services.AddScoped<ICabinetServices, CabinetServices>();
builder.Services.AddScoped<ICookieService, CookieService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICartTokenService, CartTokenService>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICheckoutService, CheckoutService>();
builder.Services.AddScoped<ICheckoutRepository, CheckoutRepository>();
builder.Services.AddScoped<IDiscountService, DiscountService>();
builder.Services.AddScoped<ILoyaltyRepository, LoyaltyRepository>();
builder.Services.AddTransient<ExceptionHandlingMiddleware>();
builder.Services.AddTransient<IPMiddleware>();

AddCacheService(builder.Services, builder.Configuration);

var logDir = Path.Combine(AppContext.BaseDirectory, "Logs");
Directory.CreateDirectory(logDir);

var mainLog = Path.Combine(logDir, "app.log");
var jsonLog = Path.Combine(logDir, "query.log");

if (File.Exists(mainLog))
{
    File.WriteAllText(mainLog, string.Empty);
}

if (File.Exists(jsonLog))
{
    File.WriteAllText(jsonLog, string.Empty);
}

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.File(
        mainLog,
        restrictedToMinimumLevel: LogEventLevel.Information,
        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File(
        jsonLog,
        restrictedToMinimumLevel: LogEventLevel.Debug,
        outputTemplate: "{Message:lj}{NewLine}")
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

app.Logger.LogInformation(
    "Starting BoardGameStore with database provider {DatabaseProvider} and cache provider {CacheProvider}.",
    databaseProvider,
    cacheProvider);

await InitialiseDatabaseAsync(app);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseStaticFiles();
app.UseRouting();
app.UseCors("spa");
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<IPMiddleware>();
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers();
app.Run();

static void ConfigureDatabase(DbContextOptionsBuilder optionsBuilder, IConfiguration configuration)
{
    var provider = configuration["Database:Provider"]?.Trim().ToLowerInvariant() ?? "sqlite";
    var connectionString = configuration.GetConnectionString("DefaultConnection") ?? "Data Source=boardstore.db";

    switch (provider)
    {
        case "sqlite":
            connectionString = NormaliseSqliteConnectionString(connectionString, AppContext.BaseDirectory);
            optionsBuilder.UseSqlite(connectionString);
            break;

        case "sqlserver":
            optionsBuilder.UseSqlServer(connectionString);
            break;

        default:
            throw new InvalidOperationException(
                $"Unsupported database provider '{provider}'. Supported values are 'Sqlite' and 'SqlServer'.");
    }
}

static void AddCacheService(IServiceCollection services, IConfiguration configuration)
{
    var provider = configuration["Cache:Provider"]?.Trim().ToLowerInvariant() ?? "memory";

    switch (provider)
    {
        case "memory":
            services.AddSingleton<ICacheService, MemoryCacheService>();
            break;

        case "redis":
            var redisConnectionString = configuration.GetConnectionString("Redis");
            if (string.IsNullOrWhiteSpace(redisConnectionString))
            {
                throw new InvalidOperationException(
                    "Cache provider is set to Redis, but ConnectionStrings:Redis is missing.");
            }

            services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(redisConnectionString));
            services.AddSingleton<ICacheService, RedisCacheService>();
            break;

        default:
            throw new InvalidOperationException(
                $"Unsupported cache provider '{provider}'. Supported values are 'Memory' and 'Redis'.");
    }
}

static string NormaliseSqliteConnectionString(string connectionString, string basePath)
{
    var builder = new SqliteConnectionStringBuilder(connectionString);

    if (string.IsNullOrWhiteSpace(builder.DataSource))
    {
        throw new InvalidOperationException("SQLite connection string must contain a Data Source value.");
    }

    if (!Path.IsPathRooted(builder.DataSource))
    {
        builder.DataSource = Path.GetFullPath(Path.Combine(basePath, builder.DataSource));
    }

    return builder.ToString();
}

static async Task InitialiseDatabaseAsync(WebApplication app)
{
    const int maxAttempts = 12;
    var delayBetweenAttempts = TimeSpan.FromSeconds(5);

    await using var scope = app.Services.CreateAsyncScope();
    var logger = scope.ServiceProvider
        .GetRequiredService<ILoggerFactory>()
        .CreateLogger("Startup");
    var db = scope.ServiceProvider.GetRequiredService<BoardStoreContext>();
    var seeder = scope.ServiceProvider.GetRequiredService<IDatabaseSeeder>();

    for (var attempt = 1; attempt <= maxAttempts; attempt++)
    {
        try
        {
            if (db.Database.IsSqlite())
            {
                await db.Database.EnsureCreatedAsync();
            }
            else
            {
                await db.Database.MigrateAsync();
            }

            logger.LogInformation("Running seed data.");
            seeder.Seed();

            logger.LogInformation("Database is ready.");
            return;
        }
        catch (Exception ex) when (attempt < maxAttempts)
        {
            logger.LogWarning(
                ex,
                "Database startup attempt {Attempt}/{MaxAttempts} failed. Retrying in {DelaySeconds} seconds.",
                attempt,
                maxAttempts,
                delayBetweenAttempts.TotalSeconds);

            await Task.Delay(delayBetweenAttempts);
        }
    }

    throw new InvalidOperationException("The database could not be initialised after multiple startup attempts.");
}
