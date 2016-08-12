using System;

namespace Ipatov.DataBindings
{
    /// <summary>
    /// Delegated data binding event callback.
    /// </summary>
    public sealed class DataBindingDelegatedEventCallback : IDataBindingEventCallback
    {
        private readonly Action _bindingAction;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="bindingAction">Action on data binding.</param>
        public DataBindingDelegatedEventCallback(Action bindingAction)
        {
            _bindingAction = bindingAction;
        }

        /// <summary>
        /// Receive data binding event.
        /// </summary>
        public void OnDataBindingEvent()
        {
            _bindingAction?.Invoke();
        }
    }
}