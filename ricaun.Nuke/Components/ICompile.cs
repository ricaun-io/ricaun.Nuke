using Nuke.Common;
using Nuke.Common.Utilities.Collections;
using ricaun.Nuke.Extensions;

namespace ricaun.Nuke.Components
{
    /// <summary>
    /// ICompile
    /// </summary>
    public interface ICompile : IClean, IHazMainProject, IHazSolution, INukeBuild
    {
        /// <summary>
        /// Target Compile
        /// </summary>
        Target Compile => _ => _
            .DependsOn(Clean)
            .Executes(() =>
            {
                ReportSummary(_ => _.AddPair("Project", MainProject.Name));
                Solution.BuildProject(MainProject, (project) =>
                    {
                        project.ShowInformation();
                    }
                );
            });
    }
}
