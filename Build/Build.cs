using Nuke.Common;
using Nuke.Common.Execution;
using ricaun.Nuke;
using ricaun.Nuke.Components;

[CheckBuildProjectConfigurations]
class Build : NukeBuild, IPublishPack
{
    string IHazContent.Folder => "Release";
    string IHazRelease.Folder => "ReleasePack";
    public static int Main() => Execute<Build>(x => x.From<IPublishPack>().Build);
}