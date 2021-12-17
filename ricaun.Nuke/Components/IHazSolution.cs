using Nuke.Common;
using Nuke.Common.ProjectModel;
using Nuke.Common.ValueInjection;

namespace ricaun.Nuke.Components
{
    public interface IHazSolution : INukeBuild
    {
        [Required] [Solution] Solution Solution => ValueInjectionUtility.TryGetValue(() => Solution);
    }

    public static class HazSolutionExtension
    {
        /// <summary>
        /// Get Solution
        /// </summary>
        /// <param name="hazSolution"></param>
        /// <returns></returns>
        public static Solution GetMainProject(this IHazSolution hazSolution) => hazSolution.Solution;
    }
}
