using FluentValidation;

namespace Wf.Application.WeatherForecast.Queries.GetWeatherForecast;

public class GetWeatherForecastQueryValidator : AbstractValidator<GetWeatherForecastQuery>
{
    public GetWeatherForecastQueryValidator()
    {
        RuleFor(query => query.Latitude).GreaterThan(0);
        RuleFor(query => query.Longitude).GreaterThan(0);
        RuleFor(query => query.Days).GreaterThan(0);
    }
}