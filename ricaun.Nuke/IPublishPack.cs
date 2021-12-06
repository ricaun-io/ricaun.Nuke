using Nuke.Common;
using ricaun.Nuke.Components;

namespace ricaun.Nuke
{
    public interface IPublishPack : ICompile, IClean, ISign, IRelease, IPack, IGitRelease, IHazSolution, INukeBuild
    {
        Target Build => _ => _
            .DependsOn(Compile)
            .Executes(() =>
            {

            });
    }
}
