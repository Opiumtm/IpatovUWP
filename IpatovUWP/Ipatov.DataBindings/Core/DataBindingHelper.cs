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
        /// <returns>Data binding.</returns>
        public static IDataBinding BindTo<T>(this IDataBindingSource<T> source, IDataBindingValueSetter<T> target)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (target == null) throw new ArgumentNullException(nameof(target));
            return new DataBinding<T>(source, target, source);
        }

        /// <summary>
        /// Bind action to event.
        /// </summary>
        /// <typeparam name="T">Bound object type.</typeparam>
        /// <param name="source">Event source.</param>
        /// <param name="action">Action.</param>
        /// <returns>Data binding.</returns>
        public static IDataBinding Action<T>(this IDataBindingEventSource source, Action action)
        {
            return new ActionBinding(source, action);
        }

        /// <summary>
        /// Bind action to event.
        /// </summary>
        /// <typeparam name="T">Bound object type.</typeparam>
        /// <param name="source">Event source.</param>
        /// <param name="action">Action.</param>
        /// <returns>Data binding.</returns>
        public static IDataBinding Action<T>(this IDataBindingSource<T> source, Action<T> action)
        {
            return new BoundActionBinding<T>(source, action);
        }

        /// <summary>
        /// Get child event source.
        /// </summary>
        /// <typeparam name="TParent">Type of parent.</typeparam>
        /// <typeparam name="T">Type of child.</typeparam>
        /// <param name="parent">Parent event source.</param>
        /// <param name="getEventSource">Function to get event source from bound object.</param>
        /// <returns>Event source.</returns>
        public static IDataBindingSource<T> Child<TParent, T>(this IDataBindingSource<TParent> parent,
            Func<TParent, IDataBindingSource<T>> getEventSource)
        {
            return new DataBindingChildSource<TParent, T>(parent, getEventSource);
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
        public static IDataBindingSource<T> Child<TParent, T>(this IDataBindingSource<TParent> parent,
            Func<TParent, T> getBoundObject, string propertyName) where TParent : class, INotifyPropertyChanged
        {
            return new DataBindingChildSource<TParent, T>(parent, obj => obj.OnProperty(getBoundObject, propertyName));
        }

        /// <summary>
        /// Get child event source.
        /// </summary>
        /// <typeparam name="TParent">Type of parent.</typeparam>
        /// <typeparam name="T">Type of child.</typeparam>
        /// <param name="parent">Parent event source.</param>
        /// <param name="property">Dependency property.</param>
        /// <returns>Event source.</returns>
        public static IDataBindingSource<T> Child<TParent, T>(this IDataBindingSource<TParent> parent,
            DependencyProperty property) where TParent : DependencyObject
        {
            return new DataBindingChildSource<TParent, T>(parent, obj => obj.OnProperty<TParent, T>(property));
        }

        /// <summary>
        /// Trigger events on property change.
        /// </summary>
        /// <typeparam name="TSrc">Bound object type.</typeparam>
        /// <typeparam name="T">Value type.</typeparam>
        /// <param name="source">Source object.</param>
        /// <param name="getValue">Get value from object.</param>
        /// <param name="propertyName">Property name.</param>
        /// <returns>Event source.</returns>
        public static IDataBindingSource<T> OnProperty<TSrc, T>(this TSrc source, Func<TSrc, T> getValue, string propertyName) where TSrc : class, INotifyPropertyChanged
        {
            return new DataBindingSource<T>(source.WeakValueAccessor(getValue), new DataBindingPropertyChangedEventSource<TSrc>(source, propertyName));
        }

        /// <summary>
        /// Trigger events on dependency property change.
        /// </summary>
        /// <typeparam name="TSrc">Bound object type.</typeparam>
        /// <typeparam name="T">Value type.</typeparam>
        /// <param name="source">Source object.</param>
        /// <param name="property">Dependency property.</param>
        /// <returns>Event source.</returns>
        public static IDataBindingSource<T> OnProperty<TSrc, T>(this TSrc source, DependencyProperty property)
            where TSrc : DependencyObject
        {
            return new DataBindingSource<T>(source.WeakValueAccessor(v => (T)v.GetValue(property)).OnUiThread(), new DataBindingDependencyPropertyChangedEventSource<TSrc>(source, property));
        }
    }
}