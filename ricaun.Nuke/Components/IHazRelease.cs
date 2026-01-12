using JetBrains.Annotations;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Utilities.Collections;
using ricaun.Nuke.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace ricaun.Nuke.Components
{
    /// <summary>
    /// IHazRelease
    /// </summary>
    public interface IHazRelease : IHazMainProject, IHazSolution, IHazSign, INukeBuild
    {
        /// <summary>
        /// Folder Release 
        /// </summary>
        [Parameter]
        string ReleaseFolder => TryGetValue(() => ReleaseFolder) ?? "ReleaseFiles";

        /// <summary>
        /// Add Version in the Release Zip File (default: false)
        /// </summary>
        [Parameter]
        bool ReleaseNameVersion => TryGetValue<bool?>(() => ReleaseNameVersion) ?? false;

        /// <summary>
        /// ReleaseDirectory
        /// </summary>
        AbsolutePath ReleaseDirectory => GetReleaseDirectory(MainProject);

        /// <summary>
        /// GetReleaseDirectory
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public AbsolutePath GetReleaseDirectory(Project project) => project.Directory / "bin" / ReleaseFolder;

        /// <summary>
        /// GetReleaseFileNameVersion
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public string GetReleaseFileNameVersion(string fileName, string version = null)
        {
            if (ReleaseNameVersion)
            {
                if (!string.IsNullOrEmpty(version))
                    fileName += $" {version}";
            }
            return fileName;
        }

        /// <summary>
        /// Create Zip File in the <see cref="ReleaseDirectory"/> from <paramref name="sourceDirectory"/>
        /// </summary>
        /// <param name="sourceDirectory"></param>
        /// <param name="fileName"></param>
        /// <param name="version"></param>
        /// <param name="fileExtension"></param>
        /// <param name="includeBaseDirectory"></param>
        /// <returns>FileName with Version <see cref="ReleaseNameVersion"/></returns>
        public string CreateReleaseFromDirectory(
            string sourceDirectory, string fileName,
            string version = null,
            string fileExtension = ".zip", bool includeBaseDirectory = false)
        {
            fileName = GetReleaseFileNameVersion(fileName, version);
            fileName = $"{fileName}{fileExtension}";

            var zipFile = ReleaseDirectory / fileName;
            ZipExtension.CreateFromDirectory(sourceDirectory, zipFile, includeBaseDirectory);

            return fileName;
        }

        /// <summary>
        /// BuildProjectsAndRelease
        /// </summary>
        /// <param name="projects"></param>
        /// <param name="releaseProjectFiles"></param>
        /// <param name="releasePackages"></param>
        /// <param name="signProjects"></param>
        public void BuildProjectsAndRelease(IEnumerable<Project> projects,
            bool releaseProjectFiles = true,
            bool releasePackages = true,
            bool signProjects = true)
        {
            foreach (var project in projects)
            {
                Solution.BuildProject(project, (project) =>
                {
                    project.ShowInformation();

                    if (signProjects) SignProject(project);

                    var releaseDirectory = project.Directory / "bin" / "Release";
                    var fileName = project.Name;
                    var version = project.GetInformationalVersion();

                    if (releaseProjectFiles)
                    {
                        var releaseFileName = CreateReleaseFromDirectory(releaseDirectory, fileName, version);
                        Serilog.Log.Information($"Release: {releaseFileName}");
                    }

                    if (releasePackages)
                    {
                        Globbing.GlobFiles(releaseDirectory, "**/*.*nupkg")
                            .ForEach(file =>
                            {
                                Serilog.Log.Information($"Copy nupkg: {file} to {ReleaseDirectory}");
                                AbsolutePathExtensions.CopyToDirectory(file, ReleaseDirectory, ExistsPolicy.MergeAndOverwriteIfNewer);
                            });
                    }
                });
            }
        }

        /// <summary>
        /// ReportSummaryProjectNames
        /// </summary>
        /// <param name="projects"></param>
        public void ReportSummaryProjectNames(IEnumerable<Project> projects)
        {
            var names = string.Join(" | ", projects.Select(e => e.Name).ToArray());
            ReportSummary(_ => _
                .AddPair("Projects", names)
            );
        }
    }
}
