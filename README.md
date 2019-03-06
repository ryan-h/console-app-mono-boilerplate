# console-app-mono-boilerplate

This repo provides a boilerplate project for creating cross-platform .net core console applications using a command-line interface (CLI) parsed by Mono.Options. The motivation behind this project is to help demonstrate the use of *commands* with the Mono.Options library, while leveraging .net core dependency injection to simplify the integration of these commands into a functional application. The project provides a good starting point that allows a developer to focus on the commands and functionality of the app without spending time on the setup.

*You must have [.NET Core 2.2 SDK](https://dotnet.microsoft.com/download) or higher installed to use this project.*

## Command Structure

The CLI command structure resembles that used by the dotnet CLI and consists of the executable (or "assembly"), the command (or "verb"), and possible command arguments and/or options.

`myconsoleapp [options] [command] [arguments] [command-options]`

*Note: \[options\] can also follow the command arguments and be included with the \[command-options\].*

### Command

The command (or "verb") is simply a command that performs an action. The commands are implemented as a console application using a `myconsoleapp {command}` convention.

### Arguments

The arguments passed on the command line are the arguments for the command. For example when you execute `myconsoleapp {command} {argument}` the `{argument}` itself is passed to the `{command}`.

### Options

The options passed on the command line are the options for the command. These options can include ones specific to the command, or general options for the executable. For example, if you execute something like `myconsoleapp {command} --verbose`, the `--verbose` option will be used for the `{command}`.

## Application Structure

This section contains essential details for creating the commands and options. An example **weather** command, and other related files, are included in the project in order to help demonstrate the setup and functionality, these files can be safely removed at anytime.

### Commands

#### Create a builder

First create a new command builder that implements the `ICommandBuilder` interface. This is added to the *Services* folder in the *Commands* project. For example:

`public class NewCommandBuilder : ICommandBuilder`

Next, implement the `BuildCommand` method to setup the command and incorporate additional functionality in the service class. See the example below and the included `WeatherCommandBuilder.cs` file, also check out [Mono.Options](https://github.com/xamarin/XamarinComponents/tree/master/XPlat/Mono.Optionshttps://github.com/xamarin/XamarinComponents/tree/master/XPlat/Mono.Options) for more details.

*Note: at a minimum an OptionSet with help output must be configured.*

``` csharp
public Command BuildCommand()
{
    var option1 = false;

    return new Command("mycommand", "The command help description.")
    {
        Options = new OptionSet
        {
            "Usage: myconsoleapp mycommand <argument1> [options]",
            "",
            "argument1:",
            "  The argument help description.",
            "",
            "Options:",
            { "o|option1", "The option1 help description.", opt =>
                {
                    // example - set to a local variable to know if the option was passed
                    option1 = opt != null;
                }
            },
            { "v|value1=", "The value1 help description.", opt =>
                {
                    if (string.IsNullOrWhiteSpace(opt))
                    {   // throw exception if the value is required, incorrect, etc
                        throw new CommandException("mycommand", "A value is required for the [value1] option.");
                    }

                    _myValue1 = opt; // example - set to a private class field
                }
            }
        },
        Run = args =>
        {
            // do something with the args and options here...
        }
    };
}
```

#### Register the service

Finally, the new service will need to be added to the configuration in *Startup.cs*.

In the `configureCommands` method add the new service:

`services.AddTransient<ICommandBuilder, NewCommandBuilder>();`

#### Async Commands

The `AsyncCommand` class can be used in place of the `Command` class if the *Run* delegate for the command needs to incorporate asynchronous calls. Here is an example of using async/await in a command service:

``` csharp
public Command BuildCommand()
{
    return new AsyncCommand("mycommand", "An example using async/await.")
    {
        Options = new OptionSet
        {
            "Usage: myconsoleapp mycommand"
        },
        Run = async args =>
        {
            // ...

            await callApiResource();
        }
    };
}
```

#### Help Output

The help descriptions for the main application, and for each registered command, will be output automatically by Mono.Options when the help flag is passed:

`myconsoleapp help|--help|-h` <br/>
`myconsoleapp mycommand --help|-h`

*Note: directions for help usage are also included in the message for some exceptions.*

##### Customize the command help

The help description can be customized by providing a help option for the command. This will override the built-in help output and allow for handling this in the *CommandBuilder* service.

``` csharp
public Command BuildCommand()
{
    var showHelp = false;
         
    var options = new OptionSet
    {
        "Usage: mycommand [options]",
        {
            "help", "", h =>
            {
                showHelp = h != null;
            },
            true // hide the option since it's already shown as a global app option
        }
    };

    return new Command("mycommand", "My command help description.")
    {
        Options = options,
        Run = args =>
        {
            if (showHelp)
            {
                // optionally output the option description (the default help output)
                Reporter.Output.WriteOptionDescriptions(options);
                // output the custom help description using the Reporter
                Reporter.Output.WriteLine("My additional custom help description.");

                return;
            }

            // ... the code to execute when help is not passed
        }
    };
}
```

### Command Context

Global application options can be access from any command builder service by using the static `CommandContext` class.

These options are defined in *Program.cs* and can also be set via an environment variable. See the predefined *Verbose* option for an example on usage and for adding additional options.

### Using the Reporter

The *Reporter* allows for sending output to the console. This class has static methods that can be used to write content at any time:

*   `Reporter.Output.WriteLine("my message")`
*   `Reporter.Error.WriteLine("my error")`
*   `Reporter.Verbose.WriteLine("my verbose message")`

*Note: only when the verbose option is passed will messages sent to the Verbose stream be written.*

## Settings Configuration

The project has been setup to use the options pattern in .net core. The pattern uses models to represent groups of related settings. The project has an example `WeatherOptions` model to demonstrate the usage.

### Add a new setting

  - Add a property for the option to the *appsettings.json* file in the CLI project
  - Add a new option model to the Common project
  - Register the option in the *Startup.cs* file using the `configureOptions` method

## Packaging

There are a few options for getting a published executable for the CLI.

#### Using the dotnet CLI

Publish the solution directly using the [dotnet](https://docs.microsoft.com/en-us/dotnet/core/tools/?tabs=netcore2x) CLI.

Run the following command to create an executable for windows:

`dotnet publish /Path/To/ConsoleAppMonoBoilerplate.sln -r win-x64`

This will create a "win-x64" directory in the release bin of the CLI project.

#### Using the Cake build script

Publish the solution using [PowerShell](https://docs.microsoft.com/en-us/powershell/) to execute the Cake build script.

To set the *runtime* to windows, first you will need to set the environment variable:

`$env:runtime="win-x64"`

Next run the build script at the root:

`./build.ps1`

This will publish the "win-x64" content to the build output directory ("dist").
