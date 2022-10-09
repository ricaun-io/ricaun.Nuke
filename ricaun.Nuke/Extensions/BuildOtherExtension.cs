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
            return Solution.GetProjects("*")
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
            return Solution.GetProjects("*")
                .Where(p => p.Name.EndsWith(projectNameEndWith, StringComparison.OrdinalIgnoreCase));
        }

        #endregion
    }
}
