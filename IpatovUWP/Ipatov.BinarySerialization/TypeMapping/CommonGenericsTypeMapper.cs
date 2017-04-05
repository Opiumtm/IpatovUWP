using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Ipatov.BinarySerialization.TypeMapping
{
    /// <summary>
    /// Common generics type mapper.
    /// </summary>
    public sealed class CommonGenericsTypeMapper : ITypeMapper
    {
        /// <summary>
        /// Type mapper.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <param name="context">Serialization context.</param>
        /// <returns>Type name to serialize.</returns>
        public SerializationTypeMapping? GetTypeName(Type type, SerializationContext context)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (type.IsConstructedGenericType || type.IsArray)
            {
                var cache = GetMappingCache(context);
                if (!cache.Cache.ContainsKey(type))
                {
                    cache.Cache[type] = DoGetTypeName(type, context);
                }
                return cache.Cache[type];
            }
            return null;
        }

        private const string Kind = "CommonGeneric";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private SerializationTypeMapping? DoGetTypeName(Type type, SerializationContext context)
        {
            if (type.IsArray)
            {
                return new SerializationTypeMapping()
                {
                    Kind = Kind,
                    Type = "Array",
                    TypeParameters = CheckArgumentMapprings(new[] { context.TypeMapper.GetTypeName(type.GetElementType(), context) })
                };
            }
            var gtype = type.GetGenericTypeDefinition();
            if (gtype == typeof(KeyValuePair<,>))
            {
                return new SerializationTypeMapping()
                {
                    Kind = Kind,
                    Type = "KeyValuePair",
                    TypeParameters = MapGenericArguments(type, context)
                };
            }
            if (gtype == typeof(Dictionary<,>))
            {
                return new SerializationTypeMapping()
                {
                    Kind = Kind,
                    Type = "Dictionary",
                    TypeParameters = MapGenericArguments(type, context)
                };
            }
            if (gtype == typeof(List<>))
            {
                return new SerializationTypeMapping()
                {
                    Kind = Kind,
                    Type = "List",
                    TypeParameters = MapGenericArguments(type, context)
                };
            }
            if (gtype == typeof(HashSet<>))
            {
                return new SerializationTypeMapping()
                {
                    Kind = Kind,
                    Type = "HashSet",
                    TypeParameters = MapGenericArguments(type, context)
                };
            }
            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private SerializationTypeMapping[] CheckArgumentMapprings<T>(T map) where T : IEnumerable<SerializationTypeMapping?>
        {
            return map.Select(t => t ?? throw new InvalidOperationException("Could not map generic type argument.")).ToArray();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private SerializationTypeMapping[] MapGenericArguments(Type type, SerializationContext context)
        {
            return CheckArgumentMapprings(type.GenericTypeArguments.Select(t => context.TypeMapper.GetTypeName(t, context)));
        }

        /// <summary>
        /// Get type from name.
        /// </summary>
        /// <param name="typeName">Type name.</param>
        /// <param name="context">Serialization context.</param>
        /// <returns>Type.</returns>
        public Type GetType(ref SerializationTypeMapping typeName, SerializationContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (typeName.Kind == Kind)
            {
                var cache = GetMappingCache(context);
                var key = typeName.ToString();
                if (!cache.ReverseCache.ContainsKey(key))
                {
                    cache.ReverseCache[key] = DoGetType(ref typeName, context);
                }
                return cache.ReverseCache[key];
            }
            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Type DoGetType(ref SerializationTypeMapping typeName, SerializationContext context)
        {
            if (typeName.TypeParameters == null || typeName.TypeParameters.Length < 1)
            {
                return null;
            }
            switch (typeName.Type)
            {
                case "KeyValuePair" when typeName.TypeParameters.Length == 2:
                    return typeof(KeyValuePair<,>).MakeGenericType(
                        CheckForNull(context.TypeMapper.GetType(ref typeName.TypeParameters[0], context)),
                        CheckForNull(context.TypeMapper.GetType(ref typeName.TypeParameters[1], context))
                        );
                case "Dictionary" when typeName.TypeParameters.Length == 2:
                    return typeof(Dictionary<,>).MakeGenericType(
                        CheckForNull(context.TypeMapper.GetType(ref typeName.TypeParameters[0], context)),
                        CheckForNull(context.TypeMapper.GetType(ref typeName.TypeParameters[1], context))
                        );
                case "List" when typeName.TypeParameters.Length == 1:
                    return typeof(List<>).MakeGenericType(
                        CheckForNull(context.TypeMapper.GetType(ref typeName.TypeParameters[0], context))
                        );
                case "HashSet" when typeName.TypeParameters.Length == 1:
                    return typeof(HashSet<>).MakeGenericType(
                        CheckForNull(context.TypeMapper.GetType(ref typeName.TypeParameters[0], context))
                        );
                case "Array" when typeName.TypeParameters.Length == 1:
                    return CheckForNull(context.TypeMapper.GetType(ref typeName.TypeParameters[0], context)).MakeArrayType();
                default:
                    return null;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Type CheckForNull(Type t)
        {
            if (t == null)
            {
                throw new InvalidOperationException("Could not map generic type argument.");
            }
            return t;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private MappingsCache GetMappingCache(SerializationContext context)
        {
            var key = typeof(CommonGenericsTypeMapper);
            if (!context.AdditionalData.ContainsKey(key))
            {
                context.AdditionalData[key] = new MappingsCache()
                {
                    Cache = new Dictionary<Type, SerializationTypeMapping?>(),
                    ReverseCache = new Dictionary<string, Type>(StringComparer.Ordinal)
                };
            }
            return (MappingsCache) context.AdditionalData[key];
        }

        private class MappingsCache
        {
            public Dictionary<Type, SerializationTypeMapping?> Cache;
            public Dictionary<string, Type> ReverseCache;
        }
    }
}