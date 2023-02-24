using DistanceCalculator.Core.Common.Primitives;
using DistanceCalculator.Evaluator.Enums;
using MediatR;

namespace DistanceCalculator.Core.Calculator.Queries;

/// <summary>
/// command to calculate distance
/// </summary>
public class CalculateDistanceQuery : IRequest<Result<ValueObjects.Distance>>
{
    public CalculateDistanceQuery(string origin, string destination, DistanceUnit unit)
    {
        Origin = origin;
        Destination = destination;
        Unit = unit;
    }

    public string Origin { get; }
    public string Destination { get; }
    public DistanceUnit Unit { get; }
}