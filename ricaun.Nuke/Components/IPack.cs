﻿using Nuke.Common;
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
    public interface IPack : IHazPack, IHazRelease, ISign, IHazGitRepository, INukeBuild
    {
        /// <summary>
        /// Target Pack
        /// </summary>
        Target Pack => _ => _
            .TriggeredBy(Sign)
            .OnlyWhenStatic(() => NugetApiUrl.SkipEmpty())
            .OnlyWhenStatic(() => NugetApiKey.SkipEmpty())
            .OnlyWhenStatic(() => IsServerBuild)
            .OnlyWhenDynamic(() => GitRepository.IsOnMainOrMasterBranch())
            .Executes(() =>
            {
                var releaseDirectory = GetReleaseDirectory(MainProject);
                Globbing.GlobFiles(releaseDirectory, "**/*.nupkg")
                   .ForEach(x =>
                   {
                       DotNetTasks.DotNetNuGetPush(s => s
                            .SetTargetPath(x)
                            .SetSource(NugetApiUrl)
                            .SetApiKey(NugetApiKey)
                            .EnableSkipDuplicate()
                       );
                   });
            });
    }
}
