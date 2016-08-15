using System;

namespace Ipatov.ServiceLocator
{
    /// <summary>
    /// Service container helper.
    /// </summary>
    public static class ServiceContainerHelper
    {
        /// <summary>
        /// Get service.
        /// </summary>
        /// <typeparam name="T">Service type.</typeparam>
        /// <param name="container">Service container.</param>
        /// <param name="throwIfNotFound">Throw exception if failed to resolve service.</param>
        /// <param name="param">Resolve parameter.</param>
        /// <returns></returns>
        public static T GetService<T>(this IServiceContainer container, bool throwIfNotFound = true, IResolveServiceParam<T> param = null) where T : class
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            var result = container.Resolve(param);
            if (throwIfNotFound && result == null)
            {
                throw new InvalidOperationException($"Can not resolve {typeof(T).FullName} serivce");
            }
            return result;
        }
    }
}