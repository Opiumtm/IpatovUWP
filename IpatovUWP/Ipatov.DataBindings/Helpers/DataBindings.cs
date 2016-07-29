using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Ipatov.DataBindings.Helpers
{
    /// <summary>
    /// Data bindings.
    /// </summary>
    public static class DataBindings
    {
        /// <summary>
        /// Bind source to target.
        /// </summary>
        /// <typeparam name="T">Type of binded value.</typeparam>
        /// <param name="source">Data binding source.</param>
        /// <param name="target">Data binding target.</param>
        /// <param name="triggerOnBind">Trigger binding immediately.</param>
        /// <returns>Data binding.</returns>
        public static IDataBinding Bind<T>(this IDataBindingSource<T> source, IDataBindingTarget<T> target, bool triggerOnBind = true)
        {
            return new DataBinding<T, T>(source, target, PassthroughConverter<T>.Instance, triggerOnBind);
        }

        /// <summary>
        /// Bind target to source.
        /// </summary>
        /// <typeparam name="T">Type of binded value.</typeparam>
        /// <param name="target">Data binding target.</param>
        /// <param name="source">Data binding source.</param>
        /// <param name="triggerOnBind">Trigger binding immediately.</param>
        /// <returns>Data binding.</returns>
        public static IDataBinding BindTo<T>(this IDataBindingTarget<T> target, IDataBindingSource<T> source, bool triggerOnBind = true)
        {
            return new DataBinding<T, T>(source, target, PassthroughConverter<T>.Instance, triggerOnBind);
        }

        /// <summary>
        /// Bind source to target.
        /// </summary>
        /// <typeparam name="TIn">Source data type.</typeparam>
        /// <typeparam name="TOut">Target data type.</typeparam>
        /// <param name="source">Data binding source.</param>
        /// <param name="target">Data binding target.</param>
        /// <param name="converter">Data binding converter.</param>
        /// <param name="triggerOnBind">Trigger binding immediately.</param>
        /// <returns>Data binding.</returns>
        public static IDataBinding Bind<TIn, TOut>(this IDataBindingSource<TIn> source, IDataBindingTarget<TOut> target, IDataBindingConverter<TIn, TOut> converter, bool triggerOnBind = true)
        {
            return new DataBinding<TIn, TOut>(source, target, converter, triggerOnBind);
        }

        /// <summary>
        /// Bind target to source.
        /// </summary>
        /// <typeparam name="TIn">Source data type.</typeparam>
        /// <typeparam name="TOut">Target data type.</typeparam>
        /// <param name="target">Data binding target.</param>
        /// <param name="source">Data binding source.</param>
        /// <param name="converter">Data binding converter.</param>
        /// <param name="triggerOnBind">Trigger binding immediately.</param>
        /// <returns>Data binding.</returns>
        public static IDataBinding BindTo<TIn, TOut>(this IDataBindingTarget<TOut> target, IDataBindingSource<TIn> source, IDataBindingConverter<TIn, TOut> converter,  bool triggerOnBind = true)
        {
            return new DataBinding<TIn, TOut>(source, target, converter, triggerOnBind);
        }

        /// <summary>
        /// Bind source to target.
        /// </summary>
        /// <typeparam name="T">Type of binded value.</typeparam>
        /// <param name="source">Data binding source.</param>
        /// <param name="path">Data binding path.</param>
        /// <param name="target">Data binding target.</param>
        /// <param name="fallbackValue">Fallback value.</param>
        /// <param name="triggerOnBind">Trigger binding immediately.</param>
        /// <returns>Data binding.</returns>
        public static IDataBinding Bind<T>(this IDataBindable source, DataBindingPath path, IDataBindingTarget<T> target, T fallbackValue = default (T), bool triggerOnBind = true)
        {
            return new DataBinding<T, T>(source.ResolveBindable(path, fallbackValue), target, PassthroughConverter<T>.Instance, triggerOnBind);
        }

        /// <summary>
        /// Bind target to source.
        /// </summary>
        /// <typeparam name="T">Type of binded value.</typeparam>
        /// <param name="target">Data binding target.</param>
        /// <param name="source">Data binding source.</param>
        /// <param name="path">Data binding path.</param>
        /// <param name="fallbackValue">Fallback value.</param>
        /// <param name="triggerOnBind">Trigger binding immediately.</param>
        /// <returns>Data binding.</returns>
        public static IDataBinding BindTo<T>(this IDataBindingTarget<T> target, IDataBindable source, DataBindingPath path, T fallbackValue = default(T), bool triggerOnBind = true)
        {
            return new DataBinding<T, T>(source.ResolveBindable(path, fallbackValue), target, PassthroughConverter<T>.Instance, triggerOnBind);
        }

        /// <summary>
        /// Bind source to target.
        /// </summary>
        /// <typeparam name="TIn">Source data type.</typeparam>
        /// <typeparam name="TOut">Target data type.</typeparam>
        /// <param name="source">Data binding source.</param>
        /// <param name="path">Data binding path.</param>
        /// <param name="target">Data binding target.</param>
        /// <param name="converter">Data binding converter.</param>
        /// <param name="fallbackValue">Fallback value.</param>
        /// <param name="triggerOnBind">Trigger binding immediately.</param>
        /// <returns>Data binding.</returns>
        public static IDataBinding Bind<TIn, TOut>(this IDataBindable source, DataBindingPath path, IDataBindingTarget<TOut> target, IDataBindingConverter<TIn, TOut> converter, TIn fallbackValue = default (TIn), bool triggerOnBind = true)
        {
            return new DataBinding<TIn, TOut>(source.ResolveBindable(path, fallbackValue), target, converter, triggerOnBind);
        }

        /// <summary>
        /// Bind target to source.
        /// </summary>
        /// <typeparam name="TIn">Source data type.</typeparam>
        /// <typeparam name="TOut">Target data type.</typeparam>
        /// <param name="target">Data binding target.</param>
        /// <param name="source">Data binding source.</param>
        /// <param name="path">Data binding path.</param>
        /// <param name="converter">Data binding converter.</param>
        /// <param name="fallbackValue">Fallback value.</param>
        /// <param name="triggerOnBind">Trigger binding immediately.</param>
        /// <returns>Data binding.</returns>
        public static IDataBinding BindTo<TIn, TOut>(this IDataBindingTarget<TOut> target, IDataBindable source, DataBindingPath path, IDataBindingConverter<TIn, TOut> converter, TIn fallbackValue = default(TIn), bool triggerOnBind = true)
        {
            return new DataBinding<TIn, TOut>(source.ResolveBindable(path, fallbackValue), target, converter, triggerOnBind);
        }

        /// <summary>
        /// Wrap value converter as data binding converter.
        /// </summary>
        /// <typeparam name="TIn">Input value type.</typeparam>
        /// <typeparam name="TOut">Output value type.</typeparam>
        /// <param name="converter">Value converter.</param>
        /// <param name="parameter">Parameter.</param>
        /// <param name="fallbackValue">Fallback value.</param>
        /// <param name="language">Language.</param>
        /// <returns></returns>
        public static IDataBindingConverter<TIn, TOut> ToBindingConverter<TIn, TOut>(this IValueConverter converter, object parameter = null, TOut fallbackValue = default (TOut), string language = null)
        {
            return new ValueConverterWrapper<TIn, TOut>(converter, parameter, fallbackValue, language);
        }

        /// <summary>
        /// Resolve data bindable.
        /// </summary>
        /// <param name="bindable">Bindable object.</param>
        /// <param name="path">Path.</param>
        /// <returns>Leaf bindable object.</returns>
        private static Tuple<IDataBindable, string> ResolveBindableLeaf(this IDataBindable bindable, DataBindingPath? path)
        {
            if (path?.Part == null)
            {
                return null;
            }
            if (path.Value.Length == 1)
            {
                return new Tuple<IDataBindable, string>(bindable, path.Value.Part);
            }
            var child = bindable.GetChildBindable(path.Value.Part);
            return child?.ResolveBindableLeaf(path.Value.Next());
        }

        /// <summary>
        /// Resolve data bindable.
        /// </summary>
        /// <param name="bindable">Bindable object.</param>
        /// <param name="path">Path.</param>
        /// <param name="fallbackValue">Fallback value.</param>
        /// <returns>Binding source.</returns>
        public static IDataBindingSource<T> ResolveBindable<T>(this IDataBindable bindable, DataBindingPath path, T fallbackValue = default(T))
        {
            var leaf = bindable.ResolveBindableLeaf(path);
            if (leaf == null)
            {
                return new StubBindingSource<T>(fallbackValue);
            }
            return leaf.Item1?.GetSource<T>(leaf.Item2) ?? new StubBindingSource<T>(fallbackValue);
        }

        /// <summary>
        /// Wrap object as action target.
        /// </summary>
        /// <typeparam name="TTarget">Object type.</typeparam>
        /// <typeparam name="T">Binding value type.</typeparam>
        /// <param name="obj">Object.</param>
        /// <param name="action">Action.</param>
        /// <returns></returns>
        public static IDataBindingTarget<T> AsActionTarget<TTarget, T>(this TTarget obj, Action<TTarget, T> action) where TTarget : class
        {
            return new ObjectActionTarget<T,TTarget>(new HardReferenceWrapper<TTarget>(obj), action);
        }

        /// <summary>
        /// Wrap object as weak action target.
        /// </summary>
        /// <typeparam name="TTarget">Object type.</typeparam>
        /// <typeparam name="T">Binding value type.</typeparam>
        /// <param name="obj">Object.</param>
        /// <param name="action">Action.</param>
        /// <returns></returns>
        public static IDataBindingTarget<T> AsWeakActionTarget<TTarget, T>(this TTarget obj, Action<TTarget, T> action) where TTarget : class
        {
            return new ObjectActionTarget<T, TTarget>(new WeakReferenceWrapper<TTarget>(obj), action);
        }

        /// <summary>
        /// Wrap dependency object as action target.
        /// </summary>
        /// <typeparam name="TTarget">Object type.</typeparam>
        /// <typeparam name="T">Binding value type.</typeparam>
        /// <param name="obj">Object.</param>
        /// <param name="action">Action.</param>
        /// <returns></returns>
        public static IDataBindingTarget<T> AsDependencyActionTarget<TTarget, T>(this TTarget obj, Action<TTarget, T> action) where TTarget : DependencyObject
        {
            return new DependencyObjectActionTarget<T, TTarget>(new HardReferenceWrapper<TTarget>(obj), action);
        }

        /// <summary>
        /// Wrap dependency object as weak action target.
        /// </summary>
        /// <typeparam name="TTarget">Object type.</typeparam>
        /// <typeparam name="T">Binding value type.</typeparam>
        /// <param name="obj">Object.</param>
        /// <param name="action">Action.</param>
        /// <returns></returns>
        public static IDataBindingTarget<T> AsWeakDependencyActionTarget<TTarget, T>(this TTarget obj, Action<TTarget, T> action) where TTarget : DependencyObject
        {
            return new DependencyObjectActionTarget<T, TTarget>(new WeakReferenceWrapper<TTarget>(obj), action);
        }
    }
}