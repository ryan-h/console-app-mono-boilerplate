using Mono.Options;

namespace ConsoleAppMonoBoilerplate.Common.Interfaces;

/// <summary>
///     A service used for building and running a command.
/// </summary>
public interface ICommandBuilder
{
    /// <summary>
    ///     Builds the command and integrates any functionality related with execution.
    /// </summary>
    /// <returns>The build command.</returns>
    Command BuildCommand();
}