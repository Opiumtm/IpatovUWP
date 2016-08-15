using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace Ipatov.ServiceLocator
{
    /// <summary>
    /// Service container implementation.
    /// </summary>
    public sealed class ServiceContainer : IServiceContainer
    {
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        private readonly Dictionary<Type, List<IDisposable>> _factoriesByType = new Dictionary<Type, List<IDisposable>>();

        private readonly Dictionary<Guid, IDisposable> _factoriesById = new Dictionary<Guid, IDisposable>();

        private readonly List<IDisposable> _factories = new List<IDisposable>();

        private bool _isDisposed;

        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose()
        {
            _lock.EnterWriteLock();
            try
            {
                List<Exception> errors = new List<Exception>();
                if (!_isDisposed)
                {
                    _isDisposed = true;
                    foreach (var f in _factories)
                    {
                        try
                        {
                            f.Dispose();
                        }
                        catch (Exception ex)
                        {
                            errors.Add(ex);
                        }
                    }
                    _factories.Clear();
                    _factoriesById.Clear();
                    _factoriesByType.Clear();
                    if (errors.Count > 0)
                    {
                        throw new AggregateException(errors);
                    }
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Register service factory.
        /// </summary>
        /// <typeparam name="T">Service type.</typeparam>
        /// <param name="factory">Factory.</param>
        public void RegisterFactory<T>(IServiceFactory<T> factory) where T : class
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            _lock.EnterWriteLock();
            try
            {
                if (_isDisposed)
                {
                    throw new ObjectDisposedException("Service container is disposed");
                }
                if (_factoriesById.ContainsKey(factory.Id))
                {
                    throw new ArgumentException($"Factory with id {factory.Id} is already registered", nameof(factory));
                }
                _factories.Add(factory);
                _factoriesById[factory.Id] = factory;
                var serviceType = typeof (T);
                if (!_factoriesByType.ContainsKey(serviceType))
                {
                    _factoriesByType[serviceType] = new List<IDisposable>();
                }
                _factoriesByType[serviceType].Add(factory);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Resolve service.
        /// </summary>
        /// <typeparam name="T">Service type.</typeparam>
        /// <param name="param">Service resolve parameter (null - default resolve).</param>
        /// <returns>Service instance (null - can not resolve).</returns>
        public T Resolve<T>(IResolveServiceParam<T> param = null) where T : class
        {
            _lock.EnterReadLock();
            try
            {
                if (_isDisposed)
                {
                    throw new ObjectDisposedException("Service container is disposed");
                }
                var serviceType = typeof (T);
                if (param?.Id != null)
                {
                    var id = param.Id.Value;
                    if (!_factoriesById.ContainsKey(id))
                    {
                        return null;
                    }
                    var factory = _factoriesById[id] as IServiceFactory<T>;
                    return factory?.Resolve(param);
                }
                if (!_factoriesByType.ContainsKey(serviceType))
                {
                    return null;
                }
                var factories = _factoriesByType[serviceType];
                return factories.OfType<IServiceFactory<T>>().OrderBy(f => f.Order).Select(f => f.Resolve(param)).FirstOrDefault(s => s != null);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }
    }
}