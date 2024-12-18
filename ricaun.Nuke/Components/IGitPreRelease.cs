using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.Utilities.Collections;
using ricaun.Nuke.Extensions;
using System.Linq;

namespace ricaun.Nuke.Components
{
    /// <summary>
    /// IGitRelease
    /// </summary>
    public interface IGitPreRelease : IGitRelease
    {
        /// <summary>
        /// PreReleaseFilter (default: ["alpha", "beta", "rc"])
        /// </summary>
        [Parameter]
        string[] PreReleaseFilter => TryGetValue(() => PreReleaseFilter) ?? new[] { "alpha", "beta", "rc" };

        /// <summary>
        /// HasPreReleaseFilter
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public bool HasPreReleaseFilter(string version)
        {
            return PreReleaseFilter.Any(e => version.Contains(e));
        }

        /// <summary>
        /// Target GitRelease
        /// </summary>
        Target GitPreRelease => _ => _
            .TriggeredBy(Release)
            .After(GitRelease)
            .Requires(() => GitRepository)
            .Requires(() => GitVersion)
            .OnlyWhenStatic(() => GitHubToken.SkipEmpty())
            .OnlyWhenStatic(() => IsServerBuild)
            .OnlyWhenDynamic(() => GitRepository.IsOnDevelopBranch())
            .OnlyWhenDynamic(() => SkipForked())
            .Executes(() =>
            {
                var project = MainProject;
                var version = project.GetInformationalVersion();
                if (HasPreReleaseFilter(version) == false)
                {
                    ReportSummary(_ => _.AddPair("Skipped", version));
                    return;
                }

                if (project.IsVersionPreRelease() == false)
                {
                    ReportSummary(_ => _.AddPair("Ignore", version));
                    return;
                }
                ReportSummary(_ => _.AddPair("Prerelease", version));
                ReleaseGitHubProject(MainProject, true);
            });
    }
}
