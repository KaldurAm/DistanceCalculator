using DistanceCalculator.Evaluator.Enums;

namespace DistanceCalculator.Evaluator.Interfaces
{
    public interface IIataDistanceCalculator
    {
        double Calculate(double originLat, double originLon, double destinationLat, double destinationLon, DistanceUnit unit);
    }
}