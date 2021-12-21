using Nuke.Common;
using ricaun.Nuke.Extensions;

namespace ricaun.Nuke.Components
{
    /// <summary>
    /// ICompile
    /// </summary>
    public interface ICompile : IClean, IHazMainProject, IHazSolution, INukeBuild
    {
        Target Compile => _ => _
            .DependsOn(Clean)
            .Executes(() =>
            {
                Solution.BuildProject(MainProject);
            });
    }
}
