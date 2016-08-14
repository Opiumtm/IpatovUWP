using System;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Data;

namespace Ipatov.DataBindings
{
    /// <summary>
    /// Data binding values helper.
    /// </summary>
    public static class DataBindingValueHelper
    {
        /// <summary>
        /// Get value.
        /// </summary>
        /// <typeparam name="T">Value type.</typeparam>
        /// <param name="getter">Value getter.</param>
        /// <returns>Task.</returns>
        public static Task<T> GetValue<T>(this IDataBindingValueGetter<T> getter)
        {
            if (getter == null) throw new ArgumentNullException(nameof(getter));
            var tcs = new TaskCompletionSource<T>();
            getter.GetValue(v => tcs.SetResult(v), ex => tcs.SetException(ex));
            return tcs.Task;
        }

        /// <summary>
        /// Set value.
        /// </summary>
        /// <typeparam name="T">Value type.</typeparam>
        /// <param name="setter">Value setter.</param>
        /// <param name="value">Value.</param>
        /// <returns>Task.</returns>
        public static Task SetValue<T>(this IDataBindingValueSetter<T> setter, T value)
        {
            if (setter == null) throw new ArgumentNullException(nameof(setter));
            var tcs = new TaskCompletionSource<bool>();
            setter.SetValue(value, () => tcs.SetResult(true), ex => tcs.SetException(ex));
            return tcs.Task;
        }

        /// <summary>
        /// Create value accessor.
        /// </summary>
        /// <typeparam name="TSrc">Source object type.</typeparam>
        /// <typeparam name="T">Value type.</typeparam>
        /// <param name="source">Data source.</param>
        /// <param name="getFunc">Value getter function.</param>
        /// <param name="setFunc">Value setter action.</param>
        /// <returns>Value access wrapper.</returns>
        public static IDataBindingAccessor<T> ValueAccessor<TSrc, T>(this TSrc source, Func<TSrc, T> getFunc, Action<TSrc, T> setFunc = null)
        {
            if (getFunc == null) throw new ArgumentNullException(nameof(getFunc));
            return new DataBindingValueAccessWrapper<T>(() => getFunc(source), v => setFunc?.Invoke(source, v));
        }

        /// <summary>
        /// Create value accessor having weak reference to source object.
        /// </summary>
        /// <typeparam name="TSrc">Source object type.</typeparam>
        /// <typeparam name="T">Value type.</typeparam>
        /// <param name="source">Data source.</param>
        /// <param name="getFunc">Value getter function.</param>
        /// <param name="setFunc">Value setter action.</param>
        /// <returns>Value access wrapper.</returns>
        public static IDataBindingAccessor<T> WeakValueAccessor<TSrc, T>(this TSrc source, Func<TSrc, T> getFunc, Action<TSrc, T> setFunc = null) where TSrc : class
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (getFunc == null) throw new ArgumentNullException(nameof(getFunc));
            return WeakValueAccessor(new WeakReference<TSrc>(source), getFunc, setFunc);
        }

        private static IDataBindingAccessor<T> WeakValueAccessor<TSrc, T>(WeakReference<TSrc> source, Func<TSrc, T> getFunc, Action<TSrc, T> setFunc = null) where TSrc : class
        {
            return new DataBindingValueAccessWrapper<T>(() =>
            {
                TSrc obj;
                if (source.TryGetTarget(out obj))
                {
                    return getFunc(obj);
                }
                throw new InvalidOperationException("Weak referenced object is collected by GC");
            }, v =>
            {
                TSrc obj;
                if (source.TryGetTarget(out obj))
                {
                    setFunc?.Invoke(obj, v);
                }
                else
                {
                    throw new InvalidOperationException("Weak referenced object is collected by GC");
                }
            });
        }

        /// <summary>
        /// Execute data access on UI thread.
        /// </summary>
        /// <typeparam name="T">Value type.</typeparam>
        /// <param name="source">Source data accessor.</param>
        /// <param name="dispatcher">UI thread dispatcher.</param>
        /// <param name="priority">Dispatcher priority.</param>
        /// <returns>Value access wrapper.</returns>
        public static IDataBindingAccessor<T> OnUiThread<T>(this IDataBindingAccessor<T> source, CoreDispatcher dispatcher = null, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            return new DataBindingOnUiThreadValueAccessWrapper<T>(source, dispatcher, priority);
        }

        /// <summary>
        /// Convert value type in data access.
        /// </summary>
        /// <typeparam name="TIn">Source value type.</typeparam>
        /// <typeparam name="TOut">Result value type.</typeparam>
        /// <param name="source">Source data accessor.</param>
        /// <param name="convert">Convert function.</param>
        /// <param name="convertBack">Backwards convert function.</param>
        /// <returns></returns>
        public static IDataBindingAccessor<TOut> Convert<TIn, TOut>(this IDataBindingAccessor<TIn> source, Func<TIn, TOut> convert, Func<TOut, TIn> convertBack = null)
        {
            return new DataBindingValueAccessTransformer<TIn,TOut>(source, convert, convertBack ?? new Func<TOut, TIn>(v =>
            {
                throw new InvalidOperationException("Backwards value converter is not provided");
            }));
        }

        /// <summary>
        /// Convert value type in data access.
        /// </summary>
        /// <typeparam name="TIn">Source value type.</typeparam>
        /// <typeparam name="TOut">Result value type.</typeparam>
        /// <param name="source">Source data accessor.</param>
        /// <param name="valueConverter">XAML UI value converter.</param>
        /// <param name="parameter">Value converter parameter.</param>
        /// <param name="language">Value converter language.</param>
        public static IDataBindingAccessor<TOut> Convert<TIn, TOut>(this IDataBindingAccessor<TIn> source, IValueConverter valueConverter, object parameter = null, string language = null)
        {
            if (valueConverter == null) throw new ArgumentNullException(nameof(valueConverter));
            return new DataBindingValueAccessTransformer<TIn, TOut>(source, 
                v => (TOut) valueConverter.Convert(v, typeof (TOut), parameter, language ?? System.Globalization.CultureInfo.CurrentCulture.Name),
                v => (TIn) valueConverter.ConvertBack(v, typeof (TIn), parameter, language ?? System.Globalization.CultureInfo.CurrentCulture.Name));
        }

        /// <summary>
        /// Add fallback value.
        /// </summary>
        /// <typeparam name="T">Value type.</typeparam>
        /// <param name="source">Source data accessor.</param>
        /// <param name="fallbackValue">Fallback value.</param>
        /// <returns></returns>
        public static IDataBindingAccessor<T> Fallback<T>(this IDataBindingAccessor<T> source,
            T fallbackValue = default(T))
        {
            return new DataBindingFallbackValueAccess<T>(source, fallbackValue);
        }
    }
}