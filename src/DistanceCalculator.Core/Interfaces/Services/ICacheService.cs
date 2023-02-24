namespace DistanceCalculator.Core.Interfaces.Services;

public interface ICacheService
{
    Task SetAsync(string key, object value, TimeSpan expiration);
    Task<T> GetAsync<T>(string key);
}