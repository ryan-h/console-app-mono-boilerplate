using System;
using ConsoleAppMonoBoilerplate.Cli.Common.Constants;

namespace ConsoleAppMonoBoilerplate.Cli.Common.Extensions
{
    /// <summary>
    ///     Provides extension methods for the <see cref="Exception"/> class.
    /// </summary>
    public static class ExceptionExtensions
    {
        #region Methods

        /// <summary>
        ///     Determines if the exception method as been flagged to display as an error.
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static bool DisplayAsError(this Exception ex)
        {
            if (ex == null)
            {
                throw new ArgumentNullException(nameof(ex));
            }

            return ex.Data.Contains(ExceptionDataKey.DisplayExceptionAsError);
        }

        #endregion
    }
}
