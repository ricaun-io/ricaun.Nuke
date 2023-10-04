using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.Utilities.Collections;
using ricaun.Nuke.Extensions;

namespace ricaun.Nuke.Components
{
    /// <summary>
    /// IGitRelease
    /// </summary>
    public interface IGitPreRelease : IGitRelease
    {
        /// <summary>
        /// Target GitRelease
        /// </summary>
        Target GitPreRelease => _ => _
            .TriggeredBy(Release)
            .Requires(() => GitRepository)
            .Requires(() => GitVersion)
            .OnlyWhenStatic(() => GitHubToken.SkipEmpty())
            .OnlyWhenStatic(() => IsServerBuild)
            .OnlyWhenDynamic(() => GitRepository.IsOnDevelopBranch())
            .Executes(() =>
            {
                var project = MainProject;
                if (project.IsVersionPreRelease() == false)
                {
                    ReportSummary(_ => _.AddPair("Ignore", $"{MainProject.Name} {MainProject.GetInformationalVersion()}"));
                    return;
                }
                ReportSummary(_ => _.AddPair("PreRelease", $"{MainProject.Name} {MainProject.GetInformationalVersion()}"));
                ReleaseGithubProject(MainProject, false);
            });
    }
}
