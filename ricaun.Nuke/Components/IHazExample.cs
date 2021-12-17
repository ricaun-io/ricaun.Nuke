using System;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.ValueInjection;
using ricaun.Nuke.Extensions;
namespace ricaun.Nuke.Components
{
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

        AbsolutePath ExampleDirectory => GetExampleDirectory(GetExampleProject());
        public Project GetExampleProject() => Solution.GetOtherProject(Name);
        public AbsolutePath GetExampleDirectory(Project project) => project.Directory / "bin" / Folder;
    }
}
