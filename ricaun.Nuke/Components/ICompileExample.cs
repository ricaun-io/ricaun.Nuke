using Nuke.Common;
using ricaun.Nuke.Extensions;

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
                Solution.BuildProject(GetExampleProject(), (project) =>
                {
                    SignProject(project);
                    var folder = ExampleDirectory;
                    var fileName = project.Name;
                    if (ReleaseExample)
                    {
                        var zipFile = ReleaseDirectory / $"{fileName}.zip";
                        ZipExtension.CreateFromDirectory(folder, zipFile);
                    }
                });
            });
    }
}
