
using System;

using Nuke.Common;
using static Nuke.Common.ChangeLog.ChangelogTasks;

namespace ricaun.Nuke.Components
{
    public interface IHazChangelog : INukeBuild
    {
        // TODO: assert file exists
        string ChangelogFile => RootDirectory / ".." / "CHANGELOG.md";
        string NuGetReleaseNotes => GetNuGetReleaseNotes(ChangelogFile, (this as IHazGitRepository)?.GitRepository);
    }
}
