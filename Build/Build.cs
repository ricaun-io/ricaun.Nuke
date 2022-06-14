using Nuke.Common;
using Nuke.Common.Execution;
using ricaun.Nuke;
using ricaun.Nuke.Components;

class Build : NukeBuild, IPublishPack, ICompileExample, IShowGitVersion
{
    public static int Main() => Execute<Build>(x => x.From<IPublishPack>().Build);
}

public interface IShowGitVersion : IHazGitVersion, IClean
{
    Target ShowGitVersion => _ => _
        .TriggeredBy(Clean)
        .Executes(() =>
        {
            Serilog.Log.Information(GitVersion.BranchName);
        });
}