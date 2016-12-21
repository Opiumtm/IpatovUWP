using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ipatov.Async.Primitives
{
    /// <summary>
    /// Auto reset event.
    /// </summary>
    public sealed class AsyncAutoResetEvent : IAsyncWaitContext
    {
        private readonly List<TaskCompletionSource<Nothing>> _waiters = new List<TaskCompletionSource<Nothing>>();

        private readonly object _lock = new object();

        private bool _state;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="initialState">Initial state.</param>
        public AsyncAutoResetEvent(bool initialState)
        {
            _state = initialState;
        }

        /// <summary>
        /// Wait on context.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Task to wait.</returns>
        public Task Wait(CancellationToken cancellationToken)
        {
            using (var toDo = new ActionList(null))
            {
                lock (_lock)
                {
                    var tcs = new TaskCompletionSource<Nothing>();
                    _waiters.Add(tcs);
                    toDo.Register(() =>
                    {
                        cancellationToken.Register(() =>
                        {
                            tcs.TrySetCanceled();
                            lock (_lock)
                            {
                                _waiters.Remove(tcs);
                            }
                        });
                    });
                    return tcs.Task;
                }
            }
        }
    }
}