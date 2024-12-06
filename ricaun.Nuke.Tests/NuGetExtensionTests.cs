using NUnit.Framework;

namespace ricaun.Nuke.Tests
{
    public class NuGetExtensionTests
    {
        [Test]
        public void Test_PackageNameAndVersion()
        {
            var packages = new[] {
                "ricaun.example.1.2.3.nupkg",
                "ricaun.example.1.2.3-alpha.nupkg",
                "ricaun.example.1.2.3-alpha.1.nupkg",
                "ricaun.example.1.2.3.30.0.0-alpha.nupkg",
                "example.1.2.3.nupkg",
                "example.1.2.3-test.nupkg",
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
}