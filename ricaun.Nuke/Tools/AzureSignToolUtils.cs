﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ricaun.Nuke.Tools.NuGetKeyVaultSignTool;
using Nuke.Common.Tools.AzureSignTool;
using System.IO;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using ricaun.Nuke.Extensions;

namespace ricaun.Nuke.Tools
{
    /// <summary>
    /// Utility class for working with Azure Sign Tool.
    /// </summary>
    public class AzureSignToolUtils
    {
        private const string TimestampUrlDefault = "http://timestamp.digicert.com";
        private const string TimestampDigestDefault = "sha256";

        /// <summary>
        /// Ensures that Azure Sign Tool and NuGet Key Vault Sign Tool are installed.
        /// </summary>
        /// <exception cref="Exception">Thrown when the required packages are missing.</exception>
        public static void EnsureAzureToolIsInstalled()
        {
            DownloadAzureSignTool();
            DownloadNuGetKeyVaultSignTool();

            try
            {
                _ = AzureSignToolTasks.AzureSignToolPath;
                _ = NuGetKeyVaultSignToolTasks.NuGetKeyVaultSignToolPath;
            }
            catch (Exception ex)
            {
                var packagesToInstall = """
                    <ItemGroup>
                      <PackageDownload Include="AzureSignTool" Version="[6.0.0]" />
                      <PackageDownload Include="NuGetKeyVaultSignTool" Version="[3.2.3]" />
                    </ItemGroup>
                    """;
                throw new Exception($"Missing package reference/download, install the packages in the project: \n{packagesToInstall}", ex);
            }
        }

        private static AbsolutePath GetToolInstallationPath()
        {
            var assemblyName = typeof(AzureSignToolUtils).Assembly.GetName();
            AbsolutePath folder = (AbsolutePath) Path.GetTempPath() / assemblyName.Name / assemblyName.Version.ToString(3);
            return folder / "Tools";
        }

        private static string PackageDownload(string packageId)
        {
            var toolFolder = GetToolInstallationPath();

            if (Globbing.GlobFiles(toolFolder, $"{packageId}.exe").FirstOrDefault() is AbsolutePath packageToolExeExists)
            {
                return packageToolExeExists;
            }

            try
            {
                // Force to uninstall to remove cache files if exists.
                DotNetTasks.DotNetToolUninstall(x => x
                    .SetPackageName(packageId)
                    .SetToolInstallationPath(toolFolder)
                    .DisableProcessLogInvocation()
                    .DisableProcessLogOutput()
            );
            }
            catch { }

            DotNetTasks.DotNetToolInstall(x => x
                .SetPackageName(packageId)
                .SetToolInstallationPath(toolFolder)
            );

            if (Globbing.GlobFiles(toolFolder, $"{packageId}.exe").FirstOrDefault() is AbsolutePath packageToolExe)
            {
                return packageToolExe;
            }
            return null;
        }

        /// <summary>
        /// Download AzureSignTool if not already installed.
        /// </summary>
        public static void DownloadAzureSignTool()
        {
            var packageId = AzureSignToolTasks.AzureSignToolPackageId;
            var packageIdExe = packageId.ToUpper() + "_EXE";

            if (ToolPathResolver.TryGetEnvironmentExecutable(packageIdExe) is null)
            {
                var packageToolExe = PackageDownload(packageId);
                Environment.SetEnvironmentVariable(packageIdExe, packageToolExe);
            }

            _ = AzureSignToolTasks.AzureSignToolPath;
        }

        /// <summary>
        /// Download NuGetKeyVaultSignTool if not already installed.
        /// </summary>
        public static void DownloadNuGetKeyVaultSignTool()
        {
            var packageId = NuGetKeyVaultSignToolTasks.NuGetKeyVaultSignToolPackageId;
            var packageIdExe = packageId.ToUpper() + "_EXE";

            if (ToolPathResolver.TryGetEnvironmentExecutable(packageIdExe) is null)
            {
                var packageToolExe = PackageDownload(packageId);
                Environment.SetEnvironmentVariable(packageIdExe, packageToolExe);
            }

            _ = NuGetKeyVaultSignToolTasks.NuGetKeyVaultSignToolPath;
        }

