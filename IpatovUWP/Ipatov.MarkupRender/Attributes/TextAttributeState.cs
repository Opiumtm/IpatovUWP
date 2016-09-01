using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Ipatov.MarkupRender
{
    /// <summary>
    /// Text attributes state.
    /// </summary>
    public sealed class TextAttributeState : ITextAttributeState
    {
        /// <summary>
        /// Current snapshot.
        /// </summary>
        private readonly Dictionary<string, ITextAttribute> _current = new Dictionary<string, ITextAttribute>(StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<string, Stack<ITextAttribute>> _all = new Dictionary<string, Stack<ITextAttribute>>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Constructor.
        /// </summary>
        public TextAttributeState()
        {
        }

        /// <summary>
        /// Cloning constructor.
        /// </summary>
        /// <param name="toClone">Object to clone.</param>
        internal TextAttributeState(TextAttributeState toClone)
        {
            if (toClone != null)
            {
                foreach (var kv in toClone._all)
                {
                    var arr = kv.Value.ToArray();
                    foreach (var a in arr)
                    {
                        Add(a);
                    }
                }
            }
        }

        /// <summary>
        /// Get attributes snapshot.
        /// </summary>
        /// <returns>Snapshot.</returns>
        public IReadOnlyDictionary<string, ITextAttribute> GetSnapshot()
        {
            var r = new Dictionary<string, ITextAttribute>(StringComparer.OrdinalIgnoreCase);
            foreach (var kv in _current)
            {
                r[kv.Key] = kv.Value;
            }
            return new ReadOnlyDictionary<string, ITextAttribute>(r);
        }

        /// <summary>
        /// Add attribute.
        /// </summary>
        /// <param name="attribute">Attribute.</param>
        public void Add(ITextAttribute attribute)
        {
            if (attribute?.Name == null)
            {
                return;
            }
            if (!_all.ContainsKey(attribute.Name))
            {
                _all[attribute.Name] = new Stack<ITextAttribute>();
            }
            _all[attribute.Name].Push(attribute);
            _current[attribute.Name] = attribute;
        }

        /// <summary>
        /// Remove attribute.
        /// </summary>
        /// <param name="attributeName">Attribute name.</param>
        public void Remove(string attributeName)
        {
            if (attributeName == null)
            {
                return;
            }
            ITextAttribute top = null;
            if (_all.ContainsKey(attributeName))
            {
                var stack = _all[attributeName];
                if (stack.Count > 0)
                {
                    stack.Pop();
                }
                if (stack.Count > 0)
                {
                    top = stack.Peek();
                }
                else
                {
                    _all.Remove(attributeName);
                }
            }
            if (top != null)
            {
                _current[top.Name] = top;
            }
            else
            {
                _current.Remove(attributeName);
            }
        }

        /// <summary>
        /// Clear all attributes.
        /// </summary>
        public void Clear()
        {
            _current.Clear();
            _all.Clear();
        }

        /// <summary>
        /// Clone attribute state.
        /// </summary>
        /// <returns>Exact clone.</returns>
        public ITextAttributeState Clone()
        {
            return new TextAttributeState(this);
        }
    }
}