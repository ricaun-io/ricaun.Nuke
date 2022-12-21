using Nuke.Common;
using Nuke.Common.Execution;
using ricaun.Nuke;
using ricaun.Nuke.Components;

class Build : NukeBuild, IPublishPack, ICompileExample, ITest, IShowGitVersion
{
    public static int Main() => Execute<Build>(x => x.From<IPublishPack>().Build);
}

public interface IShowGitVersion : IHazGitVersion, IHazChangelog, IClean
{
    Target ShowGitVersion => _ => _
        .TriggeredBy(Clean)
        .Executes(() =>
        {
            // GitVersion.BranchName
            Serilog.Log.Information(GitVersion.BranchName);

            // GetReleaseNotes
            Serilog.Log.Information(GetReleaseNotes());

            // Test DownloadFile
            ricaun.Nuke.Extensions.HttpClientExtension.DownloadFileRetry(
                @"https://api.github.com/repos/ricaun-io/ricaun.Nuke/releases/latest",
                System.IO.Path.Combine(BuildAssemblyDirectory, $"latest.json"));
        });
}