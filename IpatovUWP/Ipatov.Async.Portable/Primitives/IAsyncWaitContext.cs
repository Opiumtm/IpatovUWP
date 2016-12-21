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
        /// Wait on context.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Task to wait.</returns>
        Task Wait(CancellationToken cancellationToken);
    }
}