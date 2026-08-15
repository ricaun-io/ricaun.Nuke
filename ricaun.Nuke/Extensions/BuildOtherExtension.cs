using Nuke.Common.ProjectModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ricaun.Nuke.Extensions
{
    /// <summary>
    /// BuildOtherExtension
    /// </summary>
    public static class BuildOtherExtension
    {
        #region Other
        /// <summary>
        /// Get Other Project
        /// </summary>
        /// <param name="Solution"></param>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public static Project GetOtherProject(this Solution Solution, string projectName)
        {
            return Solution.GetAllProjectsOrderByName("*")
                .FirstOrDefault(p => p.Name.Equals(projectName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Get Others Projects EndWith
        /// </summary>
        /// <param name="Solution"></param>
        /// <param name="projectNameEndWith"></param>
        /// <returns></returns>
        public static IEnumerable<Project> GetOtherProjects(this Solution Solution, string projectNameEndWith)
        {
            return Solution.GetAllProjectsOrderByName("*")
                .Where(p => p.Name.EndsWith(projectNameEndWith, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Gets all projects matching a wildcard pattern order by name.
        /// </summary>
        /// <param name="Solution">The solution to search for projects.</param>
        /// <param name="wildcardPattern">The wildcard pattern to match project names.</param>
        /// <returns>An enumerable of projects matching the wildcard pattern, ordered by name.</returns>
        public static IEnumerable<Project> GetAllProjectsOrderByName(this Solution Solution, string wildcardPattern)
        {
            return Solution.GetAllProjects(wildcardPattern)
                .OrderBy(p => p.Name);
        }

        #endregion
    }
}
