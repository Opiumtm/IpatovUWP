using System;
using System.Collections.Generic;

namespace Ipatov.BinarySerialization.TokenProviders
{
    /// <summary>
    /// Serialization tokens provider for key/value pair.
    /// </summary>
    /// <typeparam name="TKey">Type of key.</typeparam>
    /// <typeparam name="TValue">Type of value.</typeparam>
    public sealed class KeyValuePairTokensProvider<TKey, TValue> : IExternalSerializationTokensProvider<KeyValuePair<TKey, TValue>>
    {
        /// <summary>
        /// Get tokens for object properties.
        /// </summary>
        /// <param name="source">Source object.</param>
        /// <param name="context">Serialization context.</param>
        /// <returns>Tokens.</returns>
        public IEnumerable<SerializationProperty> GetProperties(KeyValuePair<TKey, TValue> source, SerializationContext context)
        {
            yield return new SerializationProperty()
            {
                Property = "Key",
                Token = source.Key.CreateSerializationToken(context)
            };
            yield return new SerializationProperty()
            {
                Property = "Value",
                Token = source.Value.CreateSerializationToken(context)
            };
        }

        /// <summary>
        /// Create object and fill its data from serialization tokens.
        /// </summary>
        /// <param name="properties">Serialization tokens.</param>
        /// <param name="context">Serialization context.</param>
        /// <returns>Object.</returns>
        public KeyValuePair<TKey, TValue> CreateObject<TEnum>(TEnum properties, SerializationContext context) where TEnum : IEnumerable<SerializationProperty>
        {
            if (properties == null) throw new ArgumentNullException(nameof(properties));
            TKey key = default(TKey);
            TValue value = default(TValue);
            bool keyProvided = false;
            bool valueProvided = false;
            foreach (var p in properties)
            {
                switch (p.Property)
                {
                    case "Key":
                        var token = p.Token;
                        key = context.ExtractValue<TKey>(ref token);
                        keyProvided = true;
                        break;
                    case "Value":
                        var token2 = p.Token;
                        value = context.ExtractValue<TValue>(ref token2);
                        valueProvided = true;
                        break;
                }
            }
            if (keyProvided && valueProvided)
            {
                return new KeyValuePair<TKey, TValue>(key, value);
            }
            throw new InvalidOperationException("Invalid serialization tokens for key/value pair.");
        }
    }
}