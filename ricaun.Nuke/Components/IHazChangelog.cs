
using Nuke.Common;
using Nuke.Common.ChangeLog;

namespace ricaun.Nuke.Components
{
    /// <summary>
    /// IHazChangelog
    /// TODO: assert file exists
    /// </summary>
    public interface IHazChangelog : INukeBuild
    {
        /// <summary>
        /// ChangelogFile
        /// </summary>
        string ChangelogFile => RootDirectory / ".." / "CHANGELOG.md";

        /// <summary>
        /// NuGetReleaseNotes
        /// </summary>
        string NuGetReleaseNotes => ChangelogTasks.GetNuGetReleaseNotes(ChangelogFile, (this as IHazGitRepository)?.GitRepository);
    }
}
