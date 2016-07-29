using System;

namespace Ipatov.DataBindings
{
    /// <summary>
    /// Data promise callback.
    /// </summary>
    /// <typeparam name="T">Type of value.</typeparam>
    public interface IDataPromiseCallback<in T>
    {
        /// <summary>
        /// Value is available.
        /// </summary>
        /// <param name="value">Value.</param>
        void ValueAvailable(T value);

        /// <summary>
        /// Promise error.
        /// </summary>
        /// <param name="error">Error.</param>
        void PromiseError(Exception error);
    }
}