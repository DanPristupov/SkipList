namespace SkipList.Core
{
    internal class SkipListSetNode<T>
    {
        public SkipListSetNode(T key, int level)
        {
            Key = key;
            Forward = new SkipListSetNode<T>[level + 1];
        }

        public T Key { get; private set; }

        public SkipListSetNode<T>[] Forward { get; private set; }
    }
}