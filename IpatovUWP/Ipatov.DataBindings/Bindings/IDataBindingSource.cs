using System;

namespace Ipatov.DataBindings
{
    /// <summary>
    /// Data binding source.
    /// </summary>
    /// <typeparam name="T">Type of binded value.</typeparam>
    public interface IDataBindingSource<out T> : IDisposable
    {
        /// <summary>
        /// Get binded value.
        /// </summary>
        /// <returns>Value promise. Null if value isn't available and binding should halt.</returns>
        IDataPromise<T> GetValue();

        /// <summary>
        /// Binded value changed.
        /// </summary>
        event EventHandler ValueChanged;
    }
}