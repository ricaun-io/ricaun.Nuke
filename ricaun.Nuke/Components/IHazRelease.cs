using System;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.ValueInjection;
using ricaun.Nuke.Extensions;
namespace ricaun.Nuke.Components
{
    public interface IHazRelease : IHazMainProject, IHazSolution, INukeBuild
    {
        /// <summary>
        /// Folder Release 
        /// </summary>
        [Parameter]
        string Folder => ValueInjectionUtility.TryGetValue(() => Folder) ?? "Release";

        AbsolutePath ReleaseDirectory => GetReleaseDirectory(MainProject);

        public AbsolutePath GetReleaseDirectory(Project project) => project.Directory / "bin" / Folder;
    }
}
