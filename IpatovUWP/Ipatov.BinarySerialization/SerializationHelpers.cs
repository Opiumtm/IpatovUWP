using System;
using System.Collections.Generic;
using System.IO;
using Ipatov.BinarySerialization.IO;

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
        /// Deep clone object.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="source">Source object.</param>
        /// <returns>Cloned object.</returns>
        public static T DeepClone<T>(this T source)
        {
            var context = new SerializationContext(new Dictionary<Type, IExternalSerializationTokensProvider>());
            var serialized = source.CreateSerializationToken(context);
            return context.ExtractValue<T>(ref serialized);
        }

        /// <summary>
        /// Serialize object.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="writer">Stream writer.</param>
        /// <param name="source">Source object.</param>
        /// <param name="context">External serialization tokens provider.</param>
        public static void Serialize<T>(this StreamWriter writer, T source, SerializationContext context)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));
            var serialized = source.CreateSerializationToken(context);
            var bwr = new StreamTokenWriter(writer);
            try
            {
                bwr.WritePreamble();
                bwr.WriteToken(context, ref serialized);
            }
            finally
            {
                bwr.Flush();
            }
        }

        /// <summary>
        /// Serialize object.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="writer">Stream writer.</param>
        /// <param name="source">Source object.</param>
        public static void Serialize<T>(this StreamWriter writer, T source)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));
            var context = new SerializationContext(new Dictionary<Type, IExternalSerializationTokensProvider>());
            writer.Serialize(source, context);
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
                    context.SerializingComplexType(c.ObjectType);
                    try
                    {
                        var provider = context.GetComplexTypeTokensProvider<T>(c.ObjectType);
                        if (provider == null)
                        {
                            throw new InvalidOperationException($"Serialization tokens provider not found for type {typeof(T).FullName}");
                        }
                        var o = provider.CreateObject(c.Properties, context);
                        context.AddReference(c.ReferenceIndex, o);
                        return o;
                    }
                    finally
                    {
                        context.FinishedSerializingComplexType();
                    }
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
            context.SerializingComplexType(source.GetType());
            try
            {
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
                var provider = context.GetComplexTypeTokensProvider<T>(source.GetType());
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
                        Properties = provider.GetProperties(source, context),
                        ReferenceIndex = context.AddReference(source)
                    }
                };
            }
            finally
            {
                context.FinishedSerializingComplexType();
            }
        }
    }
}