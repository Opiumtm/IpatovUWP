using System;

namespace Ipatov.DataBindings
{
    /// <summary>
    /// Data binding.
    /// </summary>
    public interface IDataBinding : IDisposable
    {
        /// <summary>
        /// Trigger data binding.
        /// </summary>
        void Trigger();

        /// <summary>
        /// Data binding error.
        /// </summary>
        event EventHandler<Exception> Error;

        /// <summary>
        /// Value is binded successfully.
        /// </summary>
        event EventHandler ValueBinded;
    }
}