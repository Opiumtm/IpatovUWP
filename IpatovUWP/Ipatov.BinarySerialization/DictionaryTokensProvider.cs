using System;
using System.Collections;
using System.Collections.Generic;

namespace Ipatov.BinarySerialization
{
    /// <summary>
    /// Serialization tokens provider for dictionary.
    /// </summary>
    /// <typeparam name="TKey">Type of key.</typeparam>
    /// <typeparam name="TValue">Type of value.</typeparam>
    /// <typeparam name="T">Type of dictionary.</typeparam>
    public sealed class DictionaryTokensProvider<TKey, TValue, T> : IExternalSerializationTokensProvider<T>
        where T : IDictionary<TKey, TValue>, new()
    {
        /// <summary>
        /// Instance.
        /// </summary>
        public static readonly DictionaryTokensProvider<TKey, TValue, T> Instance = new DictionaryTokensProvider<TKey, TValue, T>();

        /// <summary>
        /// Get tokens for object properties.
        /// </summary>
        /// <param name="source">Source object.</param>
        /// <param name="context">Serialization context.</param>
        /// <returns>Tokens.</returns>
        public IEnumerable<SerializationProperty> GetProperties(T source, SerializationContext context)
        {
            foreach (var kv in source)
            {
                foreach (var token in KeyValuePairTokensProvider<TKey, TValue>.Instance.GetProperties(kv, context))
                {
                    yield return token;
                }
            }
        }

        /// <summary>
        /// Create object and fill its data from serialization tokens.
        /// </summary>
        /// <param name="properties">Serialization tokens.</param>
        /// <param name="context">Serialization context.</param>
        /// <returns>Object.</returns>
        public T CreateObject(IEnumerable<SerializationProperty> properties, SerializationContext context)
        {
            if (properties == null) throw new ArgumentNullException(nameof(properties));
            var result = new T();
            foreach (var token in Split(properties))
            {
                var kv = KeyValuePairTokensProvider<TKey, TValue>.Instance.CreateObject<Pair>(token, context);
                result[kv.Key] = kv.Value;
            }
            return result;
        }

        private IEnumerable<Pair> Split(IEnumerable<SerializationProperty> properties)
        {
            Pair p = default(Pair);
            bool isf = true;
            foreach (var token in properties)
            {
                if (isf)
                {
                    p.Item1 = token;
                }
                else
                {
                    p.Item2 = token;
                    yield return p;
                    p = default(Pair);
                }
                isf = !isf;
            }
        }

        private struct Pair : IEnumerable<SerializationProperty>
        {
            public SerializationProperty Item1;
            public SerializationProperty Item2;

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public IEnumerator<SerializationProperty> GetEnumerator()
            {
                yield return Item1;
                yield return Item2;
            }
        }
    }
}