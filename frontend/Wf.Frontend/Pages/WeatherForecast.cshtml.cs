using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Wf.Frontend.Models;

namespace Wf.Frontend.Pages;

[Authorize]
public class WeatherForecastModel : PageModel
{
    private readonly ILogger<WeatherForecastModel> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public WeatherForecastVm? WeatherForecast { get; set; }
    public bool IsLoading { get; set; }
    public string? ErrorMessage { get; set; }
    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

    public WeatherForecastModel(ILogger<WeatherForecastModel> logger,
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task OnGetAsync(bool retry = false)
    {
        if (retry)
        {
            ErrorMessage = null;
        }

        await LoadWeatherData();
    }

    public async Task<IActionResult> OnPostRefreshAsync()
    {
        await LoadWeatherData();
        return Page();
    }

    private async Task LoadWeatherData()
    {
        IsLoading = true;
        ErrorMessage = null;

        try
        {
            var client = _httpClientFactory.CreateClient("WeatherApi");
            
            double latitude = _configuration.GetValue<double>("WeatherApi:DefaultLatitude", 55.7558);
            double longitude = _configuration.GetValue<double>("WeatherApi:DefaultLongitude", 37.6173);
            int days = _configuration.GetValue<int>("WeatherApi:DefaultDays", 3);
            var response = await client.GetAsync($"api/v1/Weather?latitude={latitude}&longitude={longitude}&days={days}");

            if (response.IsSuccessStatusCode)
            {
                WeatherForecast = await response.Content.ReadFromJsonAsync<WeatherForecastVm>();
            }
            else
            {
                ErrorMessage = $"Ошибка при получении данных: {response.StatusCode}";
                _logger.LogError("Не удалось получить данные о погоде: {StatusCode}", response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Произошла ошибка: {ex.Message}";
            _logger.LogError(ex, "Ошибка загрузки данных о погоде");
        }
        finally
        {
            IsLoading = false;
        }
    }
}