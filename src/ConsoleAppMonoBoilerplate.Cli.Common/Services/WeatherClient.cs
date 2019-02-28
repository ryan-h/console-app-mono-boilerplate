using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleAppMonoBoilerplate.Cli.Common.Interfaces;
using ConsoleAppMonoBoilerplate.Cli.Common.Models;
using ConsoleAppMonoBoilerplate.Cli.Common.Models.Options;
using Microsoft.Extensions.Options;

namespace ConsoleAppMonoBoilerplate.Cli.Common.Services
{
    /// <inheritdoc />
    public class WeatherClient : IWeatherClient
    {
        #region Fields

        /// <summary>
        ///     The weather configuration options.
        /// </summary>
        private readonly WeatherOptions _weatherOptions;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="WeatherClient"/> class.
        /// </summary>
        /// <param name="weatherOptionsAccessor"></param>
        public WeatherClient(IOptions<WeatherOptions> weatherOptionsAccessor)
        {
            if (weatherOptionsAccessor == null)
            {
                throw new ArgumentNullException(nameof(weatherOptionsAccessor));
            }

            _weatherOptions = weatherOptionsAccessor.Value;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public Task<List<WeatherResult>> GetWeather(int zipCode, int daysOut = 0, bool showTempsInCelsius = true)
        {
            var count = daysOut > 0 ? daysOut : _weatherOptions.DefaultDaysOut;

            // Mock the return data that would be retrieved dynamically from an API
            return Task.FromResult(Enumerable.Range(1, count)
                .Select(day => new WeatherResult
                {
                    Date = DateTime.UtcNow.AddDays(day - 1),
                    Temperature = $"32{(showTempsInCelsius ? "C" : "F")}",
                    WindDirection = "NE",
                    WindSpeed = "4 to 6 MPH",
                    Forecast = "Partly Sunny"
                }).ToList());
        }

        #endregion
    }
}
