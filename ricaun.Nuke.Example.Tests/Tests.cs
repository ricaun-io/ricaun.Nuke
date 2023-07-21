using NUnit.Framework;
using System;
using System.Threading;

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
        public void TestSleep()
        {
            Thread.Sleep(1234);
        }

#if IGNORE
        [Test]
        [Explicit("Explicit Message")]
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
#endif
#if FAIL
        [Test]
        public void TestException()
        {
            throw new Exception();
        }

        [Test]
        public void TestException2()
        {
            Console.WriteLine("Console Line");
            throw new Exception();
        }

        [Test]
        public void TestFail()
        {
            Assert.Fail("Fail Message");
        }
#endif
    }
}