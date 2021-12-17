using Nuke.Common;
using Nuke.Common.ProjectModel;
using Nuke.Common.ValueInjection;
using ricaun.Nuke.Extensions;
namespace ricaun.Nuke.Components
{
    public interface IHazMainProject : IHazSolution, INukeBuild
    {
        /// <summary>
        /// Name of the MainProject (default: <seealso cref="Solution.Name"/>)
        /// </summary>
        [Parameter]
        string Name => ValueInjectionUtility.TryGetValue(() => Name) ?? Solution.Name;

        /// <summary>
        /// MainProject (default: <seealso cref="Solution.Name"/>)
        /// </summary>
        [Parameter]
        public Project MainProject => ValueInjectionUtility.TryGetValue(() => MainProject) ?? Solution.GetOtherProject(Name);

        /// <summary>
        /// MainProject (default: <seealso cref="Solution.Name"/>)
        /// </summary>
        /// <returns></returns>
        public Project GetMainProject() => MainProject;
    }

    public static class HazMainProjectExtension
    {
        /// <summary>
        /// Get MainProject
        /// </summary>
        /// <param name="hazMainProject"></param>
        /// <returns></returns>
        public static Project GetMainProject(this IHazMainProject hazMainProject) => hazMainProject.MainProject;
    }
}