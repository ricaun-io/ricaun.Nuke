using NUnit.Framework;
using System;

namespace ricaun.Nuke.Example.Tests
{
    public class Tests
    {
        [Test]
        public void TestPass()
        {
            Assert.Pass("Pass Message");
        }

        [Test]
        public void TestConsole()
        {
            Console.WriteLine("Console Line 1");
            Console.WriteLine("Console Line 2");
            Console.WriteLine("Console Line 3");
        }

        [Test]
        [Explicit]
        public void TestExplicit()
        {

        }

        [Test]
        [Ignore("Ignore Message")]
        public void TestIgnore()
        {

        }

        [Test]
        public void TestIgnore2()
        {
            Assert.Ignore("Ignore Message");
        }

        [Test]
        public void TestException()
        {
            throw new Exception();
        }

        [Test]
        public void TestFail()
        {
            Assert.Fail("Fail Message");
        }
    }
}