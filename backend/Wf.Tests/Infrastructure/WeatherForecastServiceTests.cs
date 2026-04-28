using Microsoft.Extensions.Logging;
using Moq;
using Wf.Application.Interfaces;
using Wf.Infrastructure.WeatherApiCom;
using Wf.Infrastructure.WeatherForecast;
using Wf.Infrastructure.WeatherPersistanceCache;
using Xunit;

namespace Wf.Tests;

public class WeatherForecastServiceTests
{
    private readonly Mock<IWeatherApiComService> _weatherApiComServiceMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly Mock<ILogger<WeatherForecastService>> _loggerMock;
    private readonly WeatherForecastService _service;

    public WeatherForecastServiceTests()
    {
        _weatherApiComServiceMock = new Mock<IWeatherApiComService>();
        _cacheServiceMock = new Mock<ICacheService>();
        _loggerMock = new Mock<ILogger<WeatherForecastService>>();

        _service = new WeatherForecastService(
            _weatherApiComServiceMock.Object,
            _cacheServiceMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task GetWeatherAsync_CacheHit_ReturnsCachedForecast()
    {
        // Arrange
        var latitude = 55.7558;
        var longitude = 37.6173;
        var days = 3;
        var cancellationToken = CancellationToken.None;
        var cacheKey = $"weather_{latitude:F4}_{longitude:F4}_{days}";

        var cachedForecast = new Domain.WeatherForecast
        {
            Location = new Domain.Location { Name = "Moscow" },
            Current = new Domain.Current { TempC = 15.0 }
        };

        _cacheServiceMock
            .Setup(c => c.GetAsync<Domain.WeatherForecast>(cacheKey, cancellationToken))
            .ReturnsAsync(cachedForecast);

        // Act
        var result = await _service.GetWeatherAsync(latitude, longitude, days, cancellationToken);

        // Assert
        Assert.Same(cachedForecast, result);
        _cacheServiceMock.Verify(c => c.GetAsync<Domain.WeatherForecast>(cacheKey, cancellationToken), Times.Once);
        _weatherApiComServiceMock.VerifyNoOtherCalls();
        _cacheServiceMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<TimeSpan?>(), It.IsAny<CancellationToken>()), Times.Never);
        
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Данные о погоде получены из кэша")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task GetWeatherAsync_CacheMiss_ApiSuccess_CachesAndReturnsForecast()
    {
        // Arrange
        var latitude = 55.7558;
        var longitude = 37.6173;
        var days = 3;
        var cancellationToken = CancellationToken.None;
        var cacheKey = $"weather_{latitude:F4}_{longitude:F4}_{days}";

        var apiForecast = new Domain.WeatherForecast
        {
            Location = new Domain.Location { Name = "Moscow" },
            Current = new Domain.Current { TempC = 20.0 }
        };

        _cacheServiceMock
            .Setup(c => c.GetAsync<Domain.WeatherForecast>(cacheKey, cancellationToken))
            .ReturnsAsync((Domain.WeatherForecast?)null);

        _weatherApiComServiceMock
            .Setup(a => a.GetWeatherAsync(latitude, longitude, days, cancellationToken))
            .ReturnsAsync(apiForecast);

        // Act
        var result = await _service.GetWeatherAsync(latitude, longitude, days, cancellationToken);

        // Assert
        Assert.Same(apiForecast, result);
        _cacheServiceMock.Verify(c => c.GetAsync<Domain.WeatherForecast>(cacheKey, cancellationToken), Times.Once);
        _weatherApiComServiceMock.Verify(a => a.GetWeatherAsync(latitude, longitude, days, cancellationToken), Times.Once);
        _cacheServiceMock.Verify(c => c.SetAsync(cacheKey, apiForecast, TimeSpan.FromMinutes(30), cancellationToken), Times.Once);
        
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Debug,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Кэш-промах") && v.ToString().Contains("Запрос к API")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
        
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Данные о погоде закэшированы")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task GetWeatherAsync_CacheMiss_ApiReturnsNull_LogsWarningAndReturnsNull()
    {
        // Arrange
        var latitude = 55.7558;
        var longitude = 37.6173;
        var days = 3;
        var cancellationToken = CancellationToken.None;
        var cacheKey = $"weather_{latitude:F4}_{longitude:F4}_{days}";

        _cacheServiceMock
            .Setup(c => c.GetAsync<Domain.WeatherForecast>(cacheKey, cancellationToken))
            .ReturnsAsync((Domain.WeatherForecast?)null);

        _weatherApiComServiceMock
            .Setup(a => a.GetWeatherAsync(latitude, longitude, days, cancellationToken))
            .ReturnsAsync((Domain.WeatherForecast?)null);

        // Act
        var result = await _service.GetWeatherAsync(latitude, longitude, days, cancellationToken);

        // Assert
        Assert.Null(result);
        _cacheServiceMock.Verify(c => c.GetAsync<Domain.WeatherForecast>(cacheKey, cancellationToken), Times.Once);
        _weatherApiComServiceMock.Verify(a => a.GetWeatherAsync(latitude, longitude, days, cancellationToken), Times.Once);
        _cacheServiceMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<TimeSpan?>(), It.IsAny<CancellationToken>()), Times.Never);
        
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Не удалось получить данные о погоде из API")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task GetWeatherAsync_CacheKeyFormat_Correct()
    {
        // Arrange
        var latitude = 12.3456;
        var longitude = 98.7654;
        var days = 7;
        var cancellationToken = CancellationToken.None;
        var expectedCacheKey = $"weather_{latitude:F4}_{longitude:F4}_{days}";

        var cachedForecast = new Domain.WeatherForecast();
        _cacheServiceMock
            .Setup(c => c.GetAsync<Domain.WeatherForecast>(expectedCacheKey, cancellationToken))
            .ReturnsAsync(cachedForecast);

        // Act
        var result = await _service.GetWeatherAsync(latitude, longitude, days, cancellationToken);

        // Assert
        Assert.Same(cachedForecast, result);
        _cacheServiceMock.Verify(c => c.GetAsync<Domain.WeatherForecast>(expectedCacheKey, cancellationToken), Times.Once);
    }
}