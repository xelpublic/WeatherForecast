using FluentValidation.TestHelper;
using Wf.Application.WeatherForecast.Queries.GetWeatherForecast;
using Xunit;

namespace Wf.Tests;

public class GetWeatherForecastQueryValidatorTests
{
    private readonly GetWeatherForecastQueryValidator _validator;

    public GetWeatherForecastQueryValidatorTests()
    {
        _validator = new GetWeatherForecastQueryValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Latitude_Is_Zero()
    {
        // Arrange
        var query = new GetWeatherForecastQuery
        {
            Latitude = 0,
            Longitude = 37.6173,
            Days = 3
        };

        // Act & Assert
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(q => q.Latitude);
    }

    [Fact]
    public void Should_Have_Error_When_Latitude_Is_Negative()
    {
        // Arrange
        var query = new GetWeatherForecastQuery
        {
            Latitude = -10.5,
            Longitude = 37.6173,
            Days = 3
        };

        // Act & Assert
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(q => q.Latitude);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Latitude_Is_Positive()
    {
        // Arrange
        var query = new GetWeatherForecastQuery
        {
            Latitude = 55.7558,
            Longitude = 37.6173,
            Days = 3
        };

        // Act & Assert
        var result = _validator.TestValidate(query);
        result.ShouldNotHaveValidationErrorFor(q => q.Latitude);
    }

    [Fact]
    public void Should_Have_Error_When_Longitude_Is_Zero()
    {
        // Arrange
        var query = new GetWeatherForecastQuery
        {
            Latitude = 55.7558,
            Longitude = 0,
            Days = 3
        };

        // Act & Assert
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(q => q.Longitude);
    }

    [Fact]
    public void Should_Have_Error_When_Longitude_Is_Negative()
    {
        // Arrange
        var query = new GetWeatherForecastQuery
        {
            Latitude = 55.7558,
            Longitude = -180.0,
            Days = 3
        };

        // Act & Assert
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(q => q.Longitude);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Longitude_Is_Positive()
    {
        // Arrange
        var query = new GetWeatherForecastQuery
        {
            Latitude = 55.7558,
            Longitude = 37.6173,
            Days = 3
        };

        // Act & Assert
        var result = _validator.TestValidate(query);
        result.ShouldNotHaveValidationErrorFor(q => q.Longitude);
    }

    [Fact]
    public void Should_Have_Error_When_Days_Is_Zero()
    {
        // Arrange
        var query = new GetWeatherForecastQuery
        {
            Latitude = 55.7558,
            Longitude = 37.6173,
            Days = 0
        };

        // Act & Assert
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(q => q.Days);
    }

    [Fact]
    public void Should_Have_Error_When_Days_Is_Negative()
    {
        // Arrange
        var query = new GetWeatherForecastQuery
        {
            Latitude = 55.7558,
            Longitude = 37.6173,
            Days = -5
        };

        // Act & Assert
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(q => q.Days);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Days_Is_Positive()
    {
        // Arrange
        var query = new GetWeatherForecastQuery
        {
            Latitude = 55.7558,
            Longitude = 37.6173,
            Days = 7
        };

        // Act & Assert
        var result = _validator.TestValidate(query);
        result.ShouldNotHaveValidationErrorFor(q => q.Days);
    }

    [Fact]
    public void Should_Have_Multiple_Errors_When_Multiple_Fields_Invalid()
    {
        // Arrange
        var query = new GetWeatherForecastQuery
        {
            Latitude = -90,
            Longitude = -180,
            Days = 0
        };

        // Act & Assert
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(q => q.Latitude);
        result.ShouldHaveValidationErrorFor(q => q.Longitude);
        result.ShouldHaveValidationErrorFor(q => q.Days);
    }

    [Fact]
    public void Should_Not_Have_Any_Errors_When_All_Fields_Valid()
    {
        // Arrange
        var query = new GetWeatherForecastQuery
        {
            Latitude = 40.7128,
            Longitude = 74.0060,
            Days = 5
        };

        // Act & Assert
        var result = _validator.TestValidate(query);
        result.ShouldNotHaveAnyValidationErrors();
    }
}