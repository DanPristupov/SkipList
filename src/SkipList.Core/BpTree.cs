namespace BpTree.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text;

    public class SkipList<TKey, TValue>// : IDictionary<TKey, TValue>
        where TKey : IComparable<TKey>
    {
        private Random _random = new Random();
        private const int MaxLevel = 20;
        private readonly Comparer<TKey> _comparer;

        public SkipList()
            :this(new DefaultComparer())
        {
        }

        private SkipList(Comparer<TKey> comparer)
        {
            Contract.Requires(comparer != null);
            _comparer = comparer;
            _level = 0;
            _head = new SkipListNode<TKey, TValue>(default(TKey), default(TValue), MaxLevel);
            _nil = _head;
            for (var i = 0; i <= MaxLevel; i++)
            {
                _head.Neighbour[i] = _nil;
            }
        }

        private SkipListNode<TKey, TValue> _head;
        private SkipListNode<TKey, TValue> _nil;

        private int _level;


        public TValue this[TKey key]
        {
            get { return Search(key); }
            set { Insert(key, value); }
        }

        private void Insert(TKey key, TValue value)
        {
            var updateList = new SkipListNode<TKey, TValue>[MaxLevel+1]; // todo: I can use stackalloc here
            var node = _head;
            for (var i = _level; i >= 0; i--)
            {
                while (node.Neighbour[i] != _nil && _comparer.Compare(node.Neighbour[i].Key, key) < 0)
                {
                    node = node.Neighbour[i];
                }
                updateList[i] = node;
            }
            node = node.Neighbour[0];
            if (node != null && _comparer.Compare(node.Key, key) == 0)
            {
                node.Value = value;
                return;
            }

            var newLevel = 0;
            for (; _random.Next(0, 2) > 0 && newLevel < MaxLevel; newLevel++);
            if (newLevel > _level)
            {
                for (var i = _level + 1; i <= newLevel; i++)
                {
                    updateList[i] = _head;
                }
                _level = newLevel;
            }

            node = new SkipListNode<TKey, TValue>(key, value, newLevel);

            for (var i = 0; i <= newLevel; i++)
            {
                node.Neighbour[i] = updateList[i].Neighbour[i];
                updateList[i].Neighbour[i] = node;
            }
        }

        private TValue Search(TKey key)
        {
            var node = _head;

            for (var i = _level; i >= 0; i--)
            {
                Contract.Assert(_comparer.Compare(node.Key, key) < 0);
                while (_comparer.Compare(node.Neighbour[i].Key, key) < 0)
                {
                    node = node.Neighbour[i];
                }
            }

            Contract.Assert(_comparer.Compare(node.Key, key) < 0);
            Contract.Assert(_comparer.Compare(key, node.Neighbour[0].Key) < 0);
            node = node.Neighbour[0];
            if (node != null)
            {
                return node.Value;
            }
            throw new KeyNotFoundException();
        }

        private class DefaultComparer : Comparer<TKey>
        {
            public override int Compare(TKey x, TKey y)
            {
                return x.CompareTo(y);
            }
        }

        public string DebugString
        {
            get
            {
                var result = new StringBuilder[_level+1];
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = new StringBuilder();
                }

                var node = _head;
                var index = 0;
                node = AppendNode(result, index++, node);
                while (node != _nil)
                {
                    node = AppendNode(result, index++, node);
                }
                return string.Join(Environment.NewLine, result.Select(x => x.ToString()));
            }
        }

        private SkipListNode<TKey, TValue> AppendNode(StringBuilder[] result, int index, SkipListNode<TKey, TValue> node)
        {
            result[0].AppendFormat("{0}-", node.Key);

            for (var i = 0; i < _level; i++)
            {
                if (i < node.Neighbour.Length-1)
                {
                    result[i+1].AppendFormat("*-");
                }
                else
                {
                    result[i+1].AppendFormat("--");
                }
            }
            node = node.Neighbour[0];
            return node;
        }
    }
}
