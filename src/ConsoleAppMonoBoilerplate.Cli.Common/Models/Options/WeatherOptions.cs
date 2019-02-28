using System;

namespace ConsoleAppMonoBoilerplate.Cli.Common.Models.Options
{
    /// <summary>
    ///     The model of the weather options from configuration settings.
    /// </summary>
    [Serializable]
    public class WeatherOptions
    {
        /// <summary>
        ///     The base url to the weather api.
        /// </summary>
        public string BaseApiUrl { get; set; }

        /// <summary>
        ///     The default amount of days out.
        /// </summary>
        public int DefaultDaysOut { get; set; }
    }
}
