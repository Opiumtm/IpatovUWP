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
        /// Fill property data.
        /// </summary>
        /// <param name="properties">Serialization tokens.</param>
        /// <param name="context">Serialization context.</param>
        void FillProperties(IEnumerable<SerializationProperty> properties, SerializationContext context);
    }
}