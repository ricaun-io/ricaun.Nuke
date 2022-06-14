using System;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
namespace ricaun.Nuke.Components
{
    /// <summary>
    /// IHazContent
    /// </summary>
    public interface IHazContent : IHazMainProject, IHazSolution, INukeBuild
    {
        /// <summary>
        /// Folder Release 
        /// </summary>
        [Parameter]
        string Folder => TryGetValue(() => Folder) ?? "Release";

        /// <summary>
        /// ContentDirectory
        /// </summary>
        AbsolutePath ContentDirectory => GetContentDirectory(MainProject);

        /// <summary>
        /// GetContentDirectory
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public AbsolutePath GetContentDirectory(Project project) => project.Directory / "bin" / Folder;
    }
}
