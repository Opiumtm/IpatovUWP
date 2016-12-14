using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ipatov.Async.Messaging
{
    /// <summary>
    /// Async messaging channel.
    /// </summary>
    /// <typeparam name="TMsg">Message type.</typeparam>
    /// <typeparam name="TReply">Reply message type.</typeparam>
    public interface IAsyncChannel<TMsg, TReply> : IDisposable
    {
        /// <summary>
        /// Send message.
        /// </summary>
        /// <param name="msg">Message.</param>
        /// <param name="priority">Message priority.</param>
        /// <returns>Reply.</returns>
        Task<TReply> Send(TMsg msg, int priority = 0);

        /// <summary>
        /// Receive message.
        /// </summary>
        /// <returns>Async message.</returns>
        Task<IAsyncMessage<TMsg, TReply>> Receive();

        /// <summary>
        /// Send message.
        /// </summary>
        /// <param name="msg">Message.</param>
        /// <param name="token">Cancellation token.</param>
        /// <param name="priority">Message priority.</param>
        /// <returns>Reply.</returns>
        Task<TReply> Send(TMsg msg, CancellationToken token, int priority = 0);

        /// <summary>
        /// Receive message.
        /// </summary>
        /// <param name="token">Cancellation token.</param>
        /// <returns>Async message.</returns>
        Task<IAsyncMessage<TMsg, TReply>> Receive(CancellationToken token);
    }
}