using DistanceCalculator.Core.Interfaces.Services;
using DistanceCalculator.Infrastructure.Policy;
using DistanceCalculator.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DistanceCalculator.Infrastructure;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// configuring persistence database
    /// </summary>
    public static IServiceCollection ConfigureInfrastructurePersistence(this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }

    /// <summary>
    /// configuring distributes cache
    /// </summary>
    public static IServiceCollection ConfigureInfrastructureCache(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMemoryCache();

        return services;
    }

    /// <summary>
    /// configuring http clients services
    /// </summary>
    public static IServiceCollection ConfigureInfrastructureHttp(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<IAirportService, AirportService>(nameof(AirportService),
            options =>
            {
                options.BaseAddress = new Uri(configuration["AirportServiceOptions:Origin"]);
                options.Timeout = TimeSpan.FromSeconds(int.Parse(configuration["AirportServiceOptions:TimeoutInSeconds"]));
            })
            .SetHandlerLifetime(TimeSpan.FromMinutes(1))
            .AddPolicyHandler(RetryPolicy.GetRetryPolicy());

        return services;
    }

    /// <summary>
    /// configuring infrastructure services
    /// </summary>
    public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IAirportService, AirportService>();
        services.AddScoped<ICacheService, CacheService>();

        return services;
    }
}