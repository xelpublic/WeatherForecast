using Microsoft.Extensions.Logging;
using Wf.Application.Interfaces;
using Wf.Infrastructure.WeatherApiCom;
using Wf.Infrastructure.WeatherPersistanceCache;

namespace Wf.Infrastructure.WeatherForecast;

public class WeatherForecastService : IWeatherForecastService
{
    private readonly IWeatherApiComService _weatherApiComService;
    private readonly ICacheService _cacheService;
    private readonly ILogger<WeatherForecastService> _logger;

    public WeatherForecastService(
        IWeatherApiComService weatherApiComService,
        ICacheService cacheService,
        ILogger<WeatherForecastService> logger)
    {
        _weatherApiComService = weatherApiComService;
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<Domain.WeatherForecast?> GetWeatherAsync(double latitude, double longitude, int days,
        CancellationToken cancellationToken)
    {
        var cacheKey = $"weather_{latitude:F4}_{longitude:F4}_{days}";

        _logger.LogDebug("Проверка кэша для ключа: {CacheKey}", cacheKey);
        
        var cachedForecast = await _cacheService.GetAsync<Domain.WeatherForecast>(cacheKey, cancellationToken);
        if (cachedForecast != null)
        {
            _logger.LogInformation("Данные о погоде получены из кэша для координат: {Latitude}, {Longitude}, дней: {Days}",
                latitude, longitude, days);
            return cachedForecast;
        }

        _logger.LogDebug("Кэш-промах для ключа: {CacheKey}. Запрос к API...", cacheKey);
        
        var forecast = await _weatherApiComService.GetWeatherAsync(latitude, longitude, days, cancellationToken);

        if (forecast != null)
        {
            await _cacheService.SetAsync(cacheKey, forecast, TimeSpan.FromMinutes(30), cancellationToken);
            _logger.LogInformation("Данные о погоде закэшированы для координат: {Latitude}, {Longitude}, дней: {Days}",
                latitude, longitude, days);
        }
        else
        {
            _logger.LogWarning(
                "Не удалось получить данные о погоде из API для координат: {Latitude}, {Longitude}, дней: {Days}",
                latitude, longitude, days);
        }

        return forecast;
    }
}