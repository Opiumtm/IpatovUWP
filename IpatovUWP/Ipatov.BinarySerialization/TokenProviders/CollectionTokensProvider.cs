using System;
using System.Collections.Generic;

namespace Ipatov.BinarySerialization.TokenProviders
{
    /// <summary>
    /// Serialization tokens provider for generic collection.
    /// </summary>
    /// <typeparam name="T">Collection type.</typeparam>
    /// <typeparam name="TElement">Element type.</typeparam>
    public sealed class CollectionTokensProvider<T, TElement> : IExternalSerializationTokensProvider<T>
        where T : ICollection<TElement>, new()
    {
        /// <summary>
        /// Instance.
        /// </summary>
        public static readonly CollectionTokensProvider<T, TElement> Instance = new CollectionTokensProvider<T, TElement>();

        /// <summary>
        /// Get tokens for object properties.
        /// </summary>
        /// <param name="source">Source object.</param>
        /// <param name="context">Serialization context.</param>
        /// <returns>Tokens.</returns>
        public IEnumerable<SerializationProperty> GetProperties(T source, SerializationContext context)
        {
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
        public T CreateObject<TEnum>(TEnum properties, SerializationContext context) where TEnum : IEnumerable<SerializationProperty>
        {
            if (properties == null) throw new ArgumentNullException(nameof(properties));
            T result = new T();
            foreach (var property in properties)
            {
                switch (property.Property)
                {
                    case "Item":
                        result.Add(property.Token.ExtractValue<TElement>(context));
                        break;
                }
            }
            return result;
        }
    }
}