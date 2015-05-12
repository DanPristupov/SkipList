namespace SkipList.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class SkipListDictionary<TKey, TValue>:IDictionary<TKey, TValue>
    {
        private SkipListCollection<KeyValuePair<TKey, TValue>> _set;

        public SkipListDictionary()
            :this(Comparer<TKey>.Default)
        {
        }

        public SkipListDictionary(IComparer<TKey> comparer)
        {
            _set = new SkipListCollection<KeyValuePair<TKey, TValue>>(new KeyValuePairComparer(comparer));
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _set.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _set).GetEnumerator();
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            _set.Add(item);
        }

        public void Clear()
        {
            _set.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _set.Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            _set.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return _set.Remove(item);
        }

        public int Count
        {
            get { return _set.Count; }
        }

        public bool IsReadOnly
        {
            get { return _set.IsReadOnly; }
        }

        public bool ContainsKey(TKey key)
        {
            var node = _set.FindNode(new KeyValuePair<TKey, TValue>(key, default(TValue)));
            return node != null;
        }

        public void Add(TKey key, TValue value)
        {
            _set.Add(new KeyValuePair<TKey, TValue>(key, value));
        }

        public bool Remove(TKey key)
        {
            return _set.Remove(new KeyValuePair<TKey, TValue>(key, default(TValue)));
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            throw new System.NotImplementedException();
        }

        public TValue this[TKey key]
        {
            get
            {
                if (key == null)
                {
                    throw new ArgumentNullException();
                }
                var node = _set.FindNode(new KeyValuePair<TKey, TValue>(key, default(TValue)));
                if (node == null)
                {
                    throw new KeyNotFoundException();
                }
                return node.Key.Value;
            }
            set
            {
                if (key == null)
                {
                    throw new ArgumentNullException();
                }

                _set.Add(new KeyValuePair<TKey, TValue>(key, value));
            }
        }

        public ICollection<TKey> Keys { get; private set; }
        public ICollection<TValue> Values { get; private set; }

        internal class KeyValuePairComparer : Comparer<KeyValuePair<TKey, TValue>>
        {
            internal IComparer<TKey> keyComparer;

            public KeyValuePairComparer(IComparer<TKey> keyComparer)
            {
                if (keyComparer == null)
                {
                    this.keyComparer = Comparer<TKey>.Default;
                }
                else
                {
                    this.keyComparer = keyComparer;
                }
            }

            public override int Compare(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y)
            {
                return keyComparer.Compare(x.Key, y.Key);
            }
        }
    }
}