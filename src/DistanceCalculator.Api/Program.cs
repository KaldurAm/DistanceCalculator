using DistanceCalculator.Api.Features.Middlewares;
using DistanceCalculator.Api.Features.Swagger;
using DistanceCalculator.Api.Features.Versioning;
using DistanceCalculator.Core;
using DistanceCalculator.Infrastructure;
using Newtonsoft.Json;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

try
{
    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger();

    builder.Logging.ClearProviders();
    builder.Logging.AddSerilog(Log.Logger);

    Log.Logger.Information("Starting configure web host ({ApplicationName}) in environment ({EnvironmentName})...",
        builder.Environment.ApplicationName,
        builder.Environment.EnvironmentName);

    #region Configure host

    builder.Services.AddConfiguredSwagger();
    builder.Services.AddConfiguredVersioning();
    builder.Services.AddControllers().AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
    });
    builder.Services.AddEndpointsApiExplorer();

    #endregion

    Log.Logger.Information("Web host configured");

    #region Configure application

    builder.Services
        .ConfigureApplicationAssemblies()
        .ConfigureApplicationPipelines()
        .ConfigureApplicationComponents()
        .ConfigureApplicationOptions(builder.Configuration);

    #endregion

    Log.Logger.Information("Application configured");

    #region Configure infrastructure

    builder.Services
        .ConfigureInfrastructurePersistence(builder.Configuration)
        .ConfigureInfrastructureCache(builder.Configuration)
        .ConfigureInfrastructureHttp(builder.Configuration)
        .ConfigureInfrastructureServices();

    #endregion

    Log.Logger.Information("Infrastructure configured");

    var app = builder.Build();
    app.UseApiVersioning();
    app.UseConfiguredSwagger();
    app.UseHttpsRedirection();
    app.UseMiddleware<RequestGuidMiddleware>();
    app.UseMiddleware<ExceptionHandleMiddleware>();
    app.UseMiddleware<LoggingMiddleware>();
    app.MapControllers();

    Log.Logger.Information("Running web host ({ApplicationName})...", builder.Environment.ApplicationName);

    app.Run();
}
catch (Exception ex)
{
    Log.Logger.Fatal(ex, "Program terminated unexpectedly ({ApplicationName})!", builder.Environment.ApplicationName);
}
finally
{
    Log.CloseAndFlush();
}