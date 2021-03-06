using System;
using System.IO.Compression;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Utilities.Collections;
using ricaun.Nuke.Extensions;

namespace ricaun.Nuke.Components
{
    /// <summary>
    /// IRelease
    /// </summary>
    public interface IRelease : IHazRelease, IHazContent, ISign, INukeBuild
    {
        /// <summary>
        /// Target Release
        /// </summary>
        Target Release => _ => _
            .TriggeredBy(Sign)
            .Executes(() =>
            {
                ReleaseProject(MainProject);
            });

        /// <summary>
        /// Release Project on ContentDirectory>
        /// </summary>
        /// <param name="project"></param>
        public void ReleaseProject(Project project)
        {
            if (!FileSystemTasks.DirectoryExists(ContentDirectory))
            {
                Serilog.Log.Warning($"Skip Not found: {ContentDirectory}");
                return;
            }

            SignProject(project);

            var version = project.GetInformationalVersion();
            Serilog.Log.Information($"Release Version: {project.GetInformationalVersion()}");

            var fileName = $"{project.Name} {version}";
            var ProjectDirectory = ReleaseDirectory / fileName;

            var nupkgs = PathConstruction.GlobFiles(ContentDirectory, "**/*.nupkg");
            if (nupkgs.Count > 0)
            {
                nupkgs.ForEach(file => FileSystemTasks.CopyFileToDirectory(file, ProjectDirectory));
            }
            else
            {
                FileSystemTasks.CopyDirectoryRecursively(ContentDirectory, ProjectDirectory);
            }

            var zipFile = ReleaseDirectory / $"{fileName}.zip";
            ZipExtension.CreateFromDirectory(ProjectDirectory, zipFile);
        }
    }
}
