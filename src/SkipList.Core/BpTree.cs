namespace BpTree.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    public class SkipList<TKey, TValue>// : IDictionary<TKey, TValue>
        where TKey : IComparable<TKey>
    {
        private readonly Comparer<TKey> _comparer;

        public SkipList()
            :this(new DefaultComparer())
        {
        }

        private SkipList(Comparer<TKey> comparer)
        {
            Contract.Requires(comparer != null);
            _comparer = comparer;
        }

        private SkipListNode<TKey, TValue> _head;

        private int _level = 0;


        public TValue this[TKey key]
        {
            get { return Search(key); }
            set { throw new NotImplementedException(); }
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
