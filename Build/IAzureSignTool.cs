using Nuke.Common;
using Nuke.Common.Tools.AzureSignTool;
using Nuke.Common.Tools.NuGetKeyVaultSignTool;
using ricaun.Nuke.Components;

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
