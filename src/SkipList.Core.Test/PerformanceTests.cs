namespace SkipList.Core.Test
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using NUnit.Framework;
    using SimpleSpeedTester.Core;

    [TestFixture]
    public class PerformanceTests
    {
        private const string Iterations = "Iterations";
        private const string SkipList = "SkipList";
        private const string CsrDictionary = "ClrDictionary";

        private const int NumberOfIterations = 10000;

        [Test]
        [Ignore]
        public void Performance_Search_1()
        {
#if (DEBUG)
            Console.WriteLine("Performance tests should be run only in Release mode!");
#endif
            var numberOfItemsList = new[] {1000, 2000, 4000, 8000, 16000, 32000, 64000, 128000, 256000, 512000};
            var result = CreateResultDictionary();

            foreach (var n in numberOfItemsList)
            {
                var items = GenerateItems(n);
                var rnd = new Random();
                result[Iterations].Add(n.ToString());

                var testGroup = new TestGroup("Performance_Search_1");

                {
                    var skipList = new SkipListDictionary<int, int>();
                    for (var index = 0; index < n; index++)
                    {
                        skipList[items[index]] = items[index];
                    }

                    var testResultSummary = testGroup.PlanAndExecute("Performance_Search_" + n, () =>
                    {
                        for (var i = 0; i < NumberOfIterations; i++)
                        {
                            var value = skipList[items[rnd.Next(n)]];
                        }
                    }, 5);
                    result[SkipList].Add(testResultSummary.AverageExecutionTime.ToString("F1"));
                }

                {
                    var dictionary = new SortedDictionary<int, int>();
                    for (var index = 0; index < n; index++)
                    {
                        dictionary[items[index]] = items[index];
                    }
                    var clrResultSummary = testGroup.PlanAndExecute("Performance_Search_CLR" + n, () =>
                    {
                        for (var i = 0; i < NumberOfIterations; i++)
                        {
                            var value = dictionary[items[rnd.Next(n)]];
                        }
                    }, 5);

                    result[CsrDictionary].Add(clrResultSummary.AverageExecutionTime.ToString("F1"));
                }
            }
            PrintResult(result, "Search");
        }

        [Test]
        [Ignore]
        public void Performance_Add_1()
        {
#if (DEBUG)
            Console.WriteLine("Performance tests should be run only in Release mode!");
#endif
            // todo: implement performance test for multiple N
            var n = 500000;
            var items = GenerateItems(n);

            var testGroup = new TestGroup("Performance_Add_1");
            var testResultSummary = testGroup.PlanAndExecute("Performance_Add_" + n, () =>
            {
                var skipList = new SkipListDictionary<int, int>();
                for (var index = 0; index < n; index++)
                {
                    skipList[items[index]] = items[index];
                }
            }, 5);
            Console.WriteLine(testResultSummary);

            var clrResultSummary = testGroup.PlanAndExecute("Performance_Add_CLR" + n, () =>
            {
                var dictionary = new SortedDictionary<int, int>();
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

        private static Dictionary<string, IList<string>> CreateResultDictionary()
        {
            var result = new Dictionary<string, IList<string>>();
            result[Iterations] = new List<string>();
            result[SkipList] = new List<string>();
            result[CsrDictionary] = new List<string>();
            return result;
        }

        private void PrintResult(Dictionary<string, IList<string>> result, string testName)
        {
            PrintHeaderRow(result, testName);
            PrintResultRow(result, SkipList);
            PrintResultRow(result, CsrDictionary);
        }

        private void PrintHeaderRow(Dictionary<string, IList<string>> result, string testName)
        {
            var sb = new StringBuilder();
            sb.Append(testName);
            
            foreach (var testResult in result[Iterations])
            {
                sb.Append("\t" + testResult);
            }
            Console.WriteLine(sb.ToString());
        }

        private void PrintResultRow(Dictionary<string, IList<string>> result, string groupName)
        {
            var sb = new StringBuilder();
            sb.Append(groupName);

            foreach (var testResult in result[groupName])
            {
                sb.Append("\t" + testResult);
            }
            Console.WriteLine(sb.ToString());
        }
    }
}
