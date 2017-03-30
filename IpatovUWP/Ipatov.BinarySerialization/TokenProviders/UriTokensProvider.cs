using System;
using System.Collections.Generic;

namespace Ipatov.BinarySerialization.TokenProviders
{
    /// <summary>
    /// Serialization tokens provider for Uri class.
    /// </summary>
    public sealed class UriTokensProvider : IExternalSerializationTokensProvider<Uri>
    {
        /// <summary>
        /// Get tokens for object properties.
        /// </summary>
        /// <param name="source">Source object.</param>
        /// <param name="context">Serialization context.</param>
        /// <returns>Tokens.</returns>
        public IEnumerable<SerializationProperty> GetProperties(Uri source, SerializationContext context)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            yield return new SerializationProperty()
            {
                Property = "Uri",
                Token = source.ToString().CreateSerializationToken(context)
            };
            if (!source.IsAbsoluteUri)
            {
                yield return new SerializationProperty()
                {
                    Property = "IsAbsolute",
                    Token = source.IsAbsoluteUri.CreateSerializationToken(context)
                };
            }
        }

        /// <summary>
        /// Create object and fill its data from serialization tokens.
        /// </summary>
        /// <param name="properties">Serialization tokens.</param>
        /// <param name="context">Serialization context.</param>
        /// <returns>Object.</returns>
        public Uri CreateObject<TEnum>(TEnum properties, SerializationContext context) where TEnum : IEnumerable<SerializationProperty>
        {
            string uri = null;
            bool isAbsoulute = true;
            foreach (var property in properties)
            {
                if (property.Property == "Uri")
                {
                    uri = property.Token.ExtractValue<string>(context);
                }
                if (property.Property == "IsAbsolute")
                {
                    isAbsoulute = property.Token.ExtractValue<bool>(context);
                }
            }
            if (uri == null)
            {
                throw new InvalidOperationException("Deserialization error. No Uri data provided.");
            }
            return new Uri(uri, isAbsoulute ? UriKind.Absolute : UriKind.Relative);
        }
    }
}