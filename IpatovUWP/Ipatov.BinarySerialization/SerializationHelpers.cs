using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Ipatov.BinarySerialization.TokenProviders;

namespace Ipatov.BinarySerialization
{
    /// <summary>
    /// Serialization helper class.
    /// </summary>
    public static class SerializationHelpers
    {
        /// <summary>
        /// Deep clone object.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="source">Source object.</param>
        /// <param name="context">External serialization tokens provider.</param>
        /// <returns>Cloned object.</returns>
        public static T DeepClone<T>(this T source, SerializationContext context)
        {
            var serialized = source.CreateSerializationToken(context);
            return context.ExtractValue<T>(ref serialized);
        }

        /// <summary>
        /// Extract value from token.
        /// </summary>
        /// <typeparam name="T">Expected value type.</typeparam>
        /// <param name="token">Serialization token.</param>
        /// <param name="context">Reference cache.</param>
        /// <returns>Value.</returns>
        public static T ExtractValue<T>(this SerializationContext context, ref SerializationToken token)
        {
            if (token.TokenType == SerializationTokenType.Nothing)
            {
                return default(T);
            }
            if (token.TokenType == SerializationTokenType.Reference)
            {
                if (token.Reference == null)
                {
                    throw new InvalidOperationException("Deserialization error. Reference in token is null.");
                }
                if (token.Reference is SerializedComplexTypeReference)
                {
                    var r = (SerializedComplexTypeReference) token.Reference;
                    var obj = context.GetReference(r.ReferenceIndex);
                    if (obj == null)
                    {
                        throw new InvalidOperationException($"Deserialization error. Invalid reference index {r.ReferenceIndex}.");
                    }
                    if (obj is T)
                    {
                        return (T)obj;
                    }
                    throw new InvalidOperationException($"Deserialization error. Incompatible type of cached reference. Expected {typeof(T).FullName}, actual {obj.GetType().FullName}.");
                }
                if (token.Reference is SerializedComplexType)
                {
                    var c = (SerializedComplexType)token.Reference;
                    var provider = GetComplexTypeTokensProvider<T>(context, c.ObjectType);
                    if (provider == null)
                    {
                        throw new InvalidOperationException($"Serialization tokens provider not found for type {typeof(T).FullName}");
                    }
                    var t = typeof(T);
                    if (t == c.ObjectType)
                    {
                        var o = provider.CreateObject(c.Properties, context);
                        context.AddReference(c.ReferenceIndex, o);
                        return o;
                    }
                    if (c.ObjectType.GetTypeInfo().IsSubclassOf(t))
                    {
                        var o = provider.CreateObject(c.Properties, context);
                        context.AddReference(c.ReferenceIndex, o);
                        return o;
                    }
                    throw new InvalidOperationException($"Deserialization error. Object of type {c.ObjectType.FullName} is not subclass of expected type {t.FullName}");
                }
            }
            return ExtractValueInternal<T>(ref token);
        }

        /// <summary>
        /// Extract value from token.
        /// </summary>
        /// <typeparam name="T">Expected value type.</typeparam>
        /// <param name="token">Serialization token.</param>
        /// <returns>Value.</returns>
        private static T ExtractValueInternal<T>(ref SerializationToken token)
        {
            var extractor = SerializationToken.GetExtractor<T>();
            if (extractor != null)
            {
                return extractor.GetValue(token);
            }
            SerializationToken.CheckType(SerializationTokenType.Reference, token.TokenType);
            return (T)token.Reference;
        }

        /// <summary>
        /// Create serialization token from value.
        /// </summary>
        /// <typeparam name="T">Value type.</typeparam>
        /// <param name="value">Value.</param>
        /// <param name="context">Serialization context.</param>
        /// <returns>Serialization token.</returns>
        public static SerializationToken CreateSerializationToken<T>(this T value, SerializationContext context)
        {
            var extractor = SerializationToken.GetExtractor<T>();
            if (extractor != null)
            {
                return extractor.CreateToken(value);
            }
            return value.SerializeAsComplexType(context);
        }

        /// <summary>
        /// Serialize complex type.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="source">Source object.</param>
        /// <param name="context">Serialization context.</param>
        /// <returns>Serialization token.</returns>
        private static SerializationToken SerializeAsComplexType<T>(this T source, SerializationContext context)
        {
            if (source == null)
            {
                return new SerializationToken()
                {
                    TokenType = SerializationTokenType.Nothing
                };
            }
            var idx = context.IsReferenced(source);
            if (idx != null)
            {
                return new SerializationToken()
                {
                    TokenType = SerializationTokenType.Reference,
                    Reference = new SerializedComplexTypeReference()
                    {
                        ReferenceIndex = idx.Value
                    }
                };
            }
            var provider = GetComplexTypeTokensProvider<T>(context, source.GetType());
            if (provider == null)
            {
                throw new InvalidOperationException($"Serialization tokens provider not found for type {typeof(T).FullName}");
            }
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.Reference,
                Reference = new SerializedComplexType()
                {
                    ObjectType = source.GetType(),
                    Properties = provider.GetProperties(source, context).ToArray(),
                    ReferenceIndex = context.AddReference(source)
                }
            };
        }

