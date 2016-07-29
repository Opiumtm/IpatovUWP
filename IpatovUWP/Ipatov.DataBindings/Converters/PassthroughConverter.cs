using System;

namespace Ipatov.DataBindings
{
    /// <summary>
    /// Converter which pass through values.
    /// </summary>
    /// <typeparam name="T">Data type.</typeparam>
    public sealed class PassthroughConverter<T> : IDataBindingConverter<T, T>
    {
        // ReSharper disable once InconsistentNaming
        private static readonly Lazy<PassthroughConverter<T>> _instance = new Lazy<PassthroughConverter<T>>(() => new PassthroughConverter<T>());

        /// <summary>
        /// Defaul converter instance.
        /// </summary>
        public static PassthroughConverter<T> Instance => _instance.Value;

        /// <summary>
        /// Convert value.
        /// </summary>
        /// <param name="value">Input value.</param>
        /// <returns>Output value.</returns>
        public T Convert(T value)
        {
            return value;
        }

        /// <summary>
        /// Fallback value.
        /// </summary>
        public T FallbackValue => default(T);
    }
}