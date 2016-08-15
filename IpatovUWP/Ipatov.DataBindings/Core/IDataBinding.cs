using System;

namespace Ipatov.DataBindings
{
    /// <summary>
    /// Data binding.
    /// </summary>
    public interface IDataBinding : IDataBindingStatusEvents, IDisposable
    {
        /// <summary>
        /// Trigger data binding.
        /// </summary>
        void Trigger();
    }
}