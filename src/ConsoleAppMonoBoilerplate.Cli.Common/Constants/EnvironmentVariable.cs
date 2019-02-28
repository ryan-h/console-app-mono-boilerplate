namespace ConsoleAppMonoBoilerplate.Cli.Common.Constants
{
    /// <summary>
    ///     Stores the constants representing an environment variable.
    /// </summary>
    public static class EnvironmentVariable
    {
        #region Prefix

        /// <summary>
        ///     Holds the prefix for the application.
        /// </summary>
        private static readonly string _prefix = $"{ProgramInfo.AssemblyName.ToUpper()}_CONTEXT_";

        #endregion

        /// <summary>
        ///     Defines if verbosity is enabled.
        /// </summary>
        public static readonly string Verbose = $"{_prefix}{nameof(Verbose).ToUpper()}";
    }
}
