using Nuke.Common;
using Nuke.Common.Execution;
using ricaun.Nuke;
using ricaun.Nuke.Components;
using ricaun.Nuke.Extensions;

class Build : NukeBuild, IPublishPack, ICompileExample, ITest, IShowGitVersion, IAzureSignTool, IPrePack, ILocalAssetRelease
{
    public Build() => BuildExtension.BuildTools.dotnetOnly();
    public void ReleaseAsset(ReleaseAssets releaseAssets) { }
    IAssetRelease IHazAssetRelease.AssetRelease => new AssetRelease();
    //bool IPack.UnlistNuGet => true;
    bool ITest.TestBuildStopWhenFailed => false;
    public static int Main() => Execute<Build>(x => x.From<IPublishPack>().Build);
}
