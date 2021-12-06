using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using ricaun.Nuke.Extensions;

namespace ricaun.Nuke.Components
{
    public interface IPack : IHazPack, IHazContent, ISign, IHazGitRepository, INukeBuild
    {
        Target Pack => _ => _
            .TriggeredBy(Sign)
            .OnlyWhenStatic(() => NugetApiUrl.SkipEmpty())
            .OnlyWhenStatic(() => NugetApiKey.SkipEmpty())
            .OnlyWhenStatic(() => IsServerBuild)
            .OnlyWhenDynamic(() => GitRepository.IsOnMainOrMasterBranch())
            .Executes(() =>
            {
                PathConstruction.GlobFiles(ContentDirectory, "**/*.nupkg")
                   .NotEmpty()
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
