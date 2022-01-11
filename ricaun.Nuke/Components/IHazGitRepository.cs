using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.ValueInjection;

namespace ricaun.Nuke.Components
{
    /// <summary>
    /// IHazGitRepository
    /// </summary>
    public interface IHazGitRepository : INukeBuild
    {
        /// <summary>
        /// GitHubToken
        /// </summary>
        [Secret] [Parameter] public string GitHubToken => ValueInjectionUtility.TryGetValue(() => GitHubToken);

        /// <summary>
        /// GitRepository
        /// </summary>
        [GitRepository] GitRepository GitRepository => ValueInjectionUtility.TryGetValue(() => GitRepository);

        /// <summary>
        /// GetGitRepositoryPackageUrl (default: https://nuget.pkg.github.com/../../index.json)
        /// </summary>
        /// <returns></returns>
        public string GetGitRepositoryPackageUrl()
        {
            return $@"https://nuget.pkg.github.com/{GitRepository.Identifier}/index.json";
        }
    }
}
