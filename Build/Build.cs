using Nuke.Common;
using Nuke.Common.Execution;
using ricaun.Nuke;
using ricaun.Nuke.Components;

class AssetRelease : IAssetRelease
{
    public void ReleaseAsset(ReleaseAssets releaseAssets)
    {
        Serilog.Log.Information($"Project: {releaseAssets.Project.Name}");
        Serilog.Log.Information($"Version: {releaseAssets.Version}");
        Serilog.Log.Information($"Notes: {releaseAssets.Notes}");
        Serilog.Log.Information($"Prerelease: {releaseAssets.Prerelease}");
        foreach (var file in releaseAssets.Files)
        {
            Serilog.Log.Information($"File: {file}");
        }
    }
}

class Build : NukeBuild, IPublishPack, ICompileExample, ITest, IShowGitVersion, IAzureSignTool, IPrePack
{
    IAssetRelease IHazAssetRelease.AssetRelease => null;
    //bool IPack.UnlistNuget => true;
    bool ITest.TestBuildStopWhenFailed => false;
    public static int Main() => Execute<Build>(x => x.From<IPublishPack>().Build);
}
