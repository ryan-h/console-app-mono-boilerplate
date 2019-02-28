using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleAppMonoBoilerplate.Cli.Common.Constants;
using ConsoleAppMonoBoilerplate.Cli.Common.Exceptions;
using ConsoleAppMonoBoilerplate.Cli.Common.Interfaces;
using ConsoleAppMonoBoilerplate.Cli.Common.Utilities;
using Mono.Options;

namespace ConsoleAppMonoBoilerplate.Cli.Commands.Services
{
    /// <summary>
    ///     An example service that builds a weather command.
    /// </summary>
    public class WeatherCommandBuilder : ICommandBuilder
    {
        #region Fields

        /// <summary>
        ///     The client for consuming a weather API.
        /// </summary>
        private readonly IWeatherClient _client;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="WeatherCommandBuilder"/> class.
        /// </summary>
        /// <param name="weatherClient"></param>
        public WeatherCommandBuilder(IWeatherClient weatherClient)
        {
            _client = weatherClient ?? throw new ArgumentNullException(nameof(weatherClient));
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public Command BuildCommand()
        {
            const string commandName = "weather";
      
            var showCelsius = false;
            var daysOut = 0;

            return new AsyncCommand(commandName, "Display the current weather for a specific location.")
            {
                Options = new OptionSet
                {
                    $"Usage: {ProgramInfo.AssemblyName} {commandName} <location> [options]",
                    "",
                    "location:",
                    "  A valid zip code within the United States.",
                    "",
                    "Options:",
                    {
                        "d|days-out=", "The number of days to include in the forecast.", opt =>
                        {
                            if (string.IsNullOrWhiteSpace(opt))
                            {
                                throw new CommandException(commandName, "A value is required for the [days-out] option.");
                            }

                            if (!int.TryParse(opt, out daysOut))
                            {
                                throw new CommandException(commandName, "The value for the [days-out] option must be numeric.");
                            }
                        }
                    },
                    {
                        "c|show-celsius", "Show the temperatures in Celsius instead of Fahrenheit.", opt =>
                        {
                            showCelsius = opt != null;
                        }
                    }
                },
                Run = async args =>
                {
                    var argsList = args as List<string> ?? args.ToList();

                    if (argsList.Count == 0)
                    {
                        throw new CommandException(commandName, "Missing the required <location> argument.");
                    }

                    // any options that are required for the command could also be checked here for a value

                    if (!int.TryParse(argsList[0], out var zipCode))
                    {
                        throw new CommandException(commandName, "The <location> argument must be numeric.");
                    }

                    // get results from the injected service
                    var results = await _client.GetWeather(zipCode, daysOut, showCelsius);

                    // output the results to the console
                    Reporter.Output.WriteLine($"Weather for {zipCode}:");
                    results.ForEach(result =>
                    {
                        Reporter.Output.WriteLine();
                        Reporter.Output.WriteLine(result.Date.ToShortDateString());
                        Reporter.Output.WriteLine(result.Forecast);
                        Reporter.Output.WriteLine(
                            $"With a Temperature of {result.Temperature} and a wind speed of {result.WindSpeed} from the {result.WindDirection}."
                        );
                    });
                }
            };
        }

        #endregion
    }
}
