using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Ipatov.BinarySerialization.Reflection
{
    /// <summary>
    /// Reflected property.
    /// </summary>
    public struct ReflectedProperty
    {
        /// <summary>
        /// Property name.
        /// </summary>
        public string PropertyName => _property.Name;

        /// <summary>
        /// Property type.
        /// </summary>
        public Type PropertyType => _property.PropertyType;

        /// <summary>
        /// Object type.
        /// </summary>
        public Type ObjectType => _property.DeclaringType;

        private readonly Delegate _getter;
        private readonly Delegate _setter;
        private readonly PropertyInfo _property;

        public ReflectedProperty(PropertyInfo property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));
            _property = property;
            var getterType = typeof(Func<,>).MakeGenericType(property.DeclaringType, property.PropertyType);
            _getter = property.GetMethod.CreateDelegate(getterType);
            var setterType = typeof(Action<,>).MakeGenericType(property.DeclaringType, property.PropertyType); ;
            _setter = property.SetMethod.CreateDelegate(setterType);
        }

        /// <summary>
        /// Get property value.
        /// </summary>
        /// <typeparam name="TObj">Object type.</typeparam>
        /// <typeparam name="T">Value type.</typeparam>
        /// <param name="source">Source object.</param>
        /// <returns>Value.</returns>
        public T Get<TObj, T>(TObj source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            var d = _getter as Func<TObj, T>;
            if (d == null)
            {
                throw new ArgumentException("Invalid type parameters");
            }
            return d(source);
        }

        /// <summary>
        /// Get property value.
        /// </summary>
        /// <param name="source">Source object.</param>
        /// <returns>Value.</returns>
        public object Get(object source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return _getter.DynamicInvoke(source);
        }

        /// <summary>
        /// Get property value.
        /// </summary>
        /// <typeparam name="TObj">Object type.</typeparam>
        /// <typeparam name="T">Value type.</typeparam>
        /// <param name="source">Source object.</param>
        /// <param name="value">Value.</param>
        /// <returns>Value.</returns>
        public void Set<TObj, T>(TObj source, T value)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            var d = _setter as Action<TObj, T>;
            if (d == null)
            {
                throw new ArgumentException("Invalid type parameters");
            }
            d(source, value);
        }

        /// <summary>
        /// Get property value.
        /// </summary>
        /// <param name="source">Source object.</param>
        /// <param name="value">Value.</param>
        public void Set(object source, object value)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            _setter.DynamicInvoke(source, value);
        }
    }
}