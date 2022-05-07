using ConsoleAppMonoBoilerplate.Commands.Services;
using ConsoleAppMonoBoilerplate.Common.Constants;
using ConsoleAppMonoBoilerplate.Common.Extensions;
using ConsoleAppMonoBoilerplate.Common.Interfaces;
using ConsoleAppMonoBoilerplate.Common.Models.Options;
using ConsoleAppMonoBoilerplate.Common.Services;
using ConsoleAppMonoBoilerplate.Common.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mono.Options;

using var host = Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) =>
    {
        services.AddOptions();

        // Configure the configuration options
        services.Configure<WeatherOptions>(context.Configuration.GetSection(nameof(WeatherOptions)));
        
        // Configure the builder services for each command
        services.AddTransient<ICommandBuilder, WeatherCommandBuilder>();

        // Configure any additional services for the application
        services.AddSingleton<IWeatherClient, WeatherClient>();
    })
    .Build();

try
{
    // Define all the global program options
    var suite = new CommandSet(ProgramInfo.AssemblyName) {
        $"Usage: {ProgramInfo.AssemblyName} [options] [command] [arguments] [command-options]",
        "",
        "Options:",
        { "h|help", "Show help.", _ => {} },
        { "v|verbose", "Set the command output to be verbose.", verbose =>
            {
                if (verbose is null)
                {
                    return;
                }

                Environment.SetEnvironmentVariable(EnvironmentVariable.Verbose, bool.TrueString);
                CommandContext.IsVerbose = true;
            }
        },
        "",
        "Commands:"
    };

    // Get all the command builder services from the container
    var services = host.Services.GetServices<ICommandBuilder>();

    // Add the command for each configured builder service
    foreach (var service in services)
    {
        suite.Add(service.BuildCommand());
    }

    // Parse the args and run the command
    return suite.Run(args);
}
catch (OptionException oex)
{
    Reporter.Output.WriteLine(oex.OptionName);
    Reporter.Output.WriteLine(CommandContext.IsVerbose ? oex.ToString() : oex.Message);
    Reporter.Output.WriteLine($"Use '{ProgramInfo.AssemblyName} help' for usage.");

    return 0;
}
catch (Exception ex) when (ex.DisplayAsError())
{
    Reporter.Error.WriteLine(CommandContext.IsVerbose ? ex.ToString() : ex.Message);

    return 1;
}
catch (Exception ex) when (!ex.DisplayAsError())
{
    Reporter.Error.WriteLine(ex.ToString());

    return 1;
}
   