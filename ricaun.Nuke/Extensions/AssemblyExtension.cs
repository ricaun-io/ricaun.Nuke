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
    public static class AssemblyExtension
    {
        #region Version Dll

        /// <summary>
        /// Get Project Attributes
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public static IEnumerable<CustomAttributeData> GetAttributes(this Project project)
        {
            var assembly = project.GetAssemblyGreaterVersion();
            return assembly.CustomAttributes;
        }

        /// <summary>
        /// Get Project Version by dll
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public static Version GetVersion(this Project project)
        {
            return project.GetAssemblyGreaterVersion().GetName().Version;
        }

        /// <summary>
        /// Get Company by dll
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public static string GetCompany(this Project project)
        {
            return project.GetAssemblyGreaterVersion().GetCompany();
        }

        /// <summary>
        /// Get ProjectId
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public static string GetAppId(this Project project)
        {
            return project.ProjectId.ToString();
        }

        /// <summary>
        /// Get File Assembly Greater Version
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public static Assembly GetAssemblyGreaterVersion(this Project project)
        {
            return GetAssemblyGreaterVersion(project.Directory, $"*{project.Name}*.dll");
        }

        /// <summary>
        /// GetAssemblyGreaterVersion
        /// </summary>
        /// <param name="sourceDir"></param>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        private static Assembly GetAssemblyGreaterVersion(string sourceDir, string searchPattern = "*.dll")
        {
            Assembly assembly = null;
            Version version = new Version();
            var dllFiles = Directory.GetFiles(sourceDir, searchPattern, SearchOption.AllDirectories);
            foreach (var dll in dllFiles)
            {
                try
                {
                    var assemblyTest = Assembly.Load(File.ReadAllBytes(dll));
                    var fileVersion = assemblyTest.GetName().Version;
                    if (version < fileVersion)
                    {
                        version = fileVersion;
                        assembly = assemblyTest;
                    }
                }
                catch { }
            }
            return assembly;
        }

        /// <summary>
        /// Get File dll Greater Version
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public static string GetInformationalVersion(this Project project)
        {
            return project.GetAssemblyGreaterVersion().GetInformationalVersion();
        }

        #endregion

        #region Assembly

        /// <summary>
        /// Get InformationalVersion
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static string GetInformationalVersion(this Assembly assembly) => assembly.GetValue<AssemblyInformationalVersionAttribute>();

        /// <summary>
        /// Get FileVersion
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static string GetFileVersion(this Assembly assembly) => assembly.GetValue<AssemblyFileVersionAttribute>();

        /// <summary>
        /// Get Title
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static string GetTitle(this Assembly assembly) => assembly.GetValue<AssemblyTitleAttribute>();

        /// <summary>
        /// Get Description
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static string GetDescription(this Assembly assembly) => assembly.GetValue<AssemblyDescriptionAttribute>();

        /// <summary>
        /// Get Company
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static string GetCompany(this Assembly assembly) => assembly.GetValue<AssemblyCompanyAttribute>();

        /// <summary>
        /// Get Product
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static string GetProduct(this Assembly assembly) => assembly.GetValue<AssemblyProductAttribute>();

        /// <summary>
        /// Get Copyright
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static string GetCopyright(this Assembly assembly) => assembly.GetValue<AssemblyCopyrightAttribute>();

        /// <summary>
        /// Get Trademark
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static string GetTrademark(this Assembly assembly) => assembly.GetValue<AssemblyTrademarkAttribute>();

        /// <summary>
        /// Get Configuration
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static string GetConfiguration(this Assembly assembly) => assembly.GetValue<AssemblyConfigurationAttribute>();

        /// <summary>
        /// GetValue of CustomAttributeType 
        /// </summary>
        /// <typeparam name="TCustomAttributeType"></typeparam>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static string GetValue<TCustomAttributeType>(this Assembly assembly)
        {
            var attribute = assembly.CustomAttributes.Where(y => y.AttributeType == typeof(TCustomAttributeType)).FirstOrDefault();
            if (attribute == null) return "";
            return attribute.ConstructorArguments[0].Value.ToString();
        }
        #endregion
    }
}
