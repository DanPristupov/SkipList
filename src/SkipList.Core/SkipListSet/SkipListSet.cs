namespace SkipList.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;

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

        internal SkipListSetNode<T> FindNode(T item)
        {
            var node = Search(item);
            return node;
        }

        public void Add(T item)
        {
            Insert(item);
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
            return Search(item) != null;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(T key)
        {
            // X This block of code can be extracted as method
            var updateList = new SkipListSetNode<T>[MaxLevel + 1];
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

            if (node == _nil || _comparer.Compare(node.Key, key) != 0)
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

        public int Count { get { return _count; } }
        public bool IsReadOnly { get { return false; } }

        public IEnumerator<T> GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private SkipListSetNode<T> Search(T key)
        {
            var node = _head;

            for (var i = _level; i >= 0; i--)
            {
//                Contract.Assert(_comparer.Compare(node.Key, key) < 0);
                while (node.Forward[i] != _nil)
                {
                    var cmpResult = _comparer.Compare(node.Forward[i].Key, key);
                    if (cmpResult > 0)
                    {
                        break;
                    }
                    if (cmpResult == 0)
                    {
                        return node.Forward[i];
                    }
                    node = node.Forward[i];
                }
            }

//            Contract.Assert(_comparer.Compare(node.Key, key) < 0);
            Debug.Assert(node.Forward[0] == _nil || _comparer.Compare(key, node.Forward[0].Key) <= 0);
            node = node.Forward[0];

            if (node != _nil && _comparer.Compare(node.Key, key) == 0)
            {
                return node;
            }
            return null;
        }

        private void Insert(T key)
        {
            // TODO: May I can use the update list and assign it to new Node.Neightbours directly
            var updateList = new SkipListSetNode<T>[MaxLevel + 1];
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
                return;
            }

            var newLevel = 0;
            for (; _random.Next(0, 2) > 0 && newLevel < MaxLevel; newLevel++) ;
            if (newLevel > _level)
            {
                for (var i = _level + 1; i <= newLevel; i++)
                {
                    updateList[i] = _head;
                }
                _level = newLevel;
            }

            node = new SkipListSetNode<T>(key, newLevel);

            for (var i = 0; i <= newLevel; i++)
            {
                node.Forward[i] = updateList[i].Forward[i];
                updateList[i].Forward[i] = node;
            }
            _count++;
        }

        #region DebugString
        public string LevelStats
        {
            get
            {
                var levels = new Dictionary<int, int>();
                var result = new StringBuilder[_level + 1];
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = new StringBuilder();
                }

                var node = _head;
                node = LevelStatsHandleNode(levels, node);
                while (node != _nil)
                {
                    node = LevelStatsHandleNode(levels, node);
                }
                var output = levels
                    .OrderBy(x => x.Key)
                    .Select(x => string.Format("{0}: {1}", x.Key, x.Value));
                return string.Join(Environment.NewLine, output);
            }
        }

        public string DebugString
        {
            get
            {
                var result = new StringBuilder[_level + 1];
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

        private SkipListSetNode<T> AppendNode(StringBuilder[] result, SkipListSetNode<T> node)
        {
            result[0].Append(node.Key.ToString().PadRight(4));

            var neighbour = node.Forward[0];
            for (var i = 0; i < _level; i++)
            {
                if (i < node.Forward.Length)
                {
                    result[i + 1].AppendFormat("|--");
                }
                else
                {
                    result[i + 1].AppendFormat("---");
                }

                if (i < node.Forward.Length && node.Forward[i] == neighbour)
                {
                    result[i + 1].AppendFormat(">");
                }
                else
                {
                    result[i + 1].AppendFormat("-");
                }
            }
            node = node.Forward[0];
            return node;
        }

        private SkipListSetNode<T> LevelStatsHandleNode(Dictionary<int, int> levels, SkipListSetNode<T> node)
        {
            if (!levels.ContainsKey(node.Forward.Length))
            {
                levels[node.Forward.Length] = 1;
            }
            else
            {
                levels[node.Forward.Length]++;
            }

            node = node.Forward[0];
            return node;
        }
        #endregion

    }
}