        private static IExternalSerializationTokensProvider<T> GetComplexTypeTokensProvider<T>(SerializationContext context, Type sourceType)
        {
            if (sourceType == typeof(T))
            {
                return context.GetTokensProvider<T>();
            }
            var srcProvider = context.GetTokensProvider(sourceType);
            return GetProviderForSubclass<T>(srcProvider, sourceType);
        }

        private static readonly Dictionary<KeyValuePair<Type, Type>, IExternalSerializationTokensProvider> SubclassWrappers = new Dictionary<KeyValuePair<Type, Type>, IExternalSerializationTokensProvider>();

        private static IExternalSerializationTokensProvider<T> GetProviderForSubclass<T>(IExternalSerializationTokensProvider provider, Type subclassType)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            if (subclassType == null) throw new ArgumentNullException(nameof(subclassType));
            var key = new KeyValuePair<Type, Type>(typeof(T), subclassType);
            lock (SubclassWrappers)
            {
                if (!SubclassWrappers.ContainsKey(key))
                {
                    IExternalSerializationTokensProvider r = null;
                    var parentType = typeof(T);
                    if (subclassType.GetTypeInfo().IsSubclassOf(parentType))
                    {
                        var pt = typeof(SubclassExternalSerializationTokensProvider<,>).MakeGenericType(typeof(T), subclassType);
                        var st = typeof(IExternalSerializationTokensProvider<>).MakeGenericType(subclassType);
                        r = pt.GetTypeInfo().DeclaredConstructors.FirstOrDefault(c =>
                        {
                            var p = c.GetParameters();
                            return p.Length == 1 && p[0].ParameterType == st && c.IsPublic;
                        })?.Invoke(new object[] { provider }) as IExternalSerializationTokensProvider<T>;
                    }
                    SubclassWrappers[key] = r;
                }
                return SubclassWrappers[key] as IExternalSerializationTokensProvider<T>;
            }
        }

        private static readonly Dictionary<Type, IExternalSerializationTokensProvider> Wrappers = new Dictionary<Type, IExternalSerializationTokensProvider>();

        /// <summary>
        /// Get tokens provider wrapper for type that implements <see cref="ISerializationTokensProvider"/>.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <returns>Wrapper or none if not found.</returns>
        public static IExternalSerializationTokensProvider GetDefaultTokensProvider(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            lock (Wrappers)
            {
                if (!Wrappers.ContainsKey(type))
                {
                    IExternalSerializationTokensProvider r = null;
                    var tinfo = type.GetTypeInfo();
                    var gtinfo = type.IsConstructedGenericType ? type.GetGenericTypeDefinition() : null;
                    var haveParameterlessContstructor = tinfo.DeclaredConstructors.Any(c => c.IsPublic && c.GetParameters().Length == 0);

                    if (type == typeof(Uri))
                    {
                        r = new UriTokensProvider();
                    }

                    if (r == null)
                    {
                        if (type.IsArray)
                        {
                            var pt = typeof(ArrayTokensProvider<>).MakeGenericType(tinfo.GetElementType());
                            r = CreateExternalSerializationTokensProvider(pt);
                        }
                    }

                    if (r == null)
                    {
                        if (gtinfo == typeof(KeyValuePair<,>))
                        {
                            var pt = typeof(KeyValuePairTokensProvider<,>).MakeGenericType(tinfo.GenericTypeArguments[0], tinfo.GenericTypeArguments[1]);
                            r = CreateExternalSerializationTokensProvider(pt);
                        }
                    }

                    if (r == null)
                    {
                        if (gtinfo == typeof(Dictionary<,>) || gtinfo == typeof(SortedDictionary<,>))
                        {
                            var pt = typeof(DictionaryTokensProvider<,,>).MakeGenericType(tinfo.GenericTypeArguments[0], tinfo.GenericTypeArguments[1], type);
                            r = CreateExternalSerializationTokensProvider(pt);
                        }
                    }

                    if (r == null)
                    {
                        if (gtinfo == typeof(List<>) || gtinfo == typeof(HashSet<>) || gtinfo == typeof(SortedSet<>))
                        {
                            var pt = typeof(CollectionTokensProvider<,>).MakeGenericType(type, tinfo.GenericTypeArguments[0]);
                            r = CreateExternalSerializationTokensProvider(pt);
                        }
                    }

                    if (r == null)
                    {
                        // for wrapper
                        if (tinfo.ImplementedInterfaces.Any(t => t == typeof(ISerializationTokensProvider)) && haveParameterlessContstructor)
                        {
                            var pt = typeof(SerializationTokensProviderWrapper<>).MakeGenericType(type);
                            r = CreateExternalSerializationTokensProvider(pt);
                        }

                    }
                    Wrappers[type] = r;
                }
                return Wrappers[type];
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IExternalSerializationTokensProvider CreateExternalSerializationTokensProvider(Type pt)
        {
            return pt.GetTypeInfo().DeclaredConstructors.FirstOrDefault(c => c.GetParameters().Length == 0 && c.IsPublic)?.Invoke(null) as IExternalSerializationTokensProvider;
        }
    }
}