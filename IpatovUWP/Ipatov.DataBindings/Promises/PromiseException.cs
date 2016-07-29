using System;

namespace Ipatov.DataBindings
{
    /// <summary>
    /// Promise, which always return exception.
    /// </summary>
    /// <typeparam name="T">Type of value.</typeparam>
    public sealed class PromiseException<T> : IDataPromise<T>
    {
        private readonly Exception _error;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="error">Error.</param>
        public PromiseException(Exception error)
        {
            if (error == null) throw new ArgumentNullException(nameof(error));
            _error = error;
        }

        /// <summary>
        /// Continue data promise.
        /// </summary>
        /// <param name="callback">Data promise callback.</param>
        public void Continue(IDataPromiseCallback<T> callback)
        {
            callback?.PromiseError(_error);
        }
    }
}