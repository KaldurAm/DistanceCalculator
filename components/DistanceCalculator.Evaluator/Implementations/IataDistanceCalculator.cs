using System;
using DistanceCalculator.Evaluator.Enums;
using DistanceCalculator.Evaluator.Interfaces;

namespace DistanceCalculator.Evaluator.Implementations
{
    public class IataDistanceCalculator : IIataDistanceCalculator
    {
        /// <inheritdoc />
        public double Calculate(double originLat, double originLon, double destinationLat, double destinationLon, DistanceUnit unit)
        {
            var distance = Calculate(originLat, originLon, destinationLat, destinationLon);

            if (unit.Equals(DistanceUnit.KM))
                distance *= 1.609344;

            return distance;
        }

        private static double Calculate(double originLat, double originLon, double destinationLat, double destinationLon)
        {
            var rlat1 = Math.PI * originLat / 180;
            var rlat2 = Math.PI * destinationLat / 180;
            var theta = originLon - destinationLon;
            var rtheta = Math.PI * theta / 180;
            var dist = Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) * Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

            return dist;
        }
    }
}