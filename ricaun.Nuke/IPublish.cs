using Nuke.Common;
using ricaun.Nuke.Components;

namespace ricaun.Nuke
{
    public interface IPublish : ICompile, IClean, ISign, IRelease, IGitRelease, IHazSolution, INukeBuild
    {
        Target Build => _ => _
            .DependsOn(Compile)
            .Executes(() =>
            {

            });
    }
}
