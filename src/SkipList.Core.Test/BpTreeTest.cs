namespace BpTree.Core.Test
{
    using System;
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
    }
}
