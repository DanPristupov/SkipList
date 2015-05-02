namespace BpTree.Core
{
    using System;
    using System.Collections.ObjectModel;

    internal class SkipListNode<TKey, TValue>
        where TKey : IComparable<TKey>
    {
        public SkipListNode()
        {
            Neighbour = new Collection<SkipListNode<TKey, TValue>>();
        }

        public SkipListNode(TKey key, TValue value, int level)
            :this()
        {
            Key = key;
            Value = value;
            Height = level;
        }

        public int Height { get; set; }

        public TKey Key { get; set; }
        public TValue Value { get; set; }

        public Collection<SkipListNode<TKey, TValue>> Neighbour { get; set; }
    }
}