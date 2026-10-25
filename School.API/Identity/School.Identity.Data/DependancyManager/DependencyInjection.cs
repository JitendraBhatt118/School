using Microsoft.Extensions.DependencyInjection;
using School.Infrastructure.Extension;
using System.Reflection;

namespace School.Identity.Data.DependancyManager;

public static class DependencyInjection
{
    public static IServiceCollection RegisterRepositoryServices(this IServiceCollection services)
    {
        services.AddServicesByConvention(
            Assembly.GetExecutingAssembly(),
            ServiceLifetime.Scoped);

        return services;
    }
}
