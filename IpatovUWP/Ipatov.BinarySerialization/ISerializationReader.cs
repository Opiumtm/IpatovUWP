using System;

namespace Ipatov.BinarySerialization
{
    /// <summary>
    /// Serialized data reader.
    /// </summary>
    public interface ISerializationReader : IDisposable
    {
        /// <summary>
        /// Read serialized token.
        /// </summary>
        /// <param name="context">Serialization context.</param>
        /// <returns>Token.</returns>
        SerializationToken ReadToken(SerializationContext context);
    }
}