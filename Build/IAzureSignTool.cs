using Nuke.Common;
using Nuke.Common.Tools.AzureSignTool;
using ricaun.Nuke.Components;
using ricaun.Nuke.Tools.NuGetKeyVaultSignTool;

public interface IAzureSignTool : IClean, ICompile
{
    Target AzureSignTool => _ => _
        .TriggeredBy(Clean)
        .Before(Compile)
        .Executes(() =>
        {
            ricaun.Nuke.Tools.AzureSignToolUtils.EnsureAzureToolIsInstalled();

            Serilog.Log.Information(AzureSignToolTasks.AzureSignToolPath);
            Serilog.Log.Information(NuGetKeyVaultSignToolTasks.NuGetKeyVaultSignToolPath);
        });
}
