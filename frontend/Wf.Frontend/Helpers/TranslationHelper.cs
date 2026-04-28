namespace Wf.Frontend.Helpers;

public static class TranslationHelper
{
    private static readonly Dictionary<string, string> ConditionMapping = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        {"Sunny", "Солнечно"},
        {"Clear", "Ясно"},
        {"Partly cloudy", "Переменная облачность"},
        {"Cloudy", "Облачно"},
        {"Overcast", "Пасмурно"},
        {"Mist", "Туман"},
        {"Fog", "Туман"},
        {"Freezing fog", "Ледяной туман"},
        {"Patchy rain possible", "Возможны кратковременные дожди"},
        {"Light rain", "Небольшой дождь"},
        {"Moderate rain", "Умеренный дождь"},
        {"Heavy rain", "Сильный дождь"},
        {"Light drizzle", "Морось"},
        {"Freezing drizzle", "Ледяная морось"},
        {"Patchy snow possible", "Возможны кратковременные снегопады"},
        {"Light snow", "Небольшой снег"},
        {"Moderate snow", "Умеренный снег"},
        {"Heavy snow", "Сильный снег"},
        {"Blizzard", "Метель"},
        {"Thunderstorm", "Гроза"},
        {"Thunderstorm with rain", "Гроза с дождём"},
        {"Thunderstorm with heavy rain", "Гроза с сильным дождём"},
        {"Sleet", "Мокрый снег"},
        {"Light sleet", "Небольшой мокрый снег"},
        {"Heavy sleet", "Сильный мокрый снег"},
        {"Hail", "Град"},
        {"Light hail", "Небольшой град"},
        {"Heavy hail", "Сильный град"},
        {"Windy", "Ветрено"},
        {"Calm", "Штиль"},
        {"Breezy", "Лёгкий ветер"},
        {"Hot", "Жарко"},
        {"Cold", "Холодно"},
        {"Patchy rain nearby", "Местами возможны дожди"},
        {"Light rain shower", "Легкий дождь"},
        {"Moderate or heavy snow showers","Умеренные или сильные снегопады"}
    };

    /// <summary>
    /// текст погодных условий на русский.
    /// </summary>
    public static string TranslateCondition(string? condition)
    {
        if (string.IsNullOrEmpty(condition))
            return string.Empty;

        if (ConditionMapping.TryGetValue(condition, out var translated))
            return translated;
        
        return condition;
    }

    /// <summary>
    /// единицы измерения в русский
    /// </summary>
    public static string LocalizeUnit(string unit)
    {
        return unit switch
        {
            "km/h" => "км/ч",
            "mb" => "мб",
            "mm" => "мм",
            "km" => "км",
            _ => unit
        };
    }
}