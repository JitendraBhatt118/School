using Microsoft.Extensions.DependencyInjection;
using School.Infrastructure.Extension;
using System.Reflection;

namespace School.Identity.Business.DependancyManager;

public static class DependencyInjection
{
    public static IServiceCollection RegisterBusinessServices(this IServiceCollection services)
    {
        services.AddServicesByConvention(
            Assembly.GetExecutingAssembly(),
            ServiceLifetime.Scoped);

        return services;
    }
}
