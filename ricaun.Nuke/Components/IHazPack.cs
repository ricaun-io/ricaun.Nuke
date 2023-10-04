using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tools.DotNet;

namespace ricaun.Nuke.Components
{
    /// <summary>
    /// IHazPack
    /// </summary>
    public interface IHazPack : IHazGitRepository, INukeBuild
    {
        /// <summary>
        /// NugetApiUrl
        /// </summary>
        [Secret][Parameter] public string NugetApiUrl => TryGetValue(() => NugetApiUrl) ?? GetGitRepositoryPackageUrl();
        /// <summary>
        /// NugetApiKey
        /// </summary>
        [Secret][Parameter] public string NugetApiKey => TryGetValue(() => NugetApiKey) ?? GitHubToken;

        /// <summary>
        /// DotNetNuGetPush
        /// </summary>
        /// <param name="absolutePath"></param>
        void DotNetNuGetPush(AbsolutePath absolutePath)
        {
            DotNetTasks.DotNetNuGetPush(s => s
                .SetTargetPath(absolutePath)
                .SetSource(NugetApiUrl)
                .SetApiKey(NugetApiKey)
                .EnableSkipDuplicate()
            );
        }

        /// <summary>
        /// IsPrePackFile
        /// </summary>
        /// <param name="absolutePath"></param>
        /// <returns></returns>
        bool IsPrePackFile(AbsolutePath absolutePath)
        {
            return absolutePath.Name.Contains("-");
        }
    }
}
