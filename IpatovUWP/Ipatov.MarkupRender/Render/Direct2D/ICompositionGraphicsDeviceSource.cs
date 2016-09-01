using System;
using Windows.UI.Composition;

namespace Ipatov.MarkupRender.Direct2D
{
    /// <summary>
    /// Composition graphics device source.
    /// </summary>
    public interface ICompositionGraphicsDeviceSource
    {
        /// <summary>
        /// Get drawing device.
        /// </summary>
        /// <returns>Drawing device.</returns>
        CompositionGraphicsDevice GetDevice();

        /// <summary>
        /// Get compositor.
        /// </summary>
        /// <returns>Compositor.</returns>
        Compositor GetCompositor();

        /// <summary>
        /// Device source disposed.
        /// </summary>
        event EventHandler Disposed;
    }
}