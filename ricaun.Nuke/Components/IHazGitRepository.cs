using Nuke.Common;
using Nuke.Common.Git;

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
        [Secret] [Parameter] public string GitHubToken => TryGetValue(() => GitHubToken);

        /// <summary>
        /// GitRepository
        /// </summary>
        [GitRepository] GitRepository GitRepository => TryGetValue(() => GitRepository);

        /// <summary>
        /// GetGitRepositoryPackageUrl (default: https://nuget.pkg.github.com/repository_owner/index.json)
        /// </summary>
        /// <returns></returns>
        public string GetGitRepositoryPackageUrl()
        {
            if (GitRepository == null)
            {
                Serilog.Log.Warning($"GitRepository not found.");
                return "";
            }
            return $@"https://nuget.pkg.github.com/{GetGitRepositoryOwner()}/index.json";
        }

        /// <summary>
        /// GetGitRepositoryOwner based on the GitRepository.Identifier
        /// </summary>
        /// <returns></returns>
        public string GetGitRepositoryOwner()
        {
            if (GitRepository == null) return "";
            return GitRepository.Identifier.Split("/")[0];
        }
    }
}
