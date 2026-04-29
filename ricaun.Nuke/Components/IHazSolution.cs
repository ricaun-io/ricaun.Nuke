using Nuke.Common;
using Nuke.Common.ProjectModel;

namespace ricaun.Nuke.Components
{
    /// <summary>
    /// IHazSolution
    /// </summary>
#if NET8_0
    [System.Obsolete("net8.0 is gonna be deprecated, update your project to net10.0.")]
#endif
    public interface IHazSolution : INukeBuild
    {
        /// <summary>
        /// Solution
        /// </summary>
        [Required]
#if NET8_0
        [Solution(SuppressBuildProjectCheck = true)]
        Solution Solution => TryGetValue(() => Solution);
#else
        [SolutionX()]
        Solution Solution => TryGetValue(() => Solution);
#endif

    }

    /// <summary>
    /// HazSolutionExtension
    /// </summary>
    public static class HazSolutionExtension
    {
        /// <summary>
        /// Get Solution
        /// </summary>
        /// <param name="hazSolution"></param>
        /// <returns></returns>
        public static Solution GetSolution(this IHazSolution hazSolution) => hazSolution.Solution;
    }
}
