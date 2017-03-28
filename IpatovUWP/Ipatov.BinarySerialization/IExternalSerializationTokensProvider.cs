using System;
using System.Collections.Generic;

namespace Ipatov.BinarySerialization
{
    /// <summary>
    /// External provider for serialization tokens.
    /// </summary>
    public interface IExternalSerializationTokensProvider
    {
    }

    /// <summary>
    /// External provider for serialization tokens.
    /// </summary>
    /// <typeparam name="T">Object type.</typeparam>
    public interface IExternalSerializationTokensProvider<T> : IExternalSerializationTokensProvider
    {
        /// <summary>
        /// Get tokens for object properties.
        /// </summary>
        /// <param name="source">Source object.</param>
        /// <param name="context">Serialization context.</param>
        /// <returns>Tokens.</returns>
        IEnumerable<SerializationProperty> GetProperties(T source, SerializationContext context);

        /// <summary>
        /// Create object and fill its data from serialization tokens.
        /// </summary>
        /// <param name="properties">Serialization tokens.</param>
        /// <param name="context">Serialization context.</param>
        /// <returns>Object.</returns>
        T CreateObject(IEnumerable<SerializationProperty> properties, SerializationContext context);
    }
}