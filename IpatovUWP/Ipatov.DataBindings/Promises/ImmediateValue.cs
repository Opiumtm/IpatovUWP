namespace Ipatov.DataBindings
{
    /// <summary>
    /// Immediate data value.
    /// </summary>
    /// <typeparam name="T">Type of value.</typeparam>
    public sealed class ImmediateValue<T> : IDataPromise<T>
    {
        private readonly T _value;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="value">Immediate value.</param>
        public ImmediateValue(T value)
        {
            _value = value;
        }

        /// <summary>
        /// Continue data promise.
        /// </summary>
        /// <param name="callback">Data promise callback.</param>
        public void Continue(IDataPromiseCallback<T> callback)
        {
            callback?.ValueAvailable(_value);
        }
    }
}