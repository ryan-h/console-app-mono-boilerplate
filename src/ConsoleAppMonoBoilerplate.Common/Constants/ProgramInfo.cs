using System.Reflection;

namespace ConsoleAppMonoBoilerplate.Common.Constants;

/// <summary>
///     Stores the constants for common program information.
/// </summary>
public static class ProgramInfo
{
    /// <summary>
    ///     The name of the executable used for the application.
    /// </summary>
    public static readonly string AssemblyName = Assembly.GetEntryAssembly()?.GetName().Name?.ToLower() ?? "myconsoleapp";
}