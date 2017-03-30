using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            var tokensProvider = context.GetTokensProvider<T>();
            if (tokensProvider == null)
            {
                throw new InvalidOperationException($"Serialization tokens provider not found for type {typeof(T).FullName}");
            }
            return tokensProvider.CreateObject(tokensProvider.GetProperties(source, context), context);
        }

        /// <summary>
        /// Extract value from token.
        /// </summary>
        /// <typeparam name="T">Expected value type.</typeparam>
        /// <param name="token">Serialization token.</param>
        /// <param name="context">Reference cache.</param>
        /// <returns>Value.</returns>
        public static T ExtractValue<T>(this SerializationToken token, SerializationContext context)
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
                }
                if (token.Reference is SerializedComplexType)
                {
                    var c = (SerializedComplexType)token.Reference;
                    var provider = context.GetTokensProvider<T>();
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
            return token.ExtractValueInternal<T>();
        }

        /// <summary>
        /// Extract value from token.
        /// </summary>
        /// <typeparam name="T">Expected value type.</typeparam>
        /// <param name="token">Serialization token.</param>
        /// <returns>Value.</returns>
        private static T ExtractValueInternal<T>(this SerializationToken token)
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
            var provider = context.GetTokensProvider<T>();
            if (provider == null)
            {
                throw new InvalidOperationException($"Serialization tokens provider not found for type {typeof(T).FullName}");
            }
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.Reference,
                Reference = new SerializedComplexType()
                {
                    ObjectType = typeof(T),
                    Properties = provider.GetProperties(source, context).ToArray(),
                    ReferenceIndex = context.AddReference(source)
                }
            };
        }

        private static readonly Dictionary<Type, IExternalSerializationTokensProvider> Wrappers = new Dictionary<Type, IExternalSerializationTokensProvider>();

        /// <summary>
        /// Get tokens provider wrapper for type that implements <see cref="ISerializationTokensProvider"/>.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <returns>Wrapper or none if not found.</returns>
        public static IExternalSerializationTokensProvider GetDefaultTokensProvider(Type type)
        {
            lock (Wrappers)
            {
                if (!Wrappers.ContainsKey(type))
                {
                    IExternalSerializationTokensProvider r = null;
                    var tinfo = type.GetTypeInfo();
                    if (tinfo.ImplementedInterfaces.Any(t => t == typeof(ISerializationTokensProvider)) && tinfo.DeclaredConstructors.Any(c => c.IsPublic && c.GetParameters().Length == 0))
                    {
                        var pt = typeof(SerializationTokensProviderWrapper<>).MakeGenericType(type);
                        r = pt.GetTypeInfo().DeclaredConstructors.First(c => c.GetParameters().Length == 0).Invoke(null) as IExternalSerializationTokensProvider;
                    }
                    Wrappers[type] = r;
                }
                return Wrappers[type];
            }
        }
    }
}