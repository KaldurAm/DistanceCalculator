using System.Reflection;
using DistanceCalculator.Core.Behaviours;
using DistanceCalculator.Core.Options;
using FluentValidation;
using DistanceCalculator.Evaluator;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DistanceCalculator.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureApplicationAssemblies(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }

    public static IServiceCollection ConfigureApplicationPipelines(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));

        return services;
    }

    public static IServiceCollection ConfigureApplicationComponents(this IServiceCollection services)
    {
        services.AddDistanceCalculator();

        return services;
    }

    public static IServiceCollection ConfigureApplicationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AirportServiceOptions>(configuration.GetSection(AirportServiceOptions.SectionName));

        return services;
    }
}