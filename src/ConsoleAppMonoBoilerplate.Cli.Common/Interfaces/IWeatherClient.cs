using System.Collections.Generic;
using System.Threading.Tasks;
using ConsoleAppMonoBoilerplate.Cli.Common.Models;

namespace ConsoleAppMonoBoilerplate.Cli.Common.Interfaces
{
    /// <summary>
    ///     Represents an example client service that retrieves weather data.
    /// </summary>
    public interface IWeatherClient
    {
        /// <summary>
        ///     Gets the weather for a specific zip code.
        /// </summary>
        /// <param name="zipCode"></param>
        /// <param name="daysOut"></param>
        /// <param name="showTempsInCelsius"></param>
        /// <returns>A list of weather results</returns>
        Task<List<WeatherResult>> GetWeather(int zipCode, int daysOut = 0, bool showTempsInCelsius = true);
    }
}
