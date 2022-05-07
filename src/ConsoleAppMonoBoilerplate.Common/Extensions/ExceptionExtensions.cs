using ConsoleAppMonoBoilerplate.Common.Constants;

namespace ConsoleAppMonoBoilerplate.Common.Extensions;

/// <summary>
///     Provides extension methods for the <see cref="Exception"/> class.
/// </summary>
public static class ExceptionExtensions
{
    /// <summary>
    ///     Determines if the exception method as been flagged to display as an error.
    /// </summary>
    /// <param name="ex"></param>
    /// <returns></returns>
    public static bool DisplayAsError(this Exception? ex)
    {
        ArgumentNullException.ThrowIfNull(ex);

        return ex.Data.Contains(ExceptionDataKey.DisplayExceptionAsError);
    }
}