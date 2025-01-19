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
        /// UnlistNuGet (Default: false)
        /// </summary>
        /// <remarks>This feature only works on 'api.nuget.org' to unlist package.</remarks>
        [Parameter]
        bool UnlistNuGet => TryGetValue<bool?>(() => UnlistNuGet) ?? false;

        /// <summary>
        /// Target Pack
        /// </summary>
        Target Pack => _ => _
            .TriggeredBy(Release)
            .After(GitRelease)
            .OnlyWhenStatic(() => NuGetApiUrl.SkipEmpty())
            .OnlyWhenStatic(() => NuGetApiKey.SkipEmpty())
            .OnlyWhenStatic(() => IsServerBuild)
            .OnlyWhenDynamic(() => GitRepository.IsOnMainOrMasterBranch())
            .OnlyWhenDynamic(() => SkipForked())
            .Executes(() =>
            {
                var releaseDirectory = GetReleaseDirectory(MainProject);
                var packages = Globbing.GlobFiles(releaseDirectory, "**/*.nupkg");

                if (UnlistNuGet)
                {
                    packages.ForEach(DotNetNuGetPrerelease);
                    return;
                }

                packages.ForEach(DotNetNuGetPush);
            });
    }
}
