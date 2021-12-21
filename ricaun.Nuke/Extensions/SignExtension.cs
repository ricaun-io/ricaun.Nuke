using Nuke.Common;
using Nuke.Common.Tools.NuGet;
using Nuke.Common.Tools.SignTool;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ricaun.Nuke.Extensions
{
    /// <summary>
    /// SignExtension
    /// </summary>
    public static class SignExtension
    {
        #region Sign Util

        /// <summary>
        /// timestampServer
        /// </summary>
        const string timestampServer = "http://timestamp.digicert.com/";

        /// <summary>
        /// VerifySignFile
        /// </summary>
        /// <param name="path"></param>
        /// <param name="tempFolderDownloadFile"></param>
        /// <returns></returns>
        public static string VerifySignFile(string path, string tempFolderDownloadFile)
        {
            if (File.Exists(path))
                return path;

            var file = Path.Combine(tempFolderDownloadFile, "signfile.pfx");
            using (var client = new System.Net.WebClient())
            {
                client.DownloadFile(path, file);
            }
            return file;
        }

        /// <summary>
        /// https://github.com/DataDog/dd-trace-dotnet/blob/master/tracer/build/_build/Build.Gitlab.cs
        /// </summary>
        /// <param name="certPath"></param>
        /// <param name="certPassword"></param>
        /// <param name="binaryPath"></param>
        public static void SignBinary(string certPath, string certPassword, string binaryPath)
        {
            if (HasSignature(binaryPath)) return;

            Logger.Normal($"Signing: {binaryPath}");

            try
            {
                SignToolTasks.SignTool(x => x
                    .SetFiles(binaryPath)
                    .SetFile(certPath)
                    .SetPassword(certPassword)
                    .SetTimestampServerUrl(timestampServer)
                    .SetFileDigestAlgorithm(SignToolDigestAlgorithm.SHA256)
                );
            }
            catch (Exception)
            {
                Logger.Error($"Failed to sign file '{binaryPath}");
            }

        }

        /// <summary>
        /// Sign Nuget
        /// </summary>
        /// <param name="certPath"></param>
        /// <param name="certPassword"></param>
        /// <param name="binaryPath"></param>
        public static void SignNuGet(string certPath, string certPassword, string binaryPath)
        {
            if (HasSignature(binaryPath)) return;

            Logger.Normal($"Signing: {binaryPath}");

            try
            {
                NuGetTasks.NuGet(
                    $"sign \"{binaryPath}\"" +
                    $" -CertificatePath {certPath}" +
                    $" -CertificatePassword {certPassword}" +
                    $" -Timestamper {timestampServer} -NonInteractive",
                    logOutput: false,
                    logInvocation: false,
                    logTimestamp: false
                    ); // don't print to std out/err
            }
            catch (Exception)
            {
                // Exception doesn't say anything useful generally and don't want to expose it if it does
                // so don't log it
                Logger.Error($"Failed to sign nuget package '{binaryPath}");
            }
        }

        /// <summary>
        /// Has Signature
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        static bool HasSignature(string fileInfo)
        {
            if (fileInfo.EndsWith(".nupkg"))
            {
                try
                {
                    NuGetTasks.NuGet(
                        $"verify -Signatures \"{fileInfo}\"",
                        logOutput: false,
                        logInvocation: false,
                        logTimestamp: false
                        ); // don't print to std out/err
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            try
            {
                System.Security.Cryptography.X509Certificates.X509Certificate.CreateFromSignedFile(fileInfo);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}
