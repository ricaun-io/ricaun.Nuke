using Nuke.Common;
using Nuke.Common.Tools.AzureSignTool;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Tools.NuGet;
using ricaun.Nuke.Components;
using ricaun.Nuke.Tools.NuGetKeyVaultSignTool;

public interface IAzureSignTool : IClean, ICompile
{
    Target AzureSignTool => _ => _
        .TriggeredBy(Clean)
        .Before(Compile)
        //.Requires<NuGetKeyVaultSignToolTasks>()
        //.Requires<AzureSignToolTasks>()
        .Requires<GitVersionTasks>()
        .Requires<NuGetTasks>()
        .Executes(() =>
        {
            Serilog.Log.Information(AzureSignToolTasks.AzureSignToolPath);
            Serilog.Log.Information(NuGetKeyVaultSignToolTasks.NuGetKeyVaultSignToolPath);

            //ricaun.Nuke.Tools.AzureSignToolUtils.DownloadAzureSignTool();
            //Serilog.Log.Information("DownloadAzureSignTool");
            //ricaun.Nuke.Tools.AzureSignToolUtils.DownloadNuGetKeyVaultSignTool();
            //Serilog.Log.Information("DownloadNuGetKeyVaultSignTool");

            //ricaun.Nuke.Tools.AzureSignToolUtils.EnsureAzureToolIsInstalled();
            //Serilog.Log.Information("EnsureAzureToolIsInstalled");
        });
}
