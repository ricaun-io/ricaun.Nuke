using Nuke.Common;
using Nuke.Common.Execution;
using ricaun.Nuke;
using ricaun.Nuke.Components;

class Build : NukeBuild, IPublishPack, ICompileExample, ITest, IShowGitVersion, IAzureSignTool, IPrePack, ILocalAssetRelease
{
    public void ReleaseAsset(ReleaseAssets releaseAssets) { }
    IAssetRelease IHazAssetRelease.AssetRelease => new AssetRelease();
    //bool IPack.UnlistNuget => true;
    bool ITest.TestBuildStopWhenFailed => false;
    public static int Main() => Execute<Build>(x => x.From<IPublishPack>().Build);
}
