using System;

namespace Ipatov.Async.Primitives
{
    /// <summary>
    /// Release handle for wait region.
    /// </summary>
    public struct AsyncRegionReleaseHandle : IDisposable
    {
        private readonly int _handle;
        private readonly IAsyncWaitRegion _region;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="region">Async wait region.</param>
        /// <param name="handle">Release handle.</param>
        public AsyncRegionReleaseHandle(IAsyncWaitRegion region, int handle)
        {
            this._handle = handle;
            this._region = region;
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            _region?.Release(_handle);
        }
    }
}