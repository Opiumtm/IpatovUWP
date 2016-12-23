using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ipatov.Async.Primitives
{
    /// <summary>
    /// Async wait context.
    /// </summary>
    public interface IAsyncWaitContext
    {
        /// <summary>
        /// Wait on async handle.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Task to wait with task handle.</returns>
        Task<int> Wait(CancellationToken cancellationToken);
    }
}