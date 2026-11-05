using Nuke.Common;
using Nuke.Common.Execution;
using ricaun.Nuke;
using ricaun.Nuke.Components;
using System;

class Build : NukeBuild, IPublishPack, ICompileExample, ITest, IShowGitVersion, IAzureSignTool, IPrePack, ILocalAssetRelease
{
    public Build() => this.dotnetBuildOnly();
    public void ReleaseAsset(ReleaseAssets releaseAssets) { }
    IAssetRelease IHazAssetRelease.AssetRelease => new AssetRelease();
    //bool IPack.UnlistNuGet => true;
    bool ITest.TestBuildStopWhenFailed => false;
    public static int Main() => Execute<Build>(x => x.From<IPublishPack>().Build);
}

#if DEBUG
public class NukeInjectionDebug : INukeInjection
{
    public NukeInjectionDebug(IHazGitRepository hazGitRepository)
    {
        Serilog.Log.Information($"NukeInjectionDebug {hazGitRepository.GitRepository}");
        throw new Exception($"NukeInjectionDebug {hazGitRepository.GitRepository}");
    }
}
#endif
