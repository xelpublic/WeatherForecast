using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Wf.Infrastructure.WeatherApiCom;
using Xunit;

namespace Wf.Tests;

public class WeatherApiComServiceTests
{
    private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly Mock<ILogger<WeatherApiComService>> _loggerMock;
    private readonly WeatherApiComService _service;

    public WeatherApiComServiceTests()
    {
        _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        _configurationMock = new Mock<IConfiguration>();
        _loggerMock = new Mock<ILogger<WeatherApiComService>>();
        
        _configurationMock.Setup(c => c["WEATHERAPI_KEY"]).Returns("test-api-key");

        _service = new WeatherApiComService(
            _httpClientFactoryMock.Object,
            _configurationMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task GetWeatherAsync_ValidRequest_ReturnsWeatherForecast()
    {
        // Arrange
        var latitude = 55.7558;
        var longitude = 37.6173;
        var days = 3;
        var cancellationToken = CancellationToken.None;

        var expectedForecast = new Domain.WeatherForecast
        {
            Location = new Domain.Location { Name = "Moscow" },
            Current = new Domain.Current { TempC = 20.0 },
            Forecast = new Domain.Forecast { ForecastDays = new List<Domain.ForecastDay>() }
        };

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(expectedForecast))
            });

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("https://api.weatherapi.com")
        };
        _httpClientFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

        // Act
        var result = await _service.GetWeatherAsync(latitude, longitude, days, cancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedForecast.Location.Name, result.Location.Name);
        Assert.Equal(expectedForecast.Current.TempC, result.Current.TempC);
        handlerMock.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.Method == HttpMethod.Get &&
                req.RequestUri.ToString().Contains($"{latitude.ToString(System.Globalization.CultureInfo.InvariantCulture)},{longitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}")),
            ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task GetWeatherAsync_HttpRequestException_ReturnsNullAndLogsError()
    {
        // Arrange
        var latitude = 55.7558;
        var longitude = 37.6173;
        var days = 3;
        var cancellationToken = CancellationToken.None;

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException("Network error"));

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("https://api.weatherapi.com")
        };
        _httpClientFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

        // Act
        var result = await _service.GetWeatherAsync(latitude, longitude, days, cancellationToken);

        // Assert
        Assert.Null(result);
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("HttpRequestException при запросе к WeatherAPI")),
                It.IsAny<HttpRequestException>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task GetWeatherAsync_JsonException_ReturnsNullAndLogsError()
    {
        // Arrange
        var latitude = 55.7558;
        var longitude = 37.6173;
        var days = 3;
        var cancellationToken = CancellationToken.None;

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("invalid json")
            });

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("https://api.weatherapi.com")
        };
        _httpClientFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

        // Act
        var result = await _service.GetWeatherAsync(latitude, longitude, days, cancellationToken);

        // Assert
        Assert.Null(result);
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("JsonException при десериализации ответа от WeatherAPI")),
                It.IsAny<JsonException>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task GetWeatherAsync_GenericException_ReturnsNullAndLogsError()
    {
        // Arrange
        var latitude = 55.7558;
        var longitude = 37.6173;
        var days = 3;
        var cancellationToken = CancellationToken.None;

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new InvalidOperationException("Some other error"));

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("https://api.weatherapi.com")
        };
        _httpClientFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

        // Act
        var result = await _service.GetWeatherAsync(latitude, longitude, days, cancellationToken);

        // Assert
        Assert.Null(result);
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("исключение при получении погоды от WeatherAPI")),
                It.IsAny<InvalidOperationException>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public void Constructor_WhenApiKeyNotConfigured_ThrowsInvalidOperationException()
    {
        // Arrange
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(c => c["WEATHERAPI_KEY"]).Returns((string)null);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() =>
            new WeatherApiComService(
                _httpClientFactoryMock.Object,
                configurationMock.Object,
                _loggerMock.Object));

        Assert.Equal("WEATHERAPI_KEY not configured", exception.Message);
    }
}