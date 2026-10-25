using Microsoft.AspNetCore.Cors.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddHttpContextAccessor();

/* Add Cors */
builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", options =>
    {
        string[] allowedOrigins = builder.Configuration
                                    .GetSection("Cors:AllowedOrigins")
                                    .Get<string[]>() ?? [];

        string[] allowedHeaders = builder.Configuration
                                    .GetSection("Cors:AllowedHeaders")
                                    .Get<string[]>() ?? [];

        string[] allowedMethods = builder.Configuration
                                    .GetSection("Cors:AllowedMethods")
                                    .Get<string[]>() ?? [];

        CorsPolicyBuilder corsPolicyBuilder
            = options.WithOrigins(allowedOrigins)
                        .SetIsOriginAllowed(origin =>
                        {
                            if (!Uri.TryCreate(origin, UriKind.Absolute, out var requestUri))
                                return false;

                            return allowedOrigins.Any(configuredOrigin =>
                            {
                                if (!Uri.TryCreate(configuredOrigin, UriKind.Absolute, out var configuredUri))
                                    return false;

                                // Scheme (http/https) must match
                                if (!string.Equals(requestUri.Scheme, configuredUri.Scheme, StringComparison.OrdinalIgnoreCase))
                                    return false;

                                // Port must match
                                if (requestUri.Port != configuredUri.Port)
                                    return false;

                                // Exact match
                                if (string.Equals(requestUri.Host, configuredUri.Host, StringComparison.OrdinalIgnoreCase))
                                    return true;

                                // Wildcard subdomain match
                                return requestUri.Host.EndsWith("." + configuredUri.Host, StringComparison.OrdinalIgnoreCase);
                            });
                        })
                        .AllowCredentials();

        if (allowedHeaders.Contains("*"))
        {
            corsPolicyBuilder.AllowAnyHeader();
        }
        else
        {
            corsPolicyBuilder.WithHeaders(allowedHeaders);
        }

        if (allowedMethods.Contains("*"))
        {
            corsPolicyBuilder.AllowAnyMethod();
        }
        else
        {
            corsPolicyBuilder.WithMethods(allowedMethods);
        }
    });
});

builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

/* Below code is for limiting the rate (# of reqeusts). Do not remove it
 * as it may be useful for future. 
 */
//builder.Services.AddRateLimiter(options =>
//{
//    /* Below cofigurations is for limiting requests per host. */
//    options.AddFixedWindowLimiter("per-host", config =>
//    {
//        config.PermitLimit = 100;
//        config.Window = TimeSpan.FromMinutes(1);
//        config.QueueLimit = 0;
//    });

//    /* Below code will limit the requests per user. */
//    options.AddPolicy("per-ip", httpContext =>
//        RateLimitPartition.GetFixedWindowLimiter
//        (
//            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
//            factory: _ => new FixedWindowRateLimiterOptions
//            {
//                PermitLimit = 50,
//                Window = TimeSpan.FromMinutes(1),
//                QueueLimit = 0
//            }
//        ));

//    /* Below code is apply if above limits exceeded. */
//    options.OnRejected = async (context, token) =>
//    {
//        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
//        await context.HttpContext.Response.WriteAsync("Too many requests", token);
//    };
//});

var app = builder.Build();
if (!app.Environment.IsDevelopment() && !app.Environment.IsEnvironment("Docker"))
{
    app.UseHttpsRedirection();
}

app.UseRouting();

app.UseCors("AllowOrigin");

app.MapReverseProxy();
/* Do not remove it, this is for future usage. */
//.RequireRateLimiting("per-host");
//.RequireRateLimiting("per-ip");

app.MapControllers();

/* Do not remove it, this is for future usage. */
//app.UseRateLimiter();

app.Run();
