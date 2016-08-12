using System;

namespace Ipatov.DataBindings
{
    /// <summary>
    /// Simple synchronous value wrapper.
    /// </summary>
    /// <typeparam name="T">Value type.</typeparam>
    public sealed class DataBindingValueAccessWrapper<T> : IDataBindingAccessor<T>
    {
        private readonly Func<T> _getValue;
        private readonly Action<T> _setValue;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="getValue">Get value function.</param>
        /// <param name="setValue">Set value action.</param>
        public DataBindingValueAccessWrapper(Func<T> getValue, Action<T> setValue)
        {
            if (getValue == null) throw new ArgumentNullException(nameof(getValue));
            if (setValue == null) throw new ArgumentNullException(nameof(setValue));
            this._getValue = getValue;
            this._setValue = setValue;
        }

        /// <summary>
        /// Get value.
        /// </summary>
        /// <param name="onValue">Action on value available.</param>
        /// <param name="onError">Action on value error.</param>
        public void GetValue(Action<T> onValue, Action<Exception> onError)
        {
            try
            {
                onValue?.Invoke(_getValue());
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex);
            }
        }

        /// <summary>
        /// Set value.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="onSuccess">Action on value setter success.</param>
        /// <param name="onError">Action on error.</param>
        public void SetValue(T value, Action onSuccess, Action<Exception> onError)
        {
            try
            {
                _setValue(value);
                onSuccess?.Invoke();
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex);
            }
        }
    }
}