using System;
using System.Collections.Generic;

namespace Ipatov.BinarySerialization
{
    /// <summary>
    /// Wrapper for serialization tokens provider.
    /// </summary>
    /// <typeparam name="T">Objet type.</typeparam>
    public sealed class SerializationTokensProviderWrapper<T> : IExternalSerializationTokensProvider<T>
        where T: ISerializationTokensProvider, new()
    {
        /// <summary>
        /// Get tokens for object properties.
        /// </summary>
        /// <param name="source">Source object.</param>
        /// <param name="context">Serialization context.</param>
        /// <returns>Tokens.</returns>
        public IEnumerable<SerializationProperty> GetProperties(T source, SerializationContext context)
        {
            return source.GetProperties(context);
        }

        /// <summary>
        /// Create object and fill its data from serialization tokens.
        /// </summary>
        /// <param name="properties">Serialization tokens.</param>
        /// <param name="context">Serialization context.</param>
        /// <returns>Object.</returns>
        public T CreateObject(IEnumerable<SerializationProperty> properties, SerializationContext context)
        {
            var result = new T();
            result.FillProperties(properties, context);
            return result;
        }
    }
}