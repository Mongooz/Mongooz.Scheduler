using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mongooz.Scheduler
{
    /// <summary>
    /// Executes an operation, and optionally returns a value
    /// </summary>
    public interface IExecute
    {
        /// <summary>
        /// Execute the specified action
        /// </summary>
        /// <param name="operation">The Action to be executed</param>
        void Execute(Action operation);

        /// <summary>
        /// Execute the specified action with an argument
        /// </summary>
        /// <param name="operation">The Action to be executed</param>
        /// <param name="argument">The argument to be passed to the function</param>
        void Execute<T>(Action<T> operation, T argument);

        /// <summary>
        /// Execute the specified function
        /// </summary>
        /// <param name="operation">The Function to be executed</param>
        T Execute<T>(Func<T> operation);

        /// <summary>
        /// Execute the specified function with an argument
        /// </summary>
        /// <param name="operation">The Function to be executed</param>
        /// <param name="argument">The argument to be passed to the function</param>
        TResponse Execute<T, TResponse>(Func<T, TResponse> operation, T argument);
    }
}
