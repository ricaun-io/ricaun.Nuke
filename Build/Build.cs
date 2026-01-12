using Nuke.Common;
using Nuke.Common.Execution;
using ricaun.Nuke;
using ricaun.Nuke.Components;

class Build : NukeBuild, IPublishPack, ICompileExample, ITest, IShowGitVersion, IAzureSignTool, IPrePack, ILocalAssetRelease, ICompileAfter, ICompileBefore
{
    public void ReleaseAsset(ReleaseAssets releaseAssets) { }
    IAssetRelease IHazAssetRelease.AssetRelease => new AssetRelease();
    //bool IPack.UnlistNuGet => true;
    bool ITest.TestBuildStopWhenFailed => false;
    public static int Main() => Execute<Build>(x => x.From<IPublishPack>().Build);
}
