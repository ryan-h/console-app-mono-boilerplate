using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Mono.Options;

namespace ConsoleAppMonoBoilerplate.Cli.Commands
{
    /// <summary>
    ///     Extends the command functionality to support asynchronous operations.
    /// </summary>
    public class AsyncCommand : Command
    {
        #region Properties

        /// <summary>
        ///     The delegate used for running the command.
        /// </summary>
        public new Func<IEnumerable<string>, Task> Run { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AsyncCommand"/> class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="help"></param>
        public AsyncCommand(string name, string help = null) : base(name, help) { }

        #endregion

        #region Methods

        /// <summary>
        ///     Invokes the run delegate passing in the arguments.
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public override int Invoke(IEnumerable<string> arguments)
        {
            var rest = Options?.Parse(arguments) ?? arguments;

            try
            {
                Run?.Invoke(rest).Wait();
            }
            catch (AggregateException ae)
            {
                // re-throw the original exception preserving the state when it was captured
                ExceptionDispatchInfo.Capture(ae.InnerException).Throw();
            }

            return 0;
        }

        #endregion
    }
}
