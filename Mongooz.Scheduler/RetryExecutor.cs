using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Mongooz.Scheduler
{
    public class RetryExecutor : IDelay
    {
        private Func<Delegate, object> _execution;
        private Func<Delegate, object> _retry;
        private Action<Exception> _handleError;
        private Action _delay;
        private int _currentDelay;
        private int _numberOfRetries;

        public RetryExecutor()
        {
            _delay = () => { };
            _execution = (operation) => 
            {
                try
                {
                    return operation.DynamicInvoke();
                }
                catch (Exception exception)
                {
                    if (_handleError != null)
                    {
                        _handleError(exception.InnerException);
                    }
                    return _retry(operation);
                }
            };
            _retry = (operation) =>
            {
                _delay();
                return _execution(operation);
            };
        }

        public RetryExecutor OnError(Action<Exception> errorHandler)
        {
            var previousHandle = _handleError;
            _handleError = (exception) =>
                {
                    if (previousHandle != null)
                    {
                        previousHandle(exception);
                    }
                    errorHandler(exception);
                };
            return this;
        }

        public IDelay WithDelay(int initialDelay)
        {
            _currentDelay = initialDelay;
            _delay = () =>
            {
                _currentDelay = Math.Max(0, _currentDelay);
                Thread.Sleep(_currentDelay);
            };
            return this;
        }

        public IDelay WithExponentialBackoff(int exponent)
        {
            var previousDelay = _delay;
            _delay = () =>
                {
                    previousDelay();
                    _currentDelay *= exponent; 
                };
            return this;
        }

        public IDelay WithMaximumDelay(int maximumDelay)
        {
            var previousDelay = _delay;
            _delay = () =>
                {
                    previousDelay();
                    _currentDelay = (int)Math.Min(_currentDelay, maximumDelay);
                };
            return this;
        }

        public IDelay WithJitter(int maxJitterMilliseconds)
        {
            var previousDelay = _delay;
            _delay = () =>
            {
                int randomIncrement = new Random().Next(maxJitterMilliseconds);
                _currentDelay += randomIncrement;
                previousDelay();
            };
            return this;
        }

        public IRetry WithMaximumRetries(int maximumRetries)
        {
            var previousRetry = _retry;
            _retry = (operation) => 
            {
                if (_numberOfRetries >= maximumRetries)
                {
                    return null;
                }
                _numberOfRetries++;
                return previousRetry(operation);
            };
            return this;
        }

        public void Execute(Action operation)
        {
            _execution(operation);
        }

        public void Execute<T>(Action<T> operation, T argument)
        {
            _execution(ToAction(operation, argument));
        }

        public T Execute<T>(Func<T> operation)
        {
            return (T)_execution(operation);
        }

        public TResponse Execute<T, TResponse>(Func<T, TResponse> operation, T argument)
        {
            return (TResponse)_execution(ToFunc(operation, argument));
        }

        private Action ToAction<T>(Action<T> operation, T argument)
        {
            return () => operation(argument);
        }

        private Func<TResponse> ToFunc<T, TResponse>(Func<T, TResponse> operation, T argument)
        {
            return () => { return operation(argument); };
        }
    }
}
