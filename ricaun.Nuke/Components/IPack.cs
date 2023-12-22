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
        /// UnlistNuget (Default: false)
        /// </summary>
        /// <remarks>This feature only works on 'api.nuget.org' to unlist package.</remarks>
        [Parameter]
        bool UnlistNuget => TryGetValue<bool?>(() => UnlistNuget) ?? false;

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
                var packages = Globbing.GlobFiles(releaseDirectory, "**/*.nupkg");

                if (UnlistNuget)
                {
                    packages.ForEach(DotNetNuGetPrerelease);
                    return;
                }

                packages.ForEach(DotNetNuGetPush);
            });
    }
}
