using System;

namespace Ipatov.ServiceLocator
{
    /// <summary>
    /// Service factory.
    /// </summary>
    /// <typeparam name="T">Type of service.</typeparam>
    public interface IServiceFactory<T> : IDisposable where T : class 
    {
        /// <summary>
        /// Well-known id.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Order of resolve.
        /// </summary>
        int Order { get; }

        /// <summary>
        /// Get service.
        /// </summary>
        /// <param name="param">Service parameter (null for default behavior).</param>
        /// <returns>Service instance (null - can not resolve).</returns>
        T Resolve(IResolveServiceParam<T> param = null);
    }
}