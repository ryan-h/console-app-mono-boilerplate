using System.Text;
using ConsoleAppMonoBoilerplate.Common.Constants;

namespace ConsoleAppMonoBoilerplate.Common.Exceptions;

/// <summary>
///     Represents command errors that occur during application execution.
/// </summary>
public sealed class CommandException : Exception
{
    #region Constructors

    /// <summary>
    ///     Initialize a new instance of the <see cref="CommandException"/> class.
    /// </summary>
    /// <param name="commandName"></param>
    /// <param name="message"></param>
    public CommandException(string commandName, string message) : base(getMessage(commandName, message))
    {
        Data.Add(ExceptionDataKey.DisplayExceptionAsError, true);
    }

    /// <summary>
    ///     Initialize a new instance of the <see cref="CommandException"/> class with an inner exception.
    /// </summary>
    /// <param name="commandName"></param>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    public CommandException(string commandName, string message, Exception? innerException)
        : base(getMessage(commandName, message), innerException)
    {
        Data.Add(ExceptionDataKey.DisplayExceptionAsError, true);
    }

    #endregion

    #region Methods

    /// <summary>
    ///     Appends additional text to the message for command help usage.
    /// </summary>
    /// <param name="commandName"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    private static string getMessage(string commandName, string message)
    {
        var builder = new StringBuilder();

        builder.AppendLine(message);
        builder.AppendLine($"Use '{ProgramInfo.AssemblyName} {commandName} -h' for usage.");

        return builder.ToString();
    }

    #endregion
}