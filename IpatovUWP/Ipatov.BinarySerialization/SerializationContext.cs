using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Ipatov.BinarySerialization.Internals;
using Ipatov.BinarySerialization.Reflection;
using Ipatov.BinarySerialization.TokenProviders;

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
            _subclasses = new LazyDictionary<SubsclassDesc, IExternalSerializationTokensProvider>(GetSubclassWrapper);
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
            return GetDefaultTokensProvider(type);
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

        private readonly LazyDictionary<SubsclassDesc, IExternalSerializationTokensProvider> _subclasses;

        private IExternalSerializationTokensProvider GetSubclassWrapper(SubsclassDesc subsclassDesc)
        {
            IExternalSerializationTokensProvider r = null;
            var parentType = subsclassDesc.ParentType;
            var subclassType = subsclassDesc.SubclassType;
            if (subclassType.GetTypeInfo().IsSubclassOf(parentType))
            {
                var pt = typeof(SubclassExternalSerializationTokensProvider<,>).MakeGenericType(parentType, subclassType);
                var st = typeof(IExternalSerializationTokensProvider<>).MakeGenericType(subclassType);
                r = pt.GetTypeInfo().DeclaredConstructors.FirstOrDefault(c =>
                {
                    var p = c.GetParameters();
                    return p.Length == 1 && p[0].ParameterType == st && c.IsPublic;
                })?.Invoke(new object[] { GetTokensProvider(subclassType) }) as IExternalSerializationTokensProvider;
            }
            return r;
        }

        /// <summary>
        /// Get tokens provider for complex type.
        /// </summary>
        /// <typeparam name="T">Requested complex type.</typeparam>
        /// <param name="sourceType">Actual (maybe subclass) type.</param>
        /// <returns>Tokens provider.</returns>
        public IExternalSerializationTokensProvider<T> GetComplexTypeTokensProvider<T>(Type sourceType)
        {
            if (sourceType == typeof(T))
            {
                return GetTokensProvider<T>();
            }
            return _subclasses[new SubsclassDesc() {ParentType = typeof(T), SubclassType = sourceType}] as IExternalSerializationTokensProvider<T>;
        }

        private static readonly LazyDictionary<Type, IExternalSerializationTokensProvider> Wrappers = new LazyDictionary<Type, IExternalSerializationTokensProvider>(DoGetDefaultTokensProvider);

        private static IExternalSerializationTokensProvider DoGetDefaultTokensProvider(Type type)
        {
            IExternalSerializationTokensProvider r = null;
            var tinfo = type.GetTypeInfo();
            var gtinfo = type.IsConstructedGenericType ? type.GetGenericTypeDefinition() : null;
            var haveParameterlessContstructor = tinfo.DeclaredConstructors.Any(c => c.IsPublic && c.GetParameters().Length == 0);

            if (type == typeof(Uri))
            {
                r = new UriTokensProvider();
            }

            if (r == null)
            {
                if (type.IsArray)
                {
                    var pt = typeof(ArrayTokensProvider<>).MakeGenericType(tinfo.GetElementType());
                    r = CreateExternalSerializationTokensProvider(pt);
                }
            }

            if (r == null)
            {
                if (gtinfo == typeof(KeyValuePair<,>))
                {
                    var pt = typeof(KeyValuePairTokensProvider<,>).MakeGenericType(tinfo.GenericTypeArguments[0], tinfo.GenericTypeArguments[1]);
                    r = CreateExternalSerializationTokensProvider(pt);
                }
            }

            if (r == null)
            {
                if (gtinfo == typeof(Dictionary<,>) || gtinfo == typeof(SortedDictionary<,>))
                {
                    var pt = typeof(DictionaryTokensProvider<,,>).MakeGenericType(tinfo.GenericTypeArguments[0], tinfo.GenericTypeArguments[1], type);
                    r = CreateExternalSerializationTokensProvider(pt);
                }
            }

            if (r == null)
            {
                if (gtinfo == typeof(List<>) || gtinfo == typeof(HashSet<>) || gtinfo == typeof(SortedSet<>))
                {
                    var pt = typeof(CollectionTokensProvider<,>).MakeGenericType(type, tinfo.GenericTypeArguments[0]);
                    r = CreateExternalSerializationTokensProvider(pt);
                }
            }

            if (r == null)
            {
                // for wrapper
                if (tinfo.ImplementedInterfaces.Any(t => t == typeof(ISerializationTokensProvider)) && haveParameterlessContstructor)
                {
                    var pt = typeof(SerializationTokensProviderWrapper<>).MakeGenericType(type);
                    r = CreateExternalSerializationTokensProvider(pt);
                }
            }
            return r;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IExternalSerializationTokensProvider CreateExternalSerializationTokensProvider(Type pt)
        {
            return pt.GetTypeInfo().DeclaredConstructors.FirstOrDefault(c => c.GetParameters().Length == 0 && c.IsPublic)?.Invoke(null) as IExternalSerializationTokensProvider;
        }

        /// <summary>
        /// Get tokens provider wrapper for type that implements <see cref="ISerializationTokensProvider"/>.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <returns>Wrapper or none if not found.</returns>
        public static IExternalSerializationTokensProvider GetDefaultTokensProvider(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            return Wrappers[type];
        }

        private static readonly Dictionary<Type, IKnownTokenProviders> KnownTokenProviders = new Dictionary<Type, IKnownTokenProviders>();

        /// <summary>
        /// Get known token providers for type.
        /// </summary>
        /// <returns>Known token providers.</returns>
        public IKnownTokenProviders GetKnownTokenProviders()
        {
            var stack = GetTypeStack().Select(GetKnownTokenProviders).ToArray();
            return new CombinedTokenProvidersInfo(stack);
        }

        /// <summary>
        /// Get known token providers for type.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <returns>Known token providers.</returns>
        public static IKnownTokenProviders GetKnownTokenProviders(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            lock (KnownTokenProviders)
            {
                if (!KnownTokenProviders.ContainsKey(type))
                {
                    var attrs = type.GetTypeInfo().GetCustomAttributes<KnownTokenProvidersAttribute>().Select(a => a.GetProviders()).ToArray();
                    KnownTokenProviders[type] = new CombinedTokenProvidersInfo(attrs);
                }
                return KnownTokenProviders[type];
            }
        }


        private struct SubsclassDesc : IEquatable<SubsclassDesc>
        {
            public Type ParentType;
            public Type SubclassType;

            public bool Equals(SubsclassDesc other)
            {
                return ParentType.Equals(other.ParentType) && SubclassType.Equals(other.SubclassType);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                return obj is SubsclassDesc && Equals((SubsclassDesc) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (ParentType.GetHashCode() * 397) ^ SubclassType.GetHashCode();
                }
            }

            public static bool operator ==(SubsclassDesc left, SubsclassDesc right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(SubsclassDesc left, SubsclassDesc right)
            {
                return !left.Equals(right);
            }
        }
    }
}