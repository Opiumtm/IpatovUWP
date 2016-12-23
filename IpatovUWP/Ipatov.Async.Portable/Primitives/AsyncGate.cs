using System.Threading;
using System.Threading.Tasks;

namespace Ipatov.Async.Primitives
{
    /// <summary>
    /// Async gate.
    /// </summary>
    public sealed class AsyncGate : AsyncSignalBase, IAsyncGate
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
            var preset = Interlocked.CompareExchange(ref _presetWaiter, null, null);
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

        /// <summary>
        /// Reset gate.
        /// </summary>
        /// <returns>true if successful.</returns>
        public bool Reset()
        {
            Interlocked.Exchange(ref _presetWaiter, null);
            return true;
        }
    }
}