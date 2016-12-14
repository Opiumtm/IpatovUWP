using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ipatov.Async.Messaging
{
    /// <summary>
    /// Async message.
    /// </summary>
    /// <typeparam name="TMsg">Message type.</typeparam>
    /// <typeparam name="TReply">Reply message type.</typeparam>
    public sealed class AsyncMessage<TMsg, TReply> : IAsyncMessage<TMsg, TReply>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="data">Message.</param>
        /// <param name="token">Cancellation token.</param>
        /// <param name="priority">Message priority.</param>
        /// <param name="stateChangedCallback">State changed callback.</param>
        public AsyncMessage(TMsg data, CancellationToken token, int priority, Action<AsyncMessage<TMsg, TReply>, AsyncMessageStateChange> stateChangedCallback = null)
        {
            Data = data;
            Priority = priority;
            _tcs = new TaskCompletionSource<TReply>();
            _stateChangedCallback = stateChangedCallback;
            token.Register(() =>
            {
                _tcs.TrySetCanceled();
                _stateChangedCallback?.Invoke(this, AsyncMessageStateChange.Canceled);
            });
        }

        private readonly TaskCompletionSource<TReply> _tcs;

        private readonly Action<AsyncMessage<TMsg, TReply>, AsyncMessageStateChange> _stateChangedCallback;

        /// <inheritdoc />
        public TMsg Data { get; }

        /// <inheritdoc />
        public int Priority { get; }

        /// <inheritdoc />
        public void Reply(TReply msg)
        {
            _tcs.TrySetResult(msg);
            _stateChangedCallback?.Invoke(this, AsyncMessageStateChange.Replied);
        }

        /// <inheritdoc />
        public void Fault(Exception error)
        {
            _tcs.TrySetException(error);
            _stateChangedCallback?.Invoke(this, AsyncMessageStateChange.Faulted);
        }

        /// <summary>
        /// Get reply task.
        /// </summary>
        /// <returns>Task.</returns>
        public Task<TReply> GetTask()
        {
            return _tcs.Task;
        }
    }
}