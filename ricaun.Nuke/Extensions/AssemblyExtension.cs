﻿using Nuke.Common.ProjectModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ricaun.Nuke.Extensions
{
    /// <summary>
    /// AssemblyExtension
    /// </summary>
    public static class AssemblyExtension
    {
        #region FileVersionInfo

        /// <summary>
        /// GetVersion => ProductVersion
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public static Version GetVersion(this Project project)
        {
            var version = project.GetFileVersionInfo()?.ProductVersion;
            if (version == null) version = "0.0.0.0";
            return new Version(version);
        }

        /// <summary>
        /// GetProduct => ProductName
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public static string GetProduct(this Project project) => project.GetFileVersionInfo()?.ProductName;
        /// <summary>
        /// GetTitle => ProductName
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public static string GetTitle(this Project project) => project.GetFileVersionInfo()?.ProductName;

        /// <summary>
        /// GetFileVersion => FileVersion
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public static string GetFileVersion(this Project project) => project.GetFileVersionInfo()?.FileVersion;

        /// <summary>
        /// GetFileDescription => FileDescription
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public static string GetFileDescription(this Project project) => project.GetFileVersionInfo()?.FileDescription;

        /// <summary>
        /// GetAssemblyName => FileDescription
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public static string GetAssemblyName(this Project project) => project.GetFileVersionInfo()?.FileDescription;

        /// <summary>
        /// GetInformationalVersion => ProductVersion
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public static string GetInformationalVersion(this Project project) => project.GetFileVersionInfo()?.ProductVersion;

        /// <summary>
        /// GetCompany => CompanyName
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public static string GetCompany(this Project project) => project.GetFileVersionInfo()?.CompanyName;

        /// <summary>
        /// GetDescription => Comments
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public static string GetDescription(this Project project) => project.GetFileVersionInfo()?.Comments;

        /// <summary>
        /// GetComments => Comments
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public static string GetComments(this Project project) => project.GetFileVersionInfo()?.Comments;

        /// <summary>
        /// GetCopyright => LegalCopyright
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public static string GetCopyright(this Project project) => project.GetFileVersionInfo()?.LegalCopyright;
        /// <summary>
        /// GetTrademark => LegalTrademarks
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public static string GetTrademark(this Project project) => project.GetFileVersionInfo()?.LegalTrademarks;

        /// <summary>
        /// Get ProjectId
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public static string GetAppId(this Project project) => project.ProjectId.ToString();

        /// <summary>
        /// Get FileVersionInfo of Greater Dlls
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public static FileVersionInfo GetFileVersionInfo(this Project project)
        {
            return GetFileVersionInfoGreater(project.Directory, $"*{project.Name}*.dll");
        }

        /// <summary>
        /// GetFileVersionInfoGreater
        /// </summary>
        /// <param name="sourceDir"></param>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        private static FileVersionInfo GetFileVersionInfoGreater(string sourceDir, string searchPattern = "*.dll")
        {
            FileVersionInfo fileVersionInfo = null;
            Version version = new Version();
            var dllFiles = Directory.GetFiles(sourceDir, searchPattern, SearchOption.AllDirectories);
            foreach (var dll in dllFiles)
            {
                try
                {
                    var fileVersionInfoTest = FileVersionInfo.GetVersionInfo(dll);
                    var fileVersion = new Version(fileVersionInfoTest.FileVersion);
                    if (version < fileVersion)
                    {
                        version = fileVersion;
                        fileVersionInfo = fileVersionInfoTest;
                    }
                }
                catch { }
            }
            return fileVersionInfo;
        }

        /// <summary>
        /// ShowInformation
        /// </summary>
        /// <param name="project"></param>
        public static void ShowInformation(this Project project)
        {
            Serilog.Log.Information($"-");
            Serilog.Log.Information($"Name: {project.Name}");
            Serilog.Log.Information($"GetAppId: {project.GetAppId()}");

            if (project.GetFileVersionInfo() == null)
                Serilog.Log.Warning($"GetFileVersionInfo: {project.Name} not found!");

            Serilog.Log.Information($"GetInformationalVersion: {project.GetInformationalVersion()}");
            Serilog.Log.Information($"GetAssemblyName: {project.GetAssemblyName()}");
            Serilog.Log.Information($"GetVersion: {project.GetVersion()}");
            Serilog.Log.Information($"GetFileVersion: {project.GetFileVersion()}");
            Serilog.Log.Information($"GetTitle: {project.GetTitle()}");
            Serilog.Log.Information($"GetTrademark: {project.GetTrademark()}");
            Serilog.Log.Information($"GetCompany: {project.GetCompany()}");
            Serilog.Log.Information($"GetProduct: {project.GetProduct()}");
            Serilog.Log.Information($"GetCopyright: {project.GetCopyright()}");
            Serilog.Log.Information($"GetDescription: {project.GetDescription()}");
            Serilog.Log.Information($"GetComments: {project.GetComments()}");
            Serilog.Log.Information($"GetFileDescription: {project.GetFileDescription()}");
            Serilog.Log.Information($"-");

            //var ass = project.GetAssemblyGreaterVersion();

            //Serilog.Log.Information($"-");
            //Serilog.Log.Information($"GetInformationalVersion: {ass.GetInformationalVersion()}");
            //Serilog.Log.Information($"GetVersion: {ass.GetVersion()}");
            //Serilog.Log.Information($"GetFileVersion: {ass.GetFileVersion()}");
            //Serilog.Log.Information($"GetTitle: {ass.GetTitle()}");
            //Serilog.Log.Information($"GetTrademark: {ass.GetTrademark()}");
            //Serilog.Log.Information($"GetCompany: {ass.GetCompany()}");
            //Serilog.Log.Information($"GetProduct: {ass.GetProduct()}");
            //Serilog.Log.Information($"GetCopyright: {ass.GetCopyright()}");
            //Serilog.Log.Information($"GetDescription: {ass.GetDescription()}");
            //Serilog.Log.Information($"-");
        }

        #endregion

        #region Assembly

        /// <summary>
        /// Get Version with major.minor.build
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static string GetVersion(this Assembly assembly) => assembly.GetName().Version.ToString(3);

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
        /// Get InformationalVersion
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        private static string GetInformationalVersion(this Assembly assembly) => assembly.GetValue<AssemblyInformationalVersionAttribute>(assembly.GetVersion());

        /// <summary>
        /// Get FileVersion
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        private static string GetFileVersion(this Assembly assembly) => assembly.GetValue<AssemblyFileVersionAttribute>(assembly.GetVersion());

        /// <summary>
        /// Get Title
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        private static string GetTitle(this Assembly assembly) => assembly.GetValue<AssemblyTitleAttribute>();

        /// <summary>
        /// Get Description
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        private static string GetDescription(this Assembly assembly) => assembly.GetValue<AssemblyDescriptionAttribute>();

        /// <summary>
        /// Get Company
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        private static string GetCompany(this Assembly assembly) => assembly.GetValue<AssemblyCompanyAttribute>();

        /// <summary>
        /// Get Product
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        private static string GetProduct(this Assembly assembly) => assembly.GetValue<AssemblyProductAttribute>();

        /// <summary>
        /// Get Copyright
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        private static string GetCopyright(this Assembly assembly) => assembly.GetValue<AssemblyCopyrightAttribute>();

        /// <summary>
        /// Get Trademark
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        private static string GetTrademark(this Assembly assembly) => assembly.GetValue<AssemblyTrademarkAttribute>();

        /// <summary>
        /// Get Configuration
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        private static string GetConfiguration(this Assembly assembly) => assembly.GetValue<AssemblyConfigurationAttribute>();

        /// <summary>
        /// GetValue of CustomAttributeType 
        /// </summary>
        /// <typeparam name="TCustomAttributeType"></typeparam>
        /// <param name="assembly"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private static string GetValue<TCustomAttributeType>(this Assembly assembly, string defaultValue = "")
        {
            try
            {
                var attribute = assembly.CustomAttributes.Where(y => y.AttributeType == typeof(TCustomAttributeType)).FirstOrDefault();
                if (attribute == null) return defaultValue;
                return attribute.ConstructorArguments[0].Value.ToString();

            }
            catch (Exception)
            {
                Serilog.Log.Warning($"Exception: {typeof(TCustomAttributeType).Name}");
                return defaultValue;
            }
        }
        #endregion
    }
}
