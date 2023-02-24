using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace DistanceCalculator.Api.Features.Swagger;

/// <summary>
/// swagger application builder extension
/// </summary>
public static class ApplicationBuilder
{
    /// <summary>
    /// method uses swagger
    /// provides default swagger
    /// provides default swagger ui
    /// setup swagger ui and add api versioning to swagger ui
    /// add description for http methods
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseConfiguredSwagger(this IApplicationBuilder app)
    {
        var apiVersionDescriptionProvider = app.ApplicationServices
            .GetRequiredService<IApiVersionDescriptionProvider>();

        app.UseSwagger();

        app.UseSwaggerUI(options =>
        {
            foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint($"{description.GroupName}/swagger.json",
                    description.GroupName.ToUpperInvariant());

                options.RoutePrefix = "swagger";
            }
        });

        return app;
    }
}