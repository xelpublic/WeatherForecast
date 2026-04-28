using System.Text.Json.Serialization;

namespace Wf.Frontend.Models;

public class WeatherForecastVm
{
    [JsonPropertyName("current")]
    public Current Current { get; set; }
    
    [JsonPropertyName("forecastDays")]
    public ForecastDayVm[] ForecastDays { get; set; }
}

public class Current
{
    [JsonPropertyName("last_updated_epoch")]
    public long? LastUpdatedEpoch { get; set; }
    
    [JsonPropertyName("last_updated")]
    public string LastUpdated { get; set; }
    
    [JsonPropertyName("temp_c")]
    public double? TempC { get; set; }
    
    [JsonPropertyName("temp_f")]
    public double? TempF { get; set; }
    
    [JsonPropertyName("is_day")]
    public int? IsDay { get; set; }
    
    [JsonPropertyName("condition")]
    public Condition Condition { get; set; }
    
    [JsonPropertyName("wind_kph")]
    public double? WindKph { get; set; }
    
    [JsonPropertyName("wind_degree")]
    public int? WindDegree { get; set; }
    
    [JsonPropertyName("wind_dir")]
    public string WindDir { get; set; }
    
    [JsonPropertyName("pressure_mb")]
    public double? PressureMb { get; set; }
    
    [JsonPropertyName("precip_mm")]
    public double? PrecipMm { get; set; }
    
    [JsonPropertyName("humidity")]
    public int? Humidity { get; set; }
    
    [JsonPropertyName("cloud")]
    public int? Cloud { get; set; }
    
    [JsonPropertyName("feelslike_c")]
    public double? FeelslikeC { get; set; }
    
    [JsonPropertyName("feelslike_f")]
    public double? FeelslikeF { get; set; }
    
    [JsonPropertyName("vis_km")]
    public double? VisKm { get; set; }
    
    [JsonPropertyName("uv")]
    public double? Uv { get; set; }
    
    [JsonPropertyName("gust_kph")]
    public double? GustKph { get; set; }
}

public class Condition
{
    [JsonPropertyName("text")]
    public string Text { get; set; }
    
    [JsonPropertyName("icon")]
    public string Icon { get; set; }
    
    [JsonPropertyName("code")]
    public int? Code { get; set; }
}

public class ForecastDayVm
{
    [JsonPropertyName("date")]
    public DateTime Date { get; set; }
    
    [JsonPropertyName("date_epoch")]
    public long DateEpoch { get; set; }
    
    [JsonPropertyName("daySummary")]
    public ForecastDaySummary DaySummary { get; set; }
    
    [JsonPropertyName("hours")]
    public ForecastHour[] Hours { get; set; }
}

public class ForecastDaySummary
{
    [JsonPropertyName("maxtemp_c")]
    public double? MaxTempC { get; set; }
    
    [JsonPropertyName("maxtemp_f")]
    public double? MaxTempF { get; set; }
    
    [JsonPropertyName("mintemp_c")]
    public double? MinTempC { get; set; }
    
    [JsonPropertyName("mintemp_f")]
    public double? MinTempF { get; set; }
    
    [JsonPropertyName("avgtemp_c")]
    public double? AvgTempC { get; set; }
    
    [JsonPropertyName("avgtemp_f")]
    public double? AvgTempF { get; set; }
    
    [JsonPropertyName("maxwind_kph")]
    public double? MaxWindKph { get; set; }
    
    [JsonPropertyName("totalprecip_mm")]
    public double? TotalPrecipMm { get; set; }
    
    [JsonPropertyName("totalsnow_cm")]
    public double? TotalSnowCm { get; set; }
    
    [JsonPropertyName("avgvis_km")]
    public double? AvgVisKm { get; set; }
    
    [JsonPropertyName("avghumidity")]
    public int? AvgHumidity { get; set; }
    
    [JsonPropertyName("daily_will_it_rain")]
    public int? DailyWillItRain { get; set; }
    
    [JsonPropertyName("daily_chance_of_rain")]
    public int? DailyChanceOfRain { get; set; }
    
    [JsonPropertyName("daily_will_it_snow")]
    public int? DailyWillItSnow { get; set; }
    
    [JsonPropertyName("daily_chance_of_snow")]
    public int? DailyChanceOfSnow { get; set; }
    
    [JsonPropertyName("condition")]
    public Condition Condition { get; set; }
    
    [JsonPropertyName("uv")]
    public double? Uv { get; set; }
}

public class ForecastHour
{
    [JsonPropertyName("time_epoch")]
    public long TimeEpoch { get; set; }
    
    [JsonPropertyName("time")]
    public string Time { get; set; }
    
    [JsonPropertyName("temp_c")]
    public double? TempC { get; set; }
    
    [JsonPropertyName("temp_f")]
    public double? TempF { get; set; }
    
    [JsonPropertyName("is_day")]
    public int? IsDay { get; set; }
    
    [JsonPropertyName("condition")]
    public Condition Condition { get; set; }
    
    [JsonPropertyName("wind_kph")]
    public double? WindKph { get; set; }
    
    [JsonPropertyName("wind_degree")]
    public int? WindDegree { get; set; }
    
    [JsonPropertyName("wind_dir")]
    public string WindDir { get; set; }
    
    [JsonPropertyName("pressure_mb")]
    public double? PressureMb { get; set; }
    
    [JsonPropertyName("precip_mm")]
    public double? PrecipMm { get; set; }
    
    [JsonPropertyName("humidity")]
    public int? Humidity { get; set; }
    
    [JsonPropertyName("cloud")]
    public int? Cloud { get; set; }
    
    [JsonPropertyName("feelslike_c")]
    public double? FeelslikeC { get; set; }
    
    [JsonPropertyName("feelslike_f")]
    public double? FeelslikeF { get; set; }
    
    [JsonPropertyName("windchill_c")]
    public double? WindchillC { get; set; }
    
    [JsonPropertyName("windchill_f")]
    public double? WindchillF { get; set; }
    
    [JsonPropertyName("heatindex_c")]
    public double? HeatindexC { get; set; }
    
    [JsonPropertyName("heatindex_f")]
    public double? HeatindexF { get; set; }
    
    [JsonPropertyName("dewpoint_c")]
    public double? DewpointC { get; set; }
    
    [JsonPropertyName("dewpoint_f")]
    public double? DewpointF { get; set; }
    
    [JsonPropertyName("will_it_rain")]
    public int? WillItRain { get; set; }
    
    [JsonPropertyName("chance_of_rain")]
    public int? ChanceOfRain { get; set; }
    
    [JsonPropertyName("will_it_snow")]
    public int? WillItSnow { get; set; }
    
    [JsonPropertyName("chance_of_snow")]
    public int? ChanceOfSnow { get; set; }
    
    [JsonPropertyName("vis_km")]
    public double? VisKm { get; set; }
    
    [JsonPropertyName("gust_kph")]
    public double? GustKph { get; set; }
    
    [JsonPropertyName("uv")]
    public double? Uv { get; set; }
}