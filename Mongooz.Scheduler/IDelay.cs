using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mongooz.Scheduler
{
    /// <summary>
    /// A delayable retryable executable
    /// </summary>
    public interface IDelay : IRetry
    {
        /// <summary>
        /// Apply an exponential increase to the retry delay
        /// </summary>
        /// <param name="exponent">The exponent to apply to the delay period</param>
        /// <returns>A delayable retryable executable</returns>
        IDelay WithExponentialBackoff(int exponent);

        /// <summary>
        /// Apply an exponential increase to the retry delay
        /// </summary>
        /// <param name="maximumDelay">The maxmum delay period in milliseconds</param>
        /// <returns>A delayable retryable executable</returns>
        IDelay WithMaximumDelay(int maximumDelay);

        /// <summary>
        /// Apply a randmised increase to the retry delay
        /// </summary>
        /// <param name="maxJitterMilliseconds">The maximum number of milliseconds to add to the delay period</param>
        /// <returns>A delayable retryable executable</returns>
        IDelay WithJitter(int maxJitterMilliseconds);
    }
}
