using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.ValueInjection;
using ricaun.Nuke.Extensions;

namespace ricaun.Nuke.Components
{
    public interface IHazGitRepository : INukeBuild
    {
        public string GitHubToken => EnvironmentInfo.GetVariable<string>("GitHubToken");
        [GitRepository] GitRepository GitRepository => ValueInjectionUtility.TryGetValue(() => GitRepository);
    }
}
