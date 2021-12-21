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
        /// IHazGitRepository
        /// </summary>
        public string GitHubToken => EnvironmentInfo.GetVariable<string>("GitHubToken");

        /// <summary>
        /// GitRepository
        /// </summary>
        [GitRepository] GitRepository GitRepository => ValueInjectionUtility.TryGetValue(() => GitRepository);
    }
}
