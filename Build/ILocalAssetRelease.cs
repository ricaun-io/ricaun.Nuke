using Nuke.Common;
using ricaun.Nuke.Components;
using ricaun.Nuke.Extensions;

class AssetRelease : IAssetRelease
{
    public void ReleaseAsset(ReleaseAssets releaseAssets)
    {
        Serilog.Log.Information($"Project: {releaseAssets.Project.Name}");
        Serilog.Log.Information($"Version: {releaseAssets.Version}");
        Serilog.Log.Information($"Notes: {releaseAssets.Notes}");
        Serilog.Log.Information($"Prerelease: {releaseAssets.Prerelease}");
        foreach (var file in releaseAssets.Assets)
        {
            Serilog.Log.Information($"File: {file}");
        }
    }
}

public interface ILocalAssetRelease : IClean, ICompile, IHazAssetRelease
{
    Target LocalAssetRelease => _ => _
        .TriggeredBy(Clean)
        .Before(Compile)
        .Executes(() =>
        {
            var releaseAssets = new ReleaseAssets
            {
                Project = MainProject,
                Version = "0.0.0",
                Notes = "Release Notes",
            };
            ReleaseAsset(releaseAssets);
        });
}
