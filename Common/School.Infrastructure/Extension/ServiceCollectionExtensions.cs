using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using School.Infrastructure.Persistence.Dapper;
using School.ViewModel.Common;
using System.Reflection;
using System.Text;

namespace School.Infrastructure.Extension;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServicesByConvention(this IServiceCollection services,
        Assembly assembly,
        ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        IEnumerable<Type> types = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && !t.IsGenericType);

        foreach (Type implementationType in types)
        {
            Type? interfaceType = implementationType
                .GetInterfaces()
                .FirstOrDefault(type => type.Name == $"I{implementationType.Name}");

            if (interfaceType is not null)
            {
                services.Add(new ServiceDescriptor(interfaceType, implementationType, lifetime));
            }
        }

        return services;
    }

    public static IServiceCollection AddDapper(this IServiceCollection services,
        IConfiguration configuration,
        string connectionStringName)
    {
        services.AddDapper(configuration);
        services.AddScoped(factory =>
        {
            IDapperExecutorResolver resolver = factory.GetRequiredService<IDapperExecutorResolver>();
            return resolver.Get(connectionStringName);
        });

        return services;
    }

    public static IServiceCollection AddDapper(this IServiceCollection services,
        IConfiguration configuration)
    {
        Dictionary<string, string> connections = configuration
            .GetSection("ConnectionStrings")
            .Get<Dictionary<string, string>>() ?? [];

        services.AddSingleton(connections);
        services.AddScoped<IDapperExecutorResolver, DapperExecutorResolver>();

        return services;
    }

    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services,
        IConfiguration configuration)
    {
        JwtConfigurationsModel jwtConfigurationsModel
            = configuration.GetSection("JwtConfigurations").Get<JwtConfigurationsModel>() ?? new();

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.SaveToken = false;
            x.RequireHttpsMetadata = true;

            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtConfigurationsModel?.Issuer,
                ValidAudience = jwtConfigurationsModel?.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(jwtConfigurationsModel?.Secret ?? string.Empty)),
                ClockSkew = TimeSpan.Zero
            };

            x.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    if (context.Exception is SecurityTokenExpiredException)
                    {
                        context.Response?.Headers?.Append("Token-Expired", "true");
                    }

                    return Task.CompletedTask;
                }
            };
        });

        return services;
    }
}
