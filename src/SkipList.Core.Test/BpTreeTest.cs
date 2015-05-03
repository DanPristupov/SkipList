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
            foreach (var value in values)
            {
                skipList[value] = value;
                Assert.AreEqual(value, skipList[value]);
            }
            foreach (var value in values)
            {
                Assert.AreEqual(value, skipList[value]);
            }
        }

        [Test]
        [Ignore]
        public void Performance_Test_1()
        {
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
