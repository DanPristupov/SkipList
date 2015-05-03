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
            Forward = new SkipListNode<TKey, TValue>[level+1];
        }

        public TKey Key { get; private set; }
        public TValue Value { get; set; }

        public SkipListNode<TKey, TValue>[] Forward { get; private set; }
    }
}