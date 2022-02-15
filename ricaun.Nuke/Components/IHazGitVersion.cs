using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.ValueInjection;
using ricaun.Nuke.Extensions;

namespace ricaun.Nuke.Components
{
    /// <summary>
    /// IHazGitVersion
    /// </summary>
    public interface IHazGitVersion : INukeBuild
    {
        /// <summary>
        /// GitVersion
        /// </summary>
        [GitVersion] GitVersion GitVersion => TryGetValue(() => GitVersion);
    }
}
