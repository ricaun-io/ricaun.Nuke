using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using ricaun.Nuke.Tools.MSBuild;
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
        #region BuildTools
        /// <summary>
        /// Gets the list of supported build tools, the default order is MSBuild first, then dotnet. You can change the order or remove build tools as needed. If the list is empty, it will fall back to using 'dotnet build'.
        /// </summary>
        public static IList<BuildTool> BuildTools { get; set; } = new List<BuildTool>() {
            BuildTool.MSBuild,
            BuildTool.dotnetBuild
        };

        /// <summary>
        /// Configures the specified build tools list to contain only the MSBuild tool.
        /// </summary>
        /// <param name="buildTools">The list of build tools to modify.</param>
        /// <returns>The same <see cref="IList{BuildTool}"/> instance after clearing and adding <see cref="BuildTool.MSBuild"/>.</returns>
        /// <remarks>
        /// This method clears any existing entries in <paramref name="buildTools"/> and adds
        /// <see cref="BuildTool.MSBuild"/> as the sole build tool. Useful for scenarios where
        /// you want to force MSBuild usage only.
        /// </remarks>
        public static IList<BuildTool> MSBuildOnly(this IList<BuildTool> buildTools)
        {
            buildTools.Clear();
            buildTools.Add(BuildTool.MSBuild);
            return buildTools;
        }

        /// <summary>
        /// Configures the specified build tools list to contain only the dotnet CLI tool.
        /// </summary>
        /// <param name="buildTools">The list of build tools to modify.</param>
        /// <returns>The same <see cref="IList{BuildTool}"/> instance after clearing and adding <see cref="BuildTool.dotnetBuild"/>.</returns>
        /// <remarks>
        /// This method clears any existing entries in <paramref name="buildTools"/> and adds
        /// <see cref="BuildTool.dotnetBuild"/> as the sole build tool. Useful for scenarios where
        /// you want to force the usage of 'dotnet' only.
        /// </remarks>
        public static IList<BuildTool> dotnetBuildOnly(this IList<BuildTool> buildTools)
        {
            buildTools.Clear();
            buildTools.Add(BuildTool.dotnetBuild);
            return buildTools;
        }

        /// <summary>
        /// Specifies the supported build tools for project compilation and build operations.
        /// </summary>
        /// <remarks>Use this enumeration to select the build tool when configuring or invoking build
        /// processes. The available options correspond to common .NET build systems.</remarks>
        public enum BuildTool
        {
            /// <summary>
            /// MSBuild
            /// </summary>
            MSBuild,
            /// <summary>
            /// dotnet
            /// </summary>
            dotnetBuild
        }
        #endregion


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
            return project.GetModel().GetConfigurations().Select(e => new ConfigurationTargetPlatform()
            {
                Configuration = e.ProjectConfiguration,
                TargetPlatform = e.ProjectPlatform,
            });
#endif
        }

#if NET10_0_OR_GREATER
        internal static IReadOnlyList<Configuration> GetConfigurations(this Microsoft.VisualStudio.SolutionPersistence.Model.SolutionProjectModel solutionProjectModel)
        {
            var result = new List<Configuration>();
            var solutionModel = solutionProjectModel.Solution;
            foreach (var buildType in solutionModel.BuildTypes)
            {
                foreach (var platform in solutionModel.Platforms)
                {
                    var projectConfiguration = solutionProjectModel.GetProjectConfiguration(buildType, platform);
                    var configuration = new Configuration(
                        buildType,
                        platform,
                        projectConfiguration.BuildType ?? buildType,
                        projectConfiguration.Platform ?? platform,
                        projectConfiguration.Build,
                        projectConfiguration.Deploy
                    );

                    if (result.Any(c =>
                        c.ProjectConfiguration == configuration.ProjectConfiguration &&
                        c.ProjectPlatform == configuration.ProjectPlatform))
                        continue;

                    result.Add(configuration);
                }
            }
            return result;
        }

        internal sealed class Configuration
        {
            public string SolutionConfiguration { get; }
            public string SolutionPlatform { get; }
            public string ProjectConfiguration { get; }
            public string ProjectPlatform { get; }
            public bool Build { get; }
            public bool Deploy { get; }

            public Configuration(
                string solutionConfiguration,
                string solutionPlatform,
                string projectConfiguration,
                string projectPlatform,
                bool build,
                bool deploy)
            {
                SolutionConfiguration = solutionConfiguration;
                SolutionPlatform = solutionPlatform;
                ProjectConfiguration = projectConfiguration;
                ProjectPlatform = projectPlatform;
                Build = build;
                Deploy = deploy;
            }
            public override string ToString()
                => $"{SolutionConfiguration}|{SolutionPlatform}\t" +
                   $"{ProjectConfiguration}|{ProjectPlatform} (Build={Build})";
        }
