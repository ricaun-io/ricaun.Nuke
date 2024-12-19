using Nuke.Common;
using Nuke.Common.Git;
using ricaun.Nuke.Components;
using ricaun.Nuke.Extensions;

public interface IShowGitVersion : IHazGitVersion, IHazGitRepository, IHazChangelog, IClean, ICompile
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


            Serilog.Log.Information("Commit = {Value}", GitRepository.Commit);
            Serilog.Log.Information("Branch = {Value}", GitRepository.Branch);
            Serilog.Log.Information("Tags = {Value}", GitRepository.Tags);
            Serilog.Log.Information("Head = {Value}", GitRepository.Head);
            Serilog.Log.Information("Identifier = {Value}", GitRepository.Identifier);

            Serilog.Log.Information("IsForked = {Value}", GitRepository.IsForked());

            Serilog.Log.Information("main branch = {Value}", GitRepository.IsOnMainBranch());
            Serilog.Log.Information("main/master branch = {Value}", GitRepository.IsOnMainOrMasterBranch());
            Serilog.Log.Information("release/* branch = {Value}", GitRepository.IsOnReleaseBranch());
            Serilog.Log.Information("hotfix/* branch = {Value}", GitRepository.IsOnHotfixBranch());

            Serilog.Log.Information("Https URL = {Value}", GitRepository.HttpsUrl);
            Serilog.Log.Information("SSH URL = {Value}", GitRepository.SshUrl);

        });
}