using System;

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
            var r = new KnownTokenProviders();
            foreach (var provider in providers)
            {
                if (provider != null)
                {
                    r = r + provider.GetKnownTokenProviders();
                }
            }
            _providers = r;
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