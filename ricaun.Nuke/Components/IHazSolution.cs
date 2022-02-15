﻿using Nuke.Common;
using Nuke.Common.ProjectModel;

namespace ricaun.Nuke.Components
{
    /// <summary>
    /// IHazSolution
    /// </summary>
    public interface IHazSolution : INukeBuild
    {
        /// <summary>
        /// Solution
        /// </summary>
        [Required] [Solution] Solution Solution => TryGetValue(() => Solution);
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
