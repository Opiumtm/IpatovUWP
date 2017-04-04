using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ipatov.BinarySerialization.Reflection;

namespace Ipatov.BinarySerialization.TypeMapping
{
    /// <summary>
    /// Type mapper for specified assembly.
    /// </summary>
    public sealed class AssemblyTypesMapper : ITypeMapper
    {
        private readonly Dictionary<Type, string> _types;
        private readonly Dictionary<string, Type> _names;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="assembly">Assembly.</param>
        public AssemblyTypesMapper(Assembly assembly)
        {
            var types = assembly.DefinedTypes.Where(t => !t.AsType().IsConstructedGenericType).Select(t => new {type = t, attr = t.GetCustomAttribute<TypeIdentityAttribute>()}).Where(t => t.attr != null).ToArray();
            _types = types.ToDictionary(a => a.type.AsType(), a => a.attr.Id);
            _names = types.ToDictionary(a => a.attr.Id, a => a.type.AsType());
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
            var typeName =  _types.ContainsKey(type) ? _types[type] : null;
            if (typeName != null)
            {
                return new SerializationTypeMapping()
                {
                    Kind = "Typeid",
                    Type = typeName
                };
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
            if (typeName.Kind == "Typeid" && typeName.Type != null)
            {
                return _names.ContainsKey(typeName.Type) ? _names[typeName.Type] : null;
            }
            return null;
        }
    }
}