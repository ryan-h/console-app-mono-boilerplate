using Mono.Options;

namespace ConsoleAppMonoBoilerplate.Common.Interfaces;

/// <summary>
///     A reporter that is used to write output to the console.
/// </summary>
public interface IReporter
{
    /// <summary>
    ///     Writes a string followed by a line terminator to the text string or stream.
    /// </summary>
    /// <param name="message"></param>
    void WriteLine(string? message);

    /// <summary>
    ///     Writes a line terminator to the text string or stream.
    /// </summary>
    void WriteLine();

    /// <summary>
    ///     Writes a string to the text string or stream.
    /// </summary>
    /// <param name="message"></param>
    void Write(string? message);

    /// <summary>
    ///     Writes the option descriptions to the stream.
    /// </summary>
    /// <param name="options"></param>
    void WriteOptionDescriptions(OptionSet options);
}