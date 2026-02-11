using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
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

        #region ConfigurationTargetPlatform
        /// <summary>
        /// Configuration Target Platform
        /// </summary>
        public class ConfigurationTargetPlatform
        {
            /// <summary>
            /// Configuration
            /// </summary>
            public string Configuration { get; set; }
            /// <summary>
            /// TargetPlatform
            /// </summary>
            public string TargetPlatform { get; set; }
            /// <summary>
            /// ToString
            /// </summary>
            /// <returns></returns>
            public override string ToString() => $"{Configuration}|{TargetPlatform}";
            /// <summary>
            /// ConfigurationTargetPlatform
            /// </summary>
            /// <param name="value"></param>
            public static implicit operator ConfigurationTargetPlatform(string value)
            {
                var values = value.Split('|');
                return new ConfigurationTargetPlatform
                {
                    Configuration = values.FirstOrDefault(),
                    TargetPlatform = values.LastOrDefault()
                };
            }
        }

        /// <summary>
        /// Gets the release target platforms for the specified project.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <returns>The release target platforms.</returns>
        public static IEnumerable<ConfigurationTargetPlatform> GetReleaseTargetPlatforms(this Project project)
        {
            return project.GetConfigurationsTargetPlatform(CONFIGURATION_DEBUG, true);
        }

        /// <summary>
        /// Gets the configuration target platforms for the specified project.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <returns>The configuration target platforms.</returns>
        public static IEnumerable<ConfigurationTargetPlatform> GetConfigurationTargetPlatforms(this Project project)
        {
#if NET8_0
            return project.Configurations.Values.Distinct().Select(e => (ConfigurationTargetPlatform)e);
#else
            const string InvalidConfiguration = "?";
            return project.GetModel().ProjectConfigurationRules?
                .Where(e => e.Dimension == Microsoft.VisualStudio.SolutionPersistence.Model.BuildDimension.BuildType)
                .Select(e => new ConfigurationTargetPlatform()
                {
                    Configuration = e.ProjectValue,
                    TargetPlatform = e.SolutionPlatform,
                })
                .Where(e => e.Configuration != InvalidConfiguration)
                .Distinct() ?? Enumerable.Empty<ConfigurationTargetPlatform>();
#endif
        }

        /// <summary>
        /// Gets the configurations target platform for the specified project.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="contain">The string to contain in the configuration.</param>
        /// <param name="notContain">A flag indicating whether the configuration should not contain the specified string.</param>
        /// <returns>The configurations target platform.</returns>
        public static IEnumerable<ConfigurationTargetPlatform> GetConfigurationsTargetPlatform(this Project project, string contain, bool notContain = false)
        {
            return project.GetConfigurationTargetPlatforms().Where(e => e.Configuration.Contains(contain, StringComparison.OrdinalIgnoreCase) != notContain);
        }

        /// <summary>
        /// Builds the project with the specified configuration target platform.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="value">The configuration target platform.</param>
        /// <returns>The outputs of the build.</returns>
        public static IReadOnlyCollection<Output> Build(this Project project, ConfigurationTargetPlatform value)
        {
            Serilog.Log.Information($"Build: {project.Name} - {value}");
            return project.Build(value.Configuration, value.TargetPlatform);
        }

        /// <summary>
        /// Rebuilds the project with the specified configuration target platform.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="value">The configuration target platform.</param>
        /// <returns>The outputs of the rebuild.</returns>
        public static IReadOnlyCollection<Output> Rebuild(this Project project, ConfigurationTargetPlatform value)
        {
            Serilog.Log.Information($"Rebuild: {project.Name} - {value}");
            return project.Rebuild(value.Configuration, value.TargetPlatform);
        }

        #endregion

        #region BuildMainProject
        /// <summary>
        /// Builds the specified project.
        /// </summary>
        /// <param name="solution">The solution.</param>
        /// <param name="project">The project.</param>
        /// <param name="afterBuild">The action to perform after the build.</param>
        public static void BuildProject(this Solution solution, Project project, Action<Project> afterBuild = null)
        {
            if (project is Project)
            {
                foreach (var configuration in project.GetReleaseTargetPlatforms())
                {
                    project.Build(configuration);
                }
                afterBuild?.Invoke(project);
            }
        }

        /// <summary>
        /// Rebuilds the specified project.
        /// </summary>
        /// <param name="solution">The solution.</param>
        /// <param name="project">The project.</param>
        /// <param name="afterBuild">The action to perform after the rebuild.</param>
        public static void RebuildProject(this Solution solution, Project project, Action<Project> afterBuild = null)
        {
            if (project is Project)
            {
                foreach (var configuration in project.GetReleaseTargetPlatforms())
                {
                    project.Rebuild(configuration);
                }
                afterBuild?.Invoke(project);
            }
        }

        /// <summary>
        /// Deletes all the bin/obj folders except for the ones in the specified build project directory.
        /// </summary>
        /// <param name="solution">The solution.</param>
        /// <param name="buildProjectDirectory">The build project directory.</param>
        public static void ClearSolution(this Solution solution, AbsolutePath buildProjectDirectory)
        {
            Globbing.GlobDirectories(solution.Directory, "**/bin", "**/obj")
                .Where(x => !PathConstruction.IsDescendantPath(buildProjectDirectory, x))
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
        /// Gets the release configurations for the specified project.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <returns>The release configurations.</returns>
        public static IEnumerable<string> GetReleases(this Project project)
        {
            return project.GetConfigurations(CONFIGURATION_DEBUG, true);
        }

        /// <summary>
        /// Gets the configurations for the specified project.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="contain">The string to contain in the configuration.</param>
        /// <param name="notContain">A flag indicating whether the configuration should not contain the specified string.</param>
        /// <returns>The configurations.</returns>
        public static IEnumerable<string> GetConfigurations(this Project project, string contain, bool notContain = false)
        {
            var configurations = project.GetConfigurations()
                .Where(s => s.Contains(contain, StringComparison.OrdinalIgnoreCase) != notContain);
            return configurations;
        }

        /// <summary>
        /// Gets the configurations for the specified project.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <returns>The configurations.</returns>
        public static IEnumerable<string> GetConfigurations(this Project project)
        {
            var configurations = project.GetConfigurationTargetPlatforms()
                    .Select(e => e.Configuration)
                    .Distinct();
            return configurations;
        }

        /// <summary>
        /// Rebuilds the project with the specified configuration and target platform.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="targetPlatform">The target platform.</param>
        /// <returns>The outputs of the rebuild.</returns>
        public static IReadOnlyCollection<Output> Rebuild(this Project project, string configuration, string targetPlatform = null)
        {
            return DotNetTasks.DotNetBuild(s => s
                .SetProjectFile(project)
                .SetConfiguration(configuration)
                .TrySetTargetPlatform(targetPlatform)
                .EnableNoIncremental()
            );
            //return MSBuildTasks.MSBuild(s => s
            //    .SetTargets("Rebuild")
            //    .SetTargetPath(project)
            //    .SetConfiguration(configuration)
            //    .TrySetTargetPlatform(targetPlatform)
            //    .SetVerbosity(MSBuildVerbosity.Minimal)
            //    .SetMaxCpuCount(Environment.ProcessorCount)
            //    .DisableNodeReuse()
            //    .EnableRestore()
            //);
        }

        /// <summary>
        /// Builds the project with the specified configuration and target platform.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="targetPlatform">The target platform.</param>
        /// <returns>The outputs of the build.</returns>
        public static IReadOnlyCollection<Output> Build(this Project project, string configuration, string targetPlatform = null)
        {
            return DotNetTasks.DotNetBuild(s => s
                .SetProjectFile(project)
                .SetConfiguration(configuration)
                .TrySetTargetPlatform(targetPlatform)
            );
            //return MSBuildTasks.MSBuild(s => s
            //    .SetTargets("Build")
            //    .SetTargetPath(project)
            //    .SetConfiguration(configuration)
            //    .TrySetTargetPlatform(targetPlatform)
            //    .SetVerbosity(MSBuildVerbosity.Minimal)
            //    .SetMaxCpuCount(Environment.ProcessorCount)
            //    .DisableNodeReuse()
            //    .EnableRestore()
            //);
        }

        private static DotNetBuildSettings TrySetTargetPlatform(this DotNetBuildSettings settings, MSBuildTargetPlatform targetPlatform)
        {
            if (string.IsNullOrWhiteSpace(targetPlatform)) return settings;

            var validPlatforms = new[] {
                MSBuildTargetPlatform.MSIL,
                MSBuildTargetPlatform.x86,
                MSBuildTargetPlatform.x64,
                MSBuildTargetPlatform.arm,
                MSBuildTargetPlatform.Win32
            };
            if (validPlatforms.Contains(targetPlatform))
            {
                return settings.SetPlatform(targetPlatform);
            }

            return settings;
        }

        //private static MSBuildSettings TrySetTargetPlatform(this MSBuildSettings settings, MSBuildTargetPlatform targetPlatform)
        //{
        //    if (string.IsNullOrWhiteSpace(targetPlatform)) return settings;

        //    var validPlatforms = new[] {
        //        MSBuildTargetPlatform.MSIL,
        //        MSBuildTargetPlatform.x86,
        //        MSBuildTargetPlatform.x64,
        //        MSBuildTargetPlatform.arm,
        //        MSBuildTargetPlatform.Win32
        //    };
        //    if (validPlatforms.Contains(targetPlatform))
        //    {
        //        return settings.SetTargetPlatform(targetPlatform);
        //    }

        //    return settings;
        //}
        #endregion

        #region String
        /// <summary>
        /// Returns false if the specified string is empty or null.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns><c>true</c> if the string is not empty or null; otherwise, <c>false</c>.</returns>
        public static bool SkipEmpty(this string str)
        {
            if (str == null) return false;
            if (str.Trim() == string.Empty) return false;
            return true;
        }

        #endregion
    }
}
