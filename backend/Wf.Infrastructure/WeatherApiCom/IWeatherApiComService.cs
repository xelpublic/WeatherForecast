
namespace Wf.Infrastructure.WeatherApiCom;

/// <summary>
/// получение данных прогноза от api.weatherapi.com
/// </summary>
public interface IWeatherApiComService
{
    Task<Domain.WeatherForecast?> GetWeatherAsync(double latitude, double longitude, int days,
        CancellationToken cancellationToken);
}