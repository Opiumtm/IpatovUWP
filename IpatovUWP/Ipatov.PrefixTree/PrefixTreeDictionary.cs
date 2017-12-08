using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Ipatov.DataStructures
{
    public class PrefixTreeDictionary<TKey, TKeyElement, TValue, TKeyEnum> : IDictionary<TKey, TValue>
        where TKeyEnum : struct, IKeyElementsEnumerator<TKeyElement>
    {
        private readonly IComparer<TKeyElement> _comparer;
        private readonly IPrefixTreeKeyContext<TKey, TKeyElement, TKeyEnum> _keyContext;

        public PrefixTreeDictionary(IPrefixTreeKeyContext<TKey, TKeyElement, TKeyEnum> context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            _comparer = context.KeyElementComparer ?? Comparer<TKeyElement>.Default;
            _keyContext = context;
            _rootNode = new NodeBase(_comparer);
            _count = 0;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return EnumAll(true).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            AddOrUpdate(item.Key, item.Value, true);
        }

        public void Clear()
        {
            _rootNode = new NodeBase(_comparer);
            _count = 0;
            DataUpdated();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            var getter = _keyContext.Enumerate(item.Key);
            var ln = FindLeafRecursive(_rootNode, ref getter);
            if (ln == null)
            {
                return false;
            }
            return EqualityComparer<TValue>.Default.Equals(item.Value, ln.Value);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            foreach (var v in EnumAll(false))
            {
                array[arrayIndex] = v;
                arrayIndex++;
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            var keyGetter = _keyContext.Enumerate(item.Key);
            var node = FindNodeRecursive(_rootNode, ref keyGetter);
            if (!node.FullEquality || node.Node.LeafNode == null)
            {
                return false;
            }
            if (!EqualityComparer<TValue>.Default.Equals(node.Node.LeafNode.Value, item.Value))
            {
                return false;
            }
            node.Node.RemoveLeaf();
            ElementRemoved();
            return true;
        }

        public int Count => _count;

        public bool IsReadOnly => false;

        public void Add(TKey key, TValue value)
        {
            AddOrUpdate(key, value, true);
        }

        public bool ContainsKey(TKey key)
        {
            var getter = _keyContext.Enumerate(key);
            var ln = FindLeafRecursive(_rootNode, ref getter);
            return ln != null;
        }

        public bool Remove(TKey key)
        {
            var getter = _keyContext.Enumerate(key);
            var node = FindNodeRecursive(_rootNode, ref getter);
            if (!node.FullEquality || node.Node.LeafNode == null)
            {
                return false;
            }
            node.Node.RemoveLeaf();
            ElementRemoved();
            return true;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            var getter = _keyContext.Enumerate(key);
            var ln = FindLeafRecursive(_rootNode, ref getter);
            if (ln == null)
            {
                value = default(TValue);
                return false;
            }
            value = ln.Value;
            return true;
        }

        public TValue this[TKey key]
        {
            get
            {
                if (TryGetValue(key, out var v))
                {
                    return v;
                }
                throw new KeyNotFoundException();
            }
            set => AddOrUpdate(key, value, false);
        }

        public ICollection<TKey> Keys
        {
            get
            {
                var r = new List<TKey>();
                foreach (var ln in EnumAll(false))
                {
                    r.Add(ln.Key);
                }
                return r;
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                var r = new List<TValue>();
                foreach (var ln in EnumAll(false))
                {
                    r.Add(ln.Value);
                }
                return r;
            }
        }

        private NodeBase _rootNode;

        private int _count;

        private int _changeCounter;

        private void ElementAdded()
        {
            _count++;
            DataUpdated();
        }

        private void ElementRemoved()
        {
            _count--;
            DataUpdated();
        }

        private void AddOrUpdate(TKey key, TValue value, bool throwIfExists)
        {
            var keyGetter = _keyContext.Enumerate(key);
            var node = FindNodeRecursive(_rootNode, ref keyGetter);

            void AddToNewNode()
            {
                var newNode = node.Node.AddNode(node.LastKey.Element);
                MaybeKeyElement<TKeyElement> remaining;
                do
                {
                    remaining = keyGetter.GetNextKeyElement();
                    if (remaining.IsPresent)
                    {
                        newNode.KeyRange.Add(remaining.Element);
                    }
                } while (remaining.IsPresent);
                newNode.LeafNode = new LeafNode(newNode, value);
                ElementAdded();
            }

            if (node.FullEquality)
            {
                if (throwIfExists)
                {
                    throw new InvalidOperationException("Key already present in dictionary");
                }
                var wasNull = node.Node.LeafNode == null;
                node.Node.LeafNode = new LeafNode(node.Node, value);
                if (wasNull)
                {
                    ElementAdded();
                }
                else
                {
                    DataUpdated();
                }
                return;
            }
            if (node.Node is Node n)
            {
                if (node.Split) n.TrySplit(node.RangeRead);
                if (node.LastKey.IsPresent)
                {
                    AddToNewNode();
                }
                else
                {
                    if (n.LeafNode != null)
                    {
                        throw new InvalidOperationException("Internal prefix tree structures corrupted or algorithm failed");
                    }
                    n.LeafNode = new LeafNode(n, value);
                    ElementAdded();
                }
            }
            else if (node.LastKey.IsPresent)
            {
                AddToNewNode();
            }
            else
            {
                throw new InvalidOperationException("Internal prefix tree structures corrupted or algorithm failed");
            }
        }

        private IEnumerable<KeyValuePair<TKey, TValue>> EnumAll(bool protect)
        {
            var changeTag = Interlocked.CompareExchange(ref _changeCounter, 0, 0);

            IEnumerable<KeyValuePair<TKey, TValue>> FindLeafsRecursive(NodeBase parent)
            {
                if (protect) CheckForChanges(changeTag);
                if (parent.LeafNode != null)
                {
                    yield return new KeyValuePair<TKey, TValue>(_keyContext.ComposeKey(parent.LeafNode.GetFullKey()), parent.LeafNode.Value);
                }
                foreach (var v in parent.Children.Values)
                {
                    foreach (var v2 in FindLeafsRecursive(v))
                    {
                        yield return v2;
                    }
                }
            }

            if (protect) CheckForChanges(changeTag);

            return FindLeafsRecursive(_rootNode);
        }

        private LeafNode FindLeafRecursive(NodeBase rootNode, ref TKeyEnum getNextKey)
        {
            var key = getNextKey.GetNextKeyElement();
            if (!key.IsPresent)
            {
                return rootNode.LeafNode;
            }
            if (!rootNode.Children.ContainsKey(key.Element))
            {
                return null;
            }
            var c = rootNode.Children[key.Element];
            if (c is Node n)
            {
                foreach (var kr in n.KeyRange)
                {
                    var key2 = getNextKey.GetNextKeyElement();
                    if (!key2.IsPresent || _comparer.Compare(key2.Element, kr) != 0)
                    {
                        return null;
                    }
                }
            }
            return FindLeafRecursive(c, ref getNextKey);
        }

        private NodeFindResult FindNodeRecursive(NodeBase rootNode, ref TKeyEnum getNextKey)
        {
            var key = getNextKey.GetNextKeyElement();
            if (!key.IsPresent)
            {
                return new NodeFindResult()
                {
                    Node = rootNode,
                    FullEquality = true,
                    LastKey = key,
                    RangeRead = 0,
                    Split = false
                };
            }
            if (!rootNode.Children.ContainsKey(key.Element))
            {
                return new NodeFindResult()
                {
                    Node = rootNode,
                    FullEquality = false,
                    LastKey = key,
                    RangeRead = 0,
                    Split = false
                };
            }
            var c = rootNode.Children[key.Element];
            if (c is Node n)
            {
                int cnt = 0;
                foreach (var kr in n.KeyRange)
                {
                    var key2 = getNextKey.GetNextKeyElement();
                    if (!key2.IsPresent || _comparer.Compare(key2.Element, kr) != 0)
                    {
                        return new NodeFindResult()
                        {
                            Node = c,
                            FullEquality = false,
                            RangeRead = cnt,
                            LastKey = key2,
                            Split = true
                        };
                    }
                    cnt++;
                }
            }
            return FindNodeRecursive(c, ref getNextKey);
        }

        private struct NodeFindResult
        {
            public NodeBase Node;
            public bool FullEquality;
            public MaybeKeyElement<TKeyElement> LastKey;
            public int RangeRead;
            public bool Split;
        }

        private void CheckForChanges(int c)
        {
            var c2 = Interlocked.CompareExchange(ref _changeCounter, 0, 0);
            if (c2 != c)
            {
                throw new InvalidOperationException("Tree is changed during enumeration");
            }
        }

        private void DataUpdated()
        {
            Interlocked.Add(ref _changeCounter, 1);
        }

        private class NodeBase
        {
            protected readonly IComparer<TKeyElement> Comparer;

            public SortedDictionary<TKeyElement, Node> Children { get; protected set; }

            public LeafNode LeafNode;

            public NodeBase(IComparer<TKeyElement> comparer)
            {
                Children = new SortedDictionary<TKeyElement, Node>(comparer);
                Comparer = comparer;
            }

            public virtual void RemoveLeaf()
            {
                LeafNode = null;
            }

            public Node AddNode(TKeyElement keyPrefix)
            {
                var r = new Node(Comparer, this, keyPrefix);
                Children[keyPrefix] = r;
                return r;
            }

            public virtual void RemoveChild(TKeyElement keyPrefix)
            {
                Children.Remove(keyPrefix);
            }

            public virtual bool IsRoot => true;

            public override string ToString()
            {
                return "*ROOT*";
            }
        }

        private class Node : NodeBase
        {
            public NodeBase Parent;
            public List<TKeyElement> KeyRange;
            public readonly TKeyElement KeyPrefix;

            public Node(IComparer<TKeyElement> comparer, NodeBase parent, TKeyElement keyPrefix) : base(comparer)
            {
                KeyRange = new List<TKeyElement>();
                Parent = parent;
                KeyPrefix = keyPrefix;
            }

            public override void RemoveLeaf()
            {
                base.RemoveLeaf();
                if (CheckIfEmpty())
                {
                    return;
                }
                TryMerge();
            }

            private bool CheckIfEmpty()
            {
                if (Children.Count == 0)
                {
                    Parent.RemoveChild(KeyPrefix);
                    return true;
                }
                return false;
            }

            private void TryMerge()
            {
                if (Children.Count == 1 && LeafNode == null)
                {
                    var fc = Children.First();
                    KeyRange.Add(fc.Key);
                    fc.Value.LeafNode?.Reparent(this);
                    Children = fc.Value.Children;
                    foreach (var c in Children)
                    {
                        c.Value.Parent = this;
                    }
                    TryMerge();
                }
            }

            public override void RemoveChild(TKeyElement keyPrefix)
            {
                base.RemoveChild(keyPrefix);
                if (CheckIfEmpty())
                {
                    return;
                }
                TryMerge();
            }

            public void TrySplit(int keepRange)
            {
                if (KeyRange.Count == 0)
                {
                    return;
                }
                var newRange = new List<TKeyElement>();
                var oldRange = KeyRange;
                Node newNode = null;
                for (var i = 0; i < oldRange.Count; i++)
                {
                    if (i < keepRange)
                    {
                        newRange.Add(oldRange[i]);
                    }
                    else if (newNode == null)
                    {
                        var savedChildren = Children;
                        Children = new SortedDictionary<TKeyElement, Node>(Comparer);
                        newNode = AddNode(oldRange[i]);
                        newNode.Children = savedChildren;
                        foreach (var c in savedChildren)
                        {
                            c.Value.Parent = newNode;
                        }
                        LeafNode?.Reparent(newNode);
                        KeyRange = newRange;
                    } else
                    {
                        newNode.KeyRange.Add(oldRange[i]);
                    }
                }
                newNode?.TryMerge();
            }

            public override bool IsRoot => false;

            public override string ToString()
            {
                var s = new StringBuilder();
                s.Append(KeyPrefix);
                foreach (var k in KeyRange)
                {
                    s.Append(k);
                }
                return s.ToString();
            }
        }

        private class LeafNode
        {
            public NodeBase Parent;
            public readonly TValue Value;

            public LeafNode(NodeBase parent, TValue value)
            {
                this.Parent = parent ?? throw new ArgumentNullException(nameof(parent));
                this.Value = value;
            }

            public void Reparent(NodeBase n)
            {
                if (n == null) throw new ArgumentNullException(nameof(n));
                Parent.LeafNode = null;
                n.LeafNode = this;
                this.Parent = n;
            }

            public List<TKeyElement> GetFullKey()
            {
                var result = new List<TKeyElement>();

                void RecursiveFill(NodeBase n)
                {
                    if (n is Node nn)
                    {
                        result.InsertRange(0, nn.KeyRange);
                        result.Insert(0, nn.KeyPrefix);
                        if (nn.Parent != null)
                        {
                            RecursiveFill(nn.Parent);
                        }
                    }
                }

                RecursiveFill(Parent);

                return result;
            }
        }
    }
}