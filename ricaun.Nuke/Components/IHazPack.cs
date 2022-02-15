using Nuke.Common;

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
        [Secret] [Parameter] public string NugetApiUrl => TryGetValue(() => NugetApiUrl) ?? GetGitRepositoryPackageUrl();
        /// <summary>
        /// NugetApiKey
        /// </summary>
        [Secret] [Parameter] public string NugetApiKey => TryGetValue(() => NugetApiKey) ?? GitHubToken;
    }
}
