using System;

namespace Ipatov.DataBindings
{
    /// <summary>
    /// Promise of data to be retrieved.
    /// </summary>
    /// <typeparam name="T">Type of value.</typeparam>
    public interface IDataPromise<out T>
    {
        /// <summary>
        /// Continue data promise.
        /// </summary>
        /// <param name="callback">Data promise callback.</param>
        void Continue(IDataPromiseCallback<T> callback);
    }
}