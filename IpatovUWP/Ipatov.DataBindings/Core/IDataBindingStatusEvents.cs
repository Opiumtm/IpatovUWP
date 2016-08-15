using System;

namespace Ipatov.DataBindings
{
    /// <summary>
    /// Status events for data binding.
    /// </summary>
    public interface IDataBindingStatusEvents
    {
        /// <summary>
        /// Data binding error.
        /// </summary>
        event EventHandler<Exception> Error;

        /// <summary>
        /// Data transfer success.
        /// </summary>
        event EventHandler Success;
    }
}