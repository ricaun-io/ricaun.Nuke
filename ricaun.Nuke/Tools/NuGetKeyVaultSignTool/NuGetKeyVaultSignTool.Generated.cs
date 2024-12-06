
using JetBrains.Annotations;
using Newtonsoft.Json;
using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools;
using Nuke.Common.Utilities.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;

namespace Nuke.Common.Tools.NuGetKeyVaultSignTool;

/// <summary>
///   <p>NuGet Key Vault Sign Tool is similar to <c>nuget sign</c>, with the major difference being that it uses Azure Key Vault for performing the signing process. Similar usage configuration like <c>AzureSignTool</c>, except is used to sign nuget package.</p>
///   <p>For more details, visit the <a href="https://github.com/novotnyllc/NuGetKeyVaultSignTool">official website</a>.</p>
/// </summary>
[PublicAPI]
[ExcludeFromCodeCoverage]
[NuGetPackageRequirement(NuGetKeyVaultSignToolPackageId)]
public partial class NuGetKeyVaultSignToolTasks
    : IRequireNuGetPackage
{
    /// <summary>
    /// NuGetKeyVaultSignToolPackageId
    /// </summary>
    public const string NuGetKeyVaultSignToolPackageId = "NuGetKeyVaultSignTool";
    /// <summary>
    ///   Path to the NuGetKeyVaultSignTool executable.
    /// </summary>
    public static string NuGetKeyVaultSignToolPath =>
        ToolPathResolver.TryGetEnvironmentExecutable("NUGETKEYVAULTSIGNTOOL_EXE") ??
        NuGetToolPathResolver.GetPackageExecutable("NuGetKeyVaultSignTool", "NuGetKeyVaultSignTool.dll");
    /// <summary>
    /// NuGetKeyVaultSignToolLogger
    /// </summary>
    public static Action<OutputType, string> NuGetKeyVaultSignToolLogger { get; set; } = ProcessTasks.DefaultLogger;
    /// <summary>
    /// NuGetKeyVaultSignToolExitHandler
    /// </summary>
    public static Action<ToolSettings, IProcess> NuGetKeyVaultSignToolExitHandler { get; set; } = ProcessTasks.DefaultExitHandler;
    /// <summary>
    ///   <p>NuGet Key Vault Sign Tool is similar to <c>nuget sign</c>, with the major difference being that it uses Azure Key Vault for performing the signing process. Similar usage configuration like <c>AzureSignTool</c>, except is used to sign nuget package.</p>
    ///   <p>For more details, visit the <a href="https://github.com/novotnyllc/NuGetKeyVaultSignTool">official website</a>.</p>
    /// </summary>
    public static IReadOnlyCollection<Output> NuGetKeyVaultSignTool(ArgumentStringHandler arguments, string workingDirectory = null, IReadOnlyDictionary<string, string> environmentVariables = null, int? timeout = null, bool? logOutput = null, bool? logInvocation = null, Action<OutputType, string> logger = null, Action<IProcess> exitHandler = null)
    {
        using var process = ProcessTasks.StartProcess(NuGetKeyVaultSignToolPath, arguments, workingDirectory, environmentVariables, timeout, logOutput, logInvocation, logger ?? NuGetKeyVaultSignToolLogger);
        (exitHandler ?? (p => NuGetKeyVaultSignToolExitHandler.Invoke(null, p))).Invoke(process.AssertWaitForExit());
        return process.Output;
    }
    /// <summary>
    ///   <p>NuGet Key Vault Sign Tool is similar to <c>nuget sign</c>, with the major difference being that it uses Azure Key Vault for performing the signing process. Similar usage configuration like <c>AzureSignTool</c>, except is used to sign nuget package.</p>
    ///   <p>For more details, visit the <a href="https://github.com/novotnyllc/NuGetKeyVaultSignTool">official website</a>.</p>
    /// </summary>
    /// <remarks>
    ///   <p>This is a <a href="http://www.nuke.build/docs/authoring-builds/cli-tools.html#fluent-apis">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p>
    ///   <ul>
    ///     <li><c>&lt;file&gt;</c> via <see cref="NuGetKeyVaultSignToolSettings.File"/></li>
    ///     <li><c>--azure-key-vault-accesstoken</c> via <see cref="NuGetKeyVaultSignToolSettings.KeyVaultAccessToken"/></li>
    ///     <li><c>--azure-key-vault-certificate</c> via <see cref="NuGetKeyVaultSignToolSettings.KeyVaultCertificateName"/></li>
    ///     <li><c>--azure-key-vault-client-id</c> via <see cref="NuGetKeyVaultSignToolSettings.KeyVaultClientId"/></li>
    ///     <li><c>--azure-key-vault-client-secret</c> via <see cref="NuGetKeyVaultSignToolSettings.KeyVaultClientSecret"/></li>
    ///     <li><c>--azure-key-vault-managed-identity</c> via <see cref="NuGetKeyVaultSignToolSettings.KeyVaultManagedIdentity"/></li>
    ///     <li><c>--azure-key-vault-tenant-id</c> via <see cref="NuGetKeyVaultSignToolSettings.KeyVaultTenantId"/></li>
    ///     <li><c>--azure-key-vault-url</c> via <see cref="NuGetKeyVaultSignToolSettings.KeyVaultUrl"/></li>
    ///     <li><c>--file-digest</c> via <see cref="NuGetKeyVaultSignToolSettings.FileDigest"/></li>
    ///     <li><c>--force</c> via <see cref="NuGetKeyVaultSignToolSettings.Force"/></li>
    ///     <li><c>--output</c> via <see cref="NuGetKeyVaultSignToolSettings.Output"/></li>
    ///     <li><c>--timestamp-digest</c> via <see cref="NuGetKeyVaultSignToolSettings.TimestampDigest"/></li>
    ///     <li><c>--timestamp-rfc3161</c> via <see cref="NuGetKeyVaultSignToolSettings.TimestampRfc3161Url"/></li>
    ///   </ul>
    /// </remarks>
    public static IReadOnlyCollection<Output> NuGetKeyVaultSignTool(NuGetKeyVaultSignToolSettings toolSettings = null)
    {
        toolSettings = toolSettings ?? new NuGetKeyVaultSignToolSettings();
        using var process = ProcessTasks.StartProcess(toolSettings);
        toolSettings.ProcessExitHandler.Invoke(toolSettings, process.AssertWaitForExit());
        return process.Output;
    }
    /// <summary>
    ///   <p>NuGet Key Vault Sign Tool is similar to <c>nuget sign</c>, with the major difference being that it uses Azure Key Vault for performing the signing process. Similar usage configuration like <c>AzureSignTool</c>, except is used to sign nuget package.</p>
    ///   <p>For more details, visit the <a href="https://github.com/novotnyllc/NuGetKeyVaultSignTool">official website</a>.</p>
    /// </summary>
    /// <remarks>
    ///   <p>This is a <a href="http://www.nuke.build/docs/authoring-builds/cli-tools.html#fluent-apis">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p>
    ///   <ul>
    ///     <li><c>&lt;file&gt;</c> via <see cref="NuGetKeyVaultSignToolSettings.File"/></li>
    ///     <li><c>--azure-key-vault-accesstoken</c> via <see cref="NuGetKeyVaultSignToolSettings.KeyVaultAccessToken"/></li>
    ///     <li><c>--azure-key-vault-certificate</c> via <see cref="NuGetKeyVaultSignToolSettings.KeyVaultCertificateName"/></li>
    ///     <li><c>--azure-key-vault-client-id</c> via <see cref="NuGetKeyVaultSignToolSettings.KeyVaultClientId"/></li>
    ///     <li><c>--azure-key-vault-client-secret</c> via <see cref="NuGetKeyVaultSignToolSettings.KeyVaultClientSecret"/></li>
    ///     <li><c>--azure-key-vault-managed-identity</c> via <see cref="NuGetKeyVaultSignToolSettings.KeyVaultManagedIdentity"/></li>
    ///     <li><c>--azure-key-vault-tenant-id</c> via <see cref="NuGetKeyVaultSignToolSettings.KeyVaultTenantId"/></li>
    ///     <li><c>--azure-key-vault-url</c> via <see cref="NuGetKeyVaultSignToolSettings.KeyVaultUrl"/></li>
    ///     <li><c>--file-digest</c> via <see cref="NuGetKeyVaultSignToolSettings.FileDigest"/></li>
    ///     <li><c>--force</c> via <see cref="NuGetKeyVaultSignToolSettings.Force"/></li>
    ///     <li><c>--output</c> via <see cref="NuGetKeyVaultSignToolSettings.Output"/></li>
    ///     <li><c>--timestamp-digest</c> via <see cref="NuGetKeyVaultSignToolSettings.TimestampDigest"/></li>
    ///     <li><c>--timestamp-rfc3161</c> via <see cref="NuGetKeyVaultSignToolSettings.TimestampRfc3161Url"/></li>
    ///   </ul>
    /// </remarks>
    public static IReadOnlyCollection<Output> NuGetKeyVaultSignTool(Configure<NuGetKeyVaultSignToolSettings> configurator)
    {
        return NuGetKeyVaultSignTool(configurator(new NuGetKeyVaultSignToolSettings()));
    }
    /// <summary>
    ///   <p>NuGet Key Vault Sign Tool is similar to <c>nuget sign</c>, with the major difference being that it uses Azure Key Vault for performing the signing process. Similar usage configuration like <c>AzureSignTool</c>, except is used to sign nuget package.</p>
    ///   <p>For more details, visit the <a href="https://github.com/novotnyllc/NuGetKeyVaultSignTool">official website</a>.</p>
    /// </summary>
    /// <remarks>
    ///   <p>This is a <a href="http://www.nuke.build/docs/authoring-builds/cli-tools.html#fluent-apis">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p>
    ///   <ul>
    ///     <li><c>&lt;file&gt;</c> via <see cref="NuGetKeyVaultSignToolSettings.File"/></li>
    ///     <li><c>--azure-key-vault-accesstoken</c> via <see cref="NuGetKeyVaultSignToolSettings.KeyVaultAccessToken"/></li>
    ///     <li><c>--azure-key-vault-certificate</c> via <see cref="NuGetKeyVaultSignToolSettings.KeyVaultCertificateName"/></li>
    ///     <li><c>--azure-key-vault-client-id</c> via <see cref="NuGetKeyVaultSignToolSettings.KeyVaultClientId"/></li>
    ///     <li><c>--azure-key-vault-client-secret</c> via <see cref="NuGetKeyVaultSignToolSettings.KeyVaultClientSecret"/></li>
    ///     <li><c>--azure-key-vault-managed-identity</c> via <see cref="NuGetKeyVaultSignToolSettings.KeyVaultManagedIdentity"/></li>
    ///     <li><c>--azure-key-vault-tenant-id</c> via <see cref="NuGetKeyVaultSignToolSettings.KeyVaultTenantId"/></li>
    ///     <li><c>--azure-key-vault-url</c> via <see cref="NuGetKeyVaultSignToolSettings.KeyVaultUrl"/></li>
    ///     <li><c>--file-digest</c> via <see cref="NuGetKeyVaultSignToolSettings.FileDigest"/></li>
    ///     <li><c>--force</c> via <see cref="NuGetKeyVaultSignToolSettings.Force"/></li>
    ///     <li><c>--output</c> via <see cref="NuGetKeyVaultSignToolSettings.Output"/></li>
    ///     <li><c>--timestamp-digest</c> via <see cref="NuGetKeyVaultSignToolSettings.TimestampDigest"/></li>
    ///     <li><c>--timestamp-rfc3161</c> via <see cref="NuGetKeyVaultSignToolSettings.TimestampRfc3161Url"/></li>
    ///   </ul>
    /// </remarks>
    public static IEnumerable<(NuGetKeyVaultSignToolSettings Settings, IReadOnlyCollection<Output> Output)> NuGetKeyVaultSignTool(CombinatorialConfigure<NuGetKeyVaultSignToolSettings> configurator, int degreeOfParallelism = 1, bool completeOnFailure = false)
    {
        return configurator.Invoke(NuGetKeyVaultSignTool, NuGetKeyVaultSignToolLogger, degreeOfParallelism, completeOnFailure);
    }
}
#region NuGetKeyVaultSignToolSettings
/// <summary>
///   Used within <see cref="NuGetKeyVaultSignToolTasks"/>.
/// </summary>
[PublicAPI]
[ExcludeFromCodeCoverage]
[Serializable]
public partial class NuGetKeyVaultSignToolSettings : ToolSettings
{
    /// <summary>
    ///   Path to the NuGetKeyVaultSignTool executable.
    /// </summary>
    public override string ProcessToolPath => base.ProcessToolPath ?? NuGetKeyVaultSignToolTasks.NuGetKeyVaultSignToolPath;
    /// <summary>
    /// ProcessLogger
    /// </summary>
    public override Action<OutputType, string> ProcessLogger => base.ProcessLogger ?? NuGetKeyVaultSignToolTasks.NuGetKeyVaultSignToolLogger;
    /// <summary>
    /// ProcessExitHandler
    /// </summary>
    public override Action<ToolSettings, IProcess> ProcessExitHandler => base.ProcessExitHandler ?? NuGetKeyVaultSignToolTasks.NuGetKeyVaultSignToolExitHandler;
    /// <summary>
    ///   Package to sign.
    /// </summary>
    public virtual string File { get; internal set; }
    /// <summary>
    ///   A fully qualified URL of the key vault with the certificate that will be used for signing. An example value might be <c>https://my-vault.vault.azure.net</c>.
    /// </summary>
    public virtual string KeyVaultUrl { get; internal set; }
    /// <summary>
    ///   This is the client ID used to authenticate to Azure, which will be used to generate an access token. This parameter is not required if an access token is supplied directly with the <c>--azure-key-vault-accesstoken</c> option. If this parameter is supplied, <c>--azure-key-vault-client-secret</c> and <c>--azure-key-vault-tenant-id</c> must be supplied as well.
    /// </summary>
    public virtual string KeyVaultClientId { get; internal set; }
    /// <summary>
    ///   This is the client secret used to authenticate to Azure, which will be used to generate an access token. This parameter is not required if an access token is supplied directly with the <c>--azure-key-vault-accesstoken</c> option or when using managed identities with <c>--azure-key-vault-managed-identity</c>. If this parameter is supplied, <c>--azure-key-vault-client-id</c> and <c>--azure-key-vault-tenant-id</c> must be supplied as well.
    /// </summary>
    public virtual string KeyVaultClientSecret { get; internal set; }
    /// <summary>
    ///   This is the tenant id used to authenticate to Azure, which will be used to generate an access token. This parameter is not required if an access token is supplied directly with the <c>--azure-key-vault-accesstoken</c> option or when using managed identities with <c>--azure-key-vault-managed-identity</c>. If this parameter is supplied, <c>--azure-key-vault-client-id</c> and <c>--azure-key-vault-client-secret</c> must be supplied as well.
    /// </summary>
    public virtual string KeyVaultTenantId { get; internal set; }
    /// <summary>
    ///   The name of the certificate used to perform the signing operation.
    /// </summary>
    public virtual string KeyVaultCertificateName { get; internal set; }
    /// <summary>
    ///   An access token used to authenticate to Azure. This can be used instead of the <c>--azure-key-vault-managed-identity</c>, <c>--azure-key-vault-client-id</c> and <c>--azure-key-vault-client-secret</c> options. This is useful if NuGetKeyVaultSignTool is being used as part of another program that is already authenticated and has an access token to Azure.
    /// </summary>
    public virtual string KeyVaultAccessToken { get; internal set; }
    /// <summary>
    ///   Use the ambient Managed Identity to authenticate to Azure. This can be used instead of the <c>--azure-key-vault-accesstoken</c>, <c>--azure-key-vault-client-id</c> and <c>--azure-key-vault-client-secret</c> options. This is useful if NuGetKeyVaultSignTool is being used on a VM/service/CLI that is configured for managed identities to Azure.
    /// </summary>
    public virtual bool? KeyVaultManagedIdentity { get; internal set; }
    /// <summary>
    ///   A URL to an RFC3161 compliant timestamping service. This parameter serves the same purpose as the <c>/tr</c> option in the Windows SDK <c>signtool</c>. This parameter should be used in favor of the <c>--timestamp</c> option. Using this parameter will allow using modern, RFC3161 timestamps which also support timestamp digest algorithms other than SHA1.
    /// </summary>
    public virtual string TimestampRfc3161Url { get; internal set; }
    /// <summary>
    ///   The name of the digest algorithm used for timestamping. This parameter is ignored unless the <c>--timestamp-rfc3161</c> parameter is also supplied. The default value is <c>sha256</c>.
    /// </summary>
    public virtual NuGetKeyVaultSignToolDigestAlgorithm TimestampDigest { get; internal set; }
    /// <summary>
    ///   The name of the digest algorithm used for hashing the file being signed. The default value is <c>sha256</c>.
    /// </summary>
    public virtual NuGetKeyVaultSignToolDigestAlgorithm FileDigest { get; internal set; }
    /// <summary>
    ///   Overwrites a signature if it exists.
    /// </summary>
    public virtual bool? Force { get; internal set; }
    /// <summary>
    ///   The output file. If omitted, overwrites input.
    /// </summary>
    public virtual string Output { get; internal set; }
    /// <summary>
    /// ConfigureProcessArguments
    /// </summary>
    /// <param name="arguments"></param>
    /// <returns></returns>
    protected override Arguments ConfigureProcessArguments(Arguments arguments)
    {
        arguments
          .Add("sign")
          .Add("{value}", File)
          .Add("--azure-key-vault-url {value}", KeyVaultUrl)
          .Add("--azure-key-vault-client-id {value}", KeyVaultClientId)
          .Add("--azure-key-vault-client-secret {value}", KeyVaultClientSecret, secret: true)
          .Add("--azure-key-vault-tenant-id {value}", KeyVaultTenantId)
          .Add("--azure-key-vault-certificate {value}", KeyVaultCertificateName)
          .Add("--azure-key-vault-accesstoken {value}", KeyVaultAccessToken, secret: true)
          .Add("--azure-key-vault-managed-identity", KeyVaultManagedIdentity)
          .Add("--timestamp-rfc3161 {value}", TimestampRfc3161Url)
          .Add("--timestamp-digest {value}", TimestampDigest)
          .Add("--file-digest {value}", FileDigest)
          .Add("--force", Force)
          .Add("--output {value}", Output);
        return base.ConfigureProcessArguments(arguments);
    }
}
#endregion
#region NuGetKeyVaultSignToolSettingsExtensions
/// <summary>
///   Used within <see cref="NuGetKeyVaultSignToolTasks"/>.
/// </summary>
[PublicAPI]
[ExcludeFromCodeCoverage]
public static partial class NuGetKeyVaultSignToolSettingsExtensions
{
    #region File
    /// <summary>
    ///   <p><em>Sets <see cref="NuGetKeyVaultSignToolSettings.File"/></em></p>
    ///   <p>Package to sign.</p>
    /// </summary>
    [Pure]
    public static T SetFile<T>(this T toolSettings, string file) where T : NuGetKeyVaultSignToolSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.File = file;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="NuGetKeyVaultSignToolSettings.File"/></em></p>
    ///   <p>Package to sign.</p>
    /// </summary>
    [Pure]
    public static T ResetFile<T>(this T toolSettings) where T : NuGetKeyVaultSignToolSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.File = null;
        return toolSettings;
    }
    #endregion
    #region KeyVaultUrl
    /// <summary>
    ///   <p><em>Sets <see cref="NuGetKeyVaultSignToolSettings.KeyVaultUrl"/></em></p>
    ///   <p>A fully qualified URL of the key vault with the certificate that will be used for signing. An example value might be <c>https://my-vault.vault.azure.net</c>.</p>
    /// </summary>
    [Pure]
    public static T SetKeyVaultUrl<T>(this T toolSettings, string keyVaultUrl) where T : NuGetKeyVaultSignToolSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.KeyVaultUrl = keyVaultUrl;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="NuGetKeyVaultSignToolSettings.KeyVaultUrl"/></em></p>
    ///   <p>A fully qualified URL of the key vault with the certificate that will be used for signing. An example value might be <c>https://my-vault.vault.azure.net</c>.</p>
    /// </summary>
    [Pure]
    public static T ResetKeyVaultUrl<T>(this T toolSettings) where T : NuGetKeyVaultSignToolSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.KeyVaultUrl = null;
        return toolSettings;
    }
    #endregion
    #region KeyVaultClientId
    /// <summary>
    ///   <p><em>Sets <see cref="NuGetKeyVaultSignToolSettings.KeyVaultClientId"/></em></p>
    ///   <p>This is the client ID used to authenticate to Azure, which will be used to generate an access token. This parameter is not required if an access token is supplied directly with the <c>--azure-key-vault-accesstoken</c> option. If this parameter is supplied, <c>--azure-key-vault-client-secret</c> and <c>--azure-key-vault-tenant-id</c> must be supplied as well.</p>
    /// </summary>
    [Pure]
    public static T SetKeyVaultClientId<T>(this T toolSettings, string keyVaultClientId) where T : NuGetKeyVaultSignToolSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.KeyVaultClientId = keyVaultClientId;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="NuGetKeyVaultSignToolSettings.KeyVaultClientId"/></em></p>
    ///   <p>This is the client ID used to authenticate to Azure, which will be used to generate an access token. This parameter is not required if an access token is supplied directly with the <c>--azure-key-vault-accesstoken</c> option. If this parameter is supplied, <c>--azure-key-vault-client-secret</c> and <c>--azure-key-vault-tenant-id</c> must be supplied as well.</p>
    /// </summary>
    [Pure]
    public static T ResetKeyVaultClientId<T>(this T toolSettings) where T : NuGetKeyVaultSignToolSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.KeyVaultClientId = null;
        return toolSettings;
    }
    #endregion
    #region KeyVaultClientSecret
    /// <summary>
    ///   <p><em>Sets <see cref="NuGetKeyVaultSignToolSettings.KeyVaultClientSecret"/></em></p>
    ///   <p>This is the client secret used to authenticate to Azure, which will be used to generate an access token. This parameter is not required if an access token is supplied directly with the <c>--azure-key-vault-accesstoken</c> option or when using managed identities with <c>--azure-key-vault-managed-identity</c>. If this parameter is supplied, <c>--azure-key-vault-client-id</c> and <c>--azure-key-vault-tenant-id</c> must be supplied as well.</p>
    /// </summary>
    [Pure]
    public static T SetKeyVaultClientSecret<T>(this T toolSettings, [Secret] string keyVaultClientSecret) where T : NuGetKeyVaultSignToolSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.KeyVaultClientSecret = keyVaultClientSecret;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="NuGetKeyVaultSignToolSettings.KeyVaultClientSecret"/></em></p>
    ///   <p>This is the client secret used to authenticate to Azure, which will be used to generate an access token. This parameter is not required if an access token is supplied directly with the <c>--azure-key-vault-accesstoken</c> option or when using managed identities with <c>--azure-key-vault-managed-identity</c>. If this parameter is supplied, <c>--azure-key-vault-client-id</c> and <c>--azure-key-vault-tenant-id</c> must be supplied as well.</p>
    /// </summary>
    [Pure]
    public static T ResetKeyVaultClientSecret<T>(this T toolSettings) where T : NuGetKeyVaultSignToolSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.KeyVaultClientSecret = null;
        return toolSettings;
    }
    #endregion
    #region KeyVaultTenantId
    /// <summary>
    ///   <p><em>Sets <see cref="NuGetKeyVaultSignToolSettings.KeyVaultTenantId"/></em></p>
    ///   <p>This is the tenant id used to authenticate to Azure, which will be used to generate an access token. This parameter is not required if an access token is supplied directly with the <c>--azure-key-vault-accesstoken</c> option or when using managed identities with <c>--azure-key-vault-managed-identity</c>. If this parameter is supplied, <c>--azure-key-vault-client-id</c> and <c>--azure-key-vault-client-secret</c> must be supplied as well.</p>
    /// </summary>
    [Pure]
    public static T SetKeyVaultTenantId<T>(this T toolSettings, string keyVaultTenantId) where T : NuGetKeyVaultSignToolSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.KeyVaultTenantId = keyVaultTenantId;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="NuGetKeyVaultSignToolSettings.KeyVaultTenantId"/></em></p>
    ///   <p>This is the tenant id used to authenticate to Azure, which will be used to generate an access token. This parameter is not required if an access token is supplied directly with the <c>--azure-key-vault-accesstoken</c> option or when using managed identities with <c>--azure-key-vault-managed-identity</c>. If this parameter is supplied, <c>--azure-key-vault-client-id</c> and <c>--azure-key-vault-client-secret</c> must be supplied as well.</p>
    /// </summary>
    [Pure]
    public static T ResetKeyVaultTenantId<T>(this T toolSettings) where T : NuGetKeyVaultSignToolSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.KeyVaultTenantId = null;
        return toolSettings;
    }
    #endregion
    #region KeyVaultCertificateName
    /// <summary>
    ///   <p><em>Sets <see cref="NuGetKeyVaultSignToolSettings.KeyVaultCertificateName"/></em></p>
    ///   <p>The name of the certificate used to perform the signing operation.</p>
    /// </summary>
    [Pure]
    public static T SetKeyVaultCertificateName<T>(this T toolSettings, string keyVaultCertificateName) where T : NuGetKeyVaultSignToolSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.KeyVaultCertificateName = keyVaultCertificateName;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="NuGetKeyVaultSignToolSettings.KeyVaultCertificateName"/></em></p>
    ///   <p>The name of the certificate used to perform the signing operation.</p>
    /// </summary>
    [Pure]
    public static T ResetKeyVaultCertificateName<T>(this T toolSettings) where T : NuGetKeyVaultSignToolSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.KeyVaultCertificateName = null;
        return toolSettings;
    }
    #endregion
    #region KeyVaultAccessToken
    /// <summary>
    ///   <p><em>Sets <see cref="NuGetKeyVaultSignToolSettings.KeyVaultAccessToken"/></em></p>
    ///   <p>An access token used to authenticate to Azure. This can be used instead of the <c>--azure-key-vault-managed-identity</c>, <c>--azure-key-vault-client-id</c> and <c>--azure-key-vault-client-secret</c> options. This is useful if NuGetKeyVaultSignTool is being used as part of another program that is already authenticated and has an access token to Azure.</p>
    /// </summary>
    [Pure]
    public static T SetKeyVaultAccessToken<T>(this T toolSettings, [Secret] string keyVaultAccessToken) where T : NuGetKeyVaultSignToolSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.KeyVaultAccessToken = keyVaultAccessToken;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="NuGetKeyVaultSignToolSettings.KeyVaultAccessToken"/></em></p>
    ///   <p>An access token used to authenticate to Azure. This can be used instead of the <c>--azure-key-vault-managed-identity</c>, <c>--azure-key-vault-client-id</c> and <c>--azure-key-vault-client-secret</c> options. This is useful if NuGetKeyVaultSignTool is being used as part of another program that is already authenticated and has an access token to Azure.</p>
    /// </summary>
    [Pure]
    public static T ResetKeyVaultAccessToken<T>(this T toolSettings) where T : NuGetKeyVaultSignToolSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.KeyVaultAccessToken = null;
        return toolSettings;
    }
    #endregion
    #region KeyVaultManagedIdentity
    /// <summary>
    ///   <p><em>Sets <see cref="NuGetKeyVaultSignToolSettings.KeyVaultManagedIdentity"/></em></p>
    ///   <p>Use the ambient Managed Identity to authenticate to Azure. This can be used instead of the <c>--azure-key-vault-accesstoken</c>, <c>--azure-key-vault-client-id</c> and <c>--azure-key-vault-client-secret</c> options. This is useful if NuGetKeyVaultSignTool is being used on a VM/service/CLI that is configured for managed identities to Azure.</p>
    /// </summary>
    [Pure]
    public static T SetKeyVaultManagedIdentity<T>(this T toolSettings, bool? keyVaultManagedIdentity) where T : NuGetKeyVaultSignToolSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.KeyVaultManagedIdentity = keyVaultManagedIdentity;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="NuGetKeyVaultSignToolSettings.KeyVaultManagedIdentity"/></em></p>
    ///   <p>Use the ambient Managed Identity to authenticate to Azure. This can be used instead of the <c>--azure-key-vault-accesstoken</c>, <c>--azure-key-vault-client-id</c> and <c>--azure-key-vault-client-secret</c> options. This is useful if NuGetKeyVaultSignTool is being used on a VM/service/CLI that is configured for managed identities to Azure.</p>
    /// </summary>
    [Pure]
    public static T ResetKeyVaultManagedIdentity<T>(this T toolSettings) where T : NuGetKeyVaultSignToolSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.KeyVaultManagedIdentity = null;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Enables <see cref="NuGetKeyVaultSignToolSettings.KeyVaultManagedIdentity"/></em></p>
    ///   <p>Use the ambient Managed Identity to authenticate to Azure. This can be used instead of the <c>--azure-key-vault-accesstoken</c>, <c>--azure-key-vault-client-id</c> and <c>--azure-key-vault-client-secret</c> options. This is useful if NuGetKeyVaultSignTool is being used on a VM/service/CLI that is configured for managed identities to Azure.</p>
    /// </summary>
    [Pure]
    public static T EnableKeyVaultManagedIdentity<T>(this T toolSettings) where T : NuGetKeyVaultSignToolSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.KeyVaultManagedIdentity = true;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Disables <see cref="NuGetKeyVaultSignToolSettings.KeyVaultManagedIdentity"/></em></p>
    ///   <p>Use the ambient Managed Identity to authenticate to Azure. This can be used instead of the <c>--azure-key-vault-accesstoken</c>, <c>--azure-key-vault-client-id</c> and <c>--azure-key-vault-client-secret</c> options. This is useful if NuGetKeyVaultSignTool is being used on a VM/service/CLI that is configured for managed identities to Azure.</p>
    /// </summary>
    [Pure]
    public static T DisableKeyVaultManagedIdentity<T>(this T toolSettings) where T : NuGetKeyVaultSignToolSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.KeyVaultManagedIdentity = false;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Toggles <see cref="NuGetKeyVaultSignToolSettings.KeyVaultManagedIdentity"/></em></p>
    ///   <p>Use the ambient Managed Identity to authenticate to Azure. This can be used instead of the <c>--azure-key-vault-accesstoken</c>, <c>--azure-key-vault-client-id</c> and <c>--azure-key-vault-client-secret</c> options. This is useful if NuGetKeyVaultSignTool is being used on a VM/service/CLI that is configured for managed identities to Azure.</p>
    /// </summary>
    [Pure]
    public static T ToggleKeyVaultManagedIdentity<T>(this T toolSettings) where T : NuGetKeyVaultSignToolSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.KeyVaultManagedIdentity = !toolSettings.KeyVaultManagedIdentity;
        return toolSettings;
    }
    #endregion
    #region TimestampRfc3161Url
    /// <summary>
    ///   <p><em>Sets <see cref="NuGetKeyVaultSignToolSettings.TimestampRfc3161Url"/></em></p>
    ///   <p>A URL to an RFC3161 compliant timestamping service. This parameter serves the same purpose as the <c>/tr</c> option in the Windows SDK <c>signtool</c>. This parameter should be used in favor of the <c>--timestamp</c> option. Using this parameter will allow using modern, RFC3161 timestamps which also support timestamp digest algorithms other than SHA1.</p>
    /// </summary>
    [Pure]
    public static T SetTimestampRfc3161Url<T>(this T toolSettings, string timestampRfc3161Url) where T : NuGetKeyVaultSignToolSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.TimestampRfc3161Url = timestampRfc3161Url;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="NuGetKeyVaultSignToolSettings.TimestampRfc3161Url"/></em></p>
    ///   <p>A URL to an RFC3161 compliant timestamping service. This parameter serves the same purpose as the <c>/tr</c> option in the Windows SDK <c>signtool</c>. This parameter should be used in favor of the <c>--timestamp</c> option. Using this parameter will allow using modern, RFC3161 timestamps which also support timestamp digest algorithms other than SHA1.</p>
    /// </summary>
    [Pure]
    public static T ResetTimestampRfc3161Url<T>(this T toolSettings) where T : NuGetKeyVaultSignToolSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.TimestampRfc3161Url = null;
        return toolSettings;
    }
    #endregion
    #region TimestampDigest
    /// <summary>
    ///   <p><em>Sets <see cref="NuGetKeyVaultSignToolSettings.TimestampDigest"/></em></p>
    ///   <p>The name of the digest algorithm used for timestamping. This parameter is ignored unless the <c>--timestamp-rfc3161</c> parameter is also supplied. The default value is <c>sha256</c>.</p>
    /// </summary>
    [Pure]
    public static T SetTimestampDigest<T>(this T toolSettings, NuGetKeyVaultSignToolDigestAlgorithm timestampDigest) where T : NuGetKeyVaultSignToolSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.TimestampDigest = timestampDigest;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="NuGetKeyVaultSignToolSettings.TimestampDigest"/></em></p>
    ///   <p>The name of the digest algorithm used for timestamping. This parameter is ignored unless the <c>--timestamp-rfc3161</c> parameter is also supplied. The default value is <c>sha256</c>.</p>
    /// </summary>
    [Pure]
    public static T ResetTimestampDigest<T>(this T toolSettings) where T : NuGetKeyVaultSignToolSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.TimestampDigest = null;
        return toolSettings;
    }
    #endregion
    #region FileDigest
    /// <summary>
    ///   <p><em>Sets <see cref="NuGetKeyVaultSignToolSettings.FileDigest"/></em></p>
    ///   <p>The name of the digest algorithm used for hashing the file being signed. The default value is <c>sha256</c>.</p>
    /// </summary>
    [Pure]
    public static T SetFileDigest<T>(this T toolSettings, NuGetKeyVaultSignToolDigestAlgorithm fileDigest) where T : NuGetKeyVaultSignToolSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.FileDigest = fileDigest;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="NuGetKeyVaultSignToolSettings.FileDigest"/></em></p>
    ///   <p>The name of the digest algorithm used for hashing the file being signed. The default value is <c>sha256</c>.</p>
    /// </summary>
    [Pure]
    public static T ResetFileDigest<T>(this T toolSettings) where T : NuGetKeyVaultSignToolSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.FileDigest = null;
        return toolSettings;
    }
    #endregion
    #region Force
    /// <summary>
    ///   <p><em>Sets <see cref="NuGetKeyVaultSignToolSettings.Force"/></em></p>
    ///   <p>Overwrites a signature if it exists.</p>
    /// </summary>
    [Pure]
    public static T SetForce<T>(this T toolSettings, bool? force) where T : NuGetKeyVaultSignToolSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Force = force;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="NuGetKeyVaultSignToolSettings.Force"/></em></p>
    ///   <p>Overwrites a signature if it exists.</p>
    /// </summary>
    [Pure]
    public static T ResetForce<T>(this T toolSettings) where T : NuGetKeyVaultSignToolSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Force = null;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Enables <see cref="NuGetKeyVaultSignToolSettings.Force"/></em></p>
    ///   <p>Overwrites a signature if it exists.</p>
    /// </summary>
    [Pure]
    public static T EnableForce<T>(this T toolSettings) where T : NuGetKeyVaultSignToolSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Force = true;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Disables <see cref="NuGetKeyVaultSignToolSettings.Force"/></em></p>
    ///   <p>Overwrites a signature if it exists.</p>
    /// </summary>
    [Pure]
    public static T DisableForce<T>(this T toolSettings) where T : NuGetKeyVaultSignToolSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Force = false;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Toggles <see cref="NuGetKeyVaultSignToolSettings.Force"/></em></p>
    ///   <p>Overwrites a signature if it exists.</p>
    /// </summary>
    [Pure]
    public static T ToggleForce<T>(this T toolSettings) where T : NuGetKeyVaultSignToolSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Force = !toolSettings.Force;
        return toolSettings;
    }
    #endregion
    #region Output
    /// <summary>
    ///   <p><em>Sets <see cref="NuGetKeyVaultSignToolSettings.Output"/></em></p>
    ///   <p>The output file. If omitted, overwrites input.</p>
    /// </summary>
    [Pure]
    public static T SetOutput<T>(this T toolSettings, string output) where T : NuGetKeyVaultSignToolSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Output = output;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="NuGetKeyVaultSignToolSettings.Output"/></em></p>
    ///   <p>The output file. If omitted, overwrites input.</p>
    /// </summary>
    [Pure]
    public static T ResetOutput<T>(this T toolSettings) where T : NuGetKeyVaultSignToolSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Output = null;
        return toolSettings;
    }
    #endregion
}
#endregion
#region NuGetKeyVaultSignToolDigestAlgorithm
/// <summary>
///   Used within <see cref="NuGetKeyVaultSignToolTasks"/>.
/// </summary>
[PublicAPI]
[Serializable]
[ExcludeFromCodeCoverage]
[TypeConverter(typeof(TypeConverter<NuGetKeyVaultSignToolDigestAlgorithm>))]
public partial class NuGetKeyVaultSignToolDigestAlgorithm : Enumeration
{
    /// <summary>
    /// sha1
    /// </summary>
    public static NuGetKeyVaultSignToolDigestAlgorithm sha1 = (NuGetKeyVaultSignToolDigestAlgorithm) "sha1";
    /// <summary>
    /// sha256
    /// </summary>
    public static NuGetKeyVaultSignToolDigestAlgorithm sha256 = (NuGetKeyVaultSignToolDigestAlgorithm) "sha256";
    /// <summary>
    /// sha512
    /// </summary>
    public static NuGetKeyVaultSignToolDigestAlgorithm sha384 = (NuGetKeyVaultSignToolDigestAlgorithm) "sha384";
    /// <summary>
    /// sha512
    /// </summary>
    public static NuGetKeyVaultSignToolDigestAlgorithm sha512 = (NuGetKeyVaultSignToolDigestAlgorithm) "sha512";
    /// <summary>
    /// NuGetKeyVaultSignToolDigestAlgorithm
    /// </summary>
    /// <param name="value"></param>
    public static implicit operator NuGetKeyVaultSignToolDigestAlgorithm(string value)
    {
        return new NuGetKeyVaultSignToolDigestAlgorithm { Value = value };
    }
}
#endregion
