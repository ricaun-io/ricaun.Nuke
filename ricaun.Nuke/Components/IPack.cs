using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using ricaun.Nuke.Extensions;

namespace ricaun.Nuke.Components
{
    /// <summary>
    /// IPack
    /// </summary>
    public interface IPack : IHazPack, IGitRelease
    {
        /// <summary>
        /// Target Pack
        /// </summary>
        Target Pack => _ => _
            .TriggeredBy(Release)
            .After(GitRelease)
            .OnlyWhenStatic(() => NugetApiUrl.SkipEmpty())
            .OnlyWhenStatic(() => NugetApiKey.SkipEmpty())
            .OnlyWhenStatic(() => IsServerBuild)
            .OnlyWhenDynamic(() => GitRepository.IsOnMainOrMasterBranch())
            .Executes(() =>
            {
                var releaseDirectory = GetReleaseDirectory(MainProject);
                Globbing.GlobFiles(releaseDirectory, "**/*.nupkg")
                   .ForEach(DotNetNuGetPush);
            });
    }
}
