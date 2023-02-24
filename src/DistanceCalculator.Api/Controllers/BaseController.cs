using System.Net;
using System.Reflection;
using DistanceCalculator.Api.Common.Constants;
using DistanceCalculator.Core.ValueObjects;
using DistanceCalculator.Results.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DistanceCalculator.Api.Controllers;

/// <summary>
/// base controller provides a global service resolvers
/// </summary>
[ApiController]
public class BaseController : ControllerBase
{
    /// <summary>resolves mediator</summary>
    protected ISender Mediator =>
        HttpContext.RequestServices.GetRequiredService<ISender>() ?? throw new ArgumentNullException(nameof(ISender));

    protected ObjectResult ProblemResult(Error error)
    {
        var problem = new ProblemDetails
        {
            Title = error.Code.ToString(),
            Detail = error.Message,
            Instance = Assembly.GetEntryAssembly().GetName().Name,
            Extensions =
            {
                { "correlationId", HttpContext.Request.Headers[HeaderConstant.CorrelationId].ToString() },
            },
            Status = error.Code switch
            {
                ErrorCode.CLIENT_ERROR => (int)HttpStatusCode.BadRequest,
                ErrorCode.SERVICE_ERROR => (int)HttpStatusCode.ServiceUnavailable,
                ErrorCode.KEY_ERROR => (int)HttpStatusCode.Conflict,
                _ => (int)HttpStatusCode.InternalServerError,
            },
        };

        return new ObjectResult(problem);
    }
}