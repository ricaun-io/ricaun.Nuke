using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Utilities.Collections;
using ricaun.Nuke.Extensions;
using System;
using System.Collections.Generic;

namespace ricaun.Nuke.Components
{
    /// <summary>
    /// IHazExample
    /// </summary>
    public interface IHazExample : IHazSolution, INukeBuild, IHazRelease, IHazSign
    {
        /// <summary>
        /// Folder Release 
        /// </summary>
        [Parameter]
        string Folder => TryGetValue(() => Folder) ?? "Release";

        /// <summary>
        /// Example Project matching a wildcard pattern (*.Example)
        /// </summary>
        [Parameter]
        string Name => TryGetValue(() => Name) ?? "*.Example";

        /// <summary>
        /// ReleaseExample (default: true)
        /// </summary>
        [Parameter]
        bool ReleaseExample => TryGetValue<bool?>(() => ReleaseExample) ?? true;

        /// <summary>
        /// GetExampleProjects
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Project> GetExampleProjects() => Solution.GetAllProjects(Name);

        /// <summary>
        /// GetExampleDirectory
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public AbsolutePath GetExampleDirectory(Project project) => project.Directory / "bin" / Folder;

        /// <summary>
        /// Build Project and Release
        /// </summary>
        /// <param name="projects"></param>
        /// <param name="releaseProjectFiles"></param>
        /// <param name="releasePackages"></param>
        public void BuildProjectsAndRelease(IEnumerable<Project> projects, bool releaseProjectFiles = true, bool releasePackages = true)
        {
            foreach (var project in projects)
            {
                Solution.BuildProject(project, (project) =>
                {
                    project.ShowInformation();

                    SignProject(project);

                    var exampleDirectory = GetExampleDirectory(project);
                    var fileName = project.Name;
                    var version = project.GetInformationalVersion();

                    if (releaseProjectFiles)
                    {
                        var releaseFileName = CreateReleaseFromDirectory(exampleDirectory, fileName, version);
                        Serilog.Log.Information($"Release: {releaseFileName}");
                    }

                    if (releasePackages)
                    {
                        Globbing.GlobFiles(exampleDirectory, "**/*.nupkg")
                            .ForEach(file =>
                            {
                                Serilog.Log.Information($"Copy nupkg: {file} to {ReleaseDirectory}");
                                FileSystemTasks.CopyFileToDirectory(file, ReleaseDirectory, FileExistsPolicy.OverwriteIfNewer);
                            });
                    }
                });
            }
        }
    }
}
