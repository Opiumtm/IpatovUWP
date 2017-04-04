using System;
using System.Collections.Generic;
using Ipatov.BinarySerialization.Internals;

namespace Ipatov.BinarySerialization.TypeMapping
{
    /// <summary>
    /// Default type mapper.
    /// </summary>
    public sealed class DefaultTypeMapper : ITypeMapper
    {
        /// <summary>
        /// Instance.
        /// </summary>
        public static readonly ITypeMapper Instance = new DefaultTypeMapper();

        private static readonly LazyDictionary<string, Type> DefaultMapper = new LazyDictionary<string, Type>(MapDefault);

        private static Type MapDefault(string typeName)
        {
            return Type.GetType(typeName);
        }

        /// <summary>
        /// Type mapper.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <param name="context">Serialization context.</param>
        /// <returns>Type name to serialize.</returns>
        public SerializationTypeMapping? GetTypeName(Type type, SerializationContext context)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            return new SerializationTypeMapping()
            {
                Kind = "Typeref",
                Type = type.AssemblyQualifiedName
            };
        }

        /// <summary>
        /// Get type from name.
        /// </summary>
        /// <param name="typeName">Type name.</param>
        /// <param name="context">Serialization context.</param>
        /// <returns>Type.</returns>
        public Type GetType(ref SerializationTypeMapping typeName, SerializationContext context)
        {
            if (typeName.Kind == "Typeref" && typeName.Type != null)
            {
                return DefaultMapper[typeName.Type];
            }
            return null;
        }
    }
}