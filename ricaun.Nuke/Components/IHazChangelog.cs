
using Nuke.Common;
using Nuke.Common.ChangeLog;
using System.IO;

namespace ricaun.Nuke.Components
{
    /// <summary>
    /// IHazChangelog
    /// </summary>
    public interface IHazChangelog : INukeBuild
    {
        /// <summary>
        /// ChangelogFile
        /// </summary>
        string ChangelogFile => "CHANGELOG.md";

        /// <summary>
        /// NuGetReleaseNotes
        /// </summary>
        string NuGetReleaseNotes => ChangelogTasks.GetNuGetReleaseNotes(GetChangelogFile(), (this as IHazGitRepository)?.GitRepository);

        /// <summary>
        /// GetNuGetReleaseNotes
        /// </summary>
        /// <returns></returns>
        string GetNuGetReleaseNotes()
        {
            return GetChangelogFile() != null ? NuGetReleaseNotes : "CHANGELOG.md not found";
        }

        /// <summary>
        /// Find CHANGELOG file
        /// </summary>
        /// <returns></returns>
        string GetChangelogFile()
        {
            var parent = RootDirectory;
            var file = parent / ChangelogFile;
            var parentMax = 5;
            for (int i = 0; i < parentMax; i++)
            {
                if (File.Exists(file))
                    return file;
                parent = parent.Parent;
                file = parent / ChangelogFile;
            }
            return null;
        }
    }
}
