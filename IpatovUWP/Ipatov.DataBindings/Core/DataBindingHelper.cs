using System;
using System.ComponentModel;

namespace Ipatov.DataBindings
{
    /// <summary>
    /// Data binding helper.
    /// </summary>
    public static class DataBindingHelper
    {
        /// <summary>
        /// Add event source to value getter.
        /// </summary>
        /// <typeparam name="T">Value type.</typeparam>
        /// <param name="source">Value getter.</param>
        /// <param name="eventSource">Event source.</param>
        /// <returns></returns>
        public static IDataBindingSource<T> AsBindingSource<T>(this IDataBindingValueGetter<T> source, IDataBindingEventSource eventSource = null)
        {
            return new DataBindingSource<T>(source, eventSource);
        }

        /// <summary>
        /// Bind data source to traget.
        /// </summary>
        /// <typeparam name="T">Value type.</typeparam>
        /// <param name="source">Data binding source.</param>
        /// <param name="target">Data binding target.</param>
        /// <returns></returns>
        public static IDataBinding BindTo<T>(this IDataBindingSource<T> source, IDataBindingValueSetter<T> target)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (target == null) throw new ArgumentNullException(nameof(target));
            return new DataBinding<T>(source, target, source);
        }

        /// <summary>
        /// Get child event source.
        /// </summary>
        /// <param name="source">Parent event source.</param>
        /// <param name="getChildFunc">Child event source factory.</param>
        /// <returns>Event source.</returns>
        public static IDataBindingEventSource OnChild(this IDataBindingEventSource source, Func<object, IDataBindingEventSource> getChildFunc)
        {
            return new DataBindingChildEventSource(source, () => getChildFunc?.Invoke(source.BoundObject));
        }

        /// <summary>
        /// Get child event source for INotifyPropertyChanged.
        /// </summary>
        /// <param name="source">Source.</param>
        /// <param name="propertyName">Property name.</param>
        /// <returns>Event source.</returns>
        public static IDataBindingEventSource OnChildPropertyChanged(this IDataBindingEventSource source, string propertyName)
        {
            return new DataBindingChildEventSource(source, () =>
            {
                var ps = source.BoundObject as INotifyPropertyChanged;
                if (ps == null)
                {
                    return null;
                }
                return new DataBindingPropertyChangedEventSource(ps, propertyName);
            });
        }
    }
}