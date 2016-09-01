using System;

namespace Ipatov.MarkupRender
{
    /// <summary>
    /// Disposable wrapper for native objects.
    /// </summary>
    /// <typeparam name="T">Object type.</typeparam>
    public interface IRenderDisposableWrapper<out T> : IDisposable
    {
        /// <summary>
        /// Data value.
        /// </summary>
        T Value { get; }         
    }
}