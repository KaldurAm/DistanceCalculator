using DistanceCalculator.Core.Interfaces.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace DistanceCalculator.Infrastructure.Services;

public sealed class CacheService : ICacheService
{
    private readonly IMemoryCache _cache;
    private readonly ILogger<CacheService> _logger;

    public CacheService(IMemoryCache cache, ILogger<CacheService> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    /// <inheritdoc />
    public Task SetAsync(string key, object value, TimeSpan expiration)
    {
        _cache.Set(key, value, expiration);

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task<T> GetAsync<T>(string key)
    {
        var value = _cache.Get<T>(key);

        return Task.FromResult(value);
    }
}