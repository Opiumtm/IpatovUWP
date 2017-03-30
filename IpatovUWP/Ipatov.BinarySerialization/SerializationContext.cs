using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Ipatov.BinarySerialization
{
    /// <summary>
    /// Serialization context.
    /// </summary>
    public sealed class SerializationContext
    {
        private readonly Dictionary<int, object> _objects;
        private readonly Dictionary<object, int> _index;
        private readonly IReadOnlyDictionary<Type, IExternalSerializationTokensProvider> _tokensProviders;
        private readonly Stack<Type> _typeStack = new Stack<Type>();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="tokensProviders">Serialization tokens providers.</param>
        public SerializationContext(IReadOnlyDictionary<Type, IExternalSerializationTokensProvider> tokensProviders)
        {
            if (tokensProviders == null) throw new ArgumentNullException(nameof(tokensProviders));
            _objects = new Dictionary<int, object>();
            _index = new Dictionary<object, int>();
            _tokensProviders = tokensProviders;            
        }

        /// <summary>
        /// Add reference.
        /// </summary>
        /// <param name="obj">Object.</param>
        /// <returns>Object index.</returns>
        public int AddReference(object obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            if (_index.ContainsKey(obj))
            {
                return _index[obj];
            }
            var m = _objects.Keys.DefaultIfEmpty(-1).Max();
            var idx = m + 1;
            _objects[idx] = obj;
            _index[obj] = idx;
            return idx;
        }

        /// <summary>
        /// Add reference.
        /// </summary>
        /// <param name="idx">Index.</param>
        /// <param name="obj">Object.</param>
        /// <returns>Object index.</returns>
        public void AddReference(int idx, object obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            _objects[idx] = obj;
            _index[obj] = idx;
        }

        /// <summary>
        /// Get object reference.
        /// </summary>
        /// <param name="index">Object index.</param>
        /// <returns>Object</returns>
        public object GetReference(int index)
        {
            return _objects.ContainsKey(index) ? _objects[index] : null;
        }

        /// <summary>
        /// Test if reference already exists.
        /// </summary>
        /// <param name="obj">Object.</param>
        /// <returns>Index or null if not exists.</returns>
        public int? IsReferenced(object obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            return _index.ContainsKey(obj) ? (int?)_index[obj] : null;
        }

        /// <summary>
        /// Get serialization tokens provider.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <returns>Tokens provider.</returns>
        public IExternalSerializationTokensProvider<T> GetTokensProvider<T>()
        {
            return GetTokensProvider(typeof(T)) as IExternalSerializationTokensProvider<T>;
        }

        /// <summary>
        /// Get serialization tokens provider.
        /// </summary>
        /// <param name="type">Object type.</param>
        /// <returns>Tokens provider.</returns>
        public IExternalSerializationTokensProvider GetTokensProvider(Type type)
        {
            if (_tokensProviders.ContainsKey(type))
            {
                return _tokensProviders[type];
            }
            var known = this.GetKnownTokenProviders();
            var p = known?.GetKnownTokenProviders().GetProvider(type);
            if (p != null)
            {
                return p;
            }
            return SerializationHelpers.GetDefaultTokensProvider(type);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void SerializingComplexType(Type t)
        {
            if (t == null) throw new ArgumentNullException(nameof(t));
            _typeStack.Push(t);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void FinishedSerializingComplexType()
        {
            _typeStack.Pop();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Type[] GetTypeStack()
        {
            return _typeStack.ToArray();
        }
    }
}