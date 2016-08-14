using System;

namespace Ipatov.DataBindings
{
    /// <summary>
    /// Fallback value provider.
    /// </summary>
    /// <typeparam name="T">Value type.</typeparam>
    public sealed class DataBindingFallbackValueAccess<T> : IDataBindingAccessor<T>
    {
        private readonly IDataBindingAccessor<T> _source;

        private readonly T _fallbackValue;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="source">Source value accessor.</param>
        /// <param name="fallbackValue">Fallback value.</param>
        public DataBindingFallbackValueAccess(IDataBindingAccessor<T> source, T fallbackValue = default(T))
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            _source = source;
            _fallbackValue = fallbackValue;
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
                _source.GetValue(onValue, ex =>
                {
                    try
                    {
                        onValue?.Invoke(_fallbackValue);
                        onError?.Invoke(ex);
                    }
                    catch
                    {
                    }
                });
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
            _source.SetValue(value, onSuccess, onError);
        }
    }
}