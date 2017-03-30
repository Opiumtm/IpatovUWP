using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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

        private static readonly Dictionary<Type, IKnownTokenProviders> Instances = new Dictionary<Type, IKnownTokenProviders>();

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
            lock (Instances)
            {
                if (!Instances.ContainsKey(ProvidersType))
                {
                    var t = ProvidersType.GetTypeInfo().DeclaredConstructors.FirstOrDefault(c => c.IsPublic && c.GetParameters().Length == 0);
                    Instances[ProvidersType] = t.Invoke(null) as IKnownTokenProviders;
                }
                return Instances[ProvidersType];
            }
        }
    }
}