using Nuke.Common;
using ricaun.Nuke.Extensions;

namespace ricaun.Nuke.Components
{
    public interface ICompileExample : IHazExample, ICompile, ISign, IRelease, IHazContent, INukeBuild
    {
        Target CompileExample => _ => _
            .TriggeredBy(Compile)
            .Before(Sign)
            .Executes(() =>
            {
                Solution.BuildOtherProject(GetExampleProject(), (project) =>
                {
                    SignProject(project);
                    var folder = ExampleDirectory;
                    var fileName = project.Name;
                    var zipFile = ReleaseDirectory / $"{fileName}.zip";
                    ZipExtension.CreateFromDirectory(folder, zipFile);
                });
            });
    }
}
