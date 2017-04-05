using System;
using System.Linq;

namespace Ipatov.BinarySerialization.TypeMapping
{
    /// <summary>
    /// Composite type mapper.
    /// </summary>
    public sealed class CompositeTypeMapper : ITypeMapper
    {
        private readonly ITypeMapper[] _typeMappers;

        /// <summary>
        /// Default composite type mapper.
        /// </summary>
        public static readonly ITypeMapper DefaultTypeMapper = new CompositeTypeMapper(
            new DefaultTypeMapper(),
            new PrimitiveTypeMapper(),
            new CommonGenericsTypeMapper()
            );

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="typeMappers">Type mappers (mapper priority in reverse order, i.e. first mapper - less priority).</param>
        public CompositeTypeMapper(params ITypeMapper[] typeMappers)
        {
            if (typeMappers == null) throw new ArgumentNullException(nameof(typeMappers));
            _typeMappers = typeMappers;
        }

        /// <summary>
        /// Type mapper.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <param name="context">Serialization context.</param>
        /// <returns>Type name to serialize.</returns>
        public SerializationTypeMapping? GetTypeName(Type type, SerializationContext context)
        {
            foreach (var m in _typeMappers.Reverse())
            {
                var s = m.GetTypeName(type, context);
                if (s != null)
                {
                    return s;
                }
            }
            return null;
        }

        /// <summary>
        /// Get type from name.
        /// </summary>
        /// <param name="typeName">Type name.</param>
        /// <param name="context">Serialization context.</param>
        /// <returns>Type.</returns>
        public Type GetType(ref SerializationTypeMapping typeName, SerializationContext context)
        {
            foreach (var m in _typeMappers.Reverse())
            {
                var s = m.GetType(ref typeName, context);
                if (s != null)
                {
                    return s;
                }
            }
            return null;
        }
    }
}