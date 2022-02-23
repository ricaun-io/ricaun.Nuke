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
                foreach (var example in GetExampleProjects())
                {
                    Solution.BuildProject(example, (project) =>
                    {
                        project.ShowInformation();

                        SignProject(project);
                        var folder = GetExampleDirectory(project);
                        var fileName = project.Name;
                        if (ReleaseExample)
                        {
                            var zipFile = ReleaseDirectory / $"{fileName}.zip";
                            ZipExtension.CreateFromDirectory(folder, zipFile);
                        }
                    });
                }
            });
    }
}
