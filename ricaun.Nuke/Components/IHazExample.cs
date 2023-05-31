using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using ricaun.Nuke.Extensions;
using System;
using System.Collections.Generic;

namespace ricaun.Nuke.Components
{
    /// <summary>
    /// IHazExample
    /// </summary>
    public interface IHazExample : IHazSolution, INukeBuild
    {
        /// <summary>
        /// Folder Release 
        /// </summary>
        [Parameter]
        string Folder => TryGetValue(() => Folder) ?? "Release";

        /// <summary>
        /// Example Project Name EndWith
        /// </summary>
        [Parameter]
        string Name => TryGetValue(() => Name) ?? "Example";

        /// <summary>
        /// ReleaseExample (default: true)
        /// </summary>
        [Parameter]
        bool ReleaseExample => TryGetValue<bool?>(() => ReleaseExample) ?? true;

        /// <summary>
        /// GetExampleProjects
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Project> GetExampleProjects() => Solution.GetOtherProjects(Name);

        /// <summary>
        /// GetExampleDirectory
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public AbsolutePath GetExampleDirectory(Project project) => project.Directory / "bin" / Folder;
    }
}
