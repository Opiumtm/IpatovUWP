using System;
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
        public readonly string PropertyName;

        /// <summary>
        /// Property type.
        /// </summary>
        public readonly Type PropertyType;

        /// <summary>
        /// Object type.
        /// </summary>
        public readonly Type ObjectType;

        private readonly Delegate _getter;
        private readonly Delegate _setter;

        public ReflectedProperty(PropertyInfo property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));
            PropertyName = property.Name;
            PropertyType = property.PropertyType;
            ObjectType = property.DeclaringType;
            var getterType = typeof(Func<,>).MakeGenericType(ObjectType, PropertyType);
            _getter = property.GetMethod.CreateDelegate(getterType);
            var setterType = typeof(Action<,>).MakeGenericType(ObjectType, PropertyType); ;
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