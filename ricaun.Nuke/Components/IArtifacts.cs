using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.CI;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using ricaun.Nuke.Extensions;

namespace ricaun.Nuke.Components
{
    interface IArtifacts : IHazArtifacts, IHazContent, ISign, IHazGitRepository, INukeBuild
    {
        Target Artifacts => _ => _
            .TriggeredBy(Sign)
            .Produces(ArtifactsDirectory / "*.nupkg", ArtifactsDirectory / "*.dll")
            .OnlyWhenStatic(() => IsServerBuild)
            .Executes(() =>
            {
                if (!FileSystemTasks.DirectoryExists(ContentDirectory))
                {
                    Logger.Warn($"Skip Not found: {ContentDirectory}");
                    return;
                }

                FileSystemTasks.CopyDirectoryRecursively(ContentDirectory, ArtifactsDirectory);
            });
    }
}
