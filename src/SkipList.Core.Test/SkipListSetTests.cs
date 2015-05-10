namespace SkipList.Core.Test
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using SkipList.Core;

    [TestFixture]
    public class SkipListSetTests
    {
        [Test]
        public void General_Test_1()
        {
            var skipList = new SkipListSet<int>(Comparer<int>.Default);
            skipList.Add(15);
            skipList.Add(10);
            skipList.Add(25);
            skipList.Add(20);
            skipList.Add(30);
            skipList.Add(5);
            Console.WriteLine(skipList.DebugString);
        }

        [TestCase(new[] {15,10,25,20,30,5 })]
        [TestCase(new[] {10,20,30,40,50,60 })]
        [TestCase(new[] {60,50,40,30,20,10 })]
        public void Insert_Test_1(int[] values)
        {
            var skipList = new SkipListSet<int>(Comparer<int>.Default);
            var count = 1;
            foreach (var value in values)
            {
                Assert.IsFalse(skipList.Contains(value));
                skipList.Add(value);
                Assert.IsTrue(skipList.Contains(value));
                Assert.AreEqual(count++, skipList.Count);
            }
            foreach (var value in values)
            {
                Assert.IsTrue(skipList.Contains(value));
            }
        }

        [TestCase(new[] {15,10,25,20,30,5 })]
        [TestCase(new[] {10,20,30,40,50,60 })]
        [TestCase(new[] {60,50,40,30,20,10 })]
        public void Contains_AfterAdd(int[] keys)
        {
            var skipList = new SkipListSet<int>(Comparer<int>.Default);
            foreach (var key in keys)
            {
                Assert.IsFalse(skipList.Contains(key));
                skipList.Add(key);
                Assert.IsTrue(skipList.Contains(key));
            }

            foreach (var key in keys)
            {
                Assert.IsTrue(skipList.Contains(key));
            }
        }

        [TestCase(1, new[] {1 })]
        [TestCase(2, new[] {1, 2 })]
        [TestCase(3, new[] {1, 2, 3 })]
        [TestCase(4, new[] {1, 2, 3, 4 })]
        [TestCase(4, new[] {1, 2, 3, 4, 1 })]
        public void Count_AfterAdd(int expectedCount, int[] keys)
        {
            var skipList = new SkipListSet<int>(Comparer<int>.Default);
            foreach (var key in keys)
            {
                skipList.Add(key);
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
            var skipList = new SkipListSet<int>(Comparer<int>.Default);
            foreach (var value in values)
            {
                skipList.Add(value);
            }
            var count = values.Length;
            Assert.AreEqual(count--, skipList.Count);
            foreach (var value in values)
            {
                var removedFlag = skipList.Remove(value);
                Assert.IsTrue(removedFlag);
                Assert.IsFalse(skipList.Contains(value));
                Assert.AreEqual(count--, skipList.Count);
            }
        }


    }
}
