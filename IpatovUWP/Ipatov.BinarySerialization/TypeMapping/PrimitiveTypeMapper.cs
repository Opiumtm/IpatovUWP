using System;
using System.Collections.Generic;
using System.Linq;

namespace Ipatov.BinarySerialization.TypeMapping
{
    /// <summary>
    /// Primitive types mapper.
    /// </summary>
    public sealed class PrimitiveTypeMapper : ITypeMapper
    {
        static PrimitiveTypeMapper()
        {
            Types = new Dictionary<Type, string>()
            {
                { typeof(Byte), "Byte" },
                { typeof(SByte), "SByte" },
                { typeof(Int16), "Int16" },
                { typeof(UInt16), "UInt16" },
                { typeof(Int32), "Int32" },
                { typeof(UInt32), "UInt32" },
                { typeof(Int64), "Int64" },
                { typeof(UInt64), "UInt64" },
                { typeof(Single), "Single" },
                { typeof(Double), "Double" },
                { typeof(Decimal), "Decimal" },
                { typeof(Boolean), "Boolean" },
                { typeof(Char), "Char" },
                { typeof(Guid), "Guid" },
                { typeof(DateTime), "DateTime" },
                { typeof(TimeSpan), "TimeSpan" },
                { typeof(DateTimeOffset), "DateTimeOffset" },
                { typeof(Int32Index), "Int32Index" },
                { typeof(Byte?), "Byte?" },
                { typeof(SByte?), "SByte?" },
                { typeof(Int16?), "Int16?" },
                { typeof(UInt16?), "UInt16?" },
                { typeof(Int32?), "Int32?" },
                { typeof(UInt32?), "UInt32?" },
                { typeof(Int64?), "Int64?" },
                { typeof(UInt64?), "UInt64?" },
                { typeof(Single?), "Single?" },
                { typeof(Double?), "Double?" },
                { typeof(Decimal?), "Decimal?" },
                { typeof(Boolean?), "Boolean?" },
                { typeof(Char?), "Char?" },
                { typeof(Guid?), "Guid?" },
                { typeof(DateTime?), "DateTime?" },
                { typeof(TimeSpan?), "TimeSpan?" },
                { typeof(DateTimeOffset?), "DateTimeOffset?" },
                { typeof(Int32Index?), "Int32Index?" },
                { typeof(String), "String" },
                { typeof(Uri), "Uri" },
            };
            Names = Types.ToDictionary(kv => kv.Value, kv => kv.Key, StringComparer.Ordinal);
        }

        private static readonly Dictionary<Type, string> Types;
        private static readonly Dictionary<string, Type> Names;

        /// <summary>
        /// Type mapper.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <param name="context">Serialization context.</param>
        /// <returns>Type name to serialize.</returns>
        public SerializationTypeMapping? GetTypeName(Type type, SerializationContext context)
        {
            var tname = Types.ContainsKey(type) ? Types[type] : null;
            if (tname != null)
            {
                return new SerializationTypeMapping()
                {
                    Kind = "Primitive",
                    Type = tname
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
            if (typeName.Kind == "Primitive" && typeName.Type != null)
            {
                return Names.ContainsKey(typeName.Type) ? Names[typeName.Type] : null;
            }
            return null;
        }
    }
}