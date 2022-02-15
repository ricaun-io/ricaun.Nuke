using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.Tools.GitHub;
using Octokit;
using ricaun.Nuke.Extensions;
using System;
using System.IO;

namespace ricaun.Nuke.Components
{
    /// <summary>
    /// IGitRelease
    /// </summary>
    public interface IGitRelease : IRelease, IHazGitRepository, IHazGitVersion, IHazChangelog, INukeBuild
    {
        /// <summary>
        /// Target GitRelease
        /// </summary>
        Target GitRelease => _ => _
            .TriggeredBy(Release)
            .Requires(() => GitRepository)
            .Requires(() => GitVersion)
            .OnlyWhenStatic(() => GitHubToken.SkipEmpty())
            .OnlyWhenStatic(() => IsServerBuild)
            .OnlyWhenDynamic(() => GitRepository.IsOnMainOrMasterBranch())
            .Executes(() =>
            {
                var project = MainProject;

                if (Directory.Exists(ReleaseDirectory) == false)
                {
                    Serilog.Log.Warning($"Release Directory not Found: {ReleaseDirectory}");
                    return;
                }

                GitHubTasks.GitHubClient = new GitHubClient(new ProductHeaderValue(Solution.Name))
                {
                    Credentials = new Credentials(GitHubToken)
                };

                var gitHubName = GitRepository.GetGitHubName();
                var gitHubOwner = GitRepository.GetGitHubOwner();

                var releaseFiles = PathConstruction.GlobFiles(ReleaseDirectory, "*.zip");
                var version = project.GetInformationalVersion();

                if (GitHubExtension.CheckTags(gitHubOwner, gitHubName, version))
                {
                    Serilog.Log.Warning($"The repository already contains a Release with the tag: {version}");
                    return;
                }

                var newRelease = new NewRelease(version)
                {
                    Name = version,
                    Body = GetNuGetReleaseNotes(),
                    Draft = true,
                    TargetCommitish = GitVersion.Sha
                };

                var draft = GitHubExtension.CreatedDraft(gitHubOwner, gitHubName, newRelease);
                GitHubExtension.UploadReleaseFiles(draft, releaseFiles);
                GitHubExtension.ReleaseDraft(gitHubOwner, gitHubName, draft);
            });
    }
}
