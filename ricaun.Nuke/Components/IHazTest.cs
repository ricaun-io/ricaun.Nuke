using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using ricaun.Nuke.Components;
using ricaun.Nuke.Extensions;
using ricaun.Nuke.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ricaun.Nuke.Components
{
    /// <summary>
    /// IHazTest
    /// </summary>
    public interface IHazTest : IHazSolution, IHazGitHubActions
    {
        /// <summary>
        /// GetTestDirectory
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public AbsolutePath GetTestDirectory(Project project) => project.Directory / "bin" / "TestResults";

        /// <summary>
        /// TestProjects
        /// </summary>
        /// <param name="testProjectName"></param>
        /// <param name="testResults"></param>
        /// <param name="testBuildStopWhenFailed"></param>
        /// <param name="customDotNetTestSettings"></param>
        /// <exception cref="Exception"></exception>
        public void TestProjects(string testProjectName,
            bool testResults = true,
            bool testBuildStopWhenFailed = true,
            Func<DotNetTestSettings, DotNetTestSettings> customDotNetTestSettings = null)
        {
            var testFailed = false;
            var testProjects = Solution.GetAllProjects(testProjectName);

            foreach (var testProject in testProjects)
            {
                var testResultsDirectory = GetTestDirectory(testProject);

                var configurations = testProject.GetReleases();
                foreach (var configuration in configurations)
                {
                    var targetVersion = string.Empty;
                    try
                    {
                        Serilog.Log.Logger.Information($"Build: {testProject.Name} {configuration}");
                        testProject.Build(configuration);

                        targetVersion = testProject.GetLastTargetFrameworkVersion();

                        Serilog.Log.Logger.Information($"Test: {testProject.Name} {configuration} {targetVersion}");

                        DotNetTasks.DotNetTest(_ => _
                            .SetProjectFile(testProject)
                            .SetConfiguration(configuration)
                            .SetVerbosity(DotNetVerbosity.Normal)
                            .EnableNoBuild()
                            .SetCustomDotNetTestSettings(customDotNetTestSettings)
                            .When(testResults, _ => _
                                .SetLoggers($"trx;LogFileName={ProjectTestFileName(testProject, configuration, targetVersion)}")
                                .SetResultsDirectory(testResultsDirectory)));
                    }
                    catch (Exception ex)
                    {
                        Serilog.Log.Logger.Information($"Exception: {ex}");
                        testFailed = true;
                    }

                    if (testResults)
                        testFailed |= CheckReportTestProject(testProject, configuration, targetVersion);
                }
            }

            if (testResults)
            {
                ReportTestProjects(testProjects);
            }

            if (testFailed & testBuildStopWhenFailed)
            {
                throw new Exception($"Test Failed = {testFailed}");
            }
        }

        /// <summary>
        /// ReportTestProjects
        /// </summary>
        /// <param name="testProjects"></param>
        void ReportTestProjects(IEnumerable<Project> testProjects)
        {
            var testFiles = testProjects.SelectMany(testProject =>
                Globbing.GlobFiles(GetTestDirectory(testProject), ProjectTestFileName(testProject, "*"))
            );

            if (testFiles.IsEmpty())
            {
                Serilog.Log.Logger.Error($"ReportTestProjects: TestFiles is Empty!");
                return;
            }

            var testReport = TestReportUtils.GetTestReport(testFiles);
            var passedTests = testReport.Passed;
            var failedTests = testReport.Failed;
            var skippedTests = testReport.Skipped;

            ReportSummary(_ => _
                .When(failedTests > 0, _ => _
                    .AddPair("Failed", failedTests.ToString()))
                .AddPair("Passed", passedTests.ToString())
                .When(skippedTests > 0, _ => _
                    .AddPair("Skipped", skippedTests.ToString())));

            foreach (var testFile in testFiles)
            {
                Serilog.Log.Logger.Information($"TestFile: {testFile}");
            }

            try
            {
                ReportTestProjectsGitHubSummary(testFiles);
            }
            catch (Exception ex)
            {
                Serilog.Log.Logger.Warning($"ReportTestProjectsGitHubSummary Fail");
                Serilog.Log.Logger.Information($"ReportTestProjectsGitHubSummary {ex}");
            }
        }

        /// <summary>
        /// Simple ReportTestProjectsGitHubSummary
        /// </summary>
        /// <param name="resultFiles"></param>
        void ReportTestProjectsGitHubSummary(IEnumerable<AbsolutePath> resultFiles)
        {
            if (resultFiles.IsEmpty()) return;

            var summaryTestReport = TestRunUtil.Markdown.GetSummaryTestReports(resultFiles);
            GitHubSummary(summaryTestReport);

            foreach (var resultFile in resultFiles)
            {
                var testReport = TestRunUtil.Markdown.GetDetailsTestReport(resultFile);
                GitHubSummary(testReport);
            }
        }

        /// <summary>
        /// ProjectTestFileName
        /// </summary>
        /// <param name="testProject"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        string ProjectTestFileName(Project testProject, string configuration)
        {
            return $"{testProject.Name}_{configuration}.trx";
        }

        /// <summary>
        /// ProjectTestFileName
        /// </summary>
        /// <param name="testProject"></param>
        /// <param name="configuration"></param>
        /// <param name="targetVersion"></param>
        /// <returns></returns>
        string ProjectTestFileName(Project testProject, string configuration, string targetVersion)
        {
            if (string.IsNullOrWhiteSpace(targetVersion) == false)
                configuration += "_" + targetVersion;

            return ProjectTestFileName(testProject, configuration);
        }

        /// <summary>
        /// CheckReportTestProject
        /// </summary>
        /// <param name="testProject"></param>
        /// <param name="configuration"></param>
        /// <param name="targetVersion"></param>
        /// <returns>Return if fail some test</returns>
        bool CheckReportTestProject(Project testProject, string configuration, string targetVersion = null)
        {
            var testResultsDirectory = GetTestDirectory(testProject);

            var resultFiles = Globbing.GlobFiles(testResultsDirectory, ProjectTestFileName(testProject, configuration, targetVersion));

            if (resultFiles.IsEmpty())
            {
                Serilog.Log.Logger.Error($"CheckReportTestProject: ResultFiles is Empty!");
                return true;
            }

            var testReport = TestReportUtils.GetTestReport(resultFiles);
            var passedTests = testReport.Passed;
            var failedTests = testReport.Failed;
            var skippedTests = testReport.Skipped;
            var totalSeconds = testReport.TotalSeconds;

            var message = $"ReportTest: {testProject.Name} ({configuration}) \t Passed: {passedTests} \t Skipped: {skippedTests} \t Failed: {failedTests} \t TotalSeconds: {totalSeconds:0.00}";
            Serilog.Log.Logger.Information(message);

            if (failedTests > 0)
                Serilog.Log.Error(message);

            if (passedTests == 0 && skippedTests == 0)
                return true;

            return failedTests > 0;
        }
    }
}