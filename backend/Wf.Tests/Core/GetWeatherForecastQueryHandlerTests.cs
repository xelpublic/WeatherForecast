using AutoMapper;
using FluentAssertions;
using Moq;
using Wf.Application.Common.Exceptions;
using Wf.Application.Interfaces;
using Wf.Application.WeatherForecast.Queries.GetWeatherForecast;
using Wf.Domain;
using Xunit;

namespace Wf.Tests.Application.WeatherForecast.Queries;

public class GetWeatherForecastQueryHandlerTests
{
    private readonly Mock<IWeatherForecastService> _forecastServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetWeatherForecastQueryHandler _handler;

    public GetWeatherForecastQueryHandlerTests()
    {
        _forecastServiceMock = new Mock<IWeatherForecastService>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetWeatherForecastQueryHandler(_forecastServiceMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_WhenWeatherDataExists_ReturnsMappedViewModel()
    {
        // Arrange
        var query = new GetWeatherForecastQuery { Latitude = 55.7558, Longitude = 37.6173, Days = 3 };
        var weather = new Domain.WeatherForecast();
        var expectedVm = new WeatherForecastVm();

        _forecastServiceMock
            .Setup(s => s.GetWeatherAsync(query.Latitude, query.Longitude, query.Days, It.IsAny<CancellationToken>()))
            .ReturnsAsync(weather);
        _mapperMock
            .Setup(m => m.Map<WeatherForecastVm>(weather))
            .Returns(expectedVm);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeSameAs(expectedVm);
        _forecastServiceMock.VerifyAll();
        _mapperMock.VerifyAll();
    }

    [Fact]
    public async Task Handle_WhenWeatherDataIsNull_ThrowsWeatherDataNotFoundException()
    {
        // Arrange
        var query = new GetWeatherForecastQuery { Latitude = 55.7558, Longitude = 37.6173, Days = 3 };
        _forecastServiceMock
            .Setup(s => s.GetWeatherAsync(query.Latitude, query.Longitude, query.Days, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.WeatherForecast?)null);

        // Act & Assert
        await Assert.ThrowsAsync<WeatherDataNotFoundException>(() =>
            _handler.Handle(query, CancellationToken.None));
        _forecastServiceMock.VerifyAll();
    }
}