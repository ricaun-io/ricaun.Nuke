using Nuke.Common;
using Nuke.Common.ValueInjection;

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
        [Secret] [Parameter] public string NugetApiUrl => ValueInjectionUtility.TryGetValue(() => NugetApiUrl) ?? GetGitRepositoryPackageUrl();
        /// <summary>
        /// NugetApiKey
        /// </summary>
        [Secret] [Parameter] public string NugetApiKey => ValueInjectionUtility.TryGetValue(() => NugetApiKey) ?? GitHubToken;
    }
}
