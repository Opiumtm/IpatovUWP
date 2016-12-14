using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ipatov.Async.Messaging
{
    /// <summary>
    /// Blocking message channel. Sender would wait until message is received and replied.
    /// </summary>
    public sealed class BlockingChannel<TMsg, TReply> : IAsyncChannel<TMsg, TReply>
    {
        /// <inheritdoc />
        public void Dispose()
        {
        }

        /// <inheritdoc />
        public Task<TReply> Send(TMsg msg)
        {
            return Send(msg, new CancellationToken());
        }

        /// <inheritdoc />
        public Task<IAsyncMessage<TMsg, TReply>> Receive()
        {
            return Receive(new CancellationToken());
        }

        /// <inheritdoc />
        public Task<TReply> Send(TMsg msg, CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                return Task.FromCanceled<TReply>(token);
            }
            var toExecute = new List<Action>();
            var message = new AsyncMessage<TMsg, TReply>(msg, token, StateChangedCallback);
            lock (_stateLock)
            {
                if (_waiters.Count > 0)
                {
                    var waiter = _waiters[0];
                    _waiters.RemoveAt(0);
                    toExecute.Add(() =>
                    {
                        waiter.TrySetResult(message);
                    });
                }
                else
                {
                    _incoming.Add(message);
                }
            }
            foreach (var action in toExecute)
            {
                action();
            }
            return message.GetTask();
        }

        private void StateChangedCallback(AsyncMessage<TMsg, TReply> asyncMessage, AsyncMessageStateChange asyncMessageStateChange)
        {
            lock (_stateLock)
            {
                _incoming.Remove(asyncMessage);
            }
        }

        /// <inheritdoc />
        public Task<IAsyncMessage<TMsg, TReply>> Receive(CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                return Task.FromCanceled<IAsyncMessage<TMsg, TReply>>(token);
            }
            var toExecute = new List<Action>();
            Task<IAsyncMessage<TMsg, TReply>> result;
            lock (_stateLock)
            {
                if (_incoming.Count > 0)
                {
                    IAsyncMessage<TMsg, TReply> message = _incoming[0];
                    _incoming.RemoveAt(0);
                    result = Task.FromResult(message);
                }
                else
                {
                    var tcs = new TaskCompletionSource<IAsyncMessage<TMsg, TReply>>();
                    _waiters.Add(tcs);
                    toExecute.Add(() =>
                    {
                        token.Register(() =>
                        {
                            tcs.TrySetCanceled(token);
                            lock (_stateLock)
                            {
                                _waiters.Remove(tcs);
                            }
                        });
                    });
                    result = tcs.Task;
                }
            }
            foreach (var action in toExecute)
            {
                action();
            }
            return result;
        }

        private readonly object _stateLock = new object();

        private readonly List<AsyncMessage<TMsg, TReply>> _incoming = new List<AsyncMessage<TMsg, TReply>>();

        private readonly List<TaskCompletionSource<IAsyncMessage<TMsg, TReply>>> _waiters = new List<TaskCompletionSource<IAsyncMessage<TMsg, TReply>>>();
    }
}