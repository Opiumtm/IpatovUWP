namespace Ipatov.DataBindings
{
    /// <summary>
    /// Reference cast converter.
    /// </summary>
    /// <typeparam name="TIn">Input value type.</typeparam>
    /// <typeparam name="TOut">Output value type.</typeparam>
    public sealed class CastConverter<TIn, TOut> : IDataBindingConverter<TIn, TOut> where TOut : class 
    {
        /// <summary>
        /// Convert value.
        /// </summary>
        /// <param name="value">Input value.</param>
        /// <returns>Output value.</returns>
        public TOut Convert(TIn value)
        {
            return value as TOut;
        }

        /// <summary>
        /// Fallback value.
        /// </summary>
        public TOut FallbackValue => null;
    }
}