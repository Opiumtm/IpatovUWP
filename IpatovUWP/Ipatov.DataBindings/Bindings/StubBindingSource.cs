using System;

namespace Ipatov.DataBindings
{
    /// <summary>
    /// Source which always return same value.
    /// </summary>
    /// <typeparam name="T">Type of binded value.</typeparam>
    public sealed class StubBindingSource<T> : IDataBindingSource<T>
    {
        private readonly T _value;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="value">Value.</param>
        public StubBindingSource(T value)
        {
            _value = value;
        }

        /// <summary>
        /// Выполняет определяемые приложением задачи, связанные с удалением, высвобождением или сбросом неуправляемых ресурсов.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
        }

        /// <summary>
        /// Get binded value.
        /// </summary>
        /// <returns>Value promise. Null if value isn't available and binding should halt.</returns>
        public IDataPromise<T> GetValue()
        {
            return new ImmediateValue<T>(_value);
        }

        /// <summary>
        /// Binded value changed.
        /// </summary>
        public event EventHandler ValueChanged;
    }
}