using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using ricaun.Nuke.Extensions;

namespace ricaun.Nuke.Components
{
    /// <summary>
    /// IHazRelease
    /// </summary>
    public interface IHazRelease : IHazMainProject, IHazSolution, INukeBuild
    {
        /// <summary>
        /// Folder Release 
        /// </summary>
        [Parameter]
        string Folder => TryGetValue(() => Folder) ?? "ReleaseFiles";

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
        public AbsolutePath GetReleaseDirectory(Project project) => project.Directory / "bin" / Folder;

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
    }
}
