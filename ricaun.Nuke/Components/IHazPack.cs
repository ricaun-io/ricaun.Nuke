using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tools.DotNet;
using ricaun.Nuke.Extensions;

namespace ricaun.Nuke.Components
{
    /// <summary>
    /// IHazPack
    /// </summary>
    public interface IHazPack : IHazGitRepository, INukeBuild
    {
        /// <summary>
        /// NuGetApiUrl
        /// </summary>
        [Secret][Parameter] public string NuGetApiUrl => TryGetValue(() => NuGetApiUrl) ?? GetGitRepositoryPackageUrl();
        /// <summary>
        /// NuGetApiKey
        /// </summary>
        [Secret][Parameter] public string NuGetApiKey => TryGetValue(() => NuGetApiKey) ?? GitHubToken;

        /// <summary>
        /// DotNetNuGetPush
        /// </summary>
        /// <param name="packageFilePath"></param>
        void DotNetNuGetPush(AbsolutePath packageFilePath)
        {
            NuGetExtension.DotNetNuGetPush(NuGetApiUrl, NuGetApiKey, packageFilePath);
        }

        /// <summary>
        /// DotNetNuGetPrerelease
        /// </summary>
        /// <param name="packageFilePath"></param>
        void DotNetNuGetPrerelease(AbsolutePath packageFilePath)
        {
            var packageNameVersion = packageFilePath.Name;
            NuGetExtension.DotNetNuGetPush(NuGetApiUrl, NuGetApiKey, packageFilePath);
            NuGetExtension.NuGetUnlist(NuGetApiUrl, NuGetApiKey, packageNameVersion);
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
