﻿using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Ipatov.BinarySerialization.TokenProviders
{
    /// <summary>
    /// Wrapper for serialization tokens provider.
    /// </summary>
    /// <typeparam name="T">Objet type.</typeparam>
    public sealed class SerializationTokensProviderWrapper<T> : IExternalSerializationTokensProvider<T>
        where T: ISerializationTokensProvider, new()
    {
        /// <summary>
        /// Get tokens for object properties.
        /// </summary>
        /// <param name="source">Source object.</param>
        /// <param name="context">Serialization context.</param>
        /// <returns>Tokens.</returns>
        public IEnumerable<SerializationProperty> GetProperties(T source, SerializationContext context)
        {
            return source.GetProperties(context);
        }

        /// <summary>
        /// Create object and fill its data from serialization tokens.
        /// </summary>
        /// <param name="properties">Serialization tokens.</param>
        /// <param name="context">Serialization context.</param>
        /// <returns>Object.</returns>
        public T CreateObject<TEnum>(TEnum properties, SerializationContext context) where TEnum : IEnumerable<SerializationProperty>
        {
            var result = new T();
            using (var propEnum = properties.GetEnumerator())
            {
                while (propEnum.MoveNext())
                {
                    var p = propEnum.Current;
                    result.FillProperty(ref p, context);
                }
            }
            return result;
        }
    }
}