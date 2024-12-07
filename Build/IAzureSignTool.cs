using Nuke.Common;
using ricaun.Nuke.Components;

public interface IAzureSignTool : IClean, ICompile
{
    Target AzureSignTool => _ => _
        .TriggeredBy(Clean)
        .Before(Compile)
        .Executes(() =>
        {
            ricaun.Nuke.Tools.AzureSignToolUtils.DownloadAzureSignTool();
            Serilog.Log.Information("DownloadAzureSignTool");
            ricaun.Nuke.Tools.AzureSignToolUtils.DownloadNuGetKeyVaultSignTool();
            Serilog.Log.Information("DownloadNuGetKeyVaultSignTool");

            ricaun.Nuke.Tools.AzureSignToolUtils.EnsureAzureToolIsInstalled();
            Serilog.Log.Information("EnsureAzureToolIsInstalled");
        });
}
