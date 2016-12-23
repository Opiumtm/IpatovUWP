using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Ipatov.Async.Primitives
{
    /// <summary>
    /// Wait context helpers.
    /// </summary>
    public static class AsyncWaitContextHelpers
    {
        /// <summary>
        /// Get wait region as disposable.
        /// </summary>
        /// <param name="waitContext">Wait context.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Disposable to release handle.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task<AsyncRegionReleaseHandle> WaitRegion(this IAsyncWaitRegion waitContext, CancellationToken cancellationToken)
        {
            if (waitContext == null) throw new ArgumentNullException(nameof(waitContext));
            return new AsyncRegionReleaseHandle(waitContext, await waitContext.Wait(cancellationToken));
        }

        /// <summary>
        /// Get wait region as disposable.
        /// </summary>
        /// <param name="waitContext">Wait context.</param>
        /// <returns>Disposable to release handle.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task<AsyncRegionReleaseHandle> WaitRegion(this IAsyncWaitRegion waitContext)
        {
            if (waitContext == null) throw new ArgumentNullException(nameof(waitContext));
            return new AsyncRegionReleaseHandle(waitContext, await waitContext.Wait(CancellationToken.None));
        }
    }
}