using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ricaun.Nuke.Tools.NuGetKeyVaultSignTool;
using Nuke.Common.Tools.AzureSignTool;
using System.IO;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.IO;

namespace ricaun.Nuke.Tools
{
    /// <summary>
    /// Utility class for working with Azure Sign Tool.
    /// </summary>
    public class AzureSignToolUtils
    {
        private const string TimestampUrlDefault = "http://timestamp.digicert.com";
        private const string TimestampDigestDefault = "sha256";
        private const string NugetPackageExtension = ".nupkg";

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
            AbsolutePath folder = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            return folder / "Tools";
        }

        private static string PackageDownload(string packageId)
        {
            var toolFolder = GetToolInstallationPath();

            DotNetTasks.DotNetToolInstall(x => x
                .SetPackageName(packageId)
                .SetToolInstallationPath(toolFolder)
            );

            if (Globbing.GlobFiles(toolFolder, $"{packageId}.exe").FirstOrDefault() is AbsolutePath absolutePath)
            {
                return absolutePath;
            }
            return null;
        }

        /// <summary>
        /// Download AzureSignTool if not already installed.
        /// </summary>
        public static void DownloadAzureSignTool()
        {
            try
            {
                _ = AzureSignToolTasks.AzureSignToolPath;
            }
            catch (Exception)
            {
                var packageId = AzureSignToolTasks.AzureSignToolPackageId;
                var packageToolExe = PackageDownload(packageId);

                Environment.SetEnvironmentVariable(packageId.ToUpper() + "_EXE", packageToolExe);
            }

            _ = AzureSignToolTasks.AzureSignToolPath;
        }

        /// <summary>
        /// Download NuGetKeyVaultSignTool if not already installed.
        /// </summary>
        public static void DownloadNuGetKeyVaultSignTool()
        {
            try
            {
                _ = NuGetKeyVaultSignToolTasks.NuGetKeyVaultSignToolPath;
            }
            catch (Exception)
            {
                var packageId = NuGetKeyVaultSignToolTasks.NuGetKeyVaultSignToolPackageId;
                var packageToolExe = PackageDownload(packageId);

                Environment.SetEnvironmentVariable(packageId.ToUpper() + "_EXE", packageToolExe);
            }

            _ = NuGetKeyVaultSignToolTasks.NuGetKeyVaultSignToolPath;
        }

        private static void DownloadNuGetKeyVaultSignTool_()
        {
            try
            {
                _ = NuGetKeyVaultSignToolTasks.NuGetKeyVaultSignToolPath;
            }
            catch (Exception)
            {
                var toolFolder = GetToolInstallationPath();
                var packageId = NuGetKeyVaultSignToolTasks.NuGetKeyVaultSignToolPackageId;

                DotNetTasks.DotNetToolInstall(x => x
                    .SetPackageName(packageId)
                    .SetToolInstallationPath(toolFolder)
                );

                if (Globbing.GlobFiles(toolFolder, $"{packageId}.exe").FirstOrDefault() is AbsolutePath packageToolPath)
                {
                    Environment.SetEnvironmentVariable(packageId.ToUpper() + "_EXE", packageToolPath);
                }
            }

            _ = NuGetKeyVaultSignToolTasks.NuGetKeyVaultSignToolPath;
        }

        /// <summary>
        /// Signs the specified file using Azure Sign Tool or NuGet Key Vault Sign Tool.
        /// </summary>
        /// <param name="fileName">The name of the file to sign.</param>
        /// <param name="azureKeyVaultConfig">The Azure Key Vault configuration.</param>
        /// <param name="azureKeyVaultClientSecret">The Azure Key Vault client secret.</param>
        /// <param name="timestampUrlDefault">The default timestamp URL.</param>
        /// <param name="timestampDigestDefault">The default timestamp digest.</param>
        public static void Sign(string fileName,
            AzureKeyVaultConfig azureKeyVaultConfig, string azureKeyVaultClientSecret,
            string timestampUrlDefault = TimestampUrlDefault,
            string timestampDigestDefault = TimestampDigestDefault)
        {
            if (Path.GetExtension(fileName) == NugetPackageExtension)
            {
                DownloadNuGetKeyVaultSignTool();
                NuGetKeyVaultSignToolTasks.NuGetKeyVaultSignTool(x => x
                    .SetFile(fileName)
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
                .SetFiles(fileName)
                .SetKeyVaultCertificateName(azureKeyVaultConfig.AzureKeyVaultCertificate)
                .SetKeyVaultUrl(azureKeyVaultConfig.AzureKeyVaultUrl)
                .SetKeyVaultClientId(azureKeyVaultConfig.AzureKeyVaultClientId)
                .SetKeyVaultTenantId(azureKeyVaultConfig.AzureKeyVaultTenantId)
                .SetKeyVaultClientSecret(azureKeyVaultClientSecret)
                .SetTimestampRfc3161Url(azureKeyVaultConfig.TimestampUrl ?? timestampUrlDefault)
                .SetTimestampDigest(azureKeyVaultConfig.TimestampDigest ?? timestampDigestDefault)
            );
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
