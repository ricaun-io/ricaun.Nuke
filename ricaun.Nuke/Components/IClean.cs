using Nuke.Common;
using ricaun.Nuke.Extensions;

namespace ricaun.Nuke.Components
{
    public interface IClean : IHazSolution, INukeBuild
    {
        Target Clean => _ => _
            .Executes(() =>
            {
                Solution.ClearSolution(BuildProjectDirectory);
            });
    }
}
