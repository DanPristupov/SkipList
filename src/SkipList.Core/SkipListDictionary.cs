namespace SkipList.Core
{
    using System.Collections.Generic;

    public class SkipListDictionary<TKey, TValue>
    {
        private SkipListSet<KeyValuePair<TKey, TValue>> _set;

        public SkipListDictionary(IComparer<TKey> comparer)
        {
            _set = new SkipListSet<KeyValuePair<TKey, TValue>>(new KeyValuePairComparer(comparer));
        }

        public class KeyValuePairComparer : Comparer<KeyValuePair<TKey, TValue>>
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