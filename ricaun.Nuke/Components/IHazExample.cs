using System;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using ricaun.Nuke.Extensions;
namespace ricaun.Nuke.Components
{
    /// <summary>
    /// IHazExample
    /// </summary>
    public interface IHazExample : IHazSolution, INukeBuild
    {
        /// <summary>
        /// Folder Example 
        /// </summary>
        [Parameter]
        string Folder => TryGetValue(() => Folder) ?? "Release";

        /// <summary>
        /// Example Project Name
        /// </summary>
        [Parameter]
        string Name => TryGetValue(() => Name) ?? $"{Solution.Name}.Example";

        /// <summary>
        /// ReleaseExample (default: true)
        /// </summary>
        [Parameter]
        bool ReleaseExample => TryGetValue<bool?>(() => ReleaseExample) ?? true;

        /// <summary>
        /// ExampleDirectory
        /// </summary>
        AbsolutePath ExampleDirectory => GetExampleDirectory(GetExampleProject());

        /// <summary>
        /// GetExampleProject
        /// </summary>
        /// <returns></returns>
        public Project GetExampleProject() => Solution.GetOtherProject(Name);

        /// <summary>
        /// GetExampleDirectory
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public AbsolutePath GetExampleDirectory(Project project) => project.Directory / "bin" / Folder;
    }
}
