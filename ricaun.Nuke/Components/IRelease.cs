using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Utilities.Collections;
using ricaun.Nuke.Extensions;
using System;
using System.IO.Compression;

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
            if (!ContentDirectory.DirectoryExists())
            {
                Serilog.Log.Warning($"Skip Not found: {ContentDirectory}");
                return;
            }

            SignProject(project);

            var fileName = project.Name;
            var version = project.GetInformationalVersion();
            Serilog.Log.Information($"Release Version: {project.GetInformationalVersion()}");

            var fileNameVersion = GetReleaseFileNameVersion(project.Name, version);
            var ProjectDirectory = ReleaseDirectory / fileNameVersion;

            var nupkgs = Globbing.GlobFiles(ContentDirectory, "**/*.nupkg");
            if (nupkgs.Count > 0)
            {
                nupkgs.ForEach(file => AbsolutePathExtensions.CopyToDirectory(file, ProjectDirectory));
            }
            else
            {
                AbsolutePathExtensions.CopyToDirectory(ContentDirectory, ProjectDirectory);
            }

            var releaseFileName = CreateReleaseFromDirectory(ProjectDirectory, fileName, version);
            Serilog.Log.Information($"Release: {releaseFileName}");

            //var zipFile = ReleaseDirectory / $"{fileName}.zip";
            //ZipExtension.CreateFromDirectory(ProjectDirectory, zipFile);
        }
    }
}
