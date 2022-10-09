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

                        var exampleDirectory = GetExampleDirectory(project);
                        var fileName = project.Name;
                        var version = project.GetInformationalVersion();

                        if (ReleaseExample)
                        {
                            var releaseFileName = CreateReleaseFromDirectory(exampleDirectory, fileName, version);
                            Serilog.Log.Information($"Release: {releaseFileName}");
                            //var zipFile = ReleaseDirectory / $"{fileName}.zip";
                            //ZipExtension.CreateFromDirectory(folder, zipFile);
                        }
                    });
                }
            });
    }
}
