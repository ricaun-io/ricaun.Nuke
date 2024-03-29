﻿using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.MSBuild;
using Nuke.Common.Utilities.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ricaun.Nuke.Extensions
{
    /// <summary>
    /// BuildExtension
    /// </summary>
    public static class BuildExtension
    {
        #region Const Configuration
        /// <summary>
        /// Configuration "Debug"
        /// </summary>
        private const string CONFIGURATION_DEBUG = "Debug";
        #endregion

        #region Solution
        /// <summary>
        /// Get all Configuration With no Debug
        /// </summary>
        /// <param name="Solution"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetReleases(this Solution Solution)
        {
            return Solution.GetConfigurations(CONFIGURATION_DEBUG, true);
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
        /// Build Project
        /// </summary>
        /// <param name="Solution"></param>
        /// <param name="project"></param>
        /// <param name="afterBuild"></param>
        public static void BuildProject(this Solution Solution, Project project, Action<Project> afterBuild = null)
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
        /// Rebuild Project
        /// </summary>
        /// <param name="Solution"></param>
        /// <param name="project"></param>
        /// <param name="afterBuild"></param>
        public static void RebuildProject(this Solution Solution, Project project, Action<Project> afterBuild = null)
        {
            if (project is Project)
            {
                foreach (var configuration in project.GetReleases())
                {
                    project.Rebuild(configuration);
                }
                afterBuild?.Invoke(project);
            }
        }

        /// <summary>
        /// Delete All bin / obj folder
        /// </summary>
        /// <param name="Solution"></param>
        /// <param name="BuildProjectDirectory"></param>
        public static void ClearSolution(this Solution Solution, AbsolutePath BuildProjectDirectory)
        {
            Globbing.GlobDirectories(Solution.Directory, "**/bin", "**/obj")
                .Where(x => !PathConstruction.IsDescendantPath(BuildProjectDirectory, x))
                .ForEach((file) =>
                {
                    try
                    {
                        file.DeleteDirectory();
                    }
                    catch (Exception ex)
                    {
                        Serilog.Log.Warning(ex.Message);
                    }
                });
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
            return project.GetConfigurations(CONFIGURATION_DEBUG, true);
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
        /// Rebuild Project
        /// </summary>
        /// <param name="project"></param>
        /// <param name="configuration"></param>
        public static IReadOnlyCollection<Output> Rebuild(this Project project, string configuration)
        {
            return MSBuildTasks.MSBuild(s => s
                .SetTargets("Rebuild")
                .SetTargetPath(project)
                .SetConfiguration(configuration)
                .SetVerbosity(MSBuildVerbosity.Minimal)
                .SetMaxCpuCount(Environment.ProcessorCount)
                .DisableNodeReuse()
                .EnableRestore()
                );
        }

        /// <summary>
        /// Build Project
        /// </summary>
        /// <param name="project"></param>
        /// <param name="configuration"></param>
        public static IReadOnlyCollection<Output> Build(this Project project, string configuration)
        {
            return MSBuildTasks.MSBuild(s => s
                .SetTargets("Build")
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
