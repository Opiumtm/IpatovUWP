using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ipatov.Async.Primitives
{
    /// <summary>
    /// Async event base.
    /// </summary>
    public abstract class AsyncWaitEventBase : IAsyncWaitEvent
    {
        private readonly object _lock = new object();

        private bool _state;

        private readonly List<TaskCompletionSource<Nothing>> _waiters = new List<TaskCompletionSource<Nothing>>();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="initialState">Initial state.</param>
        protected AsyncWaitEventBase(bool initialState)
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
                    if (_state)
                    {
                        return Task.FromResult(Nothing.Value);
                    }
                }
            }
        }

        /// <summary>
        /// Action on state wait.
        /// </summary>
        /// <param name="state"></param>
        protected abstract bool OnStateWait(bool oldState, bool triggered);

        /// <summary>
        /// Set event.
        /// </summary>
        public void Set()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Reset event.
        /// </summary>
        public void Reset()
        {
            throw new System.NotImplementedException();
        }
    }
}