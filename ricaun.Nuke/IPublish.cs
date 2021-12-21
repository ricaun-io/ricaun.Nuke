using Nuke.Common;
using ricaun.Nuke.Components;

namespace ricaun.Nuke
{
    /// <summary>
    /// IPublish
    /// </summary>
    public interface IPublish : ICompile, IClean, ISign, IRelease, IGitRelease, IHazSolution, INukeBuild
    {
        /// <summary>
        /// Target Build
        /// </summary>
        Target Build => _ => _
            .DependsOn(Compile)
            .Executes(() =>
            {

            });
    }
}
