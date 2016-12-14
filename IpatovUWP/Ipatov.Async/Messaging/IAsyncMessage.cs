using System;

namespace Ipatov.Async.Messaging
{
    /// <summary>
    /// Async message.
    /// </summary>
    /// <typeparam name="TMsg">Message type.</typeparam>
    /// <typeparam name="TReply">Reply message type.</typeparam>
    public interface IAsyncMessage<out TMsg, in TReply>
    {
        /// <summary>
        /// Message data.
        /// </summary>
        TMsg Data { get; }

        /// <summary>
        /// Reply to sender.
        /// </summary>
        /// <param name="msg">Reply message.</param>
        void Reply(TReply msg);

        /// <summary>
        /// Reply with fault.
        /// </summary>
        /// <param name="error">Error.</param>
        void Fault(Exception error);
    }
}