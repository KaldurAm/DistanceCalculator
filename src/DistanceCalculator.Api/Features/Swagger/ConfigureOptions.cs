using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DistanceCalculator.Api.Features.Swagger;

/// <summary>swagger configuration options</summary>
internal class ConfigureOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly ILogger<ConfigureOptions> _logger;
    private readonly IApiVersionDescriptionProvider _provider;

    /// <summary>
    /// constructor implements logger and api versioning provider
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="provider"></param>
    public ConfigureOptions(ILogger<ConfigureOptions> logger, IApiVersionDescriptionProvider provider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
    }

    /// <inheritdoc />
    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            try
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }

    /// <summary>
    /// creates open api information for web api versioning
    /// </summary>
    /// <param name="description"></param>
    /// <returns></returns>
    private OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
        var info = new OpenApiInfo
        {
            Title = "Web Api computing the distance between iata codes",
            Version = GetAssemblyVersion(),
            Description = GetSwaggerDescription(),
        };

        if (description.IsDeprecated)
        {
            info.Description += " This API version has been deprecated";
        }

        return info;
    }

    /// <summary>
    /// retrieves version from assembly
    /// </summary>
    /// <returns></returns>
    private static string GetAssemblyVersion()
    {
        return Assembly.GetEntryAssembly()
            ?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            ?.InformationalVersion ?? "undefined";
    }

    /// <summary>
    /// retrieves descriptions from xml or md generated file to display on swagger
    /// </summary>
    /// <returns></returns>
    private string GetSwaggerDescription()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "SwaggerDescription.xml");

        try
        {
            return File.ReadAllText(path);
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }
}