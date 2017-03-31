using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ipatov.BinarySerialization.Internals;

namespace Ipatov.BinarySerialization.Reflection
{
    /// <summary>
    /// Known tokens providers.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class KnownTokenProvidersAttribute : Attribute
    {
        /// <summary>
        /// Type of <see cref="IKnownTokenProviders"/> for class.
        /// </summary>
        public Type ProvidersType { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="providersType">Providers type.</param>
        public KnownTokenProvidersAttribute(Type providersType)
        {
            ProvidersType = providersType;
        }

        private static readonly LazyDictionary<Type, IKnownTokenProviders> Instances = new LazyDictionary<Type, IKnownTokenProviders>(GetKnownTokenProviders);

        private static IKnownTokenProviders GetKnownTokenProviders(Type type)
        {
            var t = type.GetTypeInfo().DeclaredConstructors.FirstOrDefault(c => c.IsPublic && c.GetParameters().Length == 0);
            return t.Invoke(null) as IKnownTokenProviders;
        }

        /// <summary>
        /// Get providers.
        /// </summary>
        /// <returns>Providers.</returns>
        public IKnownTokenProviders GetProviders()
        {
            if (ProvidersType == null)
            {
                return null;
            }
            return Instances[ProvidersType];
        }
    }
}