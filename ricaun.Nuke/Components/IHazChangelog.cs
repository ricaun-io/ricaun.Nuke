
using Nuke.Common;
using Nuke.Common.ChangeLog;

namespace ricaun.Nuke.Components
{
    /// <summary>
    /// IHazChangelog
    /// </summary>
    public interface IHazChangelog : INukeBuild
    {
        // TODO: assert file exists
        string ChangelogFile => RootDirectory / ".." / "CHANGELOG.md";
        string NuGetReleaseNotes => ChangelogTasks.GetNuGetReleaseNotes(ChangelogFile, (this as IHazGitRepository)?.GitRepository);
    }
}
