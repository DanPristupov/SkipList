namespace BpTree.Core.Test
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using NUnit.Framework;

    [TestFixture]
    public class BpTreeTest
    {
        [Test]
        public void General_Test_1()
        {
            var skipList = new SkipList<int, int>();
            skipList[15] = 15;
//            Console.WriteLine(skipList.DebugString);
            skipList[10] = 10;
//            Console.WriteLine(skipList.DebugString);
            skipList[25] = 25;
//            Console.WriteLine(skipList.DebugString);
            skipList[20] = 20;
//            Console.WriteLine(skipList.DebugString);
            skipList[30] = 30;
//            Console.WriteLine(skipList.DebugString);
            skipList[5] = 5;
            Console.WriteLine(skipList.DebugString);
        }

        [TestCase(new[] {15,10,25,20,30,5 })]
        [TestCase(new[] {10,20,30,40,50,60 })]
        [TestCase(new[] {60,50,40,30,20,10 })]
        public void Insert_Test_1(int[] values)
        {
            var skipList = new SkipList<int, int>();
            var count = 1;
            foreach (var value in values)
            {
                skipList[value] = value;
                Assert.AreEqual(value, skipList[value]);
                Assert.AreEqual(count++, skipList.Count);
            }
            foreach (var value in values)
            {
                Assert.AreEqual(value, skipList[value]);
            }
        }

        [TestCase(new[] {15,10,25,20,30,5 })]
        [TestCase(new[] {10,20,30,40,50,60 })]
        [TestCase(new[] {60,50,40,30,20,10 })]
        public void Contains_AfterAdd(int[] keys)
        {
            var skipList = new SkipList<int, int>();
            foreach (var key in keys)
            {
                skipList[key] = key;
                Assert.IsTrue(skipList.ContainsKey(key));
            }

            foreach (var key in keys)
            {
                Assert.IsTrue(skipList.ContainsKey(key));
            }
        }

        [TestCase(1, new[] {1 })]
        [TestCase(2, new[] {1, 2 })]
        [TestCase(3, new[] {1, 2, 3 })]
        [TestCase(4, new[] {1, 2, 3, 4 })]
        [TestCase(4, new[] {1, 2, 3, 4, 1 })]
        public void Count_AfterAdd(int expectedCount, int[] keys)
        {
            var skipList = new SkipList<int, int>();
            foreach (var key in keys)
            {
                skipList[key] = key;
            }
            Assert.AreEqual(expectedCount, skipList.Count);
        }

        [TestCase(new[] {15})]
        [TestCase(new[] {15,10})]
        [TestCase(new[] {15,10,25,20,30,5 })]
        [TestCase(new[] {10,20,30,40,50,60 })]
        [TestCase(new[] {60,50,40,30,20,10 })]
        public void Remove_Test_1(int[] values)
        {
            var skipList = new SkipList<int, int>();
            foreach (var value in values)
            {
                skipList[value] = value;
            }
            var count = values.Length;
            Assert.AreEqual(count--, skipList.Count);
            foreach (var value in values)
            {
                var removedFlag = skipList.Remove(value);
                Assert.IsTrue(removedFlag);
                Assert.IsFalse(skipList.ContainsKey(value));
                Assert.AreEqual(count--, skipList.Count);
            }
        }

        [Test]
        [Ignore]
        public void Performance_Add_1()
        {
#if (DEBUG)
            Console.WriteLine("Performance tests should be run only in Release mode!");
#endif
            var n = 500000;
            var items = GenerateItems(n);

            { 
                var skipList = new SkipList<int, int>();
                var timer = Stopwatch.StartNew();
                for (var index = 0; index < n; index++)
                {
                    skipList[items[index]] = items[index];
                }
                Console.WriteLine(timer.Elapsed);
            }

//            { 
//                var dictionary = new SortedDictionary<int, int>();
//                var timer = Stopwatch.StartNew();
//                for (var index = 0; index < n; index++)
//                {
//                    dictionary[items[index]] = items[index];
//                }
//                Console.WriteLine(timer.Elapsed);
//            }
        }

        private static int[] GenerateItems(int n)
        {
            var random = new Random();
            var itemSet = new HashSet<int>();
            var items = new int[n];
            var i = 0;
            while (i < n)
            {
                var value = random.Next();
                if (itemSet.Contains(n))
                {
                    continue;
                }
                items[i] = value;
                i++;
            }
            return items;
        }
    }
}
