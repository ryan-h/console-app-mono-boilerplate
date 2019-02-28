using System;
using System.Collections.Generic;
using ConsoleAppMonoBoilerplate.Cli.Common.Constants;
using ConsoleAppMonoBoilerplate.Cli.Common.Extensions;
using ConsoleAppMonoBoilerplate.Cli.Common.Interfaces;
using ConsoleAppMonoBoilerplate.Cli.Common.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Mono.Options;

namespace ConsoleAppMonoBoilerplate.Cli
{
    public class Program
    {
        #region Methods

        /// <summary>
        ///     The entry point when executing the cli application.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static int Main(string[] args)
        {
            var services = new ServiceCollection();
            var startup = new Startup();

            try
            {
                // Add the services to the DI container
                startup.ConfigureServices(services);

                // Parse the args and run the command
                return processArgs(args, services.BuildServiceProvider());
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
        }

        /// <summary>
        ///     Process the arguments and run the selected command.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        private static int processArgs(IEnumerable<string> args, IServiceProvider serviceProvider)
        {
            // Define all the global program options
            var suite = new CommandSet(ProgramInfo.AssemblyName) {
                $"Usage: {ProgramInfo.AssemblyName} [options] [command] [arguments] [command-options]",
                "",
                "Options:",
                { "h|help", "Show help.", h => {} },
                { "v|verbose", "Set the command output to be verbose.", verbose =>
                    {
                        if (verbose == null)
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
            var services = serviceProvider.GetServices<ICommandBuilder>();

            // Add the command for each configured builder service
            foreach (var service in services)
            {
                suite.Add(service.BuildCommand());
            }

            // parse the args and run the command
            return suite.Run(args);
        }

        #endregion
    }
}
