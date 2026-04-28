using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wf.Application.WeatherForecast.Queries.GetWeatherForecast;

namespace Wf.WebApi.Controllers;

[ApiVersion("1.0")]
[ApiVersion("2.0")]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/[controller]")]
public class WeatherController : BaseController
{

    /// <summary>
    /// Получает прогноз погоды
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// GET /api/v1/Weather?latitude=55.751244&longitude=37.618423&days=3
    /// </remarks>
    /// <returns>Возвращает WeatherForecastVm</returns>
    /// <response code="200">Успех</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<WeatherForecastVm>> GetWeather(double latitude, double longitude, int days)
    {
        var query = new GetWeatherForecastQuery
        {
            Latitude = latitude,
            Longitude = longitude,
            Days = days
        };

        var vm = await Mediator.Send(query);
        return Ok(vm);
    }
}