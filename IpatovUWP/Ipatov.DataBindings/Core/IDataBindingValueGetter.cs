using System;

namespace Ipatov.DataBindings
{
    /// <summary>
    /// Async value getter.
    /// </summary>
    /// <typeparam name="T">Value type.</typeparam>
    public interface IDataBindingValueGetter<out T>
    {
        /// <summary>
        /// Get value.
        /// </summary>
        /// <param name="onValue">Action on value available.</param>
        /// <param name="onError">Action on value error.</param>
        void GetValue(Action<T> onValue, Action<Exception> onError);
    }
}