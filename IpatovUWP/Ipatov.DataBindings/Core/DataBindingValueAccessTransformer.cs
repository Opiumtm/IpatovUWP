using System;

namespace Ipatov.DataBindings
{
    /// <summary>
    /// Data binding value access transformer.
    /// </summary>
    /// <typeparam name="TIn">Source value type.</typeparam>
    /// <typeparam name="TOut">Result value type.</typeparam>
    public sealed class DataBindingValueAccessTransformer<TIn, TOut> : IDataBindingAccessor<TOut>
    {
        private readonly Func<TIn, TOut> _convert;

        private readonly Func<TOut, TIn> _convertBack;

        private readonly IDataBindingAccessor<TIn> _source;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="source">Wrapped data accessor.</param>
        /// <param name="convert">Converter function.</param>
        /// <param name="convertBack">Backwards converter function.</param>
        public DataBindingValueAccessTransformer(IDataBindingAccessor<TIn> source, Func<TIn, TOut> convert, Func<TOut, TIn> convertBack)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (convert == null) throw new ArgumentNullException(nameof(convert));
            if (convertBack == null) throw new ArgumentNullException(nameof(convertBack));
            this._convert = convert;
            this._convertBack = convertBack;
            this._source = source;
        }

        /// <summary>
        /// Get value.
        /// </summary>
        /// <param name="onValue">Action on value available.</param>
        /// <param name="onError">Action on value error.</param>
        public void GetValue(Action<TOut> onValue, Action<Exception> onError)
        {
            try
            {
                _source.GetValue(v =>
                {
                    try
                    {
                        onValue?.Invoke(_convert(v));
                    }
                    catch (Exception ex)
                    {
                        onError(ex);
                    }
                }, onError);
            }
            catch (Exception ex)
            {
                onError(ex);
            }
        }

        /// <summary>
        /// Set value.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="onSuccess">Action on value setter success.</param>
        /// <param name="onError">Action on error.</param>
        public void SetValue(TOut value, Action onSuccess, Action<Exception> onError)
        {
            try
            {
                _source.SetValue(_convertBack(value), onSuccess, onError);
            }
            catch (Exception ex)
            {
                onError(ex);
            }
        }
    }
}