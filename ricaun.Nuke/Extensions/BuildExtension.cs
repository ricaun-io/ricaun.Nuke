using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.MSBuild;
using Nuke.Common.Utilities.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.MSBuild.MSBuildTasks;

namespace ricaun.Nuke.Extensions
{
    public static class BuildExtension
    {
        #region Solution

        /// <summary>
        /// Get all Configuration With no Debug
        /// </summary>
        /// <param name="Solution"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetReleases(this Solution Solution)
        {
            return Solution.GetConfigurations("Debug", true);
        }

        /// <summary>
        /// Get Configurations
        /// </summary>
        /// <param name="Solution"></param>
        /// <param name="contain"></param>
        /// <param name="notContain"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetConfigurations(this Solution Solution, string contain, bool notContain = false)
        {
            var configurations = Solution.GetConfigurations()
                .Where(s => s.Contains(contain, StringComparison.OrdinalIgnoreCase) != notContain);
            return configurations;
        }

        /// <summary>
        /// Get Configurations
        /// </summary>
        /// <param name="Solution"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetConfigurations(this Solution Solution)
        {
            var configurations = Solution.Configurations
                    .Select(pair => pair.Value.Split("|").First())
                    .Distinct();
            return configurations;
        }
        #endregion

        #region BuildMainProject

        /// <summary>
        /// Build the Main project
        /// </summary>
        /// <param name="Solution"></param>
        private static void BuildMainProject(this Solution Solution)
        {
            if (Solution.GetMainProject() is Project project)
            {
                foreach (var configuration in project.GetReleases())
                {
                    project.Build(configuration);
                }
            }
        }

        /// <summary>
        /// Delete All bin / obj folder
        /// </summary>
        /// <param name="Solution"></param>
        /// <param name="BuildProjectDirectory"></param>
        public static void ClearSolution(this Solution Solution, AbsolutePath BuildProjectDirectory)
        {
            GlobDirectories(Solution.Directory, "**/bin", "**/obj")
                .Where(x => !IsDescendantPath(BuildProjectDirectory, x))
                .ForEach(FileSystemTasks.DeleteDirectory);
        }

        /// <summary>
        /// Get Main Project
        /// </summary>
        /// <param name="Solution"></param>
        /// <returns></returns>
        private static Project GetMainProject(this Solution Solution)
        {
            return Solution.GetProjects("*")
                .FirstOrDefault(p => p.Name.Equals(Solution.Name, StringComparison.OrdinalIgnoreCase));
        }

        #endregion

        #region Project
        /// <summary>
        /// Get Project Configurations with no Debug
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetReleases(this Project project)
        {
            return project.GetConfigurations("Debug", true);
        }

        /// <summary>
        /// Get Project Configurations
        /// </summary>
        /// <param name="project"></param>
        /// <param name="contain"></param>
        /// <param name="notContain"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetConfigurations(this Project project, string contain, bool notContain = false)
        {
            var configurations = project.GetConfigurations()
                .Where(s => s.Contains(contain, StringComparison.OrdinalIgnoreCase) != notContain);
            return configurations;
        }

        /// <summary>
        /// Get Project Configurations
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetConfigurations(this Project project)
        {
            var configurations = project.Configurations
                    .Select(pair => pair.Value.Split("|").First())
                    .Distinct();
            return configurations;
        }

        /// <summary>
        /// Build Project
        /// </summary>
        /// <param name="project"></param>
        /// <param name="configuration"></param>
        public static void Build(this Project project, string configuration)
        {
            MSBuild(s => s
                .SetTargets("Rebuild")
                .SetTargetPath(project)
                .SetConfiguration(configuration)
                .SetVerbosity(MSBuildVerbosity.Minimal)
                .SetMaxCpuCount(Environment.ProcessorCount)
                .DisableNodeReuse()
                .EnableRestore()
                );
        }
        #endregion

        #region String
        /// <summary>
        /// Return false if <paramref name="str"/> empty or null
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool SkipEmpty(this string str)
        {
            if (str == null) return false;
            if (str.Trim() == string.Empty) return false;
            return true;
        }

        #endregion
    }
}