        /// <summary>
        /// Signs the specified file using Azure Sign Tool or NuGet Key Vault Sign Tool.
        /// </summary>
        /// <param name="filePath">The name of the file to sign.</param>
        /// <param name="azureKeyVaultConfig">The Azure Key Vault configuration.</param>
        /// <param name="azureKeyVaultClientSecret">The Azure Key Vault client secret.</param>
        /// <param name="timestampUrlDefault">The default timestamp URL.</param>
        /// <param name="timestampDigestDefault">The default timestamp digest.</param>
        public static void Sign(string filePath,
            AzureKeyVaultConfig azureKeyVaultConfig, string azureKeyVaultClientSecret,
            string timestampUrlDefault = TimestampUrlDefault,
            string timestampDigestDefault = TimestampDigestDefault)
        {
            try
            {
                if (SignExtension.HasSignature(filePath))
                    return;

                if (NuGetExtension.IsNuGetFile(filePath))
                {
                    DownloadNuGetKeyVaultSignTool();
                    NuGetKeyVaultSignToolTasks.NuGetKeyVaultSignTool(x => x
                        .SetFile(filePath)
                        .SetKeyVaultCertificateName(azureKeyVaultConfig.AzureKeyVaultCertificate)
                        .SetKeyVaultUrl(azureKeyVaultConfig.AzureKeyVaultUrl)
                        .SetKeyVaultClientId(azureKeyVaultConfig.AzureKeyVaultClientId)
                        .SetKeyVaultTenantId(azureKeyVaultConfig.AzureKeyVaultTenantId)
                        .SetKeyVaultClientSecret(azureKeyVaultClientSecret)
                        .SetTimestampRfc3161Url(azureKeyVaultConfig.TimestampUrl ?? timestampUrlDefault)
                        .SetTimestampDigest(azureKeyVaultConfig.TimestampDigest ?? timestampDigestDefault)
                    );
                    return;
                }

                DownloadAzureSignTool();
                AzureSignToolTasks.AzureSignTool(x => x
                    .SetFiles(filePath)
                    .SetKeyVaultCertificateName(azureKeyVaultConfig.AzureKeyVaultCertificate)
                    .SetKeyVaultUrl(azureKeyVaultConfig.AzureKeyVaultUrl)
                    .SetKeyVaultClientId(azureKeyVaultConfig.AzureKeyVaultClientId)
                    .SetKeyVaultTenantId(azureKeyVaultConfig.AzureKeyVaultTenantId)
                    .SetKeyVaultClientSecret(azureKeyVaultClientSecret)
                    .SetTimestampRfc3161Url(azureKeyVaultConfig.TimestampUrl ?? timestampUrlDefault)
                    .SetTimestampDigest(azureKeyVaultConfig.TimestampDigest ?? timestampDigestDefault)
                );
            }
            catch (Exception ex)
            {
                Serilog.Log.Error($"Azure Sign Error: {Path.GetFileName(filePath)} - {ex.Message}");
                Serilog.Log.Information(ex.ToString());
            }
        }
    }

    /// <summary>
    /// Represents the configuration for Azure Key Vault.
    /// </summary>
    public class AzureKeyVaultConfig
    {
        /// <summary>
        /// Gets or sets the Azure Key Vault certificate.
        /// </summary>
        public string AzureKeyVaultCertificate { get; set; }

        /// <summary>
        /// Gets or sets the Azure Key Vault URL.
        /// </summary>
        public string AzureKeyVaultUrl { get; set; }

        /// <summary>
        /// Gets or sets the Azure Key Vault client ID.
        /// </summary>
        public string AzureKeyVaultClientId { get; set; }

        /// <summary>
        /// Gets or sets the Azure Key Vault tenant ID.
        /// </summary>
        public string AzureKeyVaultTenantId { get; set; }

        /// <summary>
        /// Gets or sets the timestamp URL.
        /// </summary>
        public string TimestampUrl { get; set; }

        /// <summary>
        /// Gets or sets the timestamp digest.
        /// </summary>
        public string TimestampDigest { get; set; }

        /// <summary>
        /// Creates an instance of <see cref="AzureKeyVaultConfig"/> from the specified JSON content.
        /// </summary>
        /// <param name="jsonContent">The JSON content representing the Azure Key Vault configuration.</param>
        /// <returns>An instance of <see cref="AzureKeyVaultConfig"/>.</returns>
        public static AzureKeyVaultConfig Create(string jsonContent)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<AzureKeyVaultConfig>(jsonContent);
            }
            catch { }
            return default;
        }

        /// <summary>
        /// Checks if the Azure Key Vault configuration is valid.
        /// </summary>
        /// <returns><c>true</c> if the configuration is valid; otherwise, <c>false</c>.</returns>
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(AzureKeyVaultCertificate) &&
                   !string.IsNullOrEmpty(AzureKeyVaultUrl) &&
                   !string.IsNullOrEmpty(AzureKeyVaultClientId) &&
                   !string.IsNullOrEmpty(AzureKeyVaultTenantId);
        }
    }
}
