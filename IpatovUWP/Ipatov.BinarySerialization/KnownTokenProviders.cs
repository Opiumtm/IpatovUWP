using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Ipatov.BinarySerialization
{
    /// <summary>
    /// Known token providers.
    /// </summary>
    public struct KnownTokenProviders
    {
        private readonly IReadOnlyDictionary<Type, IExternalSerializationTokensProvider> _providers;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="providers">Providers.</param>
        public KnownTokenProviders(IReadOnlyDictionary<Type, IExternalSerializationTokensProvider> providers)
        {
            _providers = providers;
        }

        /// <summary>
        /// Create context.
        /// </summary>
        /// <returns>Context.</returns>
        public SerializationContext CreateContext()
        {
            return new SerializationContext(_providers ?? new Dictionary<Type, IExternalSerializationTokensProvider>());
        }

        /// <summary>
        /// Get token provider.
        /// </summary>
        /// <param name="type">type.</param>
        /// <returns>Token provider.</returns>
        public IExternalSerializationTokensProvider GetProvider(Type type)
        {
            if (_providers == null)
            {
                return null;
            }
            return _providers.ContainsKey(type) ? _providers[type] : null;
        }

        /// <summary>
        /// Get token providers.
        /// </summary>
        /// <returns>Token providers.</returns>
        public IReadOnlyDictionary<Type, IExternalSerializationTokensProvider> GetProviders()
        {
            return _providers;
        }
    }
}