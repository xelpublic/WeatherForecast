using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Wf.Infrastructure.WeatherPersistanceCache;

public class RedisCacheService : ICacheService
{
    private readonly IDistributedCache _distributedCache;
    private readonly ILogger<RedisCacheService> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public RedisCacheService(IDistributedCache distributedCache, ILogger<RedisCacheService> logger)
    {
        _distributedCache = distributedCache;
        _logger = logger;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = false
        };
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
    {
        try
        {
            var cachedData = await _distributedCache.GetStringAsync(key, cancellationToken);
            if (string.IsNullOrEmpty(cachedData))
            {
                _logger.LogDebug("Кэш-промах для ключа: {Key}", key);
                return null;
            }

            _logger.LogDebug("Кэш-попадание для ключа: {Key}", key);
            return JsonSerializer.Deserialize<T>(cachedData, _jsonOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка получения кэша для ключа: {Key}", key);
            return null;
        }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default) where T : class
    {
        try
        {
            var serializedData = JsonSerializer.Serialize(value, _jsonOptions);
            var options = new DistributedCacheEntryOptions();

            if (expiration.HasValue)
            {
                options.SetAbsoluteExpiration(expiration.Value);
            }
            else
            {
                options.SetAbsoluteExpiration(TimeSpan.FromMinutes(30));
            }

            await _distributedCache.SetStringAsync(key, serializedData, options, cancellationToken);
            _logger.LogDebug("Кэш установлен для ключа: {Key} с истечением: {Expiration}", key, expiration?.ToString() ?? "30 минут");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка установки кэша для ключа: {Key}", key);
        }
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            await _distributedCache.RemoveAsync(key, cancellationToken);
            _logger.LogDebug("Кэш удален для ключа: {Key}", key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка удаления кэша для ключа: {Key}", key);
        }
    }

    public async Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            var cachedData = await _distributedCache.GetStringAsync(key, cancellationToken);
            return !string.IsNullOrEmpty(cachedData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка проверки существования кэша для ключа: {Key}", key);
            return false;
        }
    }
}