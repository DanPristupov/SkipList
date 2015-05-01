namespace BpTree.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    public class SkipList<TKey, TValue>// : IDictionary<TKey, TValue>
        where TKey : IComparable<TKey>
    {
        private Random _random = new Random();
        private const int MaxLevel = 4;
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
            _head = new SkipListNode<TKey, TValue>();
            _nil = _head;
            for (var i = 0; i <= MaxLevel; i++)
            {
                _head.Neighbour.Add(_nil);
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
            
            var level = _random.Next(0, _level);
            if (level > _level)
            {
                for (var i = _level + 1; i < level; i++)
                {
                    updateList[i] = _head;
                }
                _level = level;
            }
            node = new SkipListNode<TKey, TValue>(key, value, level);

            for (var i = 0; i <= level; i++)
            {
//                node.Neighbour[i] = updateList[i].Neighbour[i];
                node.Neighbour.Add(updateList[i].Neighbour[i]);
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
    }
}
