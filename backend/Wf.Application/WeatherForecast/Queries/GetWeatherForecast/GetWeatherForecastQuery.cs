using MediatR;

namespace Wf.Application.WeatherForecast.Queries.GetWeatherForecast;

public class GetWeatherForecastQuery : IRequest<WeatherForecastVm>
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int Days { get; set; }
}