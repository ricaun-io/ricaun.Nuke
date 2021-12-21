using Nuke.Common;
using ricaun.Nuke.Components;

namespace ricaun.Nuke
{
    /// <summary>
    /// IPublishPack
    /// </summary>
    public interface IPublishPack : ICompile, IClean, ISign, IRelease, IPack, IGitRelease, IHazSolution, INukeBuild
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
