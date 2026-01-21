using Nuke.Common;
using Nuke.Common.ProjectModel;
using System.Collections.Generic;

namespace ricaun.Nuke.Components
{
    /// <summary>
    /// IHazExample
    /// </summary>
    public interface IHazExample : IHazRelease
    {
        /// <summary>
        /// Example Project matching a wildcard pattern (*.Example)
        /// </summary>
        [Parameter]
        string Name => TryGetValue(() => Name) ?? "*.Example";

        /// <summary>
        /// ReleaseExample (default: true)
        /// </summary>
        [Parameter]
        bool ReleaseExample => TryGetValue<bool?>(() => ReleaseExample) ?? true;

        /// <summary>
        /// GetExampleProjects
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Project> GetExampleProjects() => Solution.GetAllProjects(Name);
    }
}
