using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ipatov.Async.Primitives
{
    /// <summary>
    /// Async access context.
    /// </summary>
    public interface IAsyncAccessContext
    {
        /// <summary>
        /// Request access async.
        /// </summary>
        /// <param name="priority">Priority.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Disposable token to release access.</returns>
        Task<IDisposable> RequestAccess(AsyncSchedulePriority priority, CancellationToken cancellationToken);
    }
}