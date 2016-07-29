using System;
using System.Globalization;
using Windows.UI.Xaml.Data;

namespace Ipatov.DataBindings
{
    /// <summary>
    /// Wrapper for IValueConverter interface.
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    public sealed class ValueConverterWrapper<TIn, TOut> : IDataBindingConverter<TIn, TOut>
    {
        private readonly IValueConverter _valueConverter;

        private readonly object _parameter;

        private readonly string _language;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="valueConverter">Value converter.</param>
        /// <param name="parameter">Converter parameter.</param>
        /// <param name="fallbackValue">Fallback value.</param>
        /// <param name="language">Converter language.</param>
        public ValueConverterWrapper(IValueConverter valueConverter, object parameter = null, TOut fallbackValue = default (TOut), string language = null)
        {
            if (valueConverter == null) throw new ArgumentNullException(nameof(valueConverter));
            _valueConverter = valueConverter;
            _parameter = parameter;
            _language = language ?? CultureInfo.CurrentCulture.Name;
            FallbackValue = fallbackValue;
        }

        /// <summary>
        /// Convert value.
        /// </summary>
        /// <param name="value">Input value.</param>
        /// <returns>Output value.</returns>
        public TOut Convert(TIn value)
        {
            return (TOut) _valueConverter.Convert(value, typeof (TOut), _parameter, _language);
        }

        /// <summary>
        /// Fallback value.
        /// </summary>
        public TOut FallbackValue { get; }
    }
}