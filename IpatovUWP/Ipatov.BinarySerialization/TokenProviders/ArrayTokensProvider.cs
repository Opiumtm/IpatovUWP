﻿using System;
using System.Collections.Generic;

namespace Ipatov.BinarySerialization.TokenProviders
{
    /// <summary>
    /// Serialization tokens provider for array.
    /// </summary>
    /// <typeparam name="T">Array element type.</typeparam>
    public sealed class ArrayTokensProvider<T> : IExternalSerializationTokensProvider<T[]>
    {
        /// <summary>
        /// Get tokens for object properties.
        /// </summary>
        /// <param name="source">Source object.</param>
        /// <param name="context">Serialization context.</param>
        /// <returns>Tokens.</returns>
        public IEnumerable<SerializationProperty> GetProperties(T[] source, SerializationContext context)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            yield return new SerializationProperty()
            {
                Property = "Count",
                Token = new SerializationToken()
                {
                    TokenType = SerializationTokenType.Int32Index,
                    Value = new SerializationTokenValue() { Int32IndexValue = source.Length }
                }
            };
            foreach (var item in source)
            {
                yield return new SerializationProperty()
                {
                    Property = "Item",
                    Token = item.CreateSerializationToken(context)
                };
            }
        }

        /// <summary>
        /// Create object and fill its data from serialization tokens.
        /// </summary>
        /// <param name="properties">Serialization tokens.</param>
        /// <param name="context">Serialization context.</param>
        /// <returns>Object.</returns>
        public T[] CreateObject<TEnum>(TEnum properties, SerializationContext context) where TEnum : IEnumerable<SerializationProperty>
        {
            if (properties == null) throw new ArgumentNullException(nameof(properties));
            T[] result = null;
            int idx = 0, len = 0;
            foreach (var property in properties)
            {
                switch (property.Property)
                {
                    case "Count":
                        if (result != null)
                        {
                            throw new InvalidOperationException("Invalid token in array serialization. Count already speficied.");
                        }
                        SerializationToken.CheckType(SerializationTokenType.Int32Index, property.Token.TokenType);
                        len = property.Token.Value.Int32IndexValue;
                        result = new T[len];
                        break;
                    case "Item":
                        if (result == null)
                        {
                            throw new InvalidOperationException("Invalid token in array serialization. Count is not speficied.");
                        }
                        if (idx >= len)
                        {
                            throw new InvalidOperationException("Invalid token in array serialization. Items count > array size.");
                        }
                        var p = property.Token;
                        result[idx] = context.ExtractValue<T>(ref p);
                        idx++;
                        break;
                }
            }
            if (result == null)
            {
                throw new InvalidOperationException("Invalid tokens in array serialization. No data provided.");
            }
            if (idx != len)
            {
                throw new InvalidOperationException("Invalid tokens in array serialization. Items count < array size.");
            }
            return result;
        }
    }
}