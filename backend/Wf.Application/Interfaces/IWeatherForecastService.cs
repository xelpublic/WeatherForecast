namespace Wf.Application.Interfaces;

public interface IWeatherForecastService
{
    Task<Domain.WeatherForecast?> GetWeatherAsync(double latitude, double longitude, int days,
        CancellationToken cancellationToken);
}