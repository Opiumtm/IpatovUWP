using System;
using System.Collections.Generic;
using Ipatov.BinarySerialization.Internals;

namespace Ipatov.BinarySerialization.TokenProviders
{
    /// <summary>
    /// Combined token providers info.
    /// </summary>
    public sealed class CombinedTokenProvidersInfo : IKnownTokenProviders
    {
        private readonly KnownTokenProviders _providers;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="providers">Token providers.</param>
        public CombinedTokenProvidersInfo(params IKnownTokenProviders[] providers)
        {
            if (providers == null) throw new ArgumentNullException(nameof(providers));
            var dicts = new List<IReadOnlyDictionary<Type, IExternalSerializationTokensProvider>>();
            foreach (var provider in providers)
            {
                if (provider != null)
                {
                    dicts.Add(provider.GetKnownTokenProviders().GetProviders());
                }
            }
            _providers = new KnownTokenProviders(new CompoundDictionary<Type, IExternalSerializationTokensProvider>(dicts.ToArray()));
        }

        /// <summary>
        /// Get known token providers info.
        /// </summary>
        /// <returns>Known token providers.</returns>
        public KnownTokenProviders GetKnownTokenProviders()
        {
            return _providers;
        }
    }
}