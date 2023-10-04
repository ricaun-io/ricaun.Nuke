using Nuke.Common;
using ricaun.Nuke.Components;
using ricaun.Nuke.Extensions;

public interface IShowGitVersion : IHazGitVersion, IHazChangelog, IClean, ICompile
{
    Target ShowGitVersion => _ => _
        .TriggeredBy(Clean)
        .Before(Compile)
        .Executes(() =>
        {
            // GitVersion.BranchName
            Serilog.Log.Information(GitVersion.BranchName);

            // GetReleaseNotes
            Serilog.Log.Information(GetReleaseNotes());

            try
            {
                // Test DownloadFile
                HttpClientExtension.DownloadFileRetry(
                    @"https://api.github.com/repos/ricaun-io/ricaun.Nuke/releases/latest",
                    System.IO.Path.Combine(BuildAssemblyDirectory, $"latest.json"));
            }
            catch { }
        });
}