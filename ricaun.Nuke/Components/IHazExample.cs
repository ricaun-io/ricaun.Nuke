using System;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.ValueInjection;
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
        string Folder => ValueInjectionUtility.TryGetValue(() => Folder) ?? "Release";

        /// <summary>
        /// Example Project Name
        /// </summary>
        [Parameter]
        string Name => ValueInjectionUtility.TryGetValue(() => Name) ?? $"{Solution.Name}.Example";

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
