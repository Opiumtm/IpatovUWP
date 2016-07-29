using System;
using System.Threading.Tasks;

namespace Ipatov.DataBindings
{
    /// <summary>
    /// Task source callback for data promise.
    /// </summary>
    /// <typeparam name="T">Type of value.</typeparam>
    public sealed class DataPromiseTaskCallback<T> : IDataPromiseCallback<T>
    {
        private readonly TaskCompletionSource<T> _tcs = new TaskCompletionSource<T>();

        /// <summary>
        /// Value is available.
        /// </summary>
        /// <param name="value">Value.</param>
        public void ValueAvailable(T value)
        {
            _tcs.TrySetResult(value);
        }

        /// <summary>
        /// Promise error.
        /// </summary>
        /// <param name="error">Error.</param>
        public void PromiseError(Exception error)
        {
            if (error is TaskCanceledException)
            {
                _tcs.TrySetCanceled();
            }
            else
            {
                _tcs.TrySetException(error);
            }
        }

        /// <summary>
        /// Task.
        /// </summary>
        public Task<T> Task => _tcs.Task;
    }
}