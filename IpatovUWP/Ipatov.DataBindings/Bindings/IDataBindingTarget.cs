using System;

namespace Ipatov.DataBindings
{
    /// <summary>
    /// Data binding target.
    /// </summary>
    /// <typeparam name="T">Type of binded value.</typeparam>
    public interface IDataBindingTarget<in T>
    {
        /// <summary>
        /// Set binded value.
        /// </summary>
        /// <param name="value">Value.</param>
        void SetValue(T value);

        /// <summary>
        /// Data binding success.
        /// </summary>
        event EventHandler Success;

        /// <summary>
        /// Data binding error.
        /// </summary>
        event EventHandler<Exception> Error;
    }
}