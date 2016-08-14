using System;
using System.ComponentModel;
using Windows.UI.Xaml;

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
        /// <returns>Binding source.</returns>
        public static IDataBindingSource<T> AsBindingSource<T>(this IDataBindingValueGetter<T> source, IDataBindingEventSource<T> eventSource = null)
        {
            return new DataBindingSource<T>(source, eventSource);
        }

        /// <summary>
        /// Bind data source to traget.
        /// </summary>
        /// <typeparam name="T">Value type.</typeparam>
        /// <param name="source">Data binding source.</param>
        /// <param name="target">Data binding target.</param>
        /// <returns>Data binding.</returns>
        public static IDataBinding BindTo<T>(this IDataBindingSource<T> source, IDataBindingValueSetter<T> target)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (target == null) throw new ArgumentNullException(nameof(target));
            return new DataBinding<T>(source, target, source);
        }

        /// <summary>
        /// Get child event source.
        /// </summary>
        /// <typeparam name="TParent">Type of parent.</typeparam>
        /// <typeparam name="T">Type of child.</typeparam>
        /// <param name="parent">Parent event source.</param>
        /// <param name="getBoundObject">Function to get child bound object.</param>
        /// <param name="getEventSource">Function to get event source from bound object.</param>
        /// <returns>Event source.</returns>
        public static IDataBindingEventSource<T> Child<TParent, T>(this IDataBindingEventSource<TParent> parent,
            Func<TParent, T> getBoundObject, Func<T, IDataBindingEventSource<T>> getEventSource)
        {
            return new DataBindingEventSourceWrapper<TParent,T>(parent, getBoundObject, getEventSource);
        }

        /// <summary>
        /// Get child event source.
        /// </summary>
        /// <typeparam name="TParent">Type of parent.</typeparam>
        /// <typeparam name="T">Type of child.</typeparam>
        /// <param name="parent">Parent event source.</param>
        /// <param name="getBoundObject">Function to get child bound object.</param>
        /// <param name="propertyName">Property name.</param>
        /// <returns>Event source.</returns>
        public static IDataBindingEventSource<T> Child<TParent, T>(this IDataBindingEventSource<TParent> parent,
            Func<TParent, T> getBoundObject, string propertyName) where T : class, INotifyPropertyChanged
        {
            return new DataBindingEventSourceWrapper<TParent, T>(parent, getBoundObject, obj => obj.OnProperty(propertyName));
        }

        /// <summary>
        /// Get child event source.
        /// </summary>
        /// <typeparam name="TParent">Type of parent.</typeparam>
        /// <typeparam name="T">Type of child.</typeparam>
        /// <param name="parent">Parent event source.</param>
        /// <param name="getBoundObject">Function to get child bound object.</param>
        /// <param name="property">Dependency property.</param>
        /// <returns>Event source.</returns>
        public static IDataBindingEventSource<T> Child<TParent, T>(this IDataBindingEventSource<TParent> parent,
            Func<TParent, T> getBoundObject, DependencyProperty property) where T : DependencyObject
        {
            return new DataBindingEventSourceWrapper<TParent, T>(parent, getBoundObject, obj => obj.OnProperty(property));
        }

        /// <summary>
        /// Trigger events on property change.
        /// </summary>
        /// <typeparam name="T">Bound object type.</typeparam>
        /// <param name="source">Source object.</param>
        /// <param name="propertyName">Property name.</param>
        /// <returns>Event source.</returns>
        public static IDataBindingEventSource<T> OnProperty<T>(this T source, string propertyName) where T : class, INotifyPropertyChanged
        {
            return new DataBindingPropertyChangedEventSource<T>(source, propertyName);
        }

        /// <summary>
        /// Trigger events on dependency property change.
        /// </summary>
        /// <typeparam name="T">Bound object type.</typeparam>
        /// <param name="source">Source object.</param>
        /// <param name="property">Dependency property.</param>
        /// <returns>Event source.</returns>
        public static IDataBindingEventSource<T> OnProperty<T>(this T source, DependencyProperty property)
            where T : DependencyObject
        {
            return new DataBindingDependencyPropertyChangedEventSource<T>(source, property);
        }
    }
}