using Nuke.Common;
using Nuke.Common.Tools.NuGet;
using Nuke.Common.Tools.SignTool;
using System;
using System.IO;

namespace ricaun.Nuke.Extensions
{
    /// <summary>
    /// SignExtension
    /// </summary>
    public static class SignExtension
    {
        #region Sign Util

        /// <summary>
        /// timestampServers
        /// </summary>
        static readonly string[] timestampServers = {
            "http://timestamp.digicert.com",
            "http://timestamp.comodoca.com",
        };

        /// <summary>
        /// VerifySignFile
        /// </summary>
        /// <param name="fileNamePfx"></param>
        /// <param name="tempFolderDownloadFile"></param>
        /// <returns></returns>
        public static string VerifySignFile(string fileNamePfx, string tempFolderDownloadFile)
        {
            if (File.Exists(fileNamePfx))
                return fileNamePfx;

            var file = Path.Combine(tempFolderDownloadFile, "signfile.pfx");
            HttpClientExtension.DownloadFile(fileNamePfx, file);
            return file;
        }

        /// <summary>
        /// Create a Cer Certificate with a <paramref name="fileNamePfx"/>
        /// </summary>
        /// <param name="fileNamePfx"></param>
        /// <param name="passwordPfx"></param>
        /// <param name="tempFolderDownloadFile"></param>
        /// <returns></returns>
        public static bool CreateCerFile(string fileNamePfx, string passwordPfx, string tempFolderDownloadFile)
        {
            var cert = Path.Combine(tempFolderDownloadFile, "certificate.cer");
            if (File.Exists(cert)) return true;
            return CreateCertificatesCer(fileNamePfx, passwordPfx, cert);
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

            Serilog.Log.Information($"Signing: {binaryPath}");

            foreach (var timestampServer in timestampServers)
            {
                try
                {
                    SignToolTasks.SignTool(x => x
                        .SetFiles(binaryPath)
                        .SetFile(certPath)
                        .SetPassword(certPassword)
                        .SetTimestampServerUrl(timestampServer)
                        .SetFileDigestAlgorithm(SignToolDigestAlgorithm.SHA256)
                        .EnableQuiet()
                    );
                    Serilog.Log.Information($"Signing done with {timestampServer}");
                    return;
                }
                catch (Exception)
                {
                    Serilog.Log.Warning($"Failed to sign file with {timestampServer}");
                }
            }
            Serilog.Log.Error($"Failed to sign file {binaryPath}");
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

            Serilog.Log.Information($"Signing: {binaryPath}");

            foreach (var timestampServer in timestampServers)
            {
                try
                {
                    NuGetTasks.NuGet(
                        $"sign \"{binaryPath}\"" +
                        $" -CertificatePath {certPath}" +
                        $" -CertificatePassword {certPassword}" +
                        $" -Timestamper {timestampServer} -NonInteractive",
                        logOutput: false,
                        logInvocation: false
                        ); // don't print to std out/err

                    Serilog.Log.Information($"Signing done with {timestampServer}");
                    return;
                }
                catch (Exception)
                {
                    Serilog.Log.Warning($"Failed to sign file with {timestampServer}");
                }
            }

            Serilog.Log.Error($"Failed to sign nuget package {binaryPath}");
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
                        logInvocation: false
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

        /// <summary>
        /// Create a Cer Certificate with a <paramref name="fileNamePfx"/>
        /// </summary>
        /// <param name="fileNamePfx"></param>
        /// <param name="passwordPfx"></param>
        /// <param name="outputCer"></param>
        /// <returns></returns>
        static bool CreateCertificatesCer(string fileNamePfx, string passwordPfx, string outputCer)
        {
            try
            {
                var certificates = new System.Security.Cryptography.X509Certificates.X509Certificate2(fileNamePfx, passwordPfx);
                File.WriteAllBytes(outputCer, certificates.Export(System.Security.Cryptography.X509Certificates.X509ContentType.Cert));
                return true;
            }
            catch (Exception)
            {
                Serilog.Log.Error($"Failed to create 'cer' file {outputCer}");
            }
            return false;
        }

        #endregion
    }
}
