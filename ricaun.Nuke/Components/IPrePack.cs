﻿using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.Utilities.Collections;
using ricaun.Nuke.Extensions;
using System;
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
            .OnlyWhenStatic(() => NuGetApiUrl.SkipEmpty())
            .OnlyWhenStatic(() => NuGetApiKey.SkipEmpty())
            .OnlyWhenStatic(() => IsServerBuild)
            .OnlyWhenDynamic(() => GitRepository.IsOnDevelopBranch())
            .OnlyWhenDynamic(() => SkipForked())
            .Executes(() =>
            {
                var version = MainProject.GetInformationalVersion();
                if (HasPreReleaseFilter(version) == false)
                {
                    ReportSummary(_ => _.AddPair("Skipped", version));
                    return;
                }

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

                prerelease.ForEach(DotNetNuGetPrerelease);
            });
    }
}
