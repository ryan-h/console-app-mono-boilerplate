namespace ConsoleAppMonoBoilerplate.Common.Models;

/// <summary>
///     The model of the available weather data.
/// </summary>
public class WeatherResult
{
    public DateTime Date { get; set; }

    public string? Temperature { get; set; }

    public string? WindSpeed { get; set; }

    public string? WindDirection { get; set; }

    public string? Forecast { get; set; }
}