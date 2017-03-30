using System.Collections.Generic;

namespace Ipatov.BinarySerialization.TokenProviders
{
    /// <summary>
    /// Wrapper for tokens provider of sublcasses.
    /// </summary>
    /// <typeparam name="T">Parent type.</typeparam>
    /// <typeparam name="TSub">Subclass type.</typeparam>
    public sealed class SubclassExternalSerializationTokensProvider<T, TSub> : IExternalSerializationTokensProvider<T>
        where TSub : T
    {
        private readonly IExternalSerializationTokensProvider<TSub> _wrapped;

        public SubclassExternalSerializationTokensProvider(IExternalSerializationTokensProvider<TSub> wrapped)
        {
            _wrapped = wrapped;
        }

        /// <summary>
        /// Get tokens for object properties.
        /// </summary>
        /// <param name="source">Source object.</param>
        /// <param name="context">Serialization context.</param>
        /// <returns>Tokens.</returns>
        public IEnumerable<SerializationProperty> GetProperties(T source, SerializationContext context)
        {
            return _wrapped.GetProperties((TSub) source, context);
        }

        /// <summary>
        /// Create object and fill its data from serialization tokens.
        /// </summary>
        /// <param name="properties">Serialization tokens.</param>
        /// <param name="context">Serialization context.</param>
        /// <returns>Object.</returns>
        public T CreateObject<TEnum>(TEnum properties, SerializationContext context) where TEnum : IEnumerable<SerializationProperty>
        {
            return _wrapped.CreateObject(properties, context);
        }
    }
}