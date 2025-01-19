
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

namespace ricaun.Nuke.Tools.NuGet;

/// <summary>
///   <p>The NuGet Command Line Interface (CLI) provides the full extent of NuGet functionality to install, create, publish, and manage packages.</p>
///   <p>For more details, visit the <a href="https://docs.microsoft.com/en-us/nuget/tools/nuget-exe-cli-reference">official website</a>.</p>
/// </summary>
[PublicAPI]
[ExcludeFromCodeCoverage]
[NuGetPackageRequirement(NuGetPackageId)]
public partial class NuGetTasks
    : IRequireNuGetPackage
{
    /// <summary>
    /// NuGetPackageId
    /// </summary>
    public const string NuGetPackageId = "NuGet.CommandLine";
    /// <summary>
    ///   Path to the NuGet executable.
    /// </summary>
    public static string NuGetPath =>
        ToolPathResolver.TryGetEnvironmentExecutable("NUGET_EXE") ??
        NuGetToolPathResolver.GetPackageExecutable("NuGet.CommandLine", "NuGet.exe");
    /// <summary>
    /// NuGetLogger
    /// </summary>
    public static Action<OutputType, string> NuGetLogger { get; set; } = ProcessTasks.DefaultLogger;
    /// <summary>
    /// NuGetExitHandler
    /// </summary>
    public static Action<ToolSettings, IProcess> NuGetExitHandler { get; set; } = ProcessTasks.DefaultExitHandler;
    /// <summary>
    ///   <p>The NuGet Command Line Interface (CLI) provides the full extent of NuGet functionality to install, create, publish, and manage packages.</p>
    ///   <p>For more details, visit the <a href="https://docs.microsoft.com/en-us/nuget/tools/nuget-exe-cli-reference">official website</a>.</p>
    /// </summary>
    public static IReadOnlyCollection<Output> NuGet(ArgumentStringHandler arguments, string workingDirectory = null, IReadOnlyDictionary<string, string> environmentVariables = null, int? timeout = null, bool? logOutput = null, bool? logInvocation = null, Action<OutputType, string> logger = null, Action<IProcess> exitHandler = null)
    {
        using var process = ProcessTasks.StartProcess(NuGetPath, arguments, workingDirectory, environmentVariables, timeout, logOutput, logInvocation, logger ?? NuGetLogger);
        (exitHandler ?? (p => NuGetExitHandler.Invoke(null, p))).Invoke(process.AssertWaitForExit());
        return process.Output;
    }
    /// <summary>
    ///   <p>The NuGet Command Line Interface (CLI) provides the full extent of NuGet functionality to install, create, publish, and manage packages.</p>
    ///   <p>For more details, visit the <a href="https://docs.microsoft.com/en-us/nuget/tools/nuget-exe-cli-reference">official website</a>.</p>
    /// </summary>
    /// <remarks>
    ///   <p>This is a <a href="http://www.nuke.build/docs/authoring-builds/cli-tools.html#fluent-apis">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p>
    ///   <ul>
    ///     <li><c>&lt;packageId&gt;</c> via <see cref="NuGetDeleteSettings.PackageId"/></li>
    ///     <li><c>&lt;packageVersion&gt;</c> via <see cref="NuGetDeleteSettings.PackageVersion"/></li>
    ///     <li><c>-ApiKey</c> via <see cref="NuGetDeleteSettings.ApiKey"/></li>
    ///     <li><c>-ConfigFile</c> via <see cref="NuGetDeleteSettings.ConfigFile"/></li>
    ///     <li><c>-ForceEnglishOutput</c> via <see cref="NuGetDeleteSettings.ForceEnglishOutput"/></li>
    ///     <li><c>-NonInteractive</c> via <see cref="NuGetDeleteSettings.NonInteractive"/></li>
    ///     <li><c>-NoPrompt</c> via <see cref="NuGetDeleteSettings.NoPrompt"/></li>
    ///     <li><c>-Source</c> via <see cref="NuGetDeleteSettings.Source"/></li>
    ///     <li><c>-Verbosity</c> via <see cref="NuGetDeleteSettings.Verbosity"/></li>
    ///   </ul>
    /// </remarks>
    public static IReadOnlyCollection<Output> NuGetDelete(NuGetDeleteSettings toolSettings = null)
    {
        toolSettings = toolSettings ?? new NuGetDeleteSettings();
        using var process = ProcessTasks.StartProcess(toolSettings);
        toolSettings.ProcessExitHandler.Invoke(toolSettings, process.AssertWaitForExit());
        return process.Output;
    }
    /// <summary>
    ///   <p>The NuGet Command Line Interface (CLI) provides the full extent of NuGet functionality to install, create, publish, and manage packages.</p>
    ///   <p>For more details, visit the <a href="https://docs.microsoft.com/en-us/nuget/tools/nuget-exe-cli-reference">official website</a>.</p>
    /// </summary>
    /// <remarks>
    ///   <p>This is a <a href="http://www.nuke.build/docs/authoring-builds/cli-tools.html#fluent-apis">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p>
    ///   <ul>
    ///     <li><c>&lt;packageId&gt;</c> via <see cref="NuGetDeleteSettings.PackageId"/></li>
    ///     <li><c>&lt;packageVersion&gt;</c> via <see cref="NuGetDeleteSettings.PackageVersion"/></li>
    ///     <li><c>-ApiKey</c> via <see cref="NuGetDeleteSettings.ApiKey"/></li>
    ///     <li><c>-ConfigFile</c> via <see cref="NuGetDeleteSettings.ConfigFile"/></li>
    ///     <li><c>-ForceEnglishOutput</c> via <see cref="NuGetDeleteSettings.ForceEnglishOutput"/></li>
    ///     <li><c>-NonInteractive</c> via <see cref="NuGetDeleteSettings.NonInteractive"/></li>
    ///     <li><c>-NoPrompt</c> via <see cref="NuGetDeleteSettings.NoPrompt"/></li>
    ///     <li><c>-Source</c> via <see cref="NuGetDeleteSettings.Source"/></li>
    ///     <li><c>-Verbosity</c> via <see cref="NuGetDeleteSettings.Verbosity"/></li>
    ///   </ul>
    /// </remarks>
    public static IReadOnlyCollection<Output> NuGetDelete(Configure<NuGetDeleteSettings> configurator)
    {
        return NuGetDelete(configurator(new NuGetDeleteSettings()));
    }
    /// <summary>
    ///   <p>The NuGet Command Line Interface (CLI) provides the full extent of NuGet functionality to install, create, publish, and manage packages.</p>
    ///   <p>For more details, visit the <a href="https://docs.microsoft.com/en-us/nuget/tools/nuget-exe-cli-reference">official website</a>.</p>
    /// </summary>
    /// <remarks>
    ///   <p>This is a <a href="http://www.nuke.build/docs/authoring-builds/cli-tools.html#fluent-apis">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p>
    ///   <ul>
    ///     <li><c>&lt;packageId&gt;</c> via <see cref="NuGetDeleteSettings.PackageId"/></li>
    ///     <li><c>&lt;packageVersion&gt;</c> via <see cref="NuGetDeleteSettings.PackageVersion"/></li>
    ///     <li><c>-ApiKey</c> via <see cref="NuGetDeleteSettings.ApiKey"/></li>
    ///     <li><c>-ConfigFile</c> via <see cref="NuGetDeleteSettings.ConfigFile"/></li>
    ///     <li><c>-ForceEnglishOutput</c> via <see cref="NuGetDeleteSettings.ForceEnglishOutput"/></li>
    ///     <li><c>-NonInteractive</c> via <see cref="NuGetDeleteSettings.NonInteractive"/></li>
    ///     <li><c>-NoPrompt</c> via <see cref="NuGetDeleteSettings.NoPrompt"/></li>
    ///     <li><c>-Source</c> via <see cref="NuGetDeleteSettings.Source"/></li>
    ///     <li><c>-Verbosity</c> via <see cref="NuGetDeleteSettings.Verbosity"/></li>
    ///   </ul>
    /// </remarks>
    public static IEnumerable<(NuGetDeleteSettings Settings, IReadOnlyCollection<Output> Output)> NuGetDelete(CombinatorialConfigure<NuGetDeleteSettings> configurator, int degreeOfParallelism = 1, bool completeOnFailure = false)
    {
        return configurator.Invoke(NuGetDelete, NuGetLogger, degreeOfParallelism, completeOnFailure);
    }
    /// <summary>
    ///   <p>The NuGet Command Line Interface (CLI) provides the full extent of NuGet functionality to install, create, publish, and manage packages.</p>
    ///   <p>For more details, visit the <a href="https://docs.microsoft.com/en-us/nuget/tools/nuget-exe-cli-reference">official website</a>.</p>
    /// </summary>
    /// <remarks>
    ///   <p>This is a <a href="http://www.nuke.build/docs/authoring-builds/cli-tools.html#fluent-apis">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p>
    ///   <ul>
    ///     <li><c>&lt;targetPath&gt;</c> via <see cref="NuGetSignSettings.TargetPath"/></li>
    ///     <li><c>-CertificateFingerprint</c> via <see cref="NuGetSignSettings.CertificateFingerprint"/></li>
    ///     <li><c>-CertificatePassword</c> via <see cref="NuGetSignSettings.CertificatePassword"/></li>
    ///     <li><c>-CertificatePath</c> via <see cref="NuGetSignSettings.CertificatePath"/></li>
    ///     <li><c>-CertificateStoreLocation</c> via <see cref="NuGetSignSettings.CertificateStoreLocation"/></li>
    ///     <li><c>-CertificateStoreName</c> via <see cref="NuGetSignSettings.CertificateStoreName"/></li>
    ///     <li><c>-CertificateSubjectName</c> via <see cref="NuGetSignSettings.CertificateSubjectName"/></li>
    ///     <li><c>-ConfigFile</c> via <see cref="NuGetSignSettings.ConfigFile"/></li>
    ///     <li><c>-ForceEnglishOutput</c> via <see cref="NuGetSignSettings.ForceEnglishOutput"/></li>
    ///     <li><c>-HashAlgorithm</c> via <see cref="NuGetSignSettings.HashAlgorithm"/></li>
    ///     <li><c>-NonInteractive</c> via <see cref="NuGetSignSettings.NonInteractive"/></li>
    ///     <li><c>-OutputDirectory</c> via <see cref="NuGetSignSettings.OutputDirectory"/></li>
    ///     <li><c>-Overwrite</c> via <see cref="NuGetSignSettings.Overwrite"/></li>
    ///     <li><c>-Timestamper</c> via <see cref="NuGetSignSettings.Timestamper"/></li>
    ///     <li><c>-TimestampHashAlgorithm</c> via <see cref="NuGetSignSettings.TimestampHashAlgorithm"/></li>
    ///     <li><c>-Verbosity</c> via <see cref="NuGetSignSettings.Verbosity"/></li>
    ///   </ul>
    /// </remarks>
    public static IReadOnlyCollection<Output> NuGetSign(NuGetSignSettings toolSettings = null)
    {
        toolSettings = toolSettings ?? new NuGetSignSettings();
        using var process = ProcessTasks.StartProcess(toolSettings);
        toolSettings.ProcessExitHandler.Invoke(toolSettings, process.AssertWaitForExit());
        return process.Output;
    }
    /// <summary>
    ///   <p>The NuGet Command Line Interface (CLI) provides the full extent of NuGet functionality to install, create, publish, and manage packages.</p>
    ///   <p>For more details, visit the <a href="https://docs.microsoft.com/en-us/nuget/tools/nuget-exe-cli-reference">official website</a>.</p>
    /// </summary>
    /// <remarks>
    ///   <p>This is a <a href="http://www.nuke.build/docs/authoring-builds/cli-tools.html#fluent-apis">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p>
    ///   <ul>
    ///     <li><c>&lt;targetPath&gt;</c> via <see cref="NuGetSignSettings.TargetPath"/></li>
    ///     <li><c>-CertificateFingerprint</c> via <see cref="NuGetSignSettings.CertificateFingerprint"/></li>
    ///     <li><c>-CertificatePassword</c> via <see cref="NuGetSignSettings.CertificatePassword"/></li>
    ///     <li><c>-CertificatePath</c> via <see cref="NuGetSignSettings.CertificatePath"/></li>
    ///     <li><c>-CertificateStoreLocation</c> via <see cref="NuGetSignSettings.CertificateStoreLocation"/></li>
    ///     <li><c>-CertificateStoreName</c> via <see cref="NuGetSignSettings.CertificateStoreName"/></li>
    ///     <li><c>-CertificateSubjectName</c> via <see cref="NuGetSignSettings.CertificateSubjectName"/></li>
    ///     <li><c>-ConfigFile</c> via <see cref="NuGetSignSettings.ConfigFile"/></li>
    ///     <li><c>-ForceEnglishOutput</c> via <see cref="NuGetSignSettings.ForceEnglishOutput"/></li>
    ///     <li><c>-HashAlgorithm</c> via <see cref="NuGetSignSettings.HashAlgorithm"/></li>
    ///     <li><c>-NonInteractive</c> via <see cref="NuGetSignSettings.NonInteractive"/></li>
    ///     <li><c>-OutputDirectory</c> via <see cref="NuGetSignSettings.OutputDirectory"/></li>
    ///     <li><c>-Overwrite</c> via <see cref="NuGetSignSettings.Overwrite"/></li>
    ///     <li><c>-Timestamper</c> via <see cref="NuGetSignSettings.Timestamper"/></li>
    ///     <li><c>-TimestampHashAlgorithm</c> via <see cref="NuGetSignSettings.TimestampHashAlgorithm"/></li>
    ///     <li><c>-Verbosity</c> via <see cref="NuGetSignSettings.Verbosity"/></li>
    ///   </ul>
    /// </remarks>
    public static IReadOnlyCollection<Output> NuGetSign(Configure<NuGetSignSettings> configurator)
    {
        return NuGetSign(configurator(new NuGetSignSettings()));
    }
    /// <summary>
    ///   <p>The NuGet Command Line Interface (CLI) provides the full extent of NuGet functionality to install, create, publish, and manage packages.</p>
    ///   <p>For more details, visit the <a href="https://docs.microsoft.com/en-us/nuget/tools/nuget-exe-cli-reference">official website</a>.</p>
    /// </summary>
    /// <remarks>
    ///   <p>This is a <a href="http://www.nuke.build/docs/authoring-builds/cli-tools.html#fluent-apis">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p>
    ///   <ul>
    ///     <li><c>&lt;targetPath&gt;</c> via <see cref="NuGetSignSettings.TargetPath"/></li>
    ///     <li><c>-CertificateFingerprint</c> via <see cref="NuGetSignSettings.CertificateFingerprint"/></li>
    ///     <li><c>-CertificatePassword</c> via <see cref="NuGetSignSettings.CertificatePassword"/></li>
    ///     <li><c>-CertificatePath</c> via <see cref="NuGetSignSettings.CertificatePath"/></li>
    ///     <li><c>-CertificateStoreLocation</c> via <see cref="NuGetSignSettings.CertificateStoreLocation"/></li>
    ///     <li><c>-CertificateStoreName</c> via <see cref="NuGetSignSettings.CertificateStoreName"/></li>
    ///     <li><c>-CertificateSubjectName</c> via <see cref="NuGetSignSettings.CertificateSubjectName"/></li>
    ///     <li><c>-ConfigFile</c> via <see cref="NuGetSignSettings.ConfigFile"/></li>
    ///     <li><c>-ForceEnglishOutput</c> via <see cref="NuGetSignSettings.ForceEnglishOutput"/></li>
    ///     <li><c>-HashAlgorithm</c> via <see cref="NuGetSignSettings.HashAlgorithm"/></li>
    ///     <li><c>-NonInteractive</c> via <see cref="NuGetSignSettings.NonInteractive"/></li>
    ///     <li><c>-OutputDirectory</c> via <see cref="NuGetSignSettings.OutputDirectory"/></li>
    ///     <li><c>-Overwrite</c> via <see cref="NuGetSignSettings.Overwrite"/></li>
    ///     <li><c>-Timestamper</c> via <see cref="NuGetSignSettings.Timestamper"/></li>
    ///     <li><c>-TimestampHashAlgorithm</c> via <see cref="NuGetSignSettings.TimestampHashAlgorithm"/></li>
    ///     <li><c>-Verbosity</c> via <see cref="NuGetSignSettings.Verbosity"/></li>
    ///   </ul>
    /// </remarks>
    public static IEnumerable<(NuGetSignSettings Settings, IReadOnlyCollection<Output> Output)> NuGetSign(CombinatorialConfigure<NuGetSignSettings> configurator, int degreeOfParallelism = 1, bool completeOnFailure = false)
    {
        return configurator.Invoke(NuGetSign, NuGetLogger, degreeOfParallelism, completeOnFailure);
    }
    /// <summary>
    ///   <p>The NuGet Command Line Interface (CLI) provides the full extent of NuGet functionality to install, create, publish, and manage packages.</p>
    ///   <p>For more details, visit the <a href="https://docs.microsoft.com/en-us/nuget/tools/nuget-exe-cli-reference">official website</a>.</p>
    /// </summary>
    /// <remarks>
    ///   <p>This is a <a href="http://www.nuke.build/docs/authoring-builds/cli-tools.html#fluent-apis">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p>
    ///   <ul>
    ///     <li><c>-CertificateFingerprint</c> via <see cref="NuGetVerifySettings.CertificateFingerprint"/></li>
    ///     <li><c>-ConfigFile</c> via <see cref="NuGetVerifySettings.ConfigFile"/></li>
    ///     <li><c>-ForceEnglishOutput</c> via <see cref="NuGetVerifySettings.ForceEnglishOutput"/></li>
    ///     <li><c>-NonInteractive</c> via <see cref="NuGetVerifySettings.NonInteractive"/></li>
    ///     <li><c>-Signatures</c> via <see cref="NuGetVerifySettings.TargetPath"/></li>
    ///     <li><c>-Verbosity</c> via <see cref="NuGetVerifySettings.Verbosity"/></li>
    ///   </ul>
    /// </remarks>
    public static IReadOnlyCollection<Output> NuGetVerify(NuGetVerifySettings toolSettings = null)
    {
        toolSettings = toolSettings ?? new NuGetVerifySettings();
        using var process = ProcessTasks.StartProcess(toolSettings);
        toolSettings.ProcessExitHandler.Invoke(toolSettings, process.AssertWaitForExit());
        return process.Output;
    }
    /// <summary>
    ///   <p>The NuGet Command Line Interface (CLI) provides the full extent of NuGet functionality to install, create, publish, and manage packages.</p>
    ///   <p>For more details, visit the <a href="https://docs.microsoft.com/en-us/nuget/tools/nuget-exe-cli-reference">official website</a>.</p>
    /// </summary>
    /// <remarks>
    ///   <p>This is a <a href="http://www.nuke.build/docs/authoring-builds/cli-tools.html#fluent-apis">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p>
    ///   <ul>
    ///     <li><c>-CertificateFingerprint</c> via <see cref="NuGetVerifySettings.CertificateFingerprint"/></li>
    ///     <li><c>-ConfigFile</c> via <see cref="NuGetVerifySettings.ConfigFile"/></li>
    ///     <li><c>-ForceEnglishOutput</c> via <see cref="NuGetVerifySettings.ForceEnglishOutput"/></li>
    ///     <li><c>-NonInteractive</c> via <see cref="NuGetVerifySettings.NonInteractive"/></li>
    ///     <li><c>-Signatures</c> via <see cref="NuGetVerifySettings.TargetPath"/></li>
    ///     <li><c>-Verbosity</c> via <see cref="NuGetVerifySettings.Verbosity"/></li>
    ///   </ul>
    /// </remarks>
    public static IReadOnlyCollection<Output> NuGetVerify(Configure<NuGetVerifySettings> configurator)
    {
        return NuGetVerify(configurator(new NuGetVerifySettings()));
    }
    /// <summary>
    ///   <p>The NuGet Command Line Interface (CLI) provides the full extent of NuGet functionality to install, create, publish, and manage packages.</p>
    ///   <p>For more details, visit the <a href="https://docs.microsoft.com/en-us/nuget/tools/nuget-exe-cli-reference">official website</a>.</p>
    /// </summary>
    /// <remarks>
    ///   <p>This is a <a href="http://www.nuke.build/docs/authoring-builds/cli-tools.html#fluent-apis">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p>
    ///   <ul>
    ///     <li><c>-CertificateFingerprint</c> via <see cref="NuGetVerifySettings.CertificateFingerprint"/></li>
    ///     <li><c>-ConfigFile</c> via <see cref="NuGetVerifySettings.ConfigFile"/></li>
    ///     <li><c>-ForceEnglishOutput</c> via <see cref="NuGetVerifySettings.ForceEnglishOutput"/></li>
    ///     <li><c>-NonInteractive</c> via <see cref="NuGetVerifySettings.NonInteractive"/></li>
    ///     <li><c>-Signatures</c> via <see cref="NuGetVerifySettings.TargetPath"/></li>
    ///     <li><c>-Verbosity</c> via <see cref="NuGetVerifySettings.Verbosity"/></li>
    ///   </ul>
    /// </remarks>
    public static IEnumerable<(NuGetVerifySettings Settings, IReadOnlyCollection<Output> Output)> NuGetVerify(CombinatorialConfigure<NuGetVerifySettings> configurator, int degreeOfParallelism = 1, bool completeOnFailure = false)
    {
        return configurator.Invoke(NuGetVerify, NuGetLogger, degreeOfParallelism, completeOnFailure);
    }
}
#region NuGetDeleteSettings
/// <summary>
///   Used within <see cref="NuGetTasks"/>.
/// </summary>
[PublicAPI]
[ExcludeFromCodeCoverage]
[Serializable]
public partial class NuGetDeleteSettings : ToolSettings
{
    /// <summary>
    ///   Path to the NuGet executable.
    /// </summary>
    public override string ProcessToolPath => base.ProcessToolPath ?? NuGetTasks.NuGetPath;
    /// <summary>
    /// ProcessLogger
    /// </summary>
    public override Action<OutputType, string> ProcessLogger => base.ProcessLogger ?? NuGetTasks.NuGetLogger;
    /// <summary>
    /// ProcessExitHandler
    /// </summary>
    public override Action<ToolSettings, IProcess> ProcessExitHandler => base.ProcessExitHandler ?? NuGetTasks.NuGetExitHandler;
    /// <summary>
    ///   Package Id to delete. The exact behavior depends on the source. For local folders, for instance, the package is deleted; for nuget.org the package is unlisted.
    /// </summary>
    public virtual string PackageId { get; internal set; }
    /// <summary>
    ///   Package Version to delete. The exact behavior depends on the source. For local folders, for instance, the package is deleted; for nuget.org the package is unlisted.
    /// </summary>
    public virtual string PackageVersion { get; internal set; }
    /// <summary>
    ///   The API key for the target repository. If not present, the one specified in <em>%AppData%\NuGet\NuGet.Config</em> is used.
    /// </summary>
    public virtual string ApiKey { get; internal set; }
    /// <summary>
    ///   The NuGet configuration file to apply. If not specified, <c>%AppData%\NuGet\NuGet.Config</c> (Windows) or <c>~/.nuget/NuGet/NuGet.Config</c> (Mac/Linux) is used.
    /// </summary>
    public virtual string ConfigFile { get; internal set; }
    /// <summary>
    ///   <em>(3.5+)</em> Forces nuget.exe to run using an invariant, English-based culture.
    /// </summary>
    public virtual bool? ForceEnglishOutput { get; internal set; }
    /// <summary>
    ///   Suppresses prompts for user input or confirmations.
    /// </summary>
    public virtual bool? NonInteractive { get; internal set; }
    /// <summary>
    ///   Do not prompt when deleting.
    /// </summary>
    public virtual bool? NoPrompt { get; internal set; }
    /// <summary>
    ///   Specifies the server URL. NuGet identifies a UNC or local folder source and simply copies the file there instead of pushing it using HTTP.  Also, starting with NuGet 3.4.2, this is a mandatory parameter unless the <em>NuGet.Config</em> file specifies a <em>DefaultPushSource</em> value (see <a href="https://docs.microsoft.com/en-us/nuget/consume-packages/configuring-nuget-behavior">Configuring NuGet behavior</a>).
    /// </summary>
    public virtual string Source { get; internal set; }
    /// <summary>
    ///   Specifies the amount of detail displayed in the output: <em>normal</em>, <em>quiet</em>, <em>detailed</em>.
    /// </summary>
    public virtual NuGetVerbosity Verbosity { get; internal set; }
    /// <summary>
    /// ConfigureProcessArguments
    /// </summary>
    /// <param name="arguments"></param>
    /// <returns></returns>
    protected override Arguments ConfigureProcessArguments(Arguments arguments)
    {
        arguments
          .Add("delete")
          .Add("{value}", PackageId)
          .Add("{value}", PackageVersion)
          .Add("-ApiKey {value}", ApiKey, secret: true)
          .Add("-ConfigFile {value}", ConfigFile)
          .Add("-ForceEnglishOutput", ForceEnglishOutput)
          .Add("-NonInteractive", NonInteractive)
          .Add("-NoPrompt", NoPrompt)
          .Add("-Source {value}", Source)
          .Add("-Verbosity {value}", Verbosity);
        return base.ConfigureProcessArguments(arguments);
    }
}
#endregion
#region NuGetSignSettings
/// <summary>
///   Used within <see cref="NuGetTasks"/>.
/// </summary>
[PublicAPI]
[ExcludeFromCodeCoverage]
[Serializable]
public partial class NuGetSignSettings : ToolSettings
{
    /// <summary>
    ///   Path to the NuGet executable.
    /// </summary>
    public override string ProcessToolPath => base.ProcessToolPath ?? NuGetTasks.NuGetPath;
    /// <summary>
    /// ProcessLogger
    /// </summary>
    public override Action<OutputType, string> ProcessLogger => base.ProcessLogger ?? NuGetTasks.NuGetLogger;
    /// <summary>
    /// ProcessExitHandler
    /// </summary>
    public override Action<ToolSettings, IProcess> ProcessExitHandler => base.ProcessExitHandler ?? NuGetTasks.NuGetExitHandler;
    /// <summary>
    ///   Path of the package to sign.
    /// </summary>
    public virtual string TargetPath { get; internal set; }
    /// <summary>
    ///   Specifies the fingerprint to be used to search for the certificate in a local certificate store.
    /// </summary>
    public virtual string CertificateFingerprint { get; internal set; }
    /// <summary>
    ///   Specifies the certificate password, if needed. If a certificate is password protected but no password is provided, the command will prompt for a password at run time, unless the -NonInteractive option is passed.
    /// </summary>
    public virtual string CertificatePassword { get; internal set; }
    /// <summary>
    ///   Specifies the file path to the certificate to be used in signing the package.
    /// </summary>
    public virtual string CertificatePath { get; internal set; }
    /// <summary>
    ///   Specifies the name of the X.509 certificate store use to search for the certificate. Defaults to 'CurrentUser', the X.509 certificate store used by the current user. This option should be used when specifying the certificate via -CertificateSubjectName or -CertificateFingerprint options.
    /// </summary>
    public virtual string CertificateStoreLocation { get; internal set; }
    /// <summary>
    ///   Specifies the name of the X.509 certificate store to use to search for the certificate. Defaults to 'My', the X.509 certificate store for personal certificates. This option should be used when specifying the certificate via -CertificateSubjectName or -CertificateFingerprint options.
    /// </summary>
    public virtual string CertificateStoreName { get; internal set; }
    /// <summary>
    ///   Specifies the subject name of the certificate used to search a local certificate store for the certificate. The search is a case-insensitive string comparison using the supplied value, which will find all certificates with the subject name containing that string, regardless of other subject values. The certificate store can be specified by -CertificateStoreName and -CertificateStoreLocation options.
    /// </summary>
    public virtual string CertificateSubjectName { get; internal set; }
    /// <summary>
    ///   Hash algorithm to be used to sign the package. Defaults to SHA256. Possible values are SHA256, SHA384, and SHA512.
    /// </summary>
    public virtual NuGetSignHashAlgorithm HashAlgorithm { get; internal set; }
    /// <summary>
    ///   Specifies the directory where the signed package should be saved. By default the original package is overwritten by the signed package.
    /// </summary>
    public virtual string OutputDirectory { get; internal set; }
    /// <summary>
    ///   Switch to indicate if the current signature should be overwritten. By default the command will fail if the package already has a signature.
    /// </summary>
    public virtual bool? Overwrite { get; internal set; }
    /// <summary>
    ///   URL to an RFC 3161 timestamping server.
    /// </summary>
    public virtual string Timestamper { get; internal set; }
    /// <summary>
    ///   Hash algorithm to be used by the RFC 3161 timestamp server. Defaults to SHA256.
    /// </summary>
    public virtual NuGetSignHashAlgorithm TimestampHashAlgorithm { get; internal set; }
    /// <summary>
    ///   The NuGet configuration file to apply. If not specified, <c>%AppData%\NuGet\NuGet.Config</c> (Windows) or <c>~/.nuget/NuGet/NuGet.Config</c> (Mac/Linux) is used.
    /// </summary>
    public virtual string ConfigFile { get; internal set; }
    /// <summary>
    ///   <em>(3.5+)</em> Forces nuget.exe to run using an invariant, English-based culture.
    /// </summary>
    public virtual bool? ForceEnglishOutput { get; internal set; }
    /// <summary>
    ///   Suppresses prompts for user input or confirmations.
    /// </summary>
    public virtual bool? NonInteractive { get; internal set; }
    /// <summary>
    ///   Specifies the amount of detail displayed in the output: <em>normal</em>, <em>quiet</em>, <em>detailed</em>.
    /// </summary>
    public virtual NuGetVerbosity Verbosity { get; internal set; }
    /// <summary>
    /// ConfigureProcessArguments
    /// </summary>
    /// <param name="arguments"></param>
    /// <returns></returns>
    protected override Arguments ConfigureProcessArguments(Arguments arguments)
    {
        arguments
          .Add("sign")
          .Add("{value}", TargetPath)
          .Add("-CertificateFingerprint {value}", CertificateFingerprint)
          .Add("-CertificatePassword {value}", CertificatePassword, secret: true)
          .Add("-CertificatePath {value}", CertificatePath)
          .Add("-CertificateStoreLocation {value}", CertificateStoreLocation)
          .Add("-CertificateStoreName {value}", CertificateStoreName)
          .Add("-CertificateSubjectName {value}", CertificateSubjectName)
          .Add("-HashAlgorithm {value}", HashAlgorithm)
          .Add("-OutputDirectory {value}", OutputDirectory)
          .Add("-Overwrite", Overwrite)
          .Add("-Timestamper {value}", Timestamper)
          .Add("-TimestampHashAlgorithm {value}", TimestampHashAlgorithm)
          .Add("-ConfigFile {value}", ConfigFile)
          .Add("-ForceEnglishOutput", ForceEnglishOutput)
          .Add("-NonInteractive", NonInteractive)
          .Add("-Verbosity {value}", Verbosity);
        return base.ConfigureProcessArguments(arguments);
    }
}
#endregion
#region NuGetVerifySettings
/// <summary>
///   Used within <see cref="NuGetTasks"/>.
/// </summary>
[PublicAPI]
[ExcludeFromCodeCoverage]
[Serializable]
public partial class NuGetVerifySettings : ToolSettings
{
    /// <summary>
    ///   Path to the NuGet executable.
    /// </summary>
    public override string ProcessToolPath => base.ProcessToolPath ?? NuGetTasks.NuGetPath;
    /// <summary>
    /// ProcessLogger
    /// </summary>
    public override Action<OutputType, string> ProcessLogger => base.ProcessLogger ?? NuGetTasks.NuGetLogger;
    /// <summary>
    /// ProcessExitHandler
    /// </summary>
    public override Action<ToolSettings, IProcess> ProcessExitHandler => base.ProcessExitHandler ?? NuGetTasks.NuGetExitHandler;
    /// <summary>
    ///   Path of the package to verify Signatures.
    /// </summary>
    public virtual string TargetPath { get; internal set; }
    /// <summary>
    ///   Specifies one or more SHA-256 certificate fingerprints of certificates(s) which signed packages must be signed with. A certificate SHA-256 fingerprint is a SHA-256 hash of the certificate. Multiple inputs should be semicolon separated.
    /// </summary>
    public virtual string CertificateFingerprint { get; internal set; }
    /// <summary>
    ///   The NuGet configuration file to apply. If not specified, <c>%AppData%\NuGet\NuGet.Config</c> (Windows) or <c>~/.nuget/NuGet/NuGet.Config</c> (Mac/Linux) is used.
    /// </summary>
    public virtual string ConfigFile { get; internal set; }
    /// <summary>
    ///   <em>(3.5+)</em> Forces nuget.exe to run using an invariant, English-based culture.
    /// </summary>
    public virtual bool? ForceEnglishOutput { get; internal set; }
    /// <summary>
    ///   Suppresses prompts for user input or confirmations.
    /// </summary>
    public virtual bool? NonInteractive { get; internal set; }
    /// <summary>
    ///   Specifies the amount of detail displayed in the output: <em>normal</em>, <em>quiet</em>, <em>detailed</em>.
    /// </summary>
    public virtual NuGetVerbosity Verbosity { get; internal set; }
    /// <summary>
    /// ConfigureProcessArguments
    /// </summary>
    /// <param name="arguments"></param>
    /// <returns></returns>
    protected override Arguments ConfigureProcessArguments(Arguments arguments)
    {
        arguments
          .Add("verify")
          .Add("-Signatures {value}", TargetPath)
          .Add("-CertificateFingerprint {value}", CertificateFingerprint)
          .Add("-ConfigFile {value}", ConfigFile)
          .Add("-ForceEnglishOutput", ForceEnglishOutput)
          .Add("-NonInteractive", NonInteractive)
          .Add("-Verbosity {value}", Verbosity);
        return base.ConfigureProcessArguments(arguments);
    }
}
#endregion
#region NuGetDeleteSettingsExtensions
/// <summary>
///   Used within <see cref="NuGetTasks"/>.
/// </summary>
[PublicAPI]
[ExcludeFromCodeCoverage]
public static partial class NuGetDeleteSettingsExtensions
{
    #region PackageId
    /// <summary>
    ///   <p><em>Sets <see cref="NuGetDeleteSettings.PackageId"/></em></p>
    ///   <p>Package Id to delete. The exact behavior depends on the source. For local folders, for instance, the package is deleted; for nuget.org the package is unlisted.</p>
    /// </summary>
    [Pure]
    public static T SetPackageId<T>(this T toolSettings, string packageId) where T : NuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.PackageId = packageId;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="NuGetDeleteSettings.PackageId"/></em></p>
    ///   <p>Package Id to delete. The exact behavior depends on the source. For local folders, for instance, the package is deleted; for nuget.org the package is unlisted.</p>
    /// </summary>
    [Pure]
    public static T ResetPackageId<T>(this T toolSettings) where T : NuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.PackageId = null;
        return toolSettings;
    }
    #endregion
    #region PackageVersion
    /// <summary>
    ///   <p><em>Sets <see cref="NuGetDeleteSettings.PackageVersion"/></em></p>
    ///   <p>Package Version to delete. The exact behavior depends on the source. For local folders, for instance, the package is deleted; for nuget.org the package is unlisted.</p>
    /// </summary>
    [Pure]
    public static T SetPackageVersion<T>(this T toolSettings, string packageVersion) where T : NuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.PackageVersion = packageVersion;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="NuGetDeleteSettings.PackageVersion"/></em></p>
    ///   <p>Package Version to delete. The exact behavior depends on the source. For local folders, for instance, the package is deleted; for nuget.org the package is unlisted.</p>
    /// </summary>
    [Pure]
    public static T ResetPackageVersion<T>(this T toolSettings) where T : NuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.PackageVersion = null;
        return toolSettings;
    }
    #endregion
    #region ApiKey
    /// <summary>
    ///   <p><em>Sets <see cref="NuGetDeleteSettings.ApiKey"/></em></p>
    ///   <p>The API key for the target repository. If not present, the one specified in <em>%AppData%\NuGet\NuGet.Config</em> is used.</p>
    /// </summary>
    [Pure]
    public static T SetApiKey<T>(this T toolSettings, [Secret] string apiKey) where T : NuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.ApiKey = apiKey;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="NuGetDeleteSettings.ApiKey"/></em></p>
    ///   <p>The API key for the target repository. If not present, the one specified in <em>%AppData%\NuGet\NuGet.Config</em> is used.</p>
    /// </summary>
    [Pure]
    public static T ResetApiKey<T>(this T toolSettings) where T : NuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.ApiKey = null;
        return toolSettings;
    }
    #endregion
    #region ConfigFile
    /// <summary>
    ///   <p><em>Sets <see cref="NuGetDeleteSettings.ConfigFile"/></em></p>
    ///   <p>The NuGet configuration file to apply. If not specified, <c>%AppData%\NuGet\NuGet.Config</c> (Windows) or <c>~/.nuget/NuGet/NuGet.Config</c> (Mac/Linux) is used.</p>
    /// </summary>
    [Pure]
    public static T SetConfigFile<T>(this T toolSettings, string configFile) where T : NuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.ConfigFile = configFile;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="NuGetDeleteSettings.ConfigFile"/></em></p>
    ///   <p>The NuGet configuration file to apply. If not specified, <c>%AppData%\NuGet\NuGet.Config</c> (Windows) or <c>~/.nuget/NuGet/NuGet.Config</c> (Mac/Linux) is used.</p>
    /// </summary>
    [Pure]
    public static T ResetConfigFile<T>(this T toolSettings) where T : NuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.ConfigFile = null;
        return toolSettings;
    }
    #endregion
    #region ForceEnglishOutput
    /// <summary>
    ///   <p><em>Sets <see cref="NuGetDeleteSettings.ForceEnglishOutput"/></em></p>
    ///   <p><em>(3.5+)</em> Forces nuget.exe to run using an invariant, English-based culture.</p>
    /// </summary>
    [Pure]
    public static T SetForceEnglishOutput<T>(this T toolSettings, bool? forceEnglishOutput) where T : NuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.ForceEnglishOutput = forceEnglishOutput;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="NuGetDeleteSettings.ForceEnglishOutput"/></em></p>
    ///   <p><em>(3.5+)</em> Forces nuget.exe to run using an invariant, English-based culture.</p>
    /// </summary>
    [Pure]
    public static T ResetForceEnglishOutput<T>(this T toolSettings) where T : NuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.ForceEnglishOutput = null;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Enables <see cref="NuGetDeleteSettings.ForceEnglishOutput"/></em></p>
    ///   <p><em>(3.5+)</em> Forces nuget.exe to run using an invariant, English-based culture.</p>
    /// </summary>
    [Pure]
    public static T EnableForceEnglishOutput<T>(this T toolSettings) where T : NuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.ForceEnglishOutput = true;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Disables <see cref="NuGetDeleteSettings.ForceEnglishOutput"/></em></p>
    ///   <p><em>(3.5+)</em> Forces nuget.exe to run using an invariant, English-based culture.</p>
    /// </summary>
    [Pure]
    public static T DisableForceEnglishOutput<T>(this T toolSettings) where T : NuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.ForceEnglishOutput = false;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Toggles <see cref="NuGetDeleteSettings.ForceEnglishOutput"/></em></p>
    ///   <p><em>(3.5+)</em> Forces nuget.exe to run using an invariant, English-based culture.</p>
    /// </summary>
    [Pure]
    public static T ToggleForceEnglishOutput<T>(this T toolSettings) where T : NuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.ForceEnglishOutput = !toolSettings.ForceEnglishOutput;
        return toolSettings;
    }
    #endregion
    #region NonInteractive
    /// <summary>
    ///   <p><em>Sets <see cref="NuGetDeleteSettings.NonInteractive"/></em></p>
    ///   <p>Suppresses prompts for user input or confirmations.</p>
    /// </summary>
    [Pure]
    public static T SetNonInteractive<T>(this T toolSettings, bool? nonInteractive) where T : NuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.NonInteractive = nonInteractive;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="NuGetDeleteSettings.NonInteractive"/></em></p>
    ///   <p>Suppresses prompts for user input or confirmations.</p>
    /// </summary>
    [Pure]
    public static T ResetNonInteractive<T>(this T toolSettings) where T : NuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.NonInteractive = null;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Enables <see cref="NuGetDeleteSettings.NonInteractive"/></em></p>
    ///   <p>Suppresses prompts for user input or confirmations.</p>
    /// </summary>
    [Pure]
    public static T EnableNonInteractive<T>(this T toolSettings) where T : NuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.NonInteractive = true;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Disables <see cref="NuGetDeleteSettings.NonInteractive"/></em></p>
    ///   <p>Suppresses prompts for user input or confirmations.</p>
    /// </summary>
    [Pure]
    public static T DisableNonInteractive<T>(this T toolSettings) where T : NuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.NonInteractive = false;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Toggles <see cref="NuGetDeleteSettings.NonInteractive"/></em></p>
    ///   <p>Suppresses prompts for user input or confirmations.</p>
    /// </summary>
    [Pure]
    public static T ToggleNonInteractive<T>(this T toolSettings) where T : NuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.NonInteractive = !toolSettings.NonInteractive;
        return toolSettings;
    }
    #endregion
    #region NoPrompt
    /// <summary>
    ///   <p><em>Sets <see cref="NuGetDeleteSettings.NoPrompt"/></em></p>
    ///   <p>Do not prompt when deleting.</p>
    /// </summary>
    [Pure]
    public static T SetNoPrompt<T>(this T toolSettings, bool? noPrompt) where T : NuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.NoPrompt = noPrompt;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="NuGetDeleteSettings.NoPrompt"/></em></p>
    ///   <p>Do not prompt when deleting.</p>
    /// </summary>
    [Pure]
    public static T ResetNoPrompt<T>(this T toolSettings) where T : NuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.NoPrompt = null;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Enables <see cref="NuGetDeleteSettings.NoPrompt"/></em></p>
    ///   <p>Do not prompt when deleting.</p>
    /// </summary>
    [Pure]
    public static T EnableNoPrompt<T>(this T toolSettings) where T : NuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.NoPrompt = true;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Disables <see cref="NuGetDeleteSettings.NoPrompt"/></em></p>
    ///   <p>Do not prompt when deleting.</p>
    /// </summary>
    [Pure]
    public static T DisableNoPrompt<T>(this T toolSettings) where T : NuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.NoPrompt = false;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Toggles <see cref="NuGetDeleteSettings.NoPrompt"/></em></p>
    ///   <p>Do not prompt when deleting.</p>
    /// </summary>
    [Pure]
    public static T ToggleNoPrompt<T>(this T toolSettings) where T : NuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.NoPrompt = !toolSettings.NoPrompt;
        return toolSettings;
    }
    #endregion
    #region Source
    /// <summary>
    ///   <p><em>Sets <see cref="NuGetDeleteSettings.Source"/></em></p>
    ///   <p>Specifies the server URL. NuGet identifies a UNC or local folder source and simply copies the file there instead of pushing it using HTTP.  Also, starting with NuGet 3.4.2, this is a mandatory parameter unless the <em>NuGet.Config</em> file specifies a <em>DefaultPushSource</em> value (see <a href="https://docs.microsoft.com/en-us/nuget/consume-packages/configuring-nuget-behavior">Configuring NuGet behavior</a>).</p>
    /// </summary>
    [Pure]
    public static T SetSource<T>(this T toolSettings, string source) where T : NuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Source = source;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="NuGetDeleteSettings.Source"/></em></p>
    ///   <p>Specifies the server URL. NuGet identifies a UNC or local folder source and simply copies the file there instead of pushing it using HTTP.  Also, starting with NuGet 3.4.2, this is a mandatory parameter unless the <em>NuGet.Config</em> file specifies a <em>DefaultPushSource</em> value (see <a href="https://docs.microsoft.com/en-us/nuget/consume-packages/configuring-nuget-behavior">Configuring NuGet behavior</a>).</p>
    /// </summary>
    [Pure]
    public static T ResetSource<T>(this T toolSettings) where T : NuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Source = null;
        return toolSettings;
    }
    #endregion
    #region Verbosity
    /// <summary>
    ///   <p><em>Sets <see cref="NuGetDeleteSettings.Verbosity"/></em></p>
    ///   <p>Specifies the amount of detail displayed in the output: <em>normal</em>, <em>quiet</em>, <em>detailed</em>.</p>
    /// </summary>
    [Pure]
    public static T SetVerbosity<T>(this T toolSettings, NuGetVerbosity verbosity) where T : NuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Verbosity = verbosity;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="NuGetDeleteSettings.Verbosity"/></em></p>
    ///   <p>Specifies the amount of detail displayed in the output: <em>normal</em>, <em>quiet</em>, <em>detailed</em>.</p>
    /// </summary>
    [Pure]
    public static T ResetVerbosity<T>(this T toolSettings) where T : NuGetDeleteSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Verbosity = null;
        return toolSettings;
    }
    #endregion
}
#endregion
#region NuGetSignSettingsExtensions
/// <summary>
///   Used within <see cref="NuGetTasks"/>.
/// </summary>
[PublicAPI]
[ExcludeFromCodeCoverage]
public static partial class NuGetSignSettingsExtensions
{
    #region TargetPath
    /// <summary>
    ///   <p><em>Sets <see cref="NuGetSignSettings.TargetPath"/></em></p>
    ///   <p>Path of the package to sign.</p>
    /// </summary>
    [Pure]
    public static T SetTargetPath<T>(this T toolSettings, string targetPath) where T : NuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.TargetPath = targetPath;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="NuGetSignSettings.TargetPath"/></em></p>
    ///   <p>Path of the package to sign.</p>
    /// </summary>
    [Pure]
    public static T ResetTargetPath<T>(this T toolSettings) where T : NuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.TargetPath = null;
        return toolSettings;
    }
    #endregion
    #region CertificateFingerprint
    /// <summary>
    ///   <p><em>Sets <see cref="NuGetSignSettings.CertificateFingerprint"/></em></p>
    ///   <p>Specifies the fingerprint to be used to search for the certificate in a local certificate store.</p>
    /// </summary>
    [Pure]
    public static T SetCertificateFingerprint<T>(this T toolSettings, string certificateFingerprint) where T : NuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.CertificateFingerprint = certificateFingerprint;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="NuGetSignSettings.CertificateFingerprint"/></em></p>
    ///   <p>Specifies the fingerprint to be used to search for the certificate in a local certificate store.</p>
    /// </summary>
    [Pure]
    public static T ResetCertificateFingerprint<T>(this T toolSettings) where T : NuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.CertificateFingerprint = null;
        return toolSettings;
    }
    #endregion
    #region CertificatePassword
    /// <summary>
    ///   <p><em>Sets <see cref="NuGetSignSettings.CertificatePassword"/></em></p>
    ///   <p>Specifies the certificate password, if needed. If a certificate is password protected but no password is provided, the command will prompt for a password at run time, unless the -NonInteractive option is passed.</p>
    /// </summary>
    [Pure]
    public static T SetCertificatePassword<T>(this T toolSettings, [Secret] string certificatePassword) where T : NuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.CertificatePassword = certificatePassword;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="NuGetSignSettings.CertificatePassword"/></em></p>
    ///   <p>Specifies the certificate password, if needed. If a certificate is password protected but no password is provided, the command will prompt for a password at run time, unless the -NonInteractive option is passed.</p>
    /// </summary>
    [Pure]
    public static T ResetCertificatePassword<T>(this T toolSettings) where T : NuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.CertificatePassword = null;
        return toolSettings;
    }
    #endregion
    #region CertificatePath
    /// <summary>
    ///   <p><em>Sets <see cref="NuGetSignSettings.CertificatePath"/></em></p>
    ///   <p>Specifies the file path to the certificate to be used in signing the package.</p>
    /// </summary>
    [Pure]
    public static T SetCertificatePath<T>(this T toolSettings, string certificatePath) where T : NuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.CertificatePath = certificatePath;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="NuGetSignSettings.CertificatePath"/></em></p>
    ///   <p>Specifies the file path to the certificate to be used in signing the package.</p>
    /// </summary>
    [Pure]
    public static T ResetCertificatePath<T>(this T toolSettings) where T : NuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.CertificatePath = null;
        return toolSettings;
    }
    #endregion
    #region CertificateStoreLocation
    /// <summary>
    ///   <p><em>Sets <see cref="NuGetSignSettings.CertificateStoreLocation"/></em></p>
    ///   <p>Specifies the name of the X.509 certificate store use to search for the certificate. Defaults to 'CurrentUser', the X.509 certificate store used by the current user. This option should be used when specifying the certificate via -CertificateSubjectName or -CertificateFingerprint options.</p>
    /// </summary>
    [Pure]
    public static T SetCertificateStoreLocation<T>(this T toolSettings, string certificateStoreLocation) where T : NuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.CertificateStoreLocation = certificateStoreLocation;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="NuGetSignSettings.CertificateStoreLocation"/></em></p>
    ///   <p>Specifies the name of the X.509 certificate store use to search for the certificate. Defaults to 'CurrentUser', the X.509 certificate store used by the current user. This option should be used when specifying the certificate via -CertificateSubjectName or -CertificateFingerprint options.</p>
    /// </summary>
    [Pure]
    public static T ResetCertificateStoreLocation<T>(this T toolSettings) where T : NuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.CertificateStoreLocation = null;
        return toolSettings;
    }
    #endregion
    #region CertificateStoreName
    /// <summary>
    ///   <p><em>Sets <see cref="NuGetSignSettings.CertificateStoreName"/></em></p>
    ///   <p>Specifies the name of the X.509 certificate store to use to search for the certificate. Defaults to 'My', the X.509 certificate store for personal certificates. This option should be used when specifying the certificate via -CertificateSubjectName or -CertificateFingerprint options.</p>
    /// </summary>
    [Pure]
    public static T SetCertificateStoreName<T>(this T toolSettings, string certificateStoreName) where T : NuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.CertificateStoreName = certificateStoreName;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="NuGetSignSettings.CertificateStoreName"/></em></p>
    ///   <p>Specifies the name of the X.509 certificate store to use to search for the certificate. Defaults to 'My', the X.509 certificate store for personal certificates. This option should be used when specifying the certificate via -CertificateSubjectName or -CertificateFingerprint options.</p>
    /// </summary>
    [Pure]
    public static T ResetCertificateStoreName<T>(this T toolSettings) where T : NuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.CertificateStoreName = null;
        return toolSettings;
    }
    #endregion
    #region CertificateSubjectName
    /// <summary>
    ///   <p><em>Sets <see cref="NuGetSignSettings.CertificateSubjectName"/></em></p>
    ///   <p>Specifies the subject name of the certificate used to search a local certificate store for the certificate. The search is a case-insensitive string comparison using the supplied value, which will find all certificates with the subject name containing that string, regardless of other subject values. The certificate store can be specified by -CertificateStoreName and -CertificateStoreLocation options.</p>
    /// </summary>
    [Pure]
    public static T SetCertificateSubjectName<T>(this T toolSettings, string certificateSubjectName) where T : NuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.CertificateSubjectName = certificateSubjectName;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="NuGetSignSettings.CertificateSubjectName"/></em></p>
    ///   <p>Specifies the subject name of the certificate used to search a local certificate store for the certificate. The search is a case-insensitive string comparison using the supplied value, which will find all certificates with the subject name containing that string, regardless of other subject values. The certificate store can be specified by -CertificateStoreName and -CertificateStoreLocation options.</p>
    /// </summary>
    [Pure]
    public static T ResetCertificateSubjectName<T>(this T toolSettings) where T : NuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.CertificateSubjectName = null;
        return toolSettings;
    }
    #endregion
    #region HashAlgorithm
    /// <summary>
    ///   <p><em>Sets <see cref="NuGetSignSettings.HashAlgorithm"/></em></p>
    ///   <p>Hash algorithm to be used to sign the package. Defaults to SHA256. Possible values are SHA256, SHA384, and SHA512.</p>
    /// </summary>
    [Pure]
    public static T SetHashAlgorithm<T>(this T toolSettings, NuGetSignHashAlgorithm hashAlgorithm) where T : NuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.HashAlgorithm = hashAlgorithm;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="NuGetSignSettings.HashAlgorithm"/></em></p>
    ///   <p>Hash algorithm to be used to sign the package. Defaults to SHA256. Possible values are SHA256, SHA384, and SHA512.</p>
    /// </summary>
    [Pure]
    public static T ResetHashAlgorithm<T>(this T toolSettings) where T : NuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.HashAlgorithm = null;
        return toolSettings;
    }
    #endregion
    #region OutputDirectory
    /// <summary>
    ///   <p><em>Sets <see cref="NuGetSignSettings.OutputDirectory"/></em></p>
    ///   <p>Specifies the directory where the signed package should be saved. By default the original package is overwritten by the signed package.</p>
    /// </summary>
    [Pure]
    public static T SetOutputDirectory<T>(this T toolSettings, string outputDirectory) where T : NuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.OutputDirectory = outputDirectory;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="NuGetSignSettings.OutputDirectory"/></em></p>
    ///   <p>Specifies the directory where the signed package should be saved. By default the original package is overwritten by the signed package.</p>
    /// </summary>
    [Pure]
    public static T ResetOutputDirectory<T>(this T toolSettings) where T : NuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.OutputDirectory = null;
        return toolSettings;
    }
    #endregion
    #region Overwrite
    /// <summary>
    ///   <p><em>Sets <see cref="NuGetSignSettings.Overwrite"/></em></p>
    ///   <p>Switch to indicate if the current signature should be overwritten. By default the command will fail if the package already has a signature.</p>
    /// </summary>
    [Pure]
    public static T SetOverwrite<T>(this T toolSettings, bool? overwrite) where T : NuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Overwrite = overwrite;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="NuGetSignSettings.Overwrite"/></em></p>
    ///   <p>Switch to indicate if the current signature should be overwritten. By default the command will fail if the package already has a signature.</p>
    /// </summary>
    [Pure]
    public static T ResetOverwrite<T>(this T toolSettings) where T : NuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Overwrite = null;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Enables <see cref="NuGetSignSettings.Overwrite"/></em></p>
    ///   <p>Switch to indicate if the current signature should be overwritten. By default the command will fail if the package already has a signature.</p>
    /// </summary>
    [Pure]
    public static T EnableOverwrite<T>(this T toolSettings) where T : NuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Overwrite = true;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Disables <see cref="NuGetSignSettings.Overwrite"/></em></p>
    ///   <p>Switch to indicate if the current signature should be overwritten. By default the command will fail if the package already has a signature.</p>
    /// </summary>
    [Pure]
    public static T DisableOverwrite<T>(this T toolSettings) where T : NuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Overwrite = false;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Toggles <see cref="NuGetSignSettings.Overwrite"/></em></p>
    ///   <p>Switch to indicate if the current signature should be overwritten. By default the command will fail if the package already has a signature.</p>
    /// </summary>
    [Pure]
    public static T ToggleOverwrite<T>(this T toolSettings) where T : NuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Overwrite = !toolSettings.Overwrite;
        return toolSettings;
    }
    #endregion
    #region Timestamper
    /// <summary>
    ///   <p><em>Sets <see cref="NuGetSignSettings.Timestamper"/></em></p>
    ///   <p>URL to an RFC 3161 timestamping server.</p>
    /// </summary>
    [Pure]
    public static T SetTimestamper<T>(this T toolSettings, string timestamper) where T : NuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Timestamper = timestamper;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="NuGetSignSettings.Timestamper"/></em></p>
    ///   <p>URL to an RFC 3161 timestamping server.</p>
    /// </summary>
    [Pure]
    public static T ResetTimestamper<T>(this T toolSettings) where T : NuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Timestamper = null;
        return toolSettings;
    }
    #endregion
    #region TimestampHashAlgorithm
    /// <summary>
    ///   <p><em>Sets <see cref="NuGetSignSettings.TimestampHashAlgorithm"/></em></p>
    ///   <p>Hash algorithm to be used by the RFC 3161 timestamp server. Defaults to SHA256.</p>
    /// </summary>
    [Pure]
    public static T SetTimestampHashAlgorithm<T>(this T toolSettings, NuGetSignHashAlgorithm timestampHashAlgorithm) where T : NuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.TimestampHashAlgorithm = timestampHashAlgorithm;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="NuGetSignSettings.TimestampHashAlgorithm"/></em></p>
    ///   <p>Hash algorithm to be used by the RFC 3161 timestamp server. Defaults to SHA256.</p>
    /// </summary>
    [Pure]
    public static T ResetTimestampHashAlgorithm<T>(this T toolSettings) where T : NuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.TimestampHashAlgorithm = null;
        return toolSettings;
    }
    #endregion
    #region ConfigFile
    /// <summary>
    ///   <p><em>Sets <see cref="NuGetSignSettings.ConfigFile"/></em></p>
    ///   <p>The NuGet configuration file to apply. If not specified, <c>%AppData%\NuGet\NuGet.Config</c> (Windows) or <c>~/.nuget/NuGet/NuGet.Config</c> (Mac/Linux) is used.</p>
    /// </summary>
    [Pure]
    public static T SetConfigFile<T>(this T toolSettings, string configFile) where T : NuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.ConfigFile = configFile;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="NuGetSignSettings.ConfigFile"/></em></p>
    ///   <p>The NuGet configuration file to apply. If not specified, <c>%AppData%\NuGet\NuGet.Config</c> (Windows) or <c>~/.nuget/NuGet/NuGet.Config</c> (Mac/Linux) is used.</p>
    /// </summary>
    [Pure]
    public static T ResetConfigFile<T>(this T toolSettings) where T : NuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.ConfigFile = null;
        return toolSettings;
    }
    #endregion
    #region ForceEnglishOutput
    /// <summary>
    ///   <p><em>Sets <see cref="NuGetSignSettings.ForceEnglishOutput"/></em></p>
    ///   <p><em>(3.5+)</em> Forces nuget.exe to run using an invariant, English-based culture.</p>
    /// </summary>
    [Pure]
    public static T SetForceEnglishOutput<T>(this T toolSettings, bool? forceEnglishOutput) where T : NuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.ForceEnglishOutput = forceEnglishOutput;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="NuGetSignSettings.ForceEnglishOutput"/></em></p>
    ///   <p><em>(3.5+)</em> Forces nuget.exe to run using an invariant, English-based culture.</p>
    /// </summary>
    [Pure]
    public static T ResetForceEnglishOutput<T>(this T toolSettings) where T : NuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.ForceEnglishOutput = null;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Enables <see cref="NuGetSignSettings.ForceEnglishOutput"/></em></p>
    ///   <p><em>(3.5+)</em> Forces nuget.exe to run using an invariant, English-based culture.</p>
    /// </summary>
    [Pure]
    public static T EnableForceEnglishOutput<T>(this T toolSettings) where T : NuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.ForceEnglishOutput = true;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Disables <see cref="NuGetSignSettings.ForceEnglishOutput"/></em></p>
    ///   <p><em>(3.5+)</em> Forces nuget.exe to run using an invariant, English-based culture.</p>
    /// </summary>
    [Pure]
    public static T DisableForceEnglishOutput<T>(this T toolSettings) where T : NuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.ForceEnglishOutput = false;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Toggles <see cref="NuGetSignSettings.ForceEnglishOutput"/></em></p>
    ///   <p><em>(3.5+)</em> Forces nuget.exe to run using an invariant, English-based culture.</p>
    /// </summary>
    [Pure]
    public static T ToggleForceEnglishOutput<T>(this T toolSettings) where T : NuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.ForceEnglishOutput = !toolSettings.ForceEnglishOutput;
        return toolSettings;
    }
    #endregion
    #region NonInteractive
    /// <summary>
    ///   <p><em>Sets <see cref="NuGetSignSettings.NonInteractive"/></em></p>
    ///   <p>Suppresses prompts for user input or confirmations.</p>
    /// </summary>
    [Pure]
    public static T SetNonInteractive<T>(this T toolSettings, bool? nonInteractive) where T : NuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.NonInteractive = nonInteractive;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="NuGetSignSettings.NonInteractive"/></em></p>
    ///   <p>Suppresses prompts for user input or confirmations.</p>
    /// </summary>
    [Pure]
    public static T ResetNonInteractive<T>(this T toolSettings) where T : NuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.NonInteractive = null;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Enables <see cref="NuGetSignSettings.NonInteractive"/></em></p>
    ///   <p>Suppresses prompts for user input or confirmations.</p>
    /// </summary>
    [Pure]
    public static T EnableNonInteractive<T>(this T toolSettings) where T : NuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.NonInteractive = true;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Disables <see cref="NuGetSignSettings.NonInteractive"/></em></p>
    ///   <p>Suppresses prompts for user input or confirmations.</p>
    /// </summary>
    [Pure]
    public static T DisableNonInteractive<T>(this T toolSettings) where T : NuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.NonInteractive = false;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Toggles <see cref="NuGetSignSettings.NonInteractive"/></em></p>
    ///   <p>Suppresses prompts for user input or confirmations.</p>
    /// </summary>
    [Pure]
    public static T ToggleNonInteractive<T>(this T toolSettings) where T : NuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.NonInteractive = !toolSettings.NonInteractive;
        return toolSettings;
    }
    #endregion
    #region Verbosity
    /// <summary>
    ///   <p><em>Sets <see cref="NuGetSignSettings.Verbosity"/></em></p>
    ///   <p>Specifies the amount of detail displayed in the output: <em>normal</em>, <em>quiet</em>, <em>detailed</em>.</p>
    /// </summary>
    [Pure]
    public static T SetVerbosity<T>(this T toolSettings, NuGetVerbosity verbosity) where T : NuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Verbosity = verbosity;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="NuGetSignSettings.Verbosity"/></em></p>
    ///   <p>Specifies the amount of detail displayed in the output: <em>normal</em>, <em>quiet</em>, <em>detailed</em>.</p>
    /// </summary>
    [Pure]
    public static T ResetVerbosity<T>(this T toolSettings) where T : NuGetSignSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Verbosity = null;
        return toolSettings;
    }
    #endregion
}
#endregion
#region NuGetVerifySettingsExtensions
/// <summary>
///   Used within <see cref="NuGetTasks"/>.
/// </summary>
[PublicAPI]
[ExcludeFromCodeCoverage]
public static partial class NuGetVerifySettingsExtensions
{
    #region TargetPath
    /// <summary>
    ///   <p><em>Sets <see cref="NuGetVerifySettings.TargetPath"/></em></p>
    ///   <p>Path of the package to verify Signatures.</p>
    /// </summary>
    [Pure]
    public static T SetTargetPath<T>(this T toolSettings, string targetPath) where T : NuGetVerifySettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.TargetPath = targetPath;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="NuGetVerifySettings.TargetPath"/></em></p>
    ///   <p>Path of the package to verify Signatures.</p>
    /// </summary>
    [Pure]
    public static T ResetTargetPath<T>(this T toolSettings) where T : NuGetVerifySettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.TargetPath = null;
        return toolSettings;
    }
    #endregion
    #region CertificateFingerprint
    /// <summary>
    ///   <p><em>Sets <see cref="NuGetVerifySettings.CertificateFingerprint"/></em></p>
    ///   <p>Specifies one or more SHA-256 certificate fingerprints of certificates(s) which signed packages must be signed with. A certificate SHA-256 fingerprint is a SHA-256 hash of the certificate. Multiple inputs should be semicolon separated.</p>
    /// </summary>
    [Pure]
    public static T SetCertificateFingerprint<T>(this T toolSettings, string certificateFingerprint) where T : NuGetVerifySettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.CertificateFingerprint = certificateFingerprint;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="NuGetVerifySettings.CertificateFingerprint"/></em></p>
    ///   <p>Specifies one or more SHA-256 certificate fingerprints of certificates(s) which signed packages must be signed with. A certificate SHA-256 fingerprint is a SHA-256 hash of the certificate. Multiple inputs should be semicolon separated.</p>
    /// </summary>
    [Pure]
    public static T ResetCertificateFingerprint<T>(this T toolSettings) where T : NuGetVerifySettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.CertificateFingerprint = null;
        return toolSettings;
    }
    #endregion
    #region ConfigFile
    /// <summary>
    ///   <p><em>Sets <see cref="NuGetVerifySettings.ConfigFile"/></em></p>
    ///   <p>The NuGet configuration file to apply. If not specified, <c>%AppData%\NuGet\NuGet.Config</c> (Windows) or <c>~/.nuget/NuGet/NuGet.Config</c> (Mac/Linux) is used.</p>
    /// </summary>
    [Pure]
    public static T SetConfigFile<T>(this T toolSettings, string configFile) where T : NuGetVerifySettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.ConfigFile = configFile;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="NuGetVerifySettings.ConfigFile"/></em></p>
    ///   <p>The NuGet configuration file to apply. If not specified, <c>%AppData%\NuGet\NuGet.Config</c> (Windows) or <c>~/.nuget/NuGet/NuGet.Config</c> (Mac/Linux) is used.</p>
    /// </summary>
    [Pure]
    public static T ResetConfigFile<T>(this T toolSettings) where T : NuGetVerifySettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.ConfigFile = null;
        return toolSettings;
    }
    #endregion
    #region ForceEnglishOutput
    /// <summary>
    ///   <p><em>Sets <see cref="NuGetVerifySettings.ForceEnglishOutput"/></em></p>
    ///   <p><em>(3.5+)</em> Forces nuget.exe to run using an invariant, English-based culture.</p>
    /// </summary>
    [Pure]
    public static T SetForceEnglishOutput<T>(this T toolSettings, bool? forceEnglishOutput) where T : NuGetVerifySettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.ForceEnglishOutput = forceEnglishOutput;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="NuGetVerifySettings.ForceEnglishOutput"/></em></p>
    ///   <p><em>(3.5+)</em> Forces nuget.exe to run using an invariant, English-based culture.</p>
    /// </summary>
    [Pure]
    public static T ResetForceEnglishOutput<T>(this T toolSettings) where T : NuGetVerifySettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.ForceEnglishOutput = null;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Enables <see cref="NuGetVerifySettings.ForceEnglishOutput"/></em></p>
    ///   <p><em>(3.5+)</em> Forces nuget.exe to run using an invariant, English-based culture.</p>
    /// </summary>
    [Pure]
    public static T EnableForceEnglishOutput<T>(this T toolSettings) where T : NuGetVerifySettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.ForceEnglishOutput = true;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Disables <see cref="NuGetVerifySettings.ForceEnglishOutput"/></em></p>
    ///   <p><em>(3.5+)</em> Forces nuget.exe to run using an invariant, English-based culture.</p>
    /// </summary>
    [Pure]
    public static T DisableForceEnglishOutput<T>(this T toolSettings) where T : NuGetVerifySettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.ForceEnglishOutput = false;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Toggles <see cref="NuGetVerifySettings.ForceEnglishOutput"/></em></p>
    ///   <p><em>(3.5+)</em> Forces nuget.exe to run using an invariant, English-based culture.</p>
    /// </summary>
    [Pure]
    public static T ToggleForceEnglishOutput<T>(this T toolSettings) where T : NuGetVerifySettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.ForceEnglishOutput = !toolSettings.ForceEnglishOutput;
        return toolSettings;
    }
    #endregion
    #region NonInteractive
    /// <summary>
    ///   <p><em>Sets <see cref="NuGetVerifySettings.NonInteractive"/></em></p>
    ///   <p>Suppresses prompts for user input or confirmations.</p>
    /// </summary>
    [Pure]
    public static T SetNonInteractive<T>(this T toolSettings, bool? nonInteractive) where T : NuGetVerifySettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.NonInteractive = nonInteractive;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="NuGetVerifySettings.NonInteractive"/></em></p>
    ///   <p>Suppresses prompts for user input or confirmations.</p>
    /// </summary>
    [Pure]
    public static T ResetNonInteractive<T>(this T toolSettings) where T : NuGetVerifySettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.NonInteractive = null;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Enables <see cref="NuGetVerifySettings.NonInteractive"/></em></p>
    ///   <p>Suppresses prompts for user input or confirmations.</p>
    /// </summary>
    [Pure]
    public static T EnableNonInteractive<T>(this T toolSettings) where T : NuGetVerifySettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.NonInteractive = true;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Disables <see cref="NuGetVerifySettings.NonInteractive"/></em></p>
    ///   <p>Suppresses prompts for user input or confirmations.</p>
    /// </summary>
    [Pure]
    public static T DisableNonInteractive<T>(this T toolSettings) where T : NuGetVerifySettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.NonInteractive = false;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Toggles <see cref="NuGetVerifySettings.NonInteractive"/></em></p>
    ///   <p>Suppresses prompts for user input or confirmations.</p>
    /// </summary>
    [Pure]
    public static T ToggleNonInteractive<T>(this T toolSettings) where T : NuGetVerifySettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.NonInteractive = !toolSettings.NonInteractive;
        return toolSettings;
    }
    #endregion
    #region Verbosity
    /// <summary>
    ///   <p><em>Sets <see cref="NuGetVerifySettings.Verbosity"/></em></p>
    ///   <p>Specifies the amount of detail displayed in the output: <em>normal</em>, <em>quiet</em>, <em>detailed</em>.</p>
    /// </summary>
    [Pure]
    public static T SetVerbosity<T>(this T toolSettings, NuGetVerbosity verbosity) where T : NuGetVerifySettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Verbosity = verbosity;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="NuGetVerifySettings.Verbosity"/></em></p>
    ///   <p>Specifies the amount of detail displayed in the output: <em>normal</em>, <em>quiet</em>, <em>detailed</em>.</p>
    /// </summary>
    [Pure]
    public static T ResetVerbosity<T>(this T toolSettings) where T : NuGetVerifySettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.Verbosity = null;
        return toolSettings;
    }
    #endregion
}
#endregion
#region NuGetVerbosity
/// <summary>
///   Used within <see cref="NuGetTasks"/>.
/// </summary>
[PublicAPI]
[Serializable]
[ExcludeFromCodeCoverage]
[TypeConverter(typeof(TypeConverter<NuGetVerbosity>))]
public partial class NuGetVerbosity : Enumeration
{
    /// <summary>
    /// Normal
    /// </summary>
    public static NuGetVerbosity Normal = (NuGetVerbosity)"Normal";
    /// <summary>
    /// Quiet
    /// </summary>
    public static NuGetVerbosity Quiet = (NuGetVerbosity)"Quiet";
    /// <summary>
    ///  Detailed
    /// </summary>
    public static NuGetVerbosity Detailed = (NuGetVerbosity)"Detailed";
    /// <summary>
    /// NuGetVerbosity
    /// </summary>
    /// <param name="value"></param>
    public static implicit operator NuGetVerbosity(string value)
    {
        return new NuGetVerbosity { Value = value };
    }
}
#endregion
#region NuGetSignHashAlgorithm
/// <summary>
///   Used within <see cref="NuGetTasks"/>.
/// </summary>
[PublicAPI]
[Serializable]
[ExcludeFromCodeCoverage]
[TypeConverter(typeof(TypeConverter<NuGetSignHashAlgorithm>))]
public partial class NuGetSignHashAlgorithm : Enumeration
{
    /// <summary>
    /// sha256
    /// </summary>
    public static NuGetSignHashAlgorithm sha256 = (NuGetSignHashAlgorithm)"sha256";
    /// <summary>
    /// sha384
    /// </summary>
    public static NuGetSignHashAlgorithm sha384 = (NuGetSignHashAlgorithm)"sha384";
    /// <summary>
    /// sha512
    /// </summary>
    public static NuGetSignHashAlgorithm sha512 = (NuGetSignHashAlgorithm)"sha512";
    /// <summary>
    /// NuGetSignHashAlgorithm
    /// </summary>
    /// <param name="value"></param>
    public static implicit operator NuGetSignHashAlgorithm(string value)
    {
        return new NuGetSignHashAlgorithm { Value = value };
    }
}
#endregion
