using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SafeManagement.Data;

public static class SafeDbRegistration
{
    public const string ConnectionName = "DefaultConnection";

    public static IServiceCollection AddSafeSharedDatabase(this IServiceCollection services, IConfiguration config)
    {
        var cs = config.GetConnectionString(ConnectionName)
            ?? "Server=localhost;Database=IndorDB;User Id=sa;Password=123;MultipleActiveResultSets=true;TrustServerCertificate=true;Encrypt=false";
        services.AddDbContext<SafeDbContext>(o => o.UseSqlServer(cs));
        services.AddScoped<SafeHubService>();
        services.AddSingleton(new SafeSql(cs));
        services.Configure<SafeMailOptions>(config.GetSection("Email"));
        services.AddSingleton<SafeMailer>();
        services.Configure<MvcOptions>(o => o.Filters.Add<SafeDatabaseErrorFilter>());
        return services;
    }
}
