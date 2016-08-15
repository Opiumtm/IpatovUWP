using System;

namespace Ipatov.ServiceLocator
{
    /// <summary>
    /// Parameter for service resolver.
    /// </summary>
    /// <typeparam name="T">Type of serivce.</typeparam>
    public interface IResolveServiceParam<T> where T : class 
    {
        /// <summary>
        /// Well-known service id (null - no specific factory).
        /// </summary>
        Guid? Id { get; }

        /// <summary>
        /// Create instance (null - default behaivor).
        /// </summary>
        bool? Create { get; }
    }
}