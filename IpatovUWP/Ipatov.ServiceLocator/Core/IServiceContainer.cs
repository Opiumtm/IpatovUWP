using System;

namespace Ipatov.ServiceLocator
{
    /// <summary>
    /// Service container.
    /// </summary>
    public interface IServiceContainer : IDisposable
    {
        /// <summary>
        /// Register service factory.
        /// </summary>
        /// <typeparam name="T">Service type.</typeparam>
        /// <param name="factory">Factory.</param>
        void RegisterFactory<T>(IServiceFactory<T> factory) where T : class;

        /// <summary>
        /// Resolve service.
        /// </summary>
        /// <typeparam name="T">Service type.</typeparam>
        /// <param name="param">Service resolve parameter (null - default resolve).</param>
        /// <returns>Service instance (null - can not resolve).</returns>
        T Resolve<T>(IResolveServiceParam<T> param = null) where T : class;
    }
}