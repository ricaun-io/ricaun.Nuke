using System;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.ValueInjection;
using ricaun.Nuke.Extensions;
namespace ricaun.Nuke.Components
{
    /// <summary>
    /// IHazContent
    /// </summary>
    public interface IHazContent : IHazMainProject, IHazSolution, INukeBuild
    {
        /// <summary>
        /// Folder Content 
        /// </summary>
        [Parameter]
        string Folder => ValueInjectionUtility.TryGetValue(() => Folder) ?? "Release";

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
