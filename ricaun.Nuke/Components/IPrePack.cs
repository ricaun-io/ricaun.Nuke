﻿using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Utilities.Collections;
using System.Linq;

namespace ricaun.Nuke.Components
{
    /// <summary>
    /// IPrePack
    /// </summary>
    public interface IPrePack : IPack, IGitPreRelease
    {
        /// <summary>
        /// PrePack
        /// </summary>
        Target PrePack => _ => _
            .TriggeredBy(Release)
            .After(Pack)
            .After(GitPreRelease)
            //.OnlyWhenStatic(() => NugetApiUrl.SkipEmpty())
            //.OnlyWhenStatic(() => NugetApiKey.SkipEmpty())
            //.OnlyWhenStatic(() => IsServerBuild)
            //.OnlyWhenDynamic(() => GitRepository.IsOnDevelopBranch())
            .Executes(() =>
            {
                var releaseDirectory = GetReleaseDirectory(MainProject);
                var prerelease = Globbing.GlobFiles(releaseDirectory, "**/*.nupkg")
                   .Where(IsPrePackFile);

                var message = string.Join(" | ", prerelease.Select(e => e.Name));

                if (prerelease.IsEmpty())
                {
                    ReportSummary(_ => _.AddPair("Ignore", message));
                    return;
                }

                ReportSummary(_ => _.AddPair("Prerelease", message));
                prerelease.ForEach(x =>
                {
                    //DotNetTasks.DotNetNuGetPush(s => s
                    //     .SetTargetPath(x)
                    //     .SetSource(NugetApiUrl)
                    //     .SetApiKey(NugetApiKey)
                    //     .EnableSkipDuplicate()
                    //);
                });
            });

        private bool IsPrePackFile(AbsolutePath absolutePath)
        {
            return absolutePath.Name.Contains("-");
        }
    }
}