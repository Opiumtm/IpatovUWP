using System;
using System.Collections.Generic;

namespace Ipatov.BinarySerialization
{
    /// <summary>
    /// Serialization tokens provider.
    /// </summary>
    public interface ISerializationTokensProvider
    {
        /// <summary>
        /// Get tokens for object properties.
        /// </summary>
        /// <param name="context">Serialization context.</param>
        /// <returns>Tokens.</returns>
        IEnumerable<SerializationProperty> GetProperties(SerializationContext context);

        /// <summary>
        /// Fill property.
        /// </summary>
        /// <param name="property">Serializaiton token.</param>
        /// <param name="context">Context.</param>
        void FillProperty(ref SerializationProperty property, SerializationContext context);
    }
}