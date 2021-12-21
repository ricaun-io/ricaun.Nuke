using Nuke.Common;
using Nuke.Common.ProjectModel;
using Nuke.Common.ValueInjection;
using ricaun.Nuke.Extensions;
namespace ricaun.Nuke.Components
{
    /// <summary>
    /// IHazMainProject
    /// </summary>
    public interface IHazMainProject : IHazSolution, INukeBuild
    {
        /// <summary>
        /// Name of the MainProject (default: <seealso cref="Solution.Name"/>)
        /// </summary>
        [Parameter]
        string MainName => ValueInjectionUtility.TryGetValue(() => MainName) ?? Solution.Name;

        /// <summary>
        /// MainProject (default: <seealso cref="MainName"/>)
        /// </summary>
        public Project MainProject => Solution.GetOtherProject(MainName);

        /// <summary>
        /// MainProject (default: <seealso cref="MainName"/>)
        /// </summary>
        /// <returns></returns>
        public Project GetMainProject() => MainProject;
    }

    /// <summary>
    /// HazMainProjectExtension
    /// </summary>
    public static class HazMainProjectExtension
    {
        /// <summary>
        /// Get MainProject
        /// </summary>
        /// <param name="hazMainProject"></param>
        /// <returns></returns>
        public static Project GetMainProject(this IHazMainProject hazMainProject) => hazMainProject.MainProject;

        /// <summary>
        /// Get Project
        /// </summary>
        /// <param name="hazMainProject"></param>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public static Project GetProject(this IHazMainProject hazMainProject, string projectName) => hazMainProject.GetSolution().GetOtherProject(projectName);
    }
}