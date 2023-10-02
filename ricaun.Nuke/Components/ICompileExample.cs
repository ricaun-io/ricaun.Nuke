using Nuke.Common;

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
                BuildProjectsAndRelease(GetExampleProjects(), ReleaseExample, ReleaseExample);
            });
    }
}
