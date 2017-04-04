using System;

namespace Ipatov.BinarySerialization
{
    /// <summary>
    /// Type mapper.
    /// </summary>
    public interface ITypeMapper
    {
        /// <summary>
        /// Type mapper.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <param name="context">Serialization context.</param>
        /// <returns>Type name to serialize.</returns>
        SerializationTypeMapping? GetTypeName(Type type, SerializationContext context);

        /// <summary>
        /// Get type from name.
        /// </summary>
        /// <param name="typeName">Type name.</param>
        /// <param name="context">Serialization context.</param>
        /// <returns>Type.</returns>
        Type GetType(ref SerializationTypeMapping typeName, SerializationContext context);
    }
}