using DistanceCalculator.Evaluator.Implementations;
using DistanceCalculator.Evaluator.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DistanceCalculator.Evaluator
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDistanceCalculator(this IServiceCollection services)
        {
            services.AddTransient<IIataDistanceCalculator, IataDistanceCalculator>();
            return services;
        }
    }
}