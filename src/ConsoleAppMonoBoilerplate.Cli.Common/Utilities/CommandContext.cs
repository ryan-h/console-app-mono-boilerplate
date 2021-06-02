using System;
using ConsoleAppMonoBoilerplate.Cli.Common.Constants;

namespace ConsoleAppMonoBoilerplate.Cli.Common.Utilities
{
    /// <summary>
    ///     Provides access to the global command option values.
    /// </summary>
    public static class CommandContext
    {
        #region Fields

        /// <summary>
        ///     Gets the verbose flag from the environment variable.
        /// </summary>
        private static Lazy<bool> _verbose = new (() =>
            bool.Parse(Environment.GetEnvironmentVariable(EnvironmentVariable.Verbose) ?? bool.FalseString));

        #endregion

        #region Properties

        /// <summary>
        ///     Determines if verbosity is set for the current command.
        /// </summary>
        public static bool IsVerbose
        {
            get => _verbose.Value;
            set => _verbose = new Lazy<bool>(() => value);
        }

        #endregion
    }
}
