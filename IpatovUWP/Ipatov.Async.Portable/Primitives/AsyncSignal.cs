using System.Threading;
using System.Threading.Tasks;

namespace Ipatov.Async.Primitives
{
    /// <summary>
    /// Async signal.
    /// </summary>
    public sealed class AsyncSignal : AsyncSignalBase
    {
        private readonly Task<int> _completedTask = Task.FromResult(-1);

        private Task<int> _presetWaiter;

        /// <summary>
        /// Wait on async handle.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Task to wait with task handle.</returns>
        public override async Task<int> Wait(CancellationToken cancellationToken)
        {
            var preset = Interlocked.Exchange(ref _presetWaiter, null);
            if (preset != null)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return await preset;
            }
            return await base.Wait(cancellationToken);
        }

        /// <summary>
        /// Trigger signal.
        /// </summary>
        /// <returns>true if successful.</returns>
        public override bool Set()
        {
            Interlocked.Exchange(ref _presetWaiter, _completedTask);
            return base.Set();
        }
    }
}