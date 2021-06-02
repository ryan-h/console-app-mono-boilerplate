using System;
using System.IO;
using ConsoleAppMonoBoilerplate.Cli.Common.Interfaces;
using Mono.Options;

namespace ConsoleAppMonoBoilerplate.Cli.Common.Utilities
{
    /// <inheritdoc />
    public class Reporter : IReporter
    {
        #region Fields

        /// <summary>
        ///     A null reporter used when output should not be written.
        /// </summary>
        private static readonly Reporter _nullReporter = new (null);

        /// <summary>
        ///     The console writer used by the reporter.
        /// </summary>
        private readonly TextWriter _console;

        #endregion

        #region Properties

        /// <summary>
        ///     Writes messages to the output stream.
        /// </summary>
        public static IReporter Output { get; }

        /// <summary>
        ///     Writes messages to the Error stream.
        /// </summary>
        public static IReporter Error { get; }

        /// <summary>
        ///     Writes messages to the verbose stream. 
        ///     If verbosity is not enabled the message will not be written.
        /// </summary>
        public static IReporter Verbose { get; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes the static properties.
        /// </summary>
        static Reporter()
        {
            Output = new Reporter(Console.Out);
            Error = new Reporter(Console.Error);
            Verbose = CommandContext.IsVerbose
                ? new Reporter(Console.Out)
                : _nullReporter;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Reporter"/> class.
        /// </summary>
        /// <param name="console"></param>
        private Reporter(TextWriter console)
        {
            _console = console;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public void WriteLine(string message)
        {
            _console?.WriteLine(message);
        }

        /// <inheritdoc />
        public void WriteLine()
        {
            _console?.WriteLine();
        }

        /// <inheritdoc />
        public void Write(string message)
        {
            _console?.Write(message);
        }

        /// <inheritdoc />
        public void WriteOptionDescriptions(OptionSet options)
        {
            if (_console != null)
            {
                options?.WriteOptionDescriptions(_console);
            }
        }

        #endregion
    }
}
