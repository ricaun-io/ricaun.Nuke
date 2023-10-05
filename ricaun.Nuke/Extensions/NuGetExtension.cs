using Nuke.Common.IO;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.NuGet;
using System;

namespace ricaun.Nuke.Extensions
{
    /// <summary>
    /// NuGetExtension
    /// </summary>
    public static class NuGetExtension
    {
        /// <summary>
        /// TryGetPackageNameAndVersion
        /// </summary>
        /// <param name="packageFileName"></param>
        /// <param name="packageName"></param>
        /// <param name="packageVersion"></param>
        /// <returns></returns>
        public static bool TryGetPackageNameAndVersion(string packageFileName, out string packageName, out string packageVersion)
        {
            packageName = null;
            packageVersion = null;

            string pattern = @"^(.*?)\.((?:\.?[0-9]+){3,}(?:[-a-z0-9]+?\.?)*)\.nupkg$";

            var match = System.Text.RegularExpressions.Regex.Match(packageFileName, pattern);
            if (match.Success)
            {
                packageName = match.Groups[1].Value;
                packageVersion = match.Groups[2].Value;
                return true;
            }
            return false;
        }

        /// <summary>
        /// IsSourceNugetOrg
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsSourceNugetOrg(string source)
        {
            return source.StartsWith("https://api.nuget.org");
        }

        /// <summary>
        /// NugetDelete (Only works with 'api.nuget.org' to unlist package)
        /// </summary>
        /// <param name="source"></param>
        /// <param name="apiKey"></param>
        /// <param name="packageFileName"></param>
        /// <returns></returns>
        /// <remarks>https://learn.microsoft.com/en-us/nuget/reference/cli-reference/cli-ref-delete</remarks>
        public static bool NuGetUnlist(string source, string apiKey, string packageFileName)
        {
            if (IsSourceNugetOrg(source) == false)
            {
                return false;
            }

            if (TryGetPackageNameAndVersion(packageFileName, out string packageName, out string packageVersion))
                return false;

            Serilog.Log.Information($"NuGet delete: {packageName} {packageVersion}");
            try
            {
                NuGetTasks.NuGet(
                    $"delete {packageName} {packageVersion}" +
                    $" -Source {source}" +
                    $" -ApiKey {apiKey}" +
                    $" -NonInteractive",
                    logOutput: false,
                    logInvocation: false
                    ); // don't print to std out/err

                return true;
            }
            catch (Exception ex)
            {
                Serilog.Log.Information($"NuGet delete Exception: {ex}");
                return false;
            }
        }

        /// <summary>
        /// NuGetVerifySignatures
        /// </summary>
        /// <param name="packageFileName"></param>
        /// <returns></returns>
        public static bool NuGetVerifySignatures(string packageFileName)
        {
            try
            {
                NuGetTasks.NuGet(
                    $"verify -Signatures \"{packageFileName}\"",
                    logOutput: false,
                    logInvocation: false
                    ); // don't print to std out/err
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// NugetSign
        /// </summary>
        /// <param name="packageFileName"></param>
        /// <param name="certPath"></param>
        /// <param name="certPassword"></param>
        /// <param name="timestampServer"></param>
        /// <returns></returns>
        public static bool NugetSign(string packageFileName, string certPath, string certPassword, string timestampServer)
        {
            try
            {
                NuGetTasks.NuGet(
                    $"sign \"{packageFileName}\"" +
                    $" -CertificatePath {certPath}" +
                    $" -CertificatePassword {certPassword}" +
                    $" -Timestamper {timestampServer}" +
                    $" -NonInteractive",
                    logOutput: false,
                    logInvocation: false
                    ); // don't print to std out/err
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// DotNetNuGetPush
        /// </summary>
        /// <param name="source"></param>
        /// <param name="apiKey"></param>
        /// <param name="packageFilePath"></param>
        public static void DotNetNuGetPush(string source, string apiKey, string packageFilePath)
        {
            DotNetTasks.DotNetNuGetPush(s => s
                .SetTargetPath(packageFilePath)
                .SetSource(source)
                .SetApiKey(apiKey)
                .EnableSkipDuplicate()
            );
        }

    }
}
