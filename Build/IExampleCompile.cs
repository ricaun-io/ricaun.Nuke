using Nuke.Common;
using ricaun.Nuke.Components;
using ricaun.Nuke.Extensions;

public interface IExampleCompile : ICompile, ISign, IRelease, IHazContent
{
    Target ExampleCompile => _ => _
            .TriggeredBy(Compile)
            .Before(Sign)
            .Executes(() =>
            {
                Solution.BuildOtherProject("Examples", (project) =>
                {
                    SignProject(project);
                    var folder = GetContentDirectory(project);
                    var fileName = project.Name;
                    var zipFile = ReleaseDirectory / $"{fileName}.zip";
                    ZipExtension.CreateFromDirectory(folder, zipFile);
                });
            });
}
