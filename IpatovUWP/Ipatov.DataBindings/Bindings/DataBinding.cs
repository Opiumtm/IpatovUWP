using System;
using System.Threading;

namespace Ipatov.DataBindings
{
    /// <summary>
    /// Data binding.
    /// </summary>
    /// <typeparam name="TIn">Input value type.</typeparam>
    /// <typeparam name="TOut">Output value type.</typeparam>
    public sealed class DataBinding<TIn, TOut> : IDataBinding, IDataPromiseCallback<TIn>
    {
        private readonly IDataBindingSource<TIn> _source;
        private readonly IDataBindingTarget<TOut> _target;
        private readonly IDataBindingConverter<TIn, TOut> _converter;
        private int _isDisposed;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="source">Data binding source.</param>
        /// <param name="target">Data binding target.</param>
        /// <param name="converter">Data converter.</param>
        /// <param name="triggerOnCreate">Trigger binding in constructor.</param>
        public DataBinding(IDataBindingSource<TIn> source, IDataBindingTarget<TOut> target, IDataBindingConverter<TIn, TOut> converter,  bool triggerOnCreate = true)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (target == null) throw new ArgumentNullException(nameof(target));
            if (converter == null) throw new ArgumentNullException(nameof(converter));
            _source = source;
            _target = target;
            _converter = converter;
            _target.Error += TargetOnError;
            _target.Success += TargetOnSuccess;
            _source.ValueChanged += SourceOnValueChanged;
            if (triggerOnCreate)
            {
                Trigger();
            }
        }

        private void SourceOnValueChanged(object sender, EventArgs eventArgs)
        {
            Trigger();
        }

        private void TargetOnSuccess(object sender, EventArgs eventArgs)
        {
            ValueBinded?.Invoke(this, EventArgs.Empty);
        }

        private void TargetOnError(object sender, Exception exception)
        {
            Error?.Invoke(this, exception);
        }

        /// <summary>
        /// Выполняет определяемые приложением задачи, связанные с удалением, высвобождением или сбросом неуправляемых ресурсов.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            if (Interlocked.Exchange(ref _isDisposed, 1) == 0)
            {
                _target.Error -= TargetOnError;
                _target.Success -= TargetOnSuccess;
                _source.ValueChanged -= SourceOnValueChanged;
                _source.Dispose();
            }
        }

        /// <summary>
        /// Trigger data binding.
        /// </summary>
        public void Trigger()
        {
            if (Interlocked.CompareExchange(ref _isDisposed, 0, 0) == 0)
            {
                var promise = _source.GetValue();
                promise?.Continue(this);
            }
        }

        /// <summary>
        /// Data binding error.
        /// </summary>
        public event EventHandler<Exception> Error;

        /// <summary>
        /// Value is binded successfully.
        /// </summary>
        public event EventHandler ValueBinded;

        /// <summary>
        /// Value is available.
        /// </summary>
        /// <param name="value">Value.</param>
        void IDataPromiseCallback<TIn>.ValueAvailable(TIn value)
        {
            TOut convertedValue;
            try
            {
                convertedValue = _converter.Convert(value);
            }
            catch
            {
                convertedValue = _converter.FallbackValue;
            }
            _target.SetValue(convertedValue);
        }

        /// <summary>
        /// Promise error.
        /// </summary>
        /// <param name="error">Error.</param>
        void IDataPromiseCallback<TIn>.PromiseError(Exception error)
        {
            Error?.Invoke(this, error);
        }
    }
}