namespace Wf.Application.Common.Exceptions;

/// <summary>
/// Исключение, когда запрошенные погодные данные не найдены
/// </summary>
public class WeatherDataNotFoundException : Exception
{
    public WeatherDataNotFoundException(string message) : base(message) { }
    
    public WeatherDataNotFoundException(string message, Exception inner) 
        : base(message, inner) { }
}