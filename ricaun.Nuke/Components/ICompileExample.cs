using Nuke.Common;
using Nuke.Common.Utilities.Collections;
using System.Linq;

namespace ricaun.Nuke.Components
{
    /// <summary>
    /// ICompileExample
    /// </summary>
    public interface ICompileExample : IHazExample, ICompile, ISign, IRelease, IHazContent, INukeBuild
    {
        /// <summary>
        /// Target CompileExample
        /// </summary>
        Target CompileExample => _ => _
            .TriggeredBy(Compile)
            .Before(Sign)
            .Executes(() =>
            {
                ReportSummaryProjectNames(GetExampleProjects());
                BuildProjectsAndRelease(GetExampleProjects(), ReleaseExample, ReleaseExample);
            });
    }
}
