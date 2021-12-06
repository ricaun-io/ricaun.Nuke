using System;
using System.IO.Compression;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using ricaun.Nuke.Extensions;

namespace ricaun.Nuke.Components
{
    public interface IRelease : IHazRelease, IHazContent, ISign, INukeBuild
    {
        Target Release => _ => _
            .TriggeredBy(Sign)
            .Executes(() =>
            {
                ReleaseProject(Solution.GetMainProject());
            });

        /// <summary>
        /// Release Project on ContentDirectory>
        /// </summary>
        /// <param name="project"></param>
        public void ReleaseProject(Project project)
        {
            if (!FileSystemTasks.DirectoryExists(ContentDirectory))
            {
                Logger.Warn($"Skip Not found: {ContentDirectory}");
                return;
            }

            var version = project.GetInformationalVersion();
            Logger.Success($"Release Version: {project.GetInformationalVersion()}");

            var fileName = $"{project.Name} {version}";
            var ProjectDirectory = ReleaseDirectory / fileName;

            FileSystemTasks.CopyDirectoryRecursively(ContentDirectory, ProjectDirectory);

            var zipFile = ReleaseDirectory / $"{fileName}.zip";
            ZipFile.CreateFromDirectory(ProjectDirectory, zipFile, CompressionLevel.Optimal, true);
        }
    }
}
