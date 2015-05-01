namespace BpTree.Core.Test
{
    using NUnit.Framework;

    [TestFixture]
    public class BpTreeTest
    {
        [Test]
        public void General_Test_1()
        {
            var skipList = new SkipList<int, int>();
            skipList[10] = 10;
        }
    }
}
