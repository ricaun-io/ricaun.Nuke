using Nuke.Common;
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

            if (File.Exists(file))
                return file;

            if (CreateFileIfBase64(fileNamePfx, file))
            {
                Serilog.Log.Information($"SignFile Create Base64");
                return file;
            }

            Serilog.Log.Information($"SignFile Create Download");
            HttpClientExtension.DownloadFileRetry(fileNamePfx, file);
            return file;
        }

        private static bool CreateFileIfBase64(string base64, string file)
        {
            Span<byte> buffer = new Span<byte>(new byte[base64.Length]);
            if (Convert.TryFromBase64String(base64, buffer, out int _))
            {
                File.WriteAllBytes(file, buffer.ToArray());
                return true;
            }
            return false;
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
        /// Sign the specified file using the provided certificate.
        /// </summary>
        /// <param name="certPath">The path to the certificate file.</param>
        /// <param name="certPassword">The password for the certificate.</param>
        /// <param name="filePath">The path to the file to be signed.</param>
        /// <remarks>NuGet files use <see cref="NuGetExtension.NuGetSign"/>.</remarks>
        public static void Sign(string certPath, string certPassword, string filePath)
        {
            if (NuGetExtension.IsNuGetFile(filePath))
            {
                SignNuGet(certPath, certPassword, filePath);
                return;
            }
            SignBinary(certPath, certPassword, filePath);
        }

        /// <summary>
        /// https://github.com/DataDog/dd-trace-dotnet/blob/master/tracer/build/_build/Build.Gitlab.cs
        /// </summary>
        /// <param name="certPath"></param>
        /// <param name="certPassword"></param>
        /// <param name="binaryPath"></param>
        public static void SignBinary(string certPath, string certPassword, string binaryPath)
        {
            using (var utils = new PathTooLongUtils.FileMoveToTemp(binaryPath))
            {
                var filePath = utils.GetFilePath();
                if (utils.IsPathTooLong())
                {
                    var messageError = $"FilePath.Length too long: {filePath}";
                    Serilog.Log.Error(messageError);
                    throw new PathTooLongException(messageError);
                }

                if (HasSignature(filePath)) return;

                Serilog.Log.Information($"Signing [{utils.GetFilePathLong()}]: {filePath}");

                if (!SignToolTasks.SignToolPath.SkipEmpty())
                {
                    Serilog.Log.Error($"SignToolPath is not found, set SIGNTOOL_EXE enviroment variable path... {SignToolTasks.SignToolPath}");
                    return;
                }

                foreach (var timestampServer in timestampServers)
                {
                    try
                    {
                        SignToolTasks.SignTool(x => x
                            .SetFiles(filePath)
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
                Serilog.Log.Error($"Failed to sign file {filePath}");
            }
        }

        /// <summary>
        /// Sign NuGet
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
                if (NuGetExtension.NuGetSign(binaryPath, certPath, certPassword, timestampServer))
                {
                    Serilog.Log.Information($"Signing done with {timestampServer}");
                    return;
                }
                Serilog.Log.Warning($"Failed to sign file with {timestampServer}");
            }

            Serilog.Log.Error($"Failed to sign nuget package {binaryPath}");
        }

        /// <summary>
        /// Has Signature in the file or NuGet
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool HasSignature(string filePath)
        {
            if (NuGetExtension.IsNuGetFile(filePath))
            {
                return NuGetExtension.NuGetVerifySignatures(filePath);
            }

            try
            {
                using (var utils = new PathTooLongUtils.FileMoveToTemp(filePath))
                {
                    filePath = utils.GetFilePath();
                    System.Security.Cryptography.X509Certificates.X509Certificate.CreateFromSignedFile(filePath);
                }
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
