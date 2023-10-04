using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.GitHub;
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
                ReleaseGithubProject(project);
            });

        /// <summary>
        /// Release Github project with release notes
        /// </summary>
        /// <param name="project"></param>
        /// <param name="releaseAsPrerelease"></param>
        void ReleaseGithubProject(Project project, bool releaseAsPrerelease = false)
        {
            if (Directory.Exists(ReleaseDirectory) == false)
            {
                Serilog.Log.Warning($"Release Directory not Found: {ReleaseDirectory}");
                return;
            }

            Serilog.Log.Information($"GitHubTasks.GitHubClient: {Solution.Name}");

            GitHubTasks.GitHubClient = new Octokit.GitHubClient(new Octokit.ProductHeaderValue(Solution.Name))
            {
                Credentials = new Octokit.Credentials(GitHubToken)
            };

            var gitHubName = GitRepository.GetGitHubName();
            var gitHubOwner = GitRepository.GetGitHubOwner();

            var releaseFiles = Globbing.GlobFiles(ReleaseDirectory, "*.zip");
            var version = project.GetInformationalVersion();

            Serilog.Log.Information($"GitHubTasks.CheckTags: {gitHubOwner} {gitHubName} {version}");

            if (GitHubExtension.CheckTags(gitHubOwner, gitHubName, version))
            {
                Serilog.Log.Warning($"The repository already contains a Release with the tag: {version}");
                return;
            }

            var newRelease = new Octokit.NewRelease(version)
            {
                Name = version,
                Body = GetReleaseNotes(),
                Draft = true,
                TargetCommitish = GitVersion.Sha
            };

            var draft = GitHubExtension.CreatedDraft(gitHubOwner, gitHubName, newRelease);
            GitHubExtension.UploadReleaseFiles(draft, releaseFiles);
            GitHubExtension.ReleaseDraft(gitHubOwner, gitHubName, draft, releaseAsPrerelease);
        }
    }
}
