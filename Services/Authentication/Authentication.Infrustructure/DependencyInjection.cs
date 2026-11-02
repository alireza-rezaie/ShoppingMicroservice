using Authentication.Core.Repositories;
using Authentication.Core.Services;
using Authentication.Infrustructure.Data;
using Authentication.Infrustructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Authentication.Infrustructure;

public static class DependencyInjection
{
    public static IServiceCollection AddAuthenticationInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AuthDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("AuthDb"),
                sql => sql.EnableRetryOnFailure()));

        services.AddIdentityCore<ApplicationIdentityUser>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireDigit = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;
        })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<AuthDbContext>();

        services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
        services.AddSingleton<ITokenService, TokenService>();
        return services;
    }
}