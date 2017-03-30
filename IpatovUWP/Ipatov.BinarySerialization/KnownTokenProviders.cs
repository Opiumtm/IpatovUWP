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
        private readonly Dictionary<Type, IExternalSerializationTokensProvider> _providers;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="providers">Providers.</param>
        public KnownTokenProviders(Dictionary<Type, IExternalSerializationTokensProvider> providers)
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

        public static KnownTokenProviders operator +(KnownTokenProviders a, KnownTokenProviders b)
        {
            var d = new Dictionary<Type, IExternalSerializationTokensProvider>();
            if (a._providers != null)
            {
                foreach (var kv in a._providers)
                {
                    d[kv.Key] = kv.Value;
                }
            }
            if (b._providers != null)
            {
                foreach (var kv in b._providers)
                {
                    d[kv.Key] = kv.Value;
                }
            }
            return new KnownTokenProviders(d);
        }

        public static implicit operator KnownTokenProviders(Dictionary<Type, IExternalSerializationTokensProvider> src)
        {
            return new KnownTokenProviders(src);
        }
    }
}