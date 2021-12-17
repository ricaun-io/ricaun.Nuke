using Nuke.Common.ProjectModel;
using System;
using System.Linq;

namespace ricaun.Nuke.Extensions
{
    public static class BuildOtherExtension
    {
        #region Other
        /// <summary>
        /// Build the Other project
        /// </summary>
        /// <param name="Solution"></param>
        /// <param name="projectName"></param>
        /// <param name="afterBuild"></param>
        public static void BuildOtherProject(this Solution Solution, string projectName, Action<Project> afterBuild = null)
        {
            Solution.BuildOtherProject(Solution.GetOtherProject(projectName), afterBuild);
        }

        /// <summary>
        /// Build the Other project
        /// </summary>
        /// <param name="Solution"></param>
        /// <param name="projectName"></param>
        /// <param name="afterBuild"></param>
        public static void BuildOtherProject(this Solution Solution, Project project, Action<Project> afterBuild = null)
        {
            if (project is Project)
            {
                foreach (var configuration in project.GetReleases())
                {
                    project.Build(configuration);
                }
                afterBuild?.Invoke(project);
            }
        }

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

        #endregion
    }
}