#endif

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
            IReadOnlyCollection<Output> MSBuild()
                => MSBuildTasks.MSBuild(s => s
                    .SetTargets("Rebuild")
                    .SetTargetPath(project)
                    .SetConfiguration(configuration)
                    .TrySetTargetPlatform(targetPlatform)
                    .SetVerbosity(MSBuildVerbosity.Minimal)
                    .SetMaxCpuCount(Environment.ProcessorCount)
                    .DisableNodeReuse()
                    .EnableRestore()
                );

            IReadOnlyCollection<Output> DotNet() =>
                DotNetTasks.DotNetBuild(s => s
                    .SetProjectFile(project)
                    .SetConfiguration(configuration)
                    .TrySetTargetPlatform(targetPlatform)
                    .EnableNoIncremental()
                );

            return BuildUsingMSBuildOrDotNet(MSBuild, DotNet, project, configuration);
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
            IReadOnlyCollection<Output> MSBuild()
                => MSBuildTasks.MSBuild(s => s
                    .SetTargets("Build")
                    .SetTargetPath(project)
                    .SetConfiguration(configuration)
                    .TrySetTargetPlatform(targetPlatform)
                    .SetVerbosity(MSBuildVerbosity.Minimal)
                    .SetMaxCpuCount(Environment.ProcessorCount)
                    .DisableNodeReuse()
                    .EnableRestore()
                );

            IReadOnlyCollection<Output> DotNet() =>
                DotNetTasks.DotNetBuild(s => s
                    .SetProjectFile(project)
                    .SetConfiguration(configuration)
                    .TrySetTargetPlatform(targetPlatform)
                );

            return BuildUsingMSBuildOrDotNet(MSBuild, DotNet, project, configuration);
        }

        private static IReadOnlyCollection<Output> BuildUsingMSBuildOrDotNet(
            Func<IReadOnlyCollection<Output>> MSBuild,
            Func<IReadOnlyCollection<Output>> DotNet,
            Project project, string configuration)
        {
            if (BuildTools.Count == 0)
            {
                Serilog.Log.Information($"No build tools specified. Falling back to 'dotnet build' for project '{project}' with configuration '{configuration}'.");
                return DotNet();
            }

            var failToBuild = false;
            foreach (var buildTool in BuildTools)
            {
                try
                {
                    switch (buildTool)
                    {
                        case BuildTool.MSBuild:
                            if (failToBuild)
                            {
                                Serilog.Log.Warning($"Falling back to 'MSBuild' to build project '{project}' with configuration '{configuration}'.");
                            }
                            return MSBuild();
                        case BuildTool.dotnetBuild:
                            if (failToBuild)
                            {
                                Serilog.Log.Warning($"Falling back to 'dotnet build' to build project '{project}' with configuration '{configuration}'.");
                            }
                            return DotNet();
                    }
                }
                catch (Exception)
                {
                    failToBuild = true;
                }
            }

            var exception = new Exception($"Failed to build project '{project}' with configuration '{configuration}' using the specified build tools.");
            throw exception;
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

        private static MSBuildSettings TrySetTargetPlatform(this MSBuildSettings settings, MSBuildTargetPlatform targetPlatform)
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
                return settings.SetTargetPlatform(targetPlatform);
            }

            return settings;
        }
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
