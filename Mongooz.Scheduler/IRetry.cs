using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mongooz.Scheduler
{
    /// <summary>
    /// Provides retry functionality for an operation
    /// </summary>
    public interface IRetry : IExecute
    {
        /// <summary>
        /// Apply a delay between retry operations
        /// </summary>
        /// <param name="initialDelay">The initial delay to be applied</param>
        /// <returns>A delayable retrable executable</returns>
        IDelay WithDelay(int initialDelay);

        /// <summary>
        /// Apply a limit for maximum number of retries upon failure
        /// </summary>
        /// <param name="maximumRetries">The maximum number of retries on failure</param>
        /// <returns>A retrable executable</returns>
        IRetry WithMaximumRetries(int maximumRetries);
    }
}
