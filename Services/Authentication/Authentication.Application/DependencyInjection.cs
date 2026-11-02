using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Authentication.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddAuthenticationApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
        return services;
    }
}