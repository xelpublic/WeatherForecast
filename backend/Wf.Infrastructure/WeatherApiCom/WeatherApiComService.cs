using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Wf.Infrastructure.WeatherApiCom;

public class WeatherApiComService : IWeatherApiComService
{
    private readonly string _apiKey;
    private const string BaseUrl = "https://api.weatherapi.com";

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<WeatherApiComService> _logger;

    public WeatherApiComService(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<WeatherApiComService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _apiKey = configuration["WEATHERAPI_KEY"] ??
                  throw new InvalidOperationException("WEATHERAPI_KEY not configured");
        _logger = logger;
    }

    public async Task<Domain.WeatherForecast?> GetWeatherAsync(double latitude, double longitude, int days,
        CancellationToken cancellationToken)
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(BaseUrl);
            var url = $"/v1/forecast.json?key={_apiKey}&q={latitude.ToString(CultureInfo.InvariantCulture)},{longitude.ToString(CultureInfo.InvariantCulture)}&days={days}";
            var response = await httpClient.GetFromJsonAsync<Domain.WeatherForecast>(url, cancellationToken);

            return response;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HttpRequestException при запросе к WeatherAPI: {Message}", ex.Message);
            return null;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "JsonException при десериализации ответа от WeatherAPI: {Message}", ex.Message);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "исключение при получении погоды от WeatherAPI: {Message}", ex.Message);
            return null;
        }
    }
}