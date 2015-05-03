namespace SkipList.Core.Test
{
    using System;
    using System.Collections.Generic;
    using BpTree.Core;
    using NUnit.Framework;
    using SimpleSpeedTester.Core;

    [TestFixture]
    public class PerformanceTests
    {
        [Test]
        [Ignore]
        public void Performance_Add_1()
        {
#if (DEBUG)
            Console.WriteLine("Performance tests should be run only in Release mode!");
#endif
            var n = 500000;
            var items = GenerateItems(n);

            var testGroup = new TestGroup("Performance_Add_1");
            var skipList = new SkipList<int, int>();
            var testResultSummary = testGroup.PlanAndExecute("Performance_Add_" + n, () =>
            {
                for (var index = 0; index < n; index++)
                {
                    skipList[items[index]] = items[index];
                }
            }, 5);
            Console.WriteLine(testResultSummary);

            var dictionary = new SortedDictionary<int, int>();
            var clrResultSummary = testGroup.PlanAndExecute("Performance_Add_CLR" + n, () =>
            {
                for (var index = 0; index < n; index++)
                {
                    dictionary[items[index]] = items[index];
                }
            }, 5);

            Console.WriteLine(clrResultSummary);
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
