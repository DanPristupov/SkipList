namespace BpTree.Core
{
    using System;

    internal class SkipListNode<TKey, TValue>
        where TKey : IComparable<TKey>
    {
        public SkipListNode(TKey key, TValue value, int level)
        {
            Key = key;
            Value = value;
            Height = level;
            Neighbour = new SkipListNode<TKey, TValue>[level+1];
        }

        public int Height { get; set; }

        public TKey Key { get; set; }
        public TValue Value { get; set; }

        public SkipListNode<TKey, TValue>[] Neighbour { get; set; }
    }
}