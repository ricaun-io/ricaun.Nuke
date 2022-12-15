using NUnit.Framework;

namespace ricaun.Nuke.Example.Tests
{
    public class UnitTest
    {
        [Test]
        public void TestPass()
        {
            Assert.Pass();
        }

        [Explicit]
        [Test]
        public void TestFail()
        {
            Assert.Fail();
        }
    }
}