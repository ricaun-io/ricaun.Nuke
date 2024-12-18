using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.GitHub;
using Nuke.Common.Utilities.Collections;
using ricaun.Nuke.Extensions;
using System;
using System.IO;
using System.Linq;

namespace ricaun.Nuke.Components
{
    /// <summary>
    /// IGitRelease
    /// </summary>
    public interface IGitRelease : IRelease, IHazGitRepository, IHazGitVersion, IHazChangelog, IHazAssetRelease, INukeBuild
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

                if (project.IsVersionPreRelease())
                {
                    ReportSummary(_ => _.AddPair("Prerelease", project.GetInformationalVersion()));
                    var errorMessage = $"The project {project.Name} is a pre-release";
                    Serilog.Log.Error(errorMessage);
                    throw new Exception(errorMessage);
                }

                ReleaseGitHubProject(project);
            });

        /// <summary>
        /// Release GitHub project with release notes
        /// </summary>
        /// <param name="project"></param>
        /// <param name="releaseAsPrerelease"></param>
        void ReleaseGitHubProject(Project project, bool releaseAsPrerelease = false)
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

            var releaseNotes = GetReleaseNotes();
            var releaseAssets = new ReleaseAssets
            {
                Project = project,
                Version = version,
                Notes = releaseNotes,
                Assets = releaseFiles.ToArray(),
                Prerelease = releaseAsPrerelease
            };

            ReleaseAsset(releaseAssets);

            var newRelease = new Octokit.NewRelease(version)
            {
                Name = version,
                Body = releaseNotes,
                Draft = true,
                TargetCommitish = GitVersion.Sha
            };

            var draft = GitHubExtension.CreatedDraft(gitHubOwner, gitHubName, newRelease);
            GitHubExtension.UploadReleaseFiles(draft, releaseFiles);
            GitHubExtension.ReleaseDraft(gitHubOwner, gitHubName, draft, releaseAsPrerelease);
        }
    }
}
