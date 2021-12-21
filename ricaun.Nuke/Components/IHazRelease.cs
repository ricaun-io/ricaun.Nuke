﻿using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.ValueInjection;
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
        string Folder => ValueInjectionUtility.TryGetValue(() => Folder) ?? "ReleaseFiles";

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
    }
}
