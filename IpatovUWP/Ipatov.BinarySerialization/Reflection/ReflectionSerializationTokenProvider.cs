using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ipatov.BinarySerialization.Reflection
{
    /// <summary>
    /// Serialization token provider based on reflection.
    /// </summary>
    /// <typeparam name="T">Objet type..</typeparam>
    public sealed class ReflectionSerializationTokenProvider<T> : IExternalSerializationTokensProvider<T>
    {
        private static readonly Dictionary<Type, PropertyFuncs[]> Properties;

        /// <summary>
        /// Get tokens for object properties.
        /// </summary>
        /// <param name="source">Source object.</param>
        /// <param name="context">Serialization context.</param>
        /// <returns>Tokens.</returns>
        public IEnumerable<SerializationProperty> GetProperties(T source, SerializationContext context)
        {
            var t = typeof(T);
            var p = t.GetRuntimeProperty("Item");
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Create object and fill its data from serialization tokens.
        /// </summary>
        /// <param name="properties">Serialization tokens.</param>
        /// <param name="context">Serialization context.</param>
        /// <returns>Object.</returns>
        public T CreateObject<TEnum>(TEnum properties, SerializationContext context) where TEnum : IEnumerable<SerializationProperty>
        {
            throw new System.NotImplementedException();
        }

        private static PropertyFuncs[] GetProperties()
        {
            return null;
        }

        private struct PropertyFuncs
        {
            public string Name;
            public Type DataType;
            public Delegate Getter;
            public Delegate Setter;

            public object GetValue(object source)
            {
                return Getter.DynamicInvoke(source);
            }

            public void SetValue(object source, object value)
            {
                Setter.DynamicInvoke(source, value);
            }
        }
    }
}