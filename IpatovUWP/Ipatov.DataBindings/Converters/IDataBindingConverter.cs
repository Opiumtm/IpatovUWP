namespace Ipatov.DataBindings
{
    /// <summary>
    /// Data binding value converter.
    /// </summary>
    /// <typeparam name="TIn">Input value type.</typeparam>
    /// <typeparam name="TOut">Output value type.</typeparam>
    public interface IDataBindingConverter<in TIn, out TOut>
    {
        /// <summary>
        /// Convert value.
        /// </summary>
        /// <param name="value">Input value.</param>
        /// <returns>Output value.</returns>
        TOut Convert(TIn value);

        /// <summary>
        /// Fallback value.
        /// </summary>
        TOut FallbackValue { get; }
    }
}