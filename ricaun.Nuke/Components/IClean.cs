using Nuke.Common;
using ricaun.Nuke.Extensions;

namespace ricaun.Nuke.Components
{
    /// <summary>
    /// IClean
    /// </summary>
    public interface IClean : IHazSolution, INukeBuild
    {
        /// <summary>
        /// Target Clean
        /// </summary>
        Target Clean => _ => _
            .Executes(() =>
            {
                Solution.ClearSolution(BuildProjectDirectory);
            });
    }
}
