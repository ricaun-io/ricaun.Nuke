using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Git;
using Nuke.Common.Tools.GitHub;
using ricaun.Nuke.Extensions;

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
        [Secret][Parameter] public string GitHubToken => TryGetValue(() => GitHubToken);

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
                Serilog.Log.Warning($"GitRepository not found, PackageUrl not found.");
                return string.Empty;
            }
            return $@"https://nuget.pkg.github.com/{GetGitRepositoryOwner()}/index.json";
        }

        /// <summary>
        /// Gets the package user name from the current GitHub Actions actor or the Git repository owner.
        /// </summary>
        /// <remarks>Prefers the GitHub Actions actor and falls back to the repository owner. Logs a
        /// warning when the Git repository is not available.</remarks>
        /// <returns>The GitHub Actions actor when available; otherwise, the Git repository owner. Returns an empty string when
        /// no Git repository is available.</returns>
        public string GetGitRepositoryPackageUser()
        {
            if (GitRepository == null)
            {
                Serilog.Log.Warning($"GitRepository not found, PackageUser not found.");
                return string.Empty;
            }
            var userName = GitHubActions.Instance?.Actor ?? (GitRepository.IsGitHubRepository() ? GitRepository.GetGitHubOwner() : string.Empty);
            return userName;
        }

        /// <summary>
        /// Gets the package key to use for publishing based on the NuGet source and repository context.
        /// </summary>
        /// <remarks>Returns an empty string when the NuGet API URL is missing or when no Git repository
        /// is available for non-NuGet.org sources.</remarks>
        /// <param name="nuGetApiUrl">NuGet API source URL used to determine how the package key is resolved.</param>
        /// <param name="nuGetApiUser">NuGet API user name used to derive a NuGet.org token identifier.</param>
        /// <returns>A NuGet.org token identifier when the source is NuGet.org and a user is provided; otherwise the GitHub token
        /// when a repository is available; otherwise an empty string.</returns>
        public string GetGitRepositoryPackageKey(string nuGetApiUrl, string nuGetApiUser)
        {
            if (string.IsNullOrEmpty(nuGetApiUrl))
            {
                Serilog.Log.Warning($"NuGetApiUrl not found, PackageKey not found.");
                return string.Empty;
            }
            if (GitRepository == null)
            {
                Serilog.Log.Warning($"GitRepository not found, PackageKey not found.");
                return string.Empty;
            }
            if (NuGetExtension.IsSourceNuGetOrg(nuGetApiUrl))
            {
                try
                {
                    return NugetActionTokenIdUtils.GetNugetTokenId(nuGetApiUser);
                }
                catch (System.Exception ex)
                {
                    Serilog.Log.Error($"NugetTokenId failed. Exception: {ex.Message}");
                    throw;
                }
            }
            return GitHubToken;
        }

        /// <summary>
        /// GetGitRepositoryOwner based on the GitRepository.Identifier
        /// </summary>
        /// <returns></returns>
        public string GetGitRepositoryOwner()
        {
            if (GitRepository == null) return "";
            return GitRepository.Identifier?.Split("/")[0];
        }

        /// <summary>
        /// Indicates whether the forked repository is enabled.
        /// </summary>
        [Parameter]
        bool EnableForkedRepository => TryGetValue<bool?>(() => EnableForkedRepository) ?? false;

        /// <summary>
        /// Determines if the forked repository should be skipped.
        /// </summary>
        /// <returns>False if the forked repository should be skipped; otherwise, true.</returns>
        public bool SkipForked()
        {
            if (EnableForkedRepository)
                return true;

            if (IsGitRepositoryForked()) 
                return false;
            
            return true;
        }
        /// <summary>
        /// IsGitRepositoryForked
        /// </summary>
        /// <returns></returns>
        public bool IsGitRepositoryForked()
        {
            return GitRepository.IsForked();
        }
    }
}
