using System;

namespace Ipatov.DataBindings
{
    /// <summary>
    /// Data binding.
    /// </summary>
    /// <typeparam name="T">Value type.</typeparam>
    public interface IDataBinding<T> : IDisposable
    {
        /// <summary>
        /// Bound source.
        /// </summary>
        IDataBindingValueGetter<T> Source { get; }

        /// <summary>
        /// Bound target.
        /// </summary>
        IDataBindingValueSetter<T> Target { get; }

        /// <summary>
        /// Trigger data binding.
        /// </summary>
        void Trigger();

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