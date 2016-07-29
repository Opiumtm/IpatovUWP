using System;

namespace Ipatov.DataBindings
{
    /// <summary>
    /// Anonymous converter from delegate.
    /// </summary>
    /// <typeparam name="TIn">Input data type.</typeparam>
    /// <typeparam name="TOut">Output data type.</typeparam>
    public sealed class AnonymousConverter<TIn, TOut> : IDataBindingConverter<TIn, TOut>
    {
        private readonly Func<TIn, TOut> _converter;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="converter">Converter func.</param>
        /// <param name="fallbackValue">Fallback value.</param>
        public AnonymousConverter(Func<TIn, TOut> converter, TOut fallbackValue = default(TOut))
        {
            if (converter == null) throw new ArgumentNullException(nameof(converter));
            _converter = converter;
            FallbackValue = fallbackValue;
        }

        /// <summary>
        /// Convert value.
        /// </summary>
        /// <param name="value">Input value.</param>
        /// <returns>Output value.</returns>
        public TOut Convert(TIn value)
        {
            return _converter(value);
        }

        /// <summary>
        /// Fallback value.
        /// </summary>
        public TOut FallbackValue { get; }
    }
}