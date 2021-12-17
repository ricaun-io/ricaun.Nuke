using System;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.ValueInjection;
using ricaun.Nuke.Extensions;
namespace ricaun.Nuke.Components
{
    public interface IHazRelease : IHazSolution, INukeBuild
    {
        /// <summary>
        /// Folder Release 
        /// </summary>
        [Parameter]
        string Folder => ValueInjectionUtility.TryGetValue(() => Folder) ?? "Release";

        AbsolutePath ReleaseDirectory => GetReleaseDirectory(Solution.GetMainProject());

        public AbsolutePath GetReleaseDirectory(Project project) => project.Directory / "bin" / Folder;
    }
}
