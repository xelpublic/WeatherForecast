using AutoMapper;
using MediatR;
using Wf.Application.Common.Exceptions;
using Wf.Application.Interfaces;

namespace Wf.Application.WeatherForecast.Queries.GetWeatherForecast;

public class GetWeatherForecastQueryHandler
    : IRequestHandler<GetWeatherForecastQuery, WeatherForecastVm>
{
    private readonly IWeatherForecastService _forecastService;
    private readonly IMapper _mapper;

    public GetWeatherForecastQueryHandler(IWeatherForecastService forecastService, IMapper mapper)
    {
        _forecastService = forecastService;
        _mapper = mapper;
    }

    public async Task<WeatherForecastVm> Handle(GetWeatherForecastQuery request,
        CancellationToken cancellationToken)
    {
        var weather =
            await _forecastService.GetWeatherAsync(request.Latitude, request.Longitude, request.Days,
                cancellationToken);

        if (weather == null)
        {
            throw new WeatherDataNotFoundException("Данные прогноза не получены");
        }

        return _mapper.Map<WeatherForecastVm>(weather);
    }
}