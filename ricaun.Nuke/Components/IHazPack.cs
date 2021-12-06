using Nuke.Common;

namespace ricaun.Nuke.Components
{
    public interface IHazPack : IHazGitRepository, INukeBuild
    {
        /// <summary>
        /// NugetApiUrl
        /// </summary>
        public string NugetApiUrl => EnvironmentInfo.GetVariable<string>("NugetApiUrl");
        /// <summary>
        /// NugetApiKey
        /// </summary>
        public string NugetApiKey => EnvironmentInfo.GetVariable<string>("NugetApiKey");
    }
}
