using Nuke.Common;
using Nuke.Common.Execution;
using ricaun.Nuke;

[CheckBuildProjectConfigurations]
class Build : NukeBuild, IPublishPack
{
    public static int Main() => Execute<Build>(x => x.From<IPublishPack>().Build);
}