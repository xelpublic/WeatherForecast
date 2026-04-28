using AutoMapper;
using Wf.Application.Common.Mappings;
using Wf.Application.Mappings;
using Wf.Domain;

namespace Wf.Application.WeatherForecast.Queries.GetWeatherForecast;

public class WeatherForecastVm : IMapWith<Domain.WeatherForecast>
{
    public Current Current { get; set; }
    public ForecastDayVm[] ForecastDays { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.WeatherForecast, WeatherForecastVm>()
            .ForMember(dest => dest.Current,
                opt => opt.MapFrom(src => src.Current))
            .ForMember(dest => dest.ForecastDays,
                opt => opt.MapFrom(src => src.Forecast.ForecastDays)); 
    }
}

public class ForecastDayVm : IMapWith<Domain.ForecastDay>
{
    public DateTime Date { get; set; }
    
    public long DateEpoch { get; set; }
    public ForecastDaySummary DaySummary { get; set; }
    public ForecastHour[] Hours { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<ForecastDay, ForecastDayVm>()
            .ForMember(forecastVm => forecastVm.Date,
                opt => opt.MapFrom(forecast => forecast.Date))
            .ForMember(forecastVm => forecastVm.DateEpoch,
                opt => opt.MapFrom(forecast => forecast.DateEpoch))
            .ForMember(forecastVm => forecastVm.DaySummary,
                opt => opt.MapFrom(forecast => forecast.Day))
            .ForMember(forecastVm => forecastVm.Hours,
                opt => opt.MapFrom(forecast => forecast.Hours));
    }
}