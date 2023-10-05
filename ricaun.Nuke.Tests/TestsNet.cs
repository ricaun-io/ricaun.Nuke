using NUnit.Framework;

namespace ricaun.Nuke.Tests
{
#if NET
    public class TestsNet
    {
        [Test]
        public void Test1()
        {
            var packages = new[] {
                "ricaun.Nuke.1.7.1.30.0.0-alpha.nupkg",
                "ricaun.Nuke.1.7.1-alpha.nupkg",
                "example.1.2.3-test.nupkg",
                "example.1.2.3.nupkg",
                "example.1.2.3-alpha.nupkg"
            };

            foreach (var package in packages)
            {
                var isPackage = ricaun.Nuke.Extensions.NuGetExtension.TryGetPackageNameAndVersion(package, out string packageName, out string packageVersion);
                System.Console.WriteLine($"{packageName} {packageVersion}");
                Assert.IsTrue(isPackage);
            }
        }
    }
#endif
}