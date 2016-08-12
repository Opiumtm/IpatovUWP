using System;

namespace Ipatov.DataBindings
{
    /// <summary>
    /// Async value setter.
    /// </summary>
    /// <typeparam name="T">Value type.</typeparam>
    public interface IDataBindingValueSetter<in T>
    {
        /// <summary>
        /// Set value.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="onSuccess">Action on value setter success.</param>
        /// <param name="onError">Action on error.</param>
        void SetValue(T value, Action onSuccess, Action<Exception> onError);
    }
}