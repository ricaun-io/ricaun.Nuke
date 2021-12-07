using Nuke.Common;
using Nuke.Common.Execution;
using ricaun.Nuke;

[CheckBuildProjectConfigurations]
class Build : NukeBuild, IPublish
{
    public static int Main() => Execute<Build>(x => x.From<IPublish>().Build);
}