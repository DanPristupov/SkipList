namespace SkipList.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using BpTree.Core;

    public class SkipListSet<T> : ISet<T>
    {
        private const int MaxLevel = 20;

        private readonly Random _random;
        private readonly Comparer<T> _comparer;
        private readonly SkipListSetNode<T> _head;
        private readonly SkipListSetNode<T> _nil;

        private int _level = 0;
        private int _count = 0;

        public SkipListSet(Comparer<T> comparer)
        {
            Debug.Assert(comparer != null);
            _comparer = comparer;
            _random = new Random();
            _head = new SkipListSetNode<T>(default(T), MaxLevel);
            _nil = _head;

            for (var i = 0; i <= MaxLevel; i++)
            {
                _head.Forward[i] = _nil;
            }
        }


        public void Add(T item)
        {
            throw new System.NotImplementedException();
        }

        bool ISet<T>.Add(T item)
        {
            throw new System.NotImplementedException();
        }

        public void UnionWith(IEnumerable<T> other)
        {
            throw new System.NotImplementedException();
        }

        public void IntersectWith(IEnumerable<T> other)
        {
            throw new System.NotImplementedException();
        }

        public void ExceptWith(IEnumerable<T> other)
        {
            throw new System.NotImplementedException();
        }

        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            throw new System.NotImplementedException();
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            throw new System.NotImplementedException();
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            throw new System.NotImplementedException();
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            throw new System.NotImplementedException();
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            throw new System.NotImplementedException();
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            throw new System.NotImplementedException();
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            throw new System.NotImplementedException();
        }


        public void Clear()
        {
            throw new System.NotImplementedException();
        }

        public bool Contains(T item)
        {
            throw new System.NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new System.NotImplementedException();
        }

        public int Count { get; private set; }
        public bool IsReadOnly { get; private set; }

        public IEnumerator<T> GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
