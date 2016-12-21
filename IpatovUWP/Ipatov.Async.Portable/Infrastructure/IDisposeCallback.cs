using System;

namespace Ipatov.Async.Infrastructure
{
    /// <summary>
    /// Child object dispose callback.
    /// </summary>
    public interface IDisposeCallback<in T> where T : IDisposable
    {
        /// <summary>
        /// Child object disposed.
        /// </summary>
        /// <param name="instance">Instance.</param>
        void OnDisposed(T instance);
    }
}