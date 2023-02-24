using System.Reflection;
using DistanceCalculator.Api.Controllers;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DistanceCalculator.Api.Features.Swagger;

/// <summary>
/// swagger service collection extension
/// setup and add swagger
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// method adds swagger
    /// method add swagger gen
    /// setup swagger doc for "v1"
    /// setup open api info
    /// add security definition
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddConfiguredSwagger(this IServiceCollection services)
    {
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureOptions>();

        services.AddSwaggerGen(options => { options.AddXmlComment(typeof(BaseController).Assembly); });

        services.AddSwaggerGenNewtonsoftSupport();

        return services;
    }

    /// <summary>
    /// setups read xml files with description about open api
    /// </summary>
    /// <param name="options"></param>
    /// <param name="assembly"></param>
    private static void AddXmlComment(this SwaggerGenOptions options, Assembly assembly)
    {
        var xmlFile = $"{assembly.GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        options.IncludeXmlComments(xmlPath);
    }
}