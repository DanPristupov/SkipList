﻿namespace BpTree.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text;

    public class SkipList<TKey, TValue>// : IDictionary<TKey, TValue>
        where TKey : IComparable<TKey>
    {
        private const int MaxLevel = 20;

        private readonly Random _random = new Random();
        private readonly Comparer<TKey> _comparer;
        private readonly SkipListNode<TKey, TValue> _head;
        private readonly SkipListNode<TKey, TValue> _nil;
        
        private int _level = 0;
        private int _count = 0;

        public SkipList()
            :this(CreateComparer(typeof(TKey)))
        {
        }

        private SkipList(Comparer<TKey> comparer)
        {
            Contract.Requires(comparer != null);
            _comparer = comparer;
            _head = new SkipListNode<TKey, TValue>(default(TKey), default(TValue), MaxLevel);
            _nil = _head;
            for (var i = 0; i <= MaxLevel; i++)
            {
                _head.Forward[i] = _nil;
            }
        }

        private static Comparer<TKey> CreateComparer(Type keyType)
        {
            if (keyType == typeof(int))
            {
                return new Int32Comparer() as Comparer<TKey>;
            }
            return new DefaultComparer();
        }

        public int Count
        {
            get { return _count; }
        }

        public TValue this[TKey key]
        {
            get
            {
                var node = Search(key);
                if (node == null)
                {
                    throw new KeyNotFoundException();
                }
                return node.Value;
            }
            set { Insert(key, value); }
        }

        public bool ContainsKey(TKey key)
        {
            var result = Search(key);
            return result != null;
        }

        private SkipListNode<TKey, TValue> Search(TKey key)
        {
            var node = _head;

            for (var i = _level; i >= 0; i--)
            {
                Contract.Assert(_comparer.Compare(node.Key, key) < 0);
                while (node.Forward[i] != _nil && _comparer.Compare(node.Forward[i].Key, key) < 0)
                {
                    node = node.Forward[i];
                }
            }

            Contract.Assert(_comparer.Compare(node.Key, key) < 0);
            Contract.Assert(node.Forward[0] == _nil || _comparer.Compare(key, node.Forward[0].Key) <= 0);
            node = node.Forward[0];

            if (node != _nil && _comparer.Compare(node.Key, key) == 0)
            {
                return node;
            }
            return null;
        }


        private void Insert(TKey key, TValue value)
        {
            // TODO: May I can use the update list and assign it to new Node.Neightbours directly
            var updateList = new SkipListNode<TKey, TValue>[MaxLevel+1];
            var node = _head;
            for (var i = _level; i >= 0; i--)
            {
                while (node.Forward[i] != _nil && _comparer.Compare(node.Forward[i].Key, key) < 0)
                {
                    node = node.Forward[i];
                }
                updateList[i] = node;
            }
            node = node.Forward[0];
            if (node != _nil && _comparer.Compare(node.Key, key) == 0)
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
                node.Forward[i] = updateList[i].Forward[i];
                updateList[i].Forward[i] = node;
            }
            _count++;
        }

        public bool Remove(TKey key)
        {
            // X This block of code can be extracted as method
            var updateList = new SkipListNode<TKey, TValue>[MaxLevel + 1];
            var node = _head;
            for (var i = _level; i >= 0; i--)
            {
                while (node.Forward[i] != _nil && _comparer.Compare(node.Forward[i].Key, key) < 0)
                {
                    node = node.Forward[i];
                }
                updateList[i] = node;
            }
            node = node.Forward[0];
            // /X

            if (node == null || _comparer.Compare(node.Key, key) != 0)
            {
                return false;
            }

            for (var i = 0; i <= _level; i++)
            {
                if (updateList[i].Forward[i] != node)
                {
                    break;
                }
                updateList[i].Forward[i] = node.Forward[i];
            }
            while (_level > 0 && _head.Forward[_level] == _nil)
            {
                _level--;
            }
            _count--;
            return true;
        }

        #region DebugString
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
                node = AppendNode(result, node);
                while (node != _nil)
                {
                    node = AppendNode(result, node);
                }
                return string.Join(Environment.NewLine, result.Select(x => x.ToString()));
            }
        }

        private SkipListNode<TKey, TValue> AppendNode(StringBuilder[] result, SkipListNode<TKey, TValue> node)
        {
            result[0].AppendFormat("{0}-", node.Key);

            for (var i = 0; i < _level; i++)
            {
                if (i < node.Forward.Length-1)
                {
                    result[i+1].AppendFormat("*-");
                }
                else
                {
                    result[i+1].AppendFormat("--");
                }
            }
            node = node.Forward[0];
            return node;
        }
        #endregion

        #region DefaultComparers
        private class DefaultComparer : Comparer<TKey>
        {
            public override int Compare(TKey x, TKey y)
            {
                return x.CompareTo(y);
            }
        }

        private class Int32Comparer : Comparer<int>
        {
            public override int Compare(int x, int y)
            {
                if (x > y) return -1;
                if (x < y) return 1;
                return 0;
            }
        }

        #endregion
    }
}
