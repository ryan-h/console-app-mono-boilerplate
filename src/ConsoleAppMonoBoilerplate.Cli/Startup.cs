using System;
using System.IO;
using ConsoleAppMonoBoilerplate.Cli.Commands.Services;
using ConsoleAppMonoBoilerplate.Cli.Common.Interfaces;
using ConsoleAppMonoBoilerplate.Cli.Common.Models.Options;
using ConsoleAppMonoBoilerplate.Cli.Common.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleAppMonoBoilerplate.Cli
{
    public class Startup
    {
        #region Properties

        /// <summary>
        ///     The configuration for the app settings.
        /// </summary>
        public IConfigurationRoot Configuration { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        public Startup()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", true);

            Configuration = builder.Build();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Configures the application services.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            configureOptions(services);

            configureCommands(services);

            services.AddSingleton<IWeatherClient, WeatherClient>();
            
            // register additional application services here
        }

        /// <summary>
        ///     Configures the application configuration options.
        /// </summary>
        /// <param name="services"></param>
        private void configureOptions(IServiceCollection services)
        {
            services.AddOptions();

            services.Configure<WeatherOptions>(Configuration.GetSection(nameof(WeatherOptions)));
            
            // register additional configuration options here
        }

        /// <summary>
        ///     Configure the various command builder services.
        /// </summary>
        /// <param name="services"></param>
        private static void configureCommands(IServiceCollection services)
        {
            services.AddTransient<ICommandBuilder, WeatherCommandBuilder>();

            // register additional command builder services here
        }

        #endregion
    }
}
