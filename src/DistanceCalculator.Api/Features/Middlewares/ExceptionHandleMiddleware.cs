using System.Net;
using DistanceCalculator.Api.Common.Constants;
using DistanceCalculator.Core.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace DistanceCalculator.Api.Features.Middlewares;

/// <summary>
/// middleware handles every exceptions
/// </summary>
public class ExceptionHandleMiddleware
{
    private readonly ILogger<ExceptionHandleMiddleware> _logger;
    private readonly RequestDelegate _next;

    /// <summary>
    /// create <see cref="ExceptionHandleMiddleware"/> instance
    /// </summary>
    public ExceptionHandleMiddleware(RequestDelegate next, ILogger<ExceptionHandleMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// use middleware
    /// </summary>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occured while processing request");

            ProblemDetails problem = new()
            {
                Status = (int)HttpStatusCode.InternalServerError,
                Title = "Error occured while processing request",
                Detail = ex.Message,
                Instance = ex.Source,
            };

            problem.Extensions.Add("correlationId", context.Request.Headers[HeaderConstant.CorrelationId].ToString());

            if (ex is ValidatorException validationException)
            {
                var dictionary = validationException.Errors.Select(s =>
                    new KeyValuePair<string, object>(s.PropertyName, s.ErrorMessage));

                foreach (var error in dictionary)
                    problem.Extensions.Add(error);
            }

            await context.Response.WriteAsJsonAsync(problem);
        }
    }
}